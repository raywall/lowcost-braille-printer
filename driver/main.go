package main

import (
	"bytes"
	"compress/zlib"
	"encoding/binary"
	"errors"
	"flag"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"os/exec"
	"path/filepath"
	"runtime"
	"strings"
	"sync"
	"syscall"
	"time"
	"unicode/utf16"
)

const (
	defaultAddr    = "127.0.0.1:8631"
	maxLineCells   = 28
	maxMessageBody = 160
	serialWarmup   = 2200 * time.Millisecond
	serialChunk    = 32
	serialChunkGap = 5 * time.Millisecond
	serialAckWait  = 45 * time.Second
	serialAckGap   = 50 * time.Millisecond
)

func main() {
	addr := flag.String("addr", defaultAddr, "HTTP/IPP listen address")
	device := flag.String("device", "", "serial device path, for example /dev/cu.usbmodem1101 or COM3")
	out := flag.String("out", "", "debug output file used instead of a serial device")
	flag.Parse()

	transport, err := newTransport(*device, *out)
	if err != nil {
		log.Fatalf("transport: %v", err)
	}

	server := &printServer{
		transport:  transport,
		started:    time.Now(),
		printerURI: printerURIForAddr(*addr),
	}

	mux := http.NewServeMux()
	mux.HandleFunc("/", server.handleHome)
	mux.HandleFunc("/printers/braille", server.handleIPP)
	mux.HandleFunc("/printers/braille/status", server.handleHome)

	log.Printf("Braille Print Server listening on http://%s", *addr)
	log.Printf("Printer URI: %s", server.printerURI)
	if err := http.ListenAndServe(*addr, mux); err != nil {
		log.Fatal(err)
	}
}

type printServer struct {
	mu         sync.Mutex
	jobs       []printJob
	nextJobID  int
	started    time.Time
	transport  transport
	printerURI string
}

type printJob struct {
	ID        int
	Name      string
	CreatedAt time.Time
	Bytes     int
	Status    string
	Error     string
}

func (s *printServer) handleHome(w http.ResponseWriter, r *http.Request) {
	s.mu.Lock()
	jobs := append([]printJob(nil), s.jobs...)
	s.mu.Unlock()

	var body strings.Builder
	body.WriteString("<!doctype html><html><head><meta charset=\"utf-8\"><title>Braille Print Server</title>")
	body.WriteString("<style>body{font-family:system-ui,sans-serif;max-width:860px;margin:40px auto;padding:0 20px;color:#17202a}code{background:#eef2f7;padding:2px 5px;border-radius:4px}table{border-collapse:collapse;width:100%;margin-top:24px}td,th{border-bottom:1px solid #d8dee8;padding:8px;text-align:left}.ok{color:#176b3a}.err{color:#9b1c1c}</style>")
	body.WriteString("</head><body><h1>Braille Print Server</h1>")
	body.WriteString("<p>Servidor local ativo. Configure a impressora pelo URI <code>")
	body.WriteString(s.printerURI)
	body.WriteString("</code>.</p>")
	body.WriteString("<p>Transporte: <code>")
	body.WriteString(s.transport.Name())
	body.WriteString("</code></p>")
	body.WriteString("<table><thead><tr><th>ID</th><th>Nome</th><th>Bytes</th><th>Status</th><th>Erro</th></tr></thead><tbody>")
	for i := len(jobs) - 1; i >= 0; i-- {
		job := jobs[i]
		statusClass := "ok"
		if job.Error != "" {
			statusClass = "err"
		}
		body.WriteString(fmt.Sprintf("<tr><td>%d</td><td>%s</td><td>%d</td><td class=\"%s\">%s</td><td>%s</td></tr>",
			job.ID, htmlEscape(job.Name), job.Bytes, statusClass, htmlEscape(job.Status), htmlEscape(job.Error)))
	}
	body.WriteString("</tbody></table></body></html>")

	w.Header().Set("Content-Type", "text/html; charset=utf-8")
	_, _ = io.WriteString(w, body.String())
}

func (s *printServer) handleIPP(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodPost {
		http.Error(w, "IPP endpoint expects POST", http.StatusMethodNotAllowed)
		return
	}

	body, err := io.ReadAll(r.Body)
	if err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	req, err := parseIPPRequest(body)
	if err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	switch req.operationID {
	case 0x0002: // Print-Job
		s.printJob(w, req)
	case 0x0004: // Validate-Job
		writeIPPResponse(w, req.requestID, 0x0000, printerAttributes(s.printerURI))
	case 0x0005: // Create-Job
		s.createJob(w, req)
	case 0x0006: // Send-Document
		s.sendDocument(w, req)
	case 0x0009: // Get-Job-Attributes
		s.getJobAttributes(w, req)
	case 0x000B: // Get-Printer-Attributes
		writeIPPResponse(w, req.requestID, 0x0000, printerAttributes(s.printerURI))
	default:
		writeIPPResponse(w, req.requestID, 0x0501, printerAttributes(s.printerURI))
	}
}

func (s *printServer) printJob(w http.ResponseWriter, req ippRequest) {
	name := req.attrString("job-name")
	if name == "" {
		name = "Braille job"
	}

	payload := bytes.TrimSpace(req.document)
	if len(payload) == 0 {
		writeIPPResponse(w, req.requestID, 0x0400, printerAttributes(s.printerURI))
		return
	}

	text, err := documentText(payload)
	if err != nil {
		writeIPPResponse(w, req.requestID, 0x0400, printerAttributes(s.printerURI))
		return
	}

	messages := encodeBrailleMessages(text)

	s.mu.Lock()
	jobID := s.nextIDLocked()
	s.jobs = append(s.jobs, printJob{
		ID:        jobID,
		Name:      name,
		CreatedAt: time.Now(),
		Bytes:     len(payload),
		Status:    "printing",
	})
	jobIndex := len(s.jobs) - 1
	s.mu.Unlock()

	logPrintPlan(fmt.Sprintf("job %q", name), payload, text, messages)
	err = s.transport.SendMessages(messages)

	s.mu.Lock()
	if err != nil {
		s.jobs[jobIndex].Status = "error"
		s.jobs[jobIndex].Error = err.Error()
	} else {
		s.jobs[jobIndex].Status = "completed"
	}
	s.mu.Unlock()

	if err != nil {
		writeIPPResponse(w, req.requestID, 0x0500, printerAttributes(s.printerURI))
		return
	}
	writeIPPResponse(w, req.requestID, 0x0000, append(printerAttributes(s.printerURI), jobAttributes(s.printerURI, s.jobs[jobIndex])...))
}

func (s *printServer) createJob(w http.ResponseWriter, req ippRequest) {
	name := req.attrString("job-name")
	if name == "" {
		name = "Braille job"
	}

	s.mu.Lock()
	jobID := s.nextIDLocked()
	job := printJob{
		ID:        jobID,
		Name:      name,
		CreatedAt: time.Now(),
		Status:    "pending",
	}
	s.jobs = append(s.jobs, job)
	s.mu.Unlock()

	writeIPPResponse(w, req.requestID, 0x0000, append(printerAttributes(s.printerURI), jobAttributes(s.printerURI, job)...))
}

func (s *printServer) sendDocument(w http.ResponseWriter, req ippRequest) {
	jobID := req.attrInt("job-id")
	payload := bytes.TrimSpace(req.document)
	if jobID == 0 || len(payload) == 0 {
		writeIPPResponse(w, req.requestID, 0x0400, printerAttributes(s.printerURI))
		return
	}

	s.mu.Lock()
	jobIndex := -1
	for i := range s.jobs {
		if s.jobs[i].ID == jobID {
			jobIndex = i
			s.jobs[i].Bytes = len(payload)
			s.jobs[i].Status = "printing"
			break
		}
	}
	s.mu.Unlock()

	if jobIndex == -1 {
		writeIPPResponse(w, req.requestID, 0x0406, printerAttributes(s.printerURI))
		return
	}

	text, err := documentText(payload)
	if err != nil {
		s.mu.Lock()
		s.jobs[jobIndex].Status = "error"
		s.jobs[jobIndex].Error = err.Error()
		job := s.jobs[jobIndex]
		s.mu.Unlock()
		writeIPPResponse(w, req.requestID, 0x0400, append(printerAttributes(s.printerURI), jobAttributes(s.printerURI, job)...))
		return
	}

	messages := encodeBrailleMessages(text)
	logPrintPlan(fmt.Sprintf("job %d", jobID), payload, text, messages)
	err = s.transport.SendMessages(messages)

	s.mu.Lock()
	if err != nil {
		s.jobs[jobIndex].Status = "error"
		s.jobs[jobIndex].Error = err.Error()
	} else {
		s.jobs[jobIndex].Status = "completed"
	}
	job := s.jobs[jobIndex]
	s.mu.Unlock()

	if err != nil {
		writeIPPResponse(w, req.requestID, 0x0500, append(printerAttributes(s.printerURI), jobAttributes(s.printerURI, job)...))
		return
	}
	writeIPPResponse(w, req.requestID, 0x0000, append(printerAttributes(s.printerURI), jobAttributes(s.printerURI, job)...))
}

func (s *printServer) getJobAttributes(w http.ResponseWriter, req ippRequest) {
	jobID := req.attrInt("job-id")

	s.mu.Lock()
	defer s.mu.Unlock()
	for _, job := range s.jobs {
		if job.ID == jobID {
			writeIPPResponse(w, req.requestID, 0x0000, append(printerAttributes(s.printerURI), jobAttributes(s.printerURI, job)...))
			return
		}
	}

	writeIPPResponse(w, req.requestID, 0x0406, printerAttributes(s.printerURI))
}

func (s *printServer) nextIDLocked() int {
	s.nextJobID++
	return s.nextJobID
}

type transport interface {
	Name() string
	SendMessages([][]byte) error
}

func newTransport(device, out string) (transport, error) {
	if out != "" {
		return fileTransport{path: out}, nil
	}
	if device != "" {
		return serialTransport{device: device}, nil
	}
	return autoSerialTransport{}, nil
}

type logTransport struct{}

func (logTransport) Name() string { return "log" }

func (logTransport) SendMessages(messages [][]byte) error {
	log.Printf("debug print job: %d message(s), %d firmware bytes", len(messages), encodedMessagesLen(messages))
	return nil
}

type fileTransport struct {
	path string
}

func (t fileTransport) Name() string { return t.path }

func (t fileTransport) SendMessages(messages [][]byte) error {
	f, err := os.OpenFile(t.path, os.O_WRONLY|os.O_CREATE|os.O_APPEND, 0o666)
	if err != nil {
		return err
	}
	defer f.Close()
	_, err = f.Write(bytes.Join(messages, nil))
	return err
}

type serialTransport struct {
	device string
}

func (t serialTransport) Name() string { return t.device }

func (t serialTransport) SendMessages(messages [][]byte) error {
	f, err := os.OpenFile(t.device, os.O_RDWR, 0)
	if err != nil {
		return err
	}
	defer f.Close()

	log.Printf("serial: opened %s, waiting %s before writing %d message(s), %d bytes", t.device, serialWarmup, len(messages), encodedMessagesLen(messages))
	time.Sleep(serialWarmup)

	if err := configureSerial(t.device); err != nil {
		log.Printf("serial config warning for %s: %v", t.device, err)
	}

	for i, message := range messages {
		if err := writeSerialMessage(f, message); err != nil {
			return err
		}
		if err := waitSerialAck(f, serialAckWait); err != nil {
			return fmt.Errorf("message %d/%d sent but firmware did not confirm END: %w", i+1, len(messages), err)
		}
		log.Printf("serial: message %d/%d confirmed (%d bytes)", i+1, len(messages), len(message))
	}

	log.Printf("serial: wrote %d message(s), %d bytes to %s", len(messages), encodedMessagesLen(messages), t.device)
	return nil
}

type autoSerialTransport struct{}

func (autoSerialTransport) Name() string { return "auto-serial" }

func (autoSerialTransport) SendMessages(messages [][]byte) error {
	device, err := findSerialDevice()
	if err != nil {
		return err
	}
	return serialTransport{device: device}.SendMessages(messages)
}

func findSerialDevice() (string, error) {
	patterns := []string{
		"/dev/cu.usbmodem*",
		"/dev/cu.usbserial*",
		"/dev/ttyACM*",
		"/dev/ttyUSB*",
	}

	for _, pattern := range patterns {
		matches, err := filepath.Glob(pattern)
		if err != nil {
			continue
		}
		if len(matches) > 0 {
			return matches[0], nil
		}
	}

	return "", errors.New("nenhuma porta serial Arduino encontrada")
}

func configureSerial(device string) error {
	switch runtime.GOOS {
	case "darwin":
		return exec.Command("stty", "-f", device, "115200", "cs8", "-cstopb", "-parenb", "raw", "-echo").Run()
	case "linux":
		return exec.Command("stty", "-F", device, "115200", "cs8", "-cstopb", "-parenb", "raw", "-echo").Run()
	default:
		return nil
	}
}

func writeSerialMessage(f *os.File, message []byte) error {
	if err := syscall.SetNonblock(int(f.Fd()), false); err != nil {
		log.Printf("serial blocking-mode warning: %v", err)
	}

	for offset := 0; offset < len(message); offset += serialChunk {
		end := offset + serialChunk
		if end > len(message) {
			end = len(message)
		}
		if _, err := f.Write(message[offset:end]); err != nil {
			return err
		}
		time.Sleep(serialChunkGap)
	}
	return nil
}

func waitSerialAck(f *os.File, timeout time.Duration) error {
	if err := syscall.SetNonblock(int(f.Fd()), true); err != nil {
		return err
	}

	deadline := time.Now().Add(timeout)
	buf := make([]byte, 256)
	var response []byte

	for time.Now().Before(deadline) {
		n, err := f.Read(buf)
		if n > 0 {
			response = append(response, buf[:n]...)
			if bytes.Contains(response, []byte("END")) {
				return nil
			}
		}
		if err != nil && !errors.Is(err, syscall.EAGAIN) && !errors.Is(err, syscall.EWOULDBLOCK) {
			return err
		}
		time.Sleep(serialAckGap)
	}

	if len(response) > 0 {
		return fmt.Errorf("timeout waiting END after receiving %q", string(response))
	}
	return errors.New("timeout waiting END")
}

func encodedMessagesLen(messages [][]byte) int {
	total := 0
	for _, message := range messages {
		total += len(message)
	}
	return total
}

func logPrintPlan(label string, payload []byte, text string, messages [][]byte) {
	maxMessage := 0
	for _, message := range messages {
		if len(message) > maxMessage {
			maxMessage = len(message)
		}
	}
	log.Printf("%s: input=%d bytes, text=%d chars, messages=%d, firmware=%d bytes, max_message=%d bytes, preview=%q",
		label,
		len(payload),
		len([]rune(text)),
		len(messages),
		encodedMessagesLen(messages),
		maxMessage,
		previewText(text, 180),
	)
}

func previewText(text string, limit int) string {
	text = strings.Join(strings.Fields(text), " ")
	runes := []rune(text)
	if len(runes) <= limit {
		return text
	}
	return string(runes[:limit]) + "..."
}

type ippRequest struct {
	versionMajor byte
	versionMinor byte
	operationID  uint16
	requestID    uint32
	attrs        map[string][]byte
	document     []byte
}

func (r ippRequest) attrString(name string) string {
	return string(r.attrs[name])
}

func (r ippRequest) attrInt(name string) int {
	value := r.attrs[name]
	if len(value) == 4 {
		return int(binary.BigEndian.Uint32(value))
	}
	return 0
}

func parseIPPRequest(data []byte) (ippRequest, error) {
	if len(data) < 8 {
		return ippRequest{}, errors.New("invalid IPP request")
	}

	req := ippRequest{
		versionMajor: data[0],
		versionMinor: data[1],
		operationID:  binary.BigEndian.Uint16(data[2:4]),
		requestID:    binary.BigEndian.Uint32(data[4:8]),
		attrs:        map[string][]byte{},
	}

	offset := 8
	var currentName string
	for offset < len(data) {
		tag := data[offset]
		offset++
		if tag == 0x03 {
			req.document = data[offset:]
			return req, nil
		}
		if tag <= 0x0F {
			continue
		}
		if offset+2 > len(data) {
			return ippRequest{}, errors.New("invalid IPP attribute name length")
		}
		nameLen := int(binary.BigEndian.Uint16(data[offset : offset+2]))
		offset += 2
		if offset+nameLen > len(data) {
			return ippRequest{}, errors.New("invalid IPP attribute name")
		}
		if nameLen > 0 {
			currentName = string(data[offset : offset+nameLen])
		}
		offset += nameLen
		if offset+2 > len(data) {
			return ippRequest{}, errors.New("invalid IPP attribute value length")
		}
		valueLen := int(binary.BigEndian.Uint16(data[offset : offset+2]))
		offset += 2
		if offset+valueLen > len(data) {
			return ippRequest{}, errors.New("invalid IPP attribute value")
		}
		if currentName != "" && valueLen > 0 {
			req.attrs[currentName] = append([]byte(nil), data[offset:offset+valueLen]...)
		}
		offset += valueLen
	}

	return req, nil
}

type ippAttribute struct {
	group byte
	tag   byte
	name  string
	value []byte
}

func printerAttributes(uri string) []ippAttribute {
	return []ippAttribute{
		{0x01, 0x47, "attributes-charset", []byte("utf-8")},
		{0x01, 0x48, "attributes-natural-language", []byte("pt-br")},
		{0x04, 0x45, "printer-uri-supported", []byte(uri)},
		{0x04, 0x42, "uri-authentication-supported", []byte("none")},
		{0x04, 0x44, "uri-security-supported", []byte("none")},
		{0x04, 0x41, "printer-name", []byte("Impressora Braille")},
		{0x04, 0x41, "printer-info", []byte("Braille Printer local")},
		{0x04, 0x23, "printer-state", ippInt(3)},
		{0x04, 0x44, "document-format-supported", []byte("text/plain")},
		{0x04, 0x44, "document-format-supported", []byte("application/pdf")},
		{0x04, 0x44, "document-format-default", []byte("text/plain")},
		{0x04, 0x44, "ipp-versions-supported", []byte("1.1")},
		{0x04, 0x21, "operations-supported", ippInt(0x0002)},
		{0x04, 0x21, "operations-supported", ippInt(0x0004)},
		{0x04, 0x21, "operations-supported", ippInt(0x0005)},
		{0x04, 0x21, "operations-supported", ippInt(0x0006)},
		{0x04, 0x21, "operations-supported", ippInt(0x0009)},
		{0x04, 0x21, "operations-supported", ippInt(0x000B)},
	}
}

func jobAttributes(printerURI string, job printJob) []ippAttribute {
	jobURI := fmt.Sprintf("%s/jobs/%d", printerURI, job.ID)
	state := 3
	switch job.Status {
	case "pending":
		state = 3
	case "printing":
		state = 5
	case "completed":
		state = 9
	case "error":
		state = 7
	}

	return []ippAttribute{
		{0x02, 0x21, "job-id", ippInt(job.ID)},
		{0x02, 0x45, "job-uri", []byte(jobURI)},
		{0x02, 0x42, "job-state", ippInt(state)},
		{0x02, 0x41, "job-name", []byte(job.Name)},
	}
}

func writeIPPResponse(w http.ResponseWriter, requestID uint32, status uint16, attrs []ippAttribute) {
	var buf bytes.Buffer
	buf.Write([]byte{0x01, 0x01})
	_ = binary.Write(&buf, binary.BigEndian, status)
	_ = binary.Write(&buf, binary.BigEndian, requestID)

	currentGroup := byte(0)
	for _, attr := range attrs {
		if attr.group != currentGroup {
			buf.WriteByte(attr.group)
			currentGroup = attr.group
		}
		buf.WriteByte(attr.tag)
		writeIPPString(&buf, attr.name)
		writeIPPBytes(&buf, attr.value)
	}
	buf.WriteByte(0x03)

	w.Header().Set("Content-Type", "application/ipp")
	w.WriteHeader(http.StatusOK)
	_, _ = w.Write(buf.Bytes())
}

func writeIPPString(buf *bytes.Buffer, value string) {
	writeIPPBytes(buf, []byte(value))
}

func writeIPPBytes(buf *bytes.Buffer, value []byte) {
	_ = binary.Write(buf, binary.BigEndian, uint16(len(value)))
	buf.Write(value)
}

func ippInt(value int) []byte {
	data := make([]byte, 4)
	binary.BigEndian.PutUint32(data, uint32(value))
	return data
}

func printerURIForAddr(addr string) string {
	host := addr
	if strings.HasPrefix(host, "127.0.0.1:") || strings.HasPrefix(host, "0.0.0.0:") || strings.HasPrefix(host, ":") {
		_, port, ok := strings.Cut(host, ":")
		if ok && port != "" {
			host = "127.0.0.1:" + port
		} else {
			host = "127.0.0.1:8631"
		}
	}
	return "ipp://" + host + "/printers/braille"
}

func documentText(document []byte) (string, error) {
	document = bytes.TrimSpace(document)
	if bytes.HasPrefix(document, []byte("%PDF-")) {
		text := extractPDFText(document)
		if strings.TrimSpace(text) == "" {
			return "", errors.New("nao foi possivel extrair texto do PDF recebido")
		}
		return text, nil
	}
	return string(document), nil
}

func extractPDFText(document []byte) string {
	var chunks []string

	for _, stream := range pdfStreams(document) {
		chunks = append(chunks, extractPDFTextOperators(stream)...)
		if inflated, err := inflate(stream); err == nil {
			chunks = append(chunks, extractPDFTextOperators(inflated)...)
		}
	}

	if text := strings.TrimSpace(joinPDFChunks(chunks)); text != "" {
		return text
	}

	chunks = append(chunks, extractPDFStrings(document)...)
	for _, stream := range pdfStreams(document) {
		chunks = append(chunks, extractPDFStrings(stream)...)
		if inflated, err := inflate(stream); err == nil {
			chunks = append(chunks, extractPDFStrings(inflated)...)
		}
	}
	return strings.TrimSpace(strings.Join(chunks, "\n"))
}

func joinPDFChunks(chunks []string) string {
	var out strings.Builder
	for _, chunk := range chunks {
		if chunk == "\n" {
			if out.Len() > 0 && !strings.HasSuffix(out.String(), "\n") {
				out.WriteByte('\n')
			}
			continue
		}
		if out.Len() > 0 && !strings.HasSuffix(out.String(), "\n") {
			out.WriteByte(' ')
		}
		out.WriteString(chunk)
	}
	return out.String()
}

func pdfStreams(document []byte) [][]byte {
	var streams [][]byte
	searchFrom := 0
	for {
		start := bytes.Index(document[searchFrom:], []byte("stream"))
		if start == -1 {
			break
		}
		start += searchFrom + len("stream")
		if start < len(document) && document[start] == '\r' {
			start++
		}
		if start < len(document) && document[start] == '\n' {
			start++
		}

		end := bytes.Index(document[start:], []byte("endstream"))
		if end == -1 {
			break
		}
		end += start
		stream := bytes.Trim(document[start:end], "\r\n")
		if len(stream) > 0 {
			streams = append(streams, stream)
		}
		searchFrom = end + len("endstream")
	}
	return streams
}

func inflate(data []byte) ([]byte, error) {
	reader, err := zlib.NewReader(bytes.NewReader(data))
	if err != nil {
		return nil, err
	}
	defer reader.Close()
	return io.ReadAll(reader)
}

func extractPDFStrings(data []byte) []string {
	var stringsFound []string
	for i := 0; i < len(data); i++ {
		if data[i] != '(' {
			continue
		}

		text, next, ok := readPDFLiteralString(data, i+1)
		if ok && strings.TrimSpace(text) != "" {
			stringsFound = append(stringsFound, text)
		}
		i = next
	}
	return stringsFound
}

func extractPDFTextOperators(data []byte) []string {
	var chunks []string
	searchFrom := 0

	for {
		start := bytes.Index(data[searchFrom:], []byte("BT"))
		if start == -1 {
			break
		}
		start += searchFrom + len("BT")
		end := bytes.Index(data[start:], []byte("ET"))
		if end == -1 {
			break
		}
		end += start
		chunks = append(chunks, extractPDFTextBlock(data[start:end])...)
		searchFrom = end + len("ET")
	}

	return chunks
}

func extractPDFTextBlock(data []byte) []string {
	var chunks []string
	var pending []string

	for i := 0; i < len(data); {
		c := data[i]
		if isPDFSpace(c) {
			i++
			continue
		}

		switch c {
		case '(':
			text, next, ok := readPDFLiteralString(data, i+1)
			if ok {
				pending = append(pending, text)
				i = next + 1
				continue
			}
		case '<':
			if i+1 < len(data) && data[i+1] != '<' {
				text, next, ok := readPDFHexString(data, i+1)
				if ok {
					pending = append(pending, text)
					i = next + 1
					continue
				}
			}
		case '[':
			texts, next, ok := readPDFTextArray(data, i+1)
			if ok {
				pending = append(pending, texts...)
				i = next + 1
				continue
			}
		default:
			operator, next := readPDFOperator(data, i)
			if operator != "" {
				switch operator {
				case "Tj", "TJ", "'", "\"":
					text := strings.Join(pending, "")
					if strings.TrimSpace(text) != "" {
						chunks = append(chunks, text)
					}
					pending = nil
				case "T*", "Td", "TD":
					if len(chunks) > 0 && chunks[len(chunks)-1] != "\n" {
						chunks = append(chunks, "\n")
					}
					pending = nil
				}
				i = next
				continue
			}
		}

		i++
	}

	return compactPDFChunks(chunks)
}

func readPDFTextArray(data []byte, start int) ([]string, int, bool) {
	var texts []string
	for i := start; i < len(data); {
		c := data[i]
		if c == ']' {
			return texts, i, true
		}
		if isPDFSpace(c) || c == '-' || c == '+' || c == '.' || (c >= '0' && c <= '9') {
			i++
			continue
		}
		if c == '(' {
			text, next, ok := readPDFLiteralString(data, i+1)
			if !ok {
				return texts, next, false
			}
			texts = append(texts, text)
			i = next + 1
			continue
		}
		if c == '<' && i+1 < len(data) && data[i+1] != '<' {
			text, next, ok := readPDFHexString(data, i+1)
			if !ok {
				return texts, next, false
			}
			texts = append(texts, text)
			i = next + 1
			continue
		}
		i++
	}
	return texts, len(data), false
}

func readPDFHexString(data []byte, start int) (string, int, bool) {
	var hexBytes []byte
	for i := start; i < len(data); i++ {
		if data[i] == '>' {
			return decodePDFHex(hexBytes), i, true
		}
		if isPDFSpace(data[i]) {
			continue
		}
		if isHex(data[i]) {
			hexBytes = append(hexBytes, data[i])
		}
	}
	return "", len(data), false
}

func decodePDFHex(hexBytes []byte) string {
	if len(hexBytes)%2 == 1 {
		hexBytes = append(hexBytes, '0')
	}

	raw := make([]byte, 0, len(hexBytes)/2)
	for i := 0; i+1 < len(hexBytes); i += 2 {
		raw = append(raw, fromHex(hexBytes[i])<<4|fromHex(hexBytes[i+1]))
	}
	if len(raw) >= 2 && raw[0] == 0xFE && raw[1] == 0xFF {
		return utf16BEString(raw[2:])
	}
	if looksLikeUTF16BE(raw) {
		return utf16BEString(raw)
	}
	return string(raw)
}

func utf16BEString(raw []byte) string {
	values := make([]uint16, 0, len(raw)/2)
	for i := 0; i+1 < len(raw); i += 2 {
		values = append(values, binary.BigEndian.Uint16(raw[i:i+2]))
	}
	return string(utf16.Decode(values))
}

func looksLikeUTF16BE(raw []byte) bool {
	if len(raw) < 4 || len(raw)%2 != 0 {
		return false
	}
	zeroHigh := 0
	for i := 0; i+1 < len(raw); i += 2 {
		if raw[i] == 0x00 && raw[i+1] >= 0x20 {
			zeroHigh++
		}
	}
	return zeroHigh >= len(raw)/4
}

func readPDFOperator(data []byte, start int) (string, int) {
	end := start
	for end < len(data) && !isPDFSpace(data[end]) && !strings.ContainsRune("[]<>()/", rune(data[end])) {
		end++
	}
	if end == start {
		return "", start + 1
	}
	token := string(data[start:end])
	switch token {
	case "Tj", "TJ", "'", "\"", "T*", "Td", "TD":
		return token, end
	default:
		return "", end
	}
}

func compactPDFChunks(chunks []string) []string {
	var out []string
	for _, chunk := range chunks {
		if chunk == "\n" {
			if len(out) > 0 && out[len(out)-1] != "\n" {
				out = append(out, chunk)
			}
			continue
		}
		if strings.TrimSpace(chunk) != "" {
			out = append(out, chunk)
		}
	}
	return out
}

func isPDFSpace(c byte) bool {
	return c == 0x00 || c == '\t' || c == '\n' || c == '\f' || c == '\r' || c == ' '
}

func isHex(c byte) bool {
	return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')
}

func fromHex(c byte) byte {
	switch {
	case c >= '0' && c <= '9':
		return c - '0'
	case c >= 'a' && c <= 'f':
		return c - 'a' + 10
	case c >= 'A' && c <= 'F':
		return c - 'A' + 10
	default:
		return 0
	}
}

func readPDFLiteralString(data []byte, start int) (string, int, bool) {
	var out strings.Builder
	depth := 1

	for i := start; i < len(data); i++ {
		c := data[i]
		if c == '\\' {
			if i+1 >= len(data) {
				return out.String(), i, false
			}
			i++
			switch data[i] {
			case 'n':
				out.WriteByte('\n')
			case 'r':
				out.WriteByte('\r')
			case 't':
				out.WriteByte('\t')
			case 'b':
				out.WriteByte('\b')
			case 'f':
				out.WriteByte('\f')
			case '(', ')', '\\':
				out.WriteByte(data[i])
			default:
				out.WriteByte(data[i])
			}
			continue
		}
		if c == '(' {
			depth++
			out.WriteByte(c)
			continue
		}
		if c == ')' {
			depth--
			if depth == 0 {
				return out.String(), i, true
			}
			out.WriteByte(c)
			continue
		}
		if c >= 0x20 || c == '\n' || c == '\r' || c == '\t' {
			out.WriteByte(c)
		}
	}

	return out.String(), len(data), false
}

func encodeBrailleDocument(text string) []byte {
	return bytes.Join(encodeBrailleMessages(text), nil)
}

func encodeBrailleMessages(text string) [][]byte {
	var messages [][]byte
	current := []byte{0xF0}
	lineCells := 0

	text = strings.ReplaceAll(text, "\r\n", "\n")
	text = strings.ReplaceAll(text, "\r", "\n")

	flush := func() {
		if len(current) == 1 {
			return
		}
		current = append(current, 0xFF)
		messages = append(messages, current)
		current = []byte{0xF0}
		lineCells = 0
	}

	appendLineBreak := func() {
		if len(current)+1 > maxMessageBody {
			flush()
		}
		current = append(current, 0xC0)
		lineCells = 0
	}

	for _, r := range text {
		if r == '\n' {
			appendLineBreak()
			continue
		}

		if bytes, ok := legacyBrailleBytes[r]; ok {
			cellCount := len(bytes)
			if lineCells > 0 && lineCells+cellCount > maxLineCells {
				appendLineBreak()
			}
			if len(current)+len(bytes) > maxMessageBody {
				flush()
			}
			current = append(current, bytes...)
			lineCells += cellCount
		}
	}

	flush()
	if len(messages) == 0 {
		return [][]byte{{0xF0, 0xFF}}
	}
	return messages
}

var legacyBrailleBytes = map[rune][]byte{
	' ': {0x00},

	'a': {0x20}, 'á': {0x3B}, 'à': {0x35}, 'â': {0x21}, 'ã': {0x0E},
	'b': {0x30}, 'c': {0x24}, 'ç': {0x3D}, 'd': {0x26}, 'e': {0x22},
	'é': {0x3F}, 'ê': {0x31}, 'f': {0x34}, 'g': {0x36}, 'h': {0x32},
	'i': {0x14}, 'í': {0x0C}, 'j': {0x16}, 'k': {0x28}, 'l': {0x38},
	'm': {0x2C}, 'n': {0x2E}, 'o': {0x2A}, 'ó': {0x0D}, 'ô': {0x27},
	'õ': {0x15}, 'p': {0x3C}, 'q': {0x3E}, 'r': {0x3A}, 's': {0x1C},
	't': {0x1E}, 'u': {0x29}, 'ú': {0x1F}, 'ü': {0x33}, 'v': {0x39},
	'x': {0x2D}, 'y': {0x2F}, 'w': {0x17}, 'z': {0x2B},

	'A': {0x45, 0x60}, 'Á': {0x45, 0x7B}, 'À': {0x45, 0x75}, 'Â': {0x45, 0x61}, 'Ã': {0x45, 0x4E},
	'B': {0x45, 0x70}, 'C': {0x45, 0x64}, 'Ç': {0x45, 0x7D}, 'D': {0x45, 0x66}, 'E': {0x45, 0x62},
	'É': {0x45, 0x7F}, 'Ê': {0x45, 0x71}, 'F': {0x45, 0x74}, 'G': {0x45, 0x76}, 'H': {0x45, 0x72},
	'I': {0x45, 0x54}, 'Í': {0x45, 0x4C}, 'J': {0x45, 0x56}, 'K': {0x45, 0x68}, 'L': {0x45, 0x78},
	'M': {0x45, 0x6C}, 'N': {0x45, 0x6E}, 'O': {0x45, 0x6A}, 'Ó': {0x45, 0x4D}, 'Ô': {0x45, 0x67},
	'Õ': {0x45, 0x55}, 'P': {0x45, 0x7C}, 'Q': {0x45, 0x7E}, 'R': {0x45, 0x7A}, 'S': {0x45, 0x5C},
	'T': {0x45, 0x5E}, 'U': {0x45, 0x69}, 'Ú': {0x45, 0x1F}, 'Ü': {0x45, 0x33}, 'V': {0x45, 0x79},
	'X': {0x45, 0x6D}, 'Y': {0x45, 0x6F}, 'W': {0x45, 0x57}, 'Z': {0x45, 0x6B},

	'0': {0x8F, 0x96}, '1': {0x8F, 0xA0}, '2': {0x8F, 0xB0}, '3': {0x8F, 0xA4}, '4': {0x8F, 0xA6},
	'5': {0x8F, 0xA2}, '6': {0x8F, 0xB4}, '7': {0x8F, 0xB6}, '8': {0x8F, 0xB2}, '9': {0x8F, 0x94},

	'\n': {0xC0}, ',': {0xD0}, ';': {0xD8}, ':': {0xD2}, '?': {0xD1}, '!': {0xDA},
	'.': {0xC8}, '-': {0xC9}, '_': {0xC9, 0xC9}, '(': {0xF1}, ')': {0xCE},
	'[': {0xFB}, ']': {0xDF}, '"': {0xD9}, '*': {0xCA}, '&': {0xFD},
	'/': {0xC1, 0xD0}, '|': {0xC7}, '$': {0xC3}, '%': {0xC7, 0xCB},
	'§': {0xDC, 0xDC}, '+': {0xDA}, '>': {0xEA}, '<': {0xD5}, 'º': {0xCB},
}

func htmlEscape(value string) string {
	value = strings.ReplaceAll(value, "&", "&amp;")
	value = strings.ReplaceAll(value, "<", "&lt;")
	value = strings.ReplaceAll(value, ">", "&gt;")
	value = strings.ReplaceAll(value, "\"", "&quot;")
	return value
}
