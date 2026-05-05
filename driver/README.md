# Braille Print Server

Servidor local para expor a impressora Braille como uma fila IPP e encaminhar os trabalhos para o firmware Arduino.

## Executar em modo debug

```bash
go run . -addr 127.0.0.1:8631
```

Abra:

```text
http://127.0.0.1:8631/
```

URI da impressora:

```text
ipp://localhost:8631/printers/braille
```

## Saida para arquivo

Use esta opção enquanto a serial real ainda não está conectada:

```bash
go run . -out /tmp/braille-printer.bin
```

## Saida para dispositivo

```bash
go run . -device /dev/cu.usbmodem1101
```

No Windows, use algo como:

```powershell
go run . -device COM3
```

## Estado atual

Este MVP implementa operações IPP básicas para validação, atributos da impressora e envio de trabalhos em texto puro. O próximo passo é melhorar a compatibilidade com spoolers reais, detectar automaticamente a porta serial e trocar o envio simples por um protocolo com confirmação e checksum.
