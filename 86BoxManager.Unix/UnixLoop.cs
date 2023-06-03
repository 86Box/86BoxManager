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
        private readonly UnixExecutor _executor;
        private readonly PosixSignalRegistration _posix;

        public UnixLoop(IMessageReceiver callback, UnixExecutor executor)
        {
            _callback = callback;
            _executor = executor;
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

        private void SendCommand(IVm vm, string message)
        {
            var vmName = vm.Name;
            var client = _executor.GetClient(vmName);
            if (client == null)
                return;
            var text = message + "\n";
            var block = Encoding.UTF8.GetBytes(text);
            client.Send(block);
        }

        public void DoVmRequestStop(IVm vm) => SendCommand(vm, "shutdown");
        public void DoVmForceStop(IVm vm) => SendCommand(vm, "shutdownnoprompt");
        public void DoVmPause(IVm vm) => SendCommand(vm, "pause");
        public void DoVmResume(IVm vm) => DoVmPause(vm);
        public void DoVmCtrlAltDel(IVm vm) => SendCommand(vm, "cad");
        public void DoVmHardReset(IVm vm) => SendCommand(vm, "reset");
        public void DoVmConfigure(IVm vm) => SendCommand(vm, "showsettings");

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