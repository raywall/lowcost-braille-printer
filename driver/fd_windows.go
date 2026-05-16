//go:build windows

package main

import (
	"errors"
	"os"
)

// Windows does not have EAGAIN/EWOULDBLOCK; these sentinels will never match
// real Windows I/O errors, preserving correct non-matching behaviour.
var errAgain = errors.New("would block")
var errWouldBlock = errors.New("would block")

// setNonblocking is a no-op on Windows; serial port timeouts are controlled
// via COM port settings rather than file descriptor flags.
func setNonblocking(_ *os.File, _ bool) error {
	return nil
}
