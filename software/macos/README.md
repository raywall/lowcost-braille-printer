# Braille Printer Mac

Projeto Xcode nativo para macOS inspirado no editor Windows original.

## Recursos

- Editor de texto simples.
- Abrir arquivos `.txt` e `.rtf`.
- Salvar como `.txt`.
- Traducao Braille usando a mesma tabela de `software/windows/Braille Translator/BrailleSystem.cs`.
- Envio serial direto para Arduino em 115200 8N1.
- Aguarda o reset do Arduino antes de enviar dados.
- Painel de comandos manuais equivalente ao `ControleForm` do Windows.

## Abrir no Xcode

```bash
open software/macos/BraillePrinterMac.xcodeproj
```

## Compilar pelo terminal

```bash
xcodebuild -project software/macos/BraillePrinterMac.xcodeproj -scheme BraillePrinterMac -configuration Debug build
```
