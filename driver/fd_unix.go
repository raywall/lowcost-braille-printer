//go:build !windows

package main

import (
	"os"
	"syscall"
)

var errAgain = syscall.EAGAIN
var errWouldBlock = syscall.EWOULDBLOCK

func setNonblocking(f *os.File, nonblocking bool) error {
	return syscall.SetNonblock(int(f.Fd()), nonblocking)
}
