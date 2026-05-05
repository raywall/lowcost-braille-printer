package main

import (
	"bytes"
	"encoding/binary"
	"errors"
	"flag"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"strings"
	"sync"
	"time"
	"unicode"
)

const (
	defaultAddr = "127.0.0.1:8631"
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
	case 0x000B: // Get-Printer-Attributes
		writeIPPResponse(w, req.requestID, 0x0000, printerAttributes(s.printerURI))
	default:
		writeIPPResponse(w, req.requestID, 0x0501, printerAttributes(s.printerURI))
	}
}

func (s *printServer) printJob(w http.ResponseWriter, req ippRequest) {
	name := req.attrs["job-name"]
	if name == "" {
		name = "Braille job"
	}

	payload := bytes.TrimSpace(req.document)
	if len(payload) == 0 {
		writeIPPResponse(w, req.requestID, 0x0400, printerAttributes(s.printerURI))
		return
	}

	firmwareBytes := encodeBrailleDocument(string(payload))

	s.mu.Lock()
	job := printJob{
		ID:        len(s.jobs) + 1,
		Name:      name,
		CreatedAt: time.Now(),
		Bytes:     len(payload),
		Status:    "printing",
	}
	s.jobs = append(s.jobs, job)
	jobIndex := len(s.jobs) - 1
	s.mu.Unlock()

	err := s.transport.Send(firmwareBytes)

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
	writeIPPResponse(w, req.requestID, 0x0000, printerAttributes(s.printerURI))
}

type transport interface {
	Name() string
	Send([]byte) error
}

func newTransport(device, out string) (transport, error) {
	if out != "" {
		return fileTransport{path: out}, nil
	}
	if device != "" {
		return fileTransport{path: device}, nil
	}
	return logTransport{}, nil
}

type logTransport struct{}

func (logTransport) Name() string { return "log" }

func (logTransport) Send(data []byte) error {
	log.Printf("debug print job: %d firmware bytes", len(data))
	return nil
}

type fileTransport struct {
	path string
}

func (t fileTransport) Name() string { return t.path }

func (t fileTransport) Send(data []byte) error {
	f, err := os.OpenFile(t.path, os.O_WRONLY|os.O_CREATE|os.O_APPEND, 0o666)
	if err != nil {
		return err
	}
	defer f.Close()
	_, err = f.Write(data)
	return err
}

type ippRequest struct {
	versionMajor byte
	versionMinor byte
	operationID  uint16
	requestID    uint32
	attrs        map[string]string
	document     []byte
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
		attrs:        map[string]string{},
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
			req.attrs[currentName] = string(data[offset : offset+valueLen])
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
		{0x04, 0x45, "printer-uri-supported", []byte(uri)},
		{0x04, 0x42, "uri-authentication-supported", []byte("none")},
		{0x04, 0x44, "uri-security-supported", []byte("none")},
		{0x04, 0x41, "printer-name", []byte("Impressora Braille")},
		{0x04, 0x41, "printer-info", []byte("Braille Printer local")},
		{0x04, 0x23, "printer-state", ippInt(3)},
		{0x04, 0x44, "document-format-supported", []byte("text/plain")},
		{0x04, 0x44, "document-format-default", []byte("text/plain")},
		{0x04, 0x44, "ipp-versions-supported", []byte("1.1")},
		{0x04, 0x21, "operations-supported", ippInt(0x0002)},
		{0x04, 0x21, "operations-supported", ippInt(0x0004)},
		{0x04, 0x21, "operations-supported", ippInt(0x000B)},
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
			host = "localhost:" + port
		} else {
			host = "localhost:8631"
		}
	}
	return "ipp://" + host + "/printers/braille"
}

func encodeBrailleDocument(text string) []byte {
	var out []byte
	out = append(out, 0xF0)

	column := 0
	for _, r := range text {
		if r == '\r' {
			continue
		}
		if r == '\n' || column >= 28 {
			out = append(out, 0xFF)
			column = 0
			if r == '\n' {
				continue
			}
		}

		if unicode.IsSpace(r) {
			out = append(out, 0x00)
			column++
			continue
		}

		for _, cell := range brailleCells(r) {
			out = append(out, cell)
			column++
			if column >= 28 {
				out = append(out, 0xFF)
				column = 0
			}
		}
	}

	if len(out) == 1 || out[len(out)-1] != 0xFF {
		out = append(out, 0xFF)
	}
	return out
}

func brailleCells(r rune) []byte {
	if unicode.IsDigit(r) {
		return []byte{0x3C, digitToBraille(r)}
	}
	if unicode.IsUpper(r) {
		return []byte{0x20, letterToBraille(unicode.ToLower(r))}
	}
	cell := letterToBraille(unicode.ToLower(r))
	if cell != 0 {
		return []byte{cell}
	}
	return []byte{0x00}
}

func digitToBraille(r rune) byte {
	switch r {
	case '1':
		return 0x01
	case '2':
		return 0x03
	case '3':
		return 0x09
	case '4':
		return 0x19
	case '5':
		return 0x11
	case '6':
		return 0x0B
	case '7':
		return 0x1B
	case '8':
		return 0x13
	case '9':
		return 0x0A
	case '0':
		return 0x1A
	default:
		return 0
	}
}

func letterToBraille(r rune) byte {
	switch r {
	case 'a', 'á', 'à', 'â', 'ã':
		return 0x01
	case 'b':
		return 0x03
	case 'c', 'ç':
		return 0x09
	case 'd':
		return 0x19
	case 'e', 'é', 'ê':
		return 0x11
	case 'f':
		return 0x0B
	case 'g':
		return 0x1B
	case 'h':
		return 0x13
	case 'i', 'í':
		return 0x0A
	case 'j':
		return 0x1A
	case 'k':
		return 0x05
	case 'l':
		return 0x07
	case 'm':
		return 0x0D
	case 'n':
		return 0x1D
	case 'o', 'ó', 'ô', 'õ':
		return 0x15
	case 'p':
		return 0x0F
	case 'q':
		return 0x1F
	case 'r':
		return 0x17
	case 's':
		return 0x0E
	case 't':
		return 0x1E
	case 'u', 'ú':
		return 0x25
	case 'v':
		return 0x27
	case 'w':
		return 0x3A
	case 'x':
		return 0x2D
	case 'y':
		return 0x3D
	case 'z':
		return 0x35
	default:
		return 0
	}
}

func htmlEscape(value string) string {
	value = strings.ReplaceAll(value, "&", "&amp;")
	value = strings.ReplaceAll(value, "<", "&lt;")
	value = strings.ReplaceAll(value, ">", "&gt;")
	value = strings.ReplaceAll(value, "\"", "&quot;")
	return value
}
