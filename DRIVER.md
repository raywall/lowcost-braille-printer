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
ipp://localhost:8631/printers/braille
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

Nesta primeira versao, o servidor converte texto puro para bytes compativeis com o protocolo atual do firmware:

- `0xF0`: inicio de impressao.
- `0xFF`: quebra/final de linha.
- `0x00`: espaco.
- demais bytes: celas Braille de 6 pontos.

Regras iniciais:

- linhas com ate 28 celas;
- letras minusculas mapeadas para Braille basico;
- letras maiusculas recebem indicador de maiuscula antes da cela;
- numeros recebem indicador numerico antes da cela;
- acentos comuns em portugues sao normalizados para a letra base nesta fase.

Esta normalizacao e proposital no MVP. A evolucao correta e implementar tabela Braille portugues completa, incluindo sinais de acentuacao, pontuacao, abreviacoes e regras de contexto.

## Transporte

O servidor possui tres modos de saida:

- `log`: modo padrao para desenvolvimento; nao envia para hardware.
- `-out arquivo`: grava os bytes convertidos em arquivo para depuracao.
- `-device porta`: escreve diretamente em uma porta/dispositivo, como `/dev/cu.usbmodem1101` ou `COM3`.

O transporte serial ainda esta isolado para que o protocolo possa evoluir sem mudar a camada IPP.

## Instalacao Da Fila

No macOS e Linux com CUPS, a fila IPP pode ser registrada com:

```bash
make install-printer
```

Isso cria uma fila chamada `Impressora_Braille` apontando para:

```text
ipp://localhost:8631/printers/braille
```

Importante: o servidor precisa estar em execucao para a fila conseguir consultar atributos e enviar trabalhos.

## Limites Atuais

- O servidor ainda nao e instalado como servico/background automaticamente.
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
