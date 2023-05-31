using System;
using _86BoxManager.API;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Mono.Unix.Native;

#pragma warning disable CA1416

namespace _86BoxManager.Unix
{
    public sealed class UnixLoop : IMessageLoop, IMessageSender, IDisposable
    {
        private readonly IMessageReceiver _callback;
        private readonly PosixSignalRegistration _posix;

        public UnixLoop(IMessageReceiver callback)
        {
            _callback = callback;
            _posix = PosixSignalRegistration.Create(PosixSignal.SIGCONT, OnPosixStop);
        }

        public void Dispose()
        {
            _posix.Dispose();
        }

        ~UnixLoop()
        {
            Dispose();
        }

        private void SendCommand(IntPtr hWnd, string message)
        {
            throw new InvalidOperationException("TODO");
        }

        public void DoVmRequestStop(IntPtr hWnd) => SendCommand(hWnd, "shutdown");
        public void DoVmForceStop(IntPtr hWnd) => SendCommand(hWnd, "shutdownnoprompt");
        public void DoVmPause(IntPtr hWnd) => SendCommand(hWnd, "pause");
        public void DoVmResume(IntPtr hWnd) => DoVmPause(hWnd);
        public void DoVmCtrlAltDel(IntPtr hWnd) => SendCommand(hWnd, "cad");
        public void DoVmHardReset(IntPtr hWnd) => SendCommand(hWnd, "reset");
        public void DoVmConfigure(IntPtr hWnd) => SendCommand(hWnd, "showsettings");

        public IntPtr GetHandle() => new(Environment.ProcessId);

        private void OnPosixStop(PosixSignalContext obj)
        {
            obj.Cancel = true;
            var proc = Process.GetCurrentProcess();
            var pid = Environment.ProcessId;
            var fileMsg = GetMsgFileName(proc, pid);
            if (!File.Exists(fileMsg))
                return;
            var contents = File.ReadAllText(fileMsg, Encoding.UTF8);
            File.Delete(fileMsg);
            var vmName = contents.Trim();
            _callback.OnManagerStartVm(vmName);
        }

        public void DoManagerStartVm(IntPtr hWnd, string vmName)
        {
            var pid = hWnd.ToInt32();
            var proc = Process.GetProcessById(pid);
            var fileMsg = GetMsgFileName(proc, pid);
            var contents = string.Empty + vmName;
            File.WriteAllText(fileMsg, contents, Encoding.UTF8);
            Syscall.kill(pid, Signum.SIGCONT);
        }

        private static string GetMsgFileName(Process proc, int pid)
        {
            var mod = proc.MainModule!;
            var fileName = mod.FileName;
            var fileDir = Path.GetDirectoryName(fileName)!;
            var fileMsg = Path.Combine(fileDir, $"p_{pid}.tmp");
            return fileMsg;
        }
    }
}