import AppKit
import Foundation
import UniformTypeIdentifiers

@MainActor
final class PrinterController: ObservableObject {
    enum StatusKind {
        case idle
        case success
        case failure
    }

    @Published var documentText = ""
    @Published var documentURL: URL?
    @Published var availablePorts: [String] = []
    @Published var selectedPort = ""
    @Published var status = "Pronto"
    @Published var statusKind: StatusKind = .idle

    private let translator = BrailleTranslator()
    private let serial = SerialConnection()

    var documentTitle: String {
        documentURL?.lastPathComponent ?? "Sem titulo"
    }

    var lineCount: Int {
        max(1, documentText.split(separator: "\n", omittingEmptySubsequences: false).count)
    }

    var encodedBytes: [UInt8] {
        translator.messageBytes(for: documentText)
    }

    var encodedPreview: String {
        encodedBytes.map { String(format: "%02X", $0) }.joined(separator: " ")
    }

    func refreshPorts() {
        availablePorts = SerialConnection.availablePorts()
        if selectedPort.isEmpty, let first = availablePorts.first {
            selectedPort = first
        }
    }

    func newDocument() {
        documentText = ""
        documentURL = nil
        setStatus("Novo documento", .idle)
    }

    func openDocument() {
        let panel = NSOpenPanel()
        panel.allowedContentTypes = [.plainText, .rtf, .utf8PlainText]
        panel.allowsMultipleSelection = false

        guard panel.runModal() == .OK, let url = panel.url else { return }

        do {
            if url.pathExtension.lowercased() == "rtf" {
                let attributed = try NSAttributedString(url: url, options: [:], documentAttributes: nil)
                documentText = attributed.string
            } else {
                documentText = try String(contentsOf: url, encoding: .utf8)
            }
            documentURL = url
            setStatus("Arquivo aberto", .success)
        } catch {
            setStatus("Erro ao abrir arquivo: \(error.localizedDescription)", .failure)
        }
    }

    func saveDocument() {
        let panel = NSSavePanel()
        panel.allowedContentTypes = [.plainText]
        panel.nameFieldStringValue = documentURL?.lastPathComponent ?? "braille.txt"

        guard panel.runModal() == .OK, let url = panel.url else { return }

        do {
            try documentText.write(to: url, atomically: true, encoding: .utf8)
            documentURL = url
            setStatus("Arquivo salvo", .success)
        } catch {
            setStatus("Erro ao salvar arquivo: \(error.localizedDescription)", .failure)
        }
    }

    func printBraille() {
        let bytes = translator.messageBytes(for: documentText)
        send(bytes: bytes, label: "texto")
    }

    func sendTestPage() {
        let text = "Braille Printer\nTeste 123\n"
        send(bytes: translator.messageBytes(for: text), label: "teste")
    }

    func sendCommand(_ command: PrinterCommand) {
        send(bytes: command.bytes, label: command.title)
    }

    private func send(bytes: [UInt8], label: String) {
        refreshPorts()
        let port = selectedPort.isEmpty ? availablePorts.first : selectedPort

        guard let port else {
            setStatus("Nenhuma porta serial encontrada", .failure)
            return
        }

        setStatus("Enviando \(label) para \(port)...", .idle)

        Task {
            do {
                try await serial.send(bytes: bytes, to: port)
                await MainActor.run {
                    self.setStatus("Envio concluido para \(port)", .success)
                }
            } catch {
                await MainActor.run {
                    self.setStatus("Erro no envio: \(error.localizedDescription)", .failure)
                }
            }
        }
    }

    private func setStatus(_ value: String, _ kind: StatusKind) {
        status = value
        statusKind = kind
    }
}

enum PrinterCommand {
    case mark
    case left
    case right
    case paperForward
    case paperBack
    case zUp
    case zDown
    case home
    case locatePaper

    var title: String {
        switch self {
        case .mark: return "marcar"
        case .left: return "esquerda"
        case .right: return "direita"
        case .paperForward: return "avancar folha"
        case .paperBack: return "retroceder folha"
        case .zUp: return "subir"
        case .zDown: return "baixar"
        case .home: return "inicio"
        case .locatePaper: return "localizar folha"
        }
    }

    var bytes: [UInt8] {
        switch self {
        case .mark: return [0xE0, 0xC1, 0xFF]
        case .left: return [0xE0, 0xC2, 0xFF]
        case .right: return [0xE0, 0xC3, 0xFF]
        case .paperForward: return [0xE0, 0xC4, 0xFF]
        case .paperBack: return [0xE0, 0xC5, 0xFF]
        case .zUp: return [0xE0, 0xC6, 0xFF]
        case .zDown: return [0xE0, 0xC7, 0xFF]
        case .home: return [0xE0, 0xC8, 0xFF]
        case .locatePaper: return [0xE0, 0xC9, 0xFF]
        }
    }
}
