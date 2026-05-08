package main

import (
	"bytes"
	"compress/zlib"
	"strings"
	"testing"
)

func TestEncodeBrailleDocumentMatchesLegacySoftwareProtocol(t *testing.T) {
	got := encodeBrailleDocument("Abc 1\ná_")
	want := []byte{
		0xF0,
		0x45, 0x60, // A
		0x30,       // b
		0x24,       // c
		0x00,       // space
		0x8F, 0xA0, // 1
		0xC0,       // newline
		0x3B,       // á
		0xC9, 0xC9, // _
		0xFF, // final marker appended by BraillePrinterMessage
	}

	if !bytes.Equal(got, want) {
		t.Fatalf("encodeBrailleDocument = % X, want % X", got, want)
	}
}

func TestEncodeBrailleDocumentWrapsLinesAndSplitsMessages(t *testing.T) {
	got := encodeBrailleMessages(strings.Repeat("a", 200))

	if len(got) < 2 {
		t.Fatalf("expected multiple firmware messages, got %d: % X", len(got), bytes.Join(got, nil))
	}

	for _, message := range got {
		if message[0] != 0xF0 || message[len(message)-1] != 0xFF {
			t.Fatalf("message must be framed with F0/FF: % X", message)
		}
		if len(message) > maxMessageBody+2 {
			t.Fatalf("message too large: got %d bytes, max body %d", len(message), maxMessageBody)
		}
	}

	joined := bytes.Join(got, nil)
	if !bytes.Contains(joined, []byte{0xC0}) {
		t.Fatalf("expected wrapped line marker C0 in % X", joined)
	}
}

func TestDocumentTextExtractsPDFLiteralStrings(t *testing.T) {
	var compressed bytes.Buffer
	writer := zlib.NewWriter(&compressed)
	_, _ = writer.Write([]byte("BT (Ola TextEdit) Tj ET"))
	_ = writer.Close()

	pdf := append([]byte("%PDF-1.4\n1 0 obj\n<< /Filter /FlateDecode >>\nstream\n"), compressed.Bytes()...)
	pdf = append(pdf, []byte("\nendstream\nendobj\n%%EOF")...)

	got, err := documentText(pdf)
	if err != nil {
		t.Fatal(err)
	}
	if got != "Ola TextEdit" {
		t.Fatalf("documentText(pdf) = %q, want %q", got, "Ola TextEdit")
	}
}

func TestDocumentTextExtractsPDFTextOperatorsOnly(t *testing.T) {
	var compressed bytes.Buffer
	writer := zlib.NewWriter(&compressed)
	_, _ = writer.Write([]byte("BT [(Ola) 25 <205465787445646974>] TJ T* <FEFF004100E700E3006F> Tj ET"))
	_ = writer.Close()

	pdf := append([]byte("%PDF-1.4\n/Title (Nao imprimir metadado)\n1 0 obj\n<< /Filter /FlateDecode >>\nstream\n"), compressed.Bytes()...)
	pdf = append(pdf, []byte("\nendstream\nendobj\n%%EOF")...)

	got, err := documentText(pdf)
	if err != nil {
		t.Fatal(err)
	}
	want := "Ola TextEdit\nAção"
	if got != want {
		t.Fatalf("documentText(pdf) = %q, want %q", got, want)
	}
}
