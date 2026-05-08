DRIVER_DIR := driver
DRIVER_BIN := $(DRIVER_DIR)/bin/braille-print-server
ADDR ?= 127.0.0.1:8631
DEVICE ?=
OUT ?=
PRINTER_NAME ?= Impressora_Braille
PRINTER_URI ?= ipp://127.0.0.1:8631/printers/braille
SERVER_URL ?= http://127.0.0.1:8631/
PREFIX ?= /usr/local
LAUNCHD_LABEL := com.brailleprinter.server
MACOS_APP_DIR := $(HOME)/Library/Application Support/BraillePrinter
MACOS_BIN := $(MACOS_APP_DIR)/braille-print-server
MACOS_LOG_DIR := $(HOME)/Library/Logs/BraillePrinter
MACOS_LAUNCH_AGENT_DIR := $(HOME)/Library/LaunchAgents
MACOS_PLIST := $(MACOS_LAUNCH_AGENT_DIR)/$(LAUNCHD_LABEL).plist
MACOS_PLIST_TEMPLATE := $(DRIVER_DIR)/packaging/macos/$(LAUNCHD_LABEL).plist
MACOS_PPD := $(DRIVER_DIR)/packaging/macos/braille-printer.ppd

.PHONY: help build run run-device run-file install install-macos install-macos-bin install-macos-launch-agent start-macos-service stop-macos-service uninstall uninstall-macos reinstall install-bin install-printer install-printer-everywhere uninstall-printer check-server clean test fmt

help:
	@echo "Braille Printer"
	@echo ""
	@echo "Targets:"
	@echo "  make build             Compila o Braille Print Server"
	@echo "  make run               Executa em modo debug/log"
	@echo "  make run-device DEVICE=/dev/cu.usbmodem1101"
	@echo "  make run-file OUT=/tmp/braille-printer.bin"
	@echo "  make install           Instala no macOS: app, servico automatico e fila IPP"
	@echo "  make uninstall         Remove servico automatico, fila IPP e app do macOS"
	@echo "  make start-macos-service"
	@echo "                           Inicia o servico automatico no macOS"
	@echo "  make stop-macos-service Para o servico automatico no macOS"
	@echo "  make install-bin       Instala o binario em $(PREFIX)/bin"
	@echo "  make install-printer   Registra a fila IPP no CUPS usando o PPD do projeto"
	@echo "  make install-printer-everywhere"
	@echo "                           Tenta registrar como IPP Everywhere"
	@echo "  make check-server      Verifica se o servidor IPP local esta respondendo"
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

install: install-macos

install-macos: build install-macos-bin install-macos-launch-agent start-macos-service install-printer
	@echo ""
	@echo "Instalacao concluida. A impressora $(PRINTER_NAME) ja deve estar disponivel no macOS."
	@echo "Painel local: $(SERVER_URL)"

install-macos-bin: build
	@test "$$(uname -s)" = "Darwin" || (echo "Este alvo e especifico do macOS." && exit 1)
	@test "$$(id -u)" != "0" || (echo "Nao use sudo neste alvo. Execute 'make install' com seu usuario normal." && exit 1)
	install -d "$(MACOS_APP_DIR)"
	install "$(DRIVER_BIN)" "$(MACOS_BIN)"
	install -d "$(MACOS_LOG_DIR)"

install-macos-launch-agent: install-macos-bin
	install -d "$(MACOS_LAUNCH_AGENT_DIR)"
	sed -e 's|{{BINARY}}|$(MACOS_BIN)|g' -e 's|{{ADDR}}|$(ADDR)|g' -e 's|{{LOG_DIR}}|$(MACOS_LOG_DIR)|g' "$(MACOS_PLIST_TEMPLATE)" > "$(MACOS_PLIST)"

start-macos-service: install-macos-launch-agent
	@launchctl bootout gui/$$(id -u) "$(MACOS_PLIST)" >/dev/null 2>&1 || true
	launchctl bootstrap gui/$$(id -u) "$(MACOS_PLIST)"
	launchctl kickstart -k gui/$$(id -u)/$(LAUNCHD_LABEL)

stop-macos-service:
	@launchctl bootout gui/$$(id -u) "$(MACOS_PLIST)" >/dev/null 2>&1 || true

uninstall: uninstall-macos

uninstall-macos: stop-macos-service uninstall-printer
	rm -f "$(MACOS_PLIST)"
	rm -f "$(MACOS_BIN)"
	-rmdir "$(MACOS_APP_DIR)" >/dev/null 2>&1
	@echo "Braille Print Server removido do macOS."

reinstall: uninstall install

install-bin: build
	install -d $(PREFIX)/bin
	install $(DRIVER_BIN) $(PREFIX)/bin/braille-print-server

check-server:
	@echo "Verificando servidor em $(SERVER_URL)"
	curl -fsS -o /dev/null $(SERVER_URL)

install-printer: build
	@command -v lpadmin >/dev/null || (echo "lpadmin nao encontrado. Este alvo requer CUPS/macOS/Linux." && exit 1)
	lpadmin -p $(PRINTER_NAME) -E -v $(PRINTER_URI) -P "$(CURDIR)/$(MACOS_PPD)" -D "Impressora Braille" -L "Local" -o printer-is-shared=false
	@echo "Fila instalada: $(PRINTER_NAME) -> $(PRINTER_URI)"

install-printer-everywhere: build
	@command -v lpadmin >/dev/null || (echo "lpadmin nao encontrado. Este alvo requer CUPS/macOS/Linux." && exit 1)
	@if curl -fsS -o /dev/null $(SERVER_URL) 2>&1; then \
		echo "Servidor IPP encontrado em $(SERVER_URL)."; \
		lpadmin -p $(PRINTER_NAME) -E -v $(PRINTER_URI) -m everywhere; \
	else \
		echo "Servidor IPP nao esta rodando; iniciando servidor temporario para registrar a fila..."; \
		$(DRIVER_BIN) -addr $(ADDR) >/tmp/braille-print-server-install.log 2>&1 & \
		pid=$$!; \
		sleep 1; \
		if ! curl -fsS -o /dev/null $(SERVER_URL) 2>&1; then \
			kill $$pid >/dev/null 2>&1 || true; \
			echo "Nao foi possivel iniciar o servidor temporario. Veja /tmp/braille-print-server-install.log"; \
			exit 1; \
		fi; \
		lpadmin -p $(PRINTER_NAME) -E -v $(PRINTER_URI) -m everywhere; \
		status=$$?; \
		kill $$pid >/dev/null 2>&1 || true; \
		exit $$status; \
	fi
	@echo "Fila instalada: $(PRINTER_NAME) -> $(PRINTER_URI)"
	@echo "Para imprimir, deixe o servidor rodando com: make run"

uninstall-printer:
	@command -v lpadmin >/dev/null || (echo "lpadmin nao encontrado. Este alvo requer CUPS/macOS/Linux." && exit 1)
	lpadmin -x $(PRINTER_NAME)

test:
	cd $(DRIVER_DIR) && go test ./...

fmt:
	cd $(DRIVER_DIR) && gofmt -w .

clean:
	rm -rf $(DRIVER_DIR)/bin
