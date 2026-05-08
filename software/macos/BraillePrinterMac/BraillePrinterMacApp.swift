import SwiftUI

@main
struct BraillePrinterMacApp: App {
    @StateObject private var controller = PrinterController()

    var body: some Scene {
        WindowGroup {
            ContentView()
                .environmentObject(controller)
        }
        .commands {
            CommandGroup(replacing: .newItem) {
                Button("Novo") {
                    controller.newDocument()
                }
                .keyboardShortcut("n", modifiers: .command)
            }

            CommandGroup(after: .saveItem) {
                Button("Abrir...") {
                    controller.openDocument()
                }
                .keyboardShortcut("o", modifiers: .command)

                Button("Salvar...") {
                    controller.saveDocument()
                }
                .keyboardShortcut("s", modifiers: .command)
            }

            CommandMenu("Impressora") {
                Button("Imprimir em Braille") {
                    controller.printBraille()
                }
                .keyboardShortcut("p", modifiers: [.command, .shift])

                Button("Atualizar portas") {
                    controller.refreshPorts()
                }
            }
        }
    }
}
