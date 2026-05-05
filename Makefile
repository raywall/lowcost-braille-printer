DRIVER_DIR := driver
DRIVER_BIN := $(DRIVER_DIR)/bin/braille-print-server
ADDR ?= 127.0.0.1:8631
DEVICE ?=
OUT ?=
PRINTER_NAME ?= Impressora_Braille
PRINTER_URI ?= ipp://localhost:8631/printers/braille
PREFIX ?= /usr/local

.PHONY: help build run run-device run-file install install-bin install-printer uninstall-printer clean test fmt

help:
	@echo "Braille Printer"
	@echo ""
	@echo "Targets:"
	@echo "  make build             Compila o Braille Print Server"
	@echo "  make run               Executa em modo debug/log"
	@echo "  make run-device DEVICE=/dev/cu.usbmodem1101"
	@echo "  make run-file OUT=/tmp/braille-printer.bin"
	@echo "  make install           Compila, instala o binario e registra a fila IPP"
	@echo "  make install-bin       Instala o binario em $(PREFIX)/bin"
	@echo "  make install-printer   Registra a fila IPP no CUPS"
	@echo "  make uninstall-printer Remove a fila IPP"
	@echo "  make test              Valida o modulo Go"
	@echo "  make clean             Remove artefatos de build"

build:
	cd $(DRIVER_DIR) && go build -o bin/braille-print-server .

run:
	cd $(DRIVER_DIR) && go run . -addr $(ADDR)

run-device:
	@test -n "$(DEVICE)" || (echo "Informe DEVICE=/dev/cu.usbmodem1101 ou DEVICE=COM3" && exit 1)
	cd $(DRIVER_DIR) && go run . -addr $(ADDR) -device $(DEVICE)

run-file:
	@test -n "$(OUT)" || (echo "Informe OUT=/tmp/braille-printer.bin" && exit 1)
	cd $(DRIVER_DIR) && go run . -addr $(ADDR) -out $(OUT)

install: build install-bin install-printer
	@echo ""
	@echo "Instalacao concluida."
	@echo "Execute o servidor com: $(PREFIX)/bin/braille-print-server -addr $(ADDR)"

install-bin: build
	install -d $(PREFIX)/bin
	install $(DRIVER_BIN) $(PREFIX)/bin/braille-print-server

install-printer:
	@command -v lpadmin >/dev/null || (echo "lpadmin nao encontrado. Este alvo requer CUPS/macOS/Linux." && exit 1)
	lpadmin -p $(PRINTER_NAME) -E -v $(PRINTER_URI) -m everywhere
	@echo "Fila instalada: $(PRINTER_NAME) -> $(PRINTER_URI)"

uninstall-printer:
	@command -v lpadmin >/dev/null || (echo "lpadmin nao encontrado. Este alvo requer CUPS/macOS/Linux." && exit 1)
	lpadmin -x $(PRINTER_NAME)

test:
	cd $(DRIVER_DIR) && go test ./...

fmt:
	cd $(DRIVER_DIR) && gofmt -w .

clean:
	rm -rf $(DRIVER_DIR)/bin
