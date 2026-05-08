import SwiftUI

struct ContentView: View {
    @EnvironmentObject private var controller: PrinterController

    var body: some View {
        NavigationSplitView {
            sidebar
                .navigationSplitViewColumnWidth(min: 260, ideal: 300)
        } detail: {
            editor
        }
        .frame(minWidth: 980, minHeight: 660)
        .onAppear {
            controller.refreshPorts()
        }
    }

    private var sidebar: some View {
        VStack(alignment: .leading, spacing: 18) {
            GroupBox("Conexao") {
                VStack(alignment: .leading, spacing: 10) {
                    Picker("Porta", selection: $controller.selectedPort) {
                        Text("Auto").tag("")
                        ForEach(controller.availablePorts, id: \.self) { port in
                            Text(port).tag(port)
                        }
                    }

                    HStack {
                        Button("Atualizar") {
                            controller.refreshPorts()
                        }

                        Button("Teste") {
                            controller.sendTestPage()
                        }
                    }
                }
                .padding(4)
            }

            GroupBox("Controles") {
                Grid(alignment: .leading, horizontalSpacing: 8, verticalSpacing: 8) {
                    GridRow {
                        Button("Marcar") { controller.sendCommand(.mark) }
                        Button("Inicio") { controller.sendCommand(.home) }
                    }
                    GridRow {
                        Button("Esquerda") { controller.sendCommand(.left) }
                        Button("Direita") { controller.sendCommand(.right) }
                    }
                    GridRow {
                        Button("Avancar") { controller.sendCommand(.paperForward) }
                        Button("Retroceder") { controller.sendCommand(.paperBack) }
                    }
                    GridRow {
                        Button("Subir") { controller.sendCommand(.zUp) }
                        Button("Baixar") { controller.sendCommand(.zDown) }
                    }
                    GridRow {
                        Button("Localizar folha") { controller.sendCommand(.locatePaper) }
                            .gridCellColumns(2)
                    }
                }
                .buttonStyle(.bordered)
                .padding(4)
            }

            GroupBox("Mensagem") {
                VStack(alignment: .leading, spacing: 8) {
                    Text("\(controller.encodedBytes.count) bytes")
                        .font(.caption)
                        .foregroundStyle(.secondary)

                    ScrollView {
                        Text(controller.encodedPreview)
                            .font(.system(.caption, design: .monospaced))
                            .textSelection(.enabled)
                            .frame(maxWidth: .infinity, alignment: .leading)
                    }
                    .frame(height: 120)
                }
                .padding(4)
            }

            Spacer()

            Text(controller.status)
                .font(.caption)
                .foregroundStyle(controller.statusKind.color)
                .textSelection(.enabled)
        }
        .padding()
    }

    private var editor: some View {
        VStack(spacing: 0) {
            toolbar

            TextEditor(text: $controller.documentText)
                .font(.system(size: 17, design: .monospaced))
                .scrollContentBackground(.hidden)
                .padding(18)
                .background(Color(nsColor: .textBackgroundColor))
        }
        .navigationTitle(controller.documentTitle)
    }

    private var toolbar: some View {
        HStack(spacing: 10) {
            Button("Novo") { controller.newDocument() }
            Button("Abrir") { controller.openDocument() }
            Button("Salvar") { controller.saveDocument() }

            Divider()
                .frame(height: 24)

            Button("Imprimir Braille") {
                controller.printBraille()
            }
            .buttonStyle(.borderedProminent)

            Spacer()

            Text("Linhas: \(controller.lineCount)")
                .font(.caption)
                .foregroundStyle(.secondary)
        }
        .padding(.horizontal, 16)
        .padding(.vertical, 10)
        .background(Color(nsColor: .windowBackgroundColor))
    }
}

private extension PrinterController.StatusKind {
    var color: Color {
        switch self {
        case .idle:
            return .secondary
        case .success:
            return .green
        case .failure:
            return .red
        }
    }
}

#Preview {
    ContentView()
}
