# Braille Print Server

Este documento descreve a proposta do driver local da impressora Braille. A solucao evita um driver kernel tradicional e usa um servidor IPP local para que o usuario possa imprimir a partir de editores comuns, como Word, LibreOffice, Bloco de Notas, TextEdit e leitores de PDF.

## Objetivo

Permitir que a impressora apareca no computador como uma impressora comum chamada `Impressora Braille`, mantendo a comunicacao fisica atual com o Arduino Mega pela porta USB serial.

Fluxo esperado:

```text
Editor de texto / PDF
        |
        v
Fila de impressao do sistema
        |
        v
Braille Print Server local via IPP
        |
        v
Conversor texto -> celas Braille
        |
        v
Porta USB serial
        |
        v
Firmware Arduino
```

## Componentes

- `firmware/`: codigo Arduino responsavel por mover os eixos, acionar o solenoide e interpretar os bytes de celas Braille.
- `driver/`: servidor local escrito em Go.
- `Makefile`: comandos padronizados para compilar, executar e registrar a fila IPP.

## Endpoints

- `GET /`: painel simples de status do servidor.
- `GET /printers/braille/status`: mesmo painel de status.
- `POST /printers/braille`: endpoint IPP da impressora.

URI padrao da impressora:

```text
ipp://127.0.0.1:8631/printers/braille
```

## Operacoes IPP do MVP

O MVP implementa as operacoes minimas para testes iniciais com clientes IPP:

- `Print-Job`
- `Validate-Job`
- `Get-Printer-Attributes`

Capacidades anunciadas:

- formato padrao: `text/plain`
- autenticacao: `none`
- seguranca: `none`
- estado da impressora: `idle`

## Conversao Braille

Nesta versao, o servidor converte texto puro usando a mesma tabela do projeto legado em `software/BraillePrinter/Braille Translator/BrailleSystem.cs`.

- `0xF0`: inicio de impressao.
- `0xFF`: final da mensagem.
- `0xC0`: quebra de linha.
- `0x00`: espaco.
- demais bytes: celas Braille, indicadores ou pontuacao conforme o dicionario legado.

Exemplos do contrato legado:

- `a` -> `0x20`
- `A` -> `0x45 0x60`
- `1` -> `0x8F 0xA0`
- `_` -> `0xC9 0xC9`

Ordem de bits usada pelo firmware:

```text
Ponto 1 -> b5
Ponto 2 -> b4
Ponto 3 -> b3
Ponto 4 -> b2
Ponto 5 -> b1
Ponto 6 -> b0
```

Exemplo: a letra `a`, formada pelo ponto 1, deve ser enviada como `0x20`, nao como `0x01`.

## Transporte

O servidor possui tres modos de saida:

- `auto-serial`: modo padrao; procura portas seriais comuns de Arduino na hora de imprimir.
- `-out arquivo`: grava os bytes convertidos em arquivo para depuracao.
- `-device porta`: escreve diretamente em uma porta/dispositivo, como `/dev/cu.usbmodem1101` ou `COM3`.

O transporte serial ainda esta isolado para que o protocolo possa evoluir sem mudar a camada IPP.

## Formatos De Entrada

A fila instalada no macOS aceita:

- `text/plain`;
- `application/pdf`.

Apps como TextEdit normalmente enviam PDF para o CUPS. Por isso, o servidor faz uma extracao de texto basica de PDFs antes de gerar as celas Braille. Essa extracao cobre PDFs simples com texto em strings literais, incluindo streams comprimidos com Flate. PDFs escaneados ou com texto convertido em desenho/imagem ainda exigem uma etapa futura de OCR ou extracao mais completa.

## Instalacao No macOS

A instalacao principal deve exigir apenas um comando:

```bash
make install
```

Esse comando:

- compila o Braille Print Server;
- instala o binario em `~/Library/Application Support/BraillePrinter`;
- instala um LaunchAgent em `~/Library/LaunchAgents/com.brailleprinter.server.plist`;
- inicia o servidor automaticamente no login;
- registra a fila `Impressora_Braille` no CUPS.

Depois disso, o usuario deve conseguir imprimir pelo menu normal de impressao dos aplicativos, sem rodar servidor manualmente.

Para remover:

```bash
make uninstall
```

Importante: no macOS, use `make install` com o usuario normal, sem `sudo`. O LaunchAgent e instalado na sessao do usuario.

## Instalacao Manual Da Fila

No macOS e Linux com CUPS, a fila IPP pode ser registrada com:

```bash
make install-printer
```

Nesta fase, o alvo padrao registra a fila usando o PPD minimo do projeto em `driver/packaging/macos/braille-printer.ppd`. Isso evita os dois caminhos que falham no macOS atual: filas `raw`, que nao sao mais aceitas, e `-m everywhere`, que exige uma implementacao IPP Everywhere completa.

Isso cria uma fila chamada `Impressora_Braille` apontando para:

```text
ipp://127.0.0.1:8631/printers/braille
```

Se a fila for instalada manualmente, o servidor ainda precisa estar em execucao para receber trabalhos de impressao. No macOS, prefira `make install`, que instala esse servidor como servico automatico.

Para desenvolvimento, ainda e possivel rodar manualmente:

```bash
make run
```

Para verificar se o servidor esta respondendo:

```bash
make check-server
```

Tambem existe um alvo experimental:

```bash
make install-printer-everywhere
```

Esse alvo tenta usar `lpadmin -m everywhere`. Ele so deve funcionar quando o servidor anunciar atributos IPP suficientes para o CUPS gerar uma fila driverless completa.

## Limites Atuais

- O instalador automatico atual cobre macOS via LaunchAgent; Windows ainda precisa de instalador/servico equivalente.
- A deteccao automatica do Arduino ainda nao foi implementada.
- O protocolo serial ainda nao tem tamanho, checksum, `READY`, `BUSY`, `DONE` e codigos de erro.
- A compatibilidade com spoolers reais ainda precisa ser ampliada com mais atributos IPP.
- O suporte inicial e para `text/plain`; PDF e DOCX devem entrar por uma etapa de extracao/conversao.

## Evolucao Recomendada

1. Melhorar o firmware com protocolo confiavel:
   - cabecalho;
   - tamanho do payload;
   - checksum/CRC;
   - ACK/NACK;
   - status da impressora;
   - timeouts para sensor e endstop.

2. Implementar serial real com controle de fluxo no servidor.
3. Detectar automaticamente a placa Arduino por VID/PID ou assinatura serial.
4. Completar a tabela Braille portugues.
5. Expandir IPP para maior compatibilidade com Windows, macOS e CUPS.
6. Criar instaladores:
   - macOS: app/helper com LaunchAgent ou LaunchDaemon;
   - Windows: servico local e criacao automatica da impressora IPP;
   - Linux: systemd user service ou system service.
