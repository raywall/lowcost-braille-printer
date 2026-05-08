import Darwin
import Foundation

final class SerialConnection {
    private let warmup: UInt64 = 2_200_000_000
    private let chunkSize = 32
    private let chunkDelay: UInt64 = 5_000_000

    static func availablePorts() -> [String] {
        let patterns = [
            "/dev/cu.usbmodem*",
            "/dev/cu.usbserial*",
            "/dev/tty.usbmodem*",
            "/dev/tty.usbserial*"
        ]

        return patterns.flatMap { pattern in
            (try? FileManager.default.contentsOfDirectory(
                atPath: (pattern as NSString).deletingLastPathComponent
            ))?
                .map { ((pattern as NSString).deletingLastPathComponent as NSString).appendingPathComponent($0) }
                .filter { path in
                    let wildcard = (pattern as NSString).lastPathComponent.replacingOccurrences(of: "*", with: "")
                    return (path as NSString).lastPathComponent.hasPrefix(wildcard)
                } ?? []
        }
        .sorted()
    }

    func send(bytes: [UInt8], to port: String) async throws {
        try await Task.detached(priority: .userInitiated) {
            let fd = open(port, O_RDWR | O_NOCTTY | O_NONBLOCK)
            guard fd >= 0 else {
                throw SerialError.openFailed(port)
            }
            defer { close(fd) }

            _ = fcntl(fd, F_SETFL, 0)
            Self.configure(fd: fd)

            try await Task.sleep(nanoseconds: self.warmup)

            var offset = 0
            while offset < bytes.count {
                let end = min(offset + self.chunkSize, bytes.count)
                let written = bytes[offset..<end].withContiguousStorageIfAvailable { buffer -> Int in
                    guard let base = buffer.baseAddress else { return -1 }
                    return write(fd, base, buffer.count)
                } ?? -1

                guard written > 0 else {
                    throw SerialError.writeFailed(port)
                }

                offset += written
                try await Task.sleep(nanoseconds: self.chunkDelay)
            }
        }.value
    }

    private static func configure(fd: Int32) {
        var options = termios()
        guard tcgetattr(fd, &options) == 0 else { return }

        cfmakeraw(&options)
        cfsetspeed(&options, speed_t(B115200))
        options.c_cflag |= tcflag_t(CLOCAL | CREAD)
        options.c_cflag &= ~tcflag_t(PARENB)
        options.c_cflag &= ~tcflag_t(CSTOPB)
        options.c_cflag &= ~tcflag_t(CSIZE)
        options.c_cflag |= tcflag_t(CS8)

        _ = tcsetattr(fd, TCSANOW, &options)
    }
}

enum SerialError: LocalizedError {
    case openFailed(String)
    case writeFailed(String)

    var errorDescription: String? {
        switch self {
        case .openFailed(let port):
            return "Nao foi possivel abrir \(port)"
        case .writeFailed(let port):
            return "Nao foi possivel escrever em \(port)"
        }
    }
}
