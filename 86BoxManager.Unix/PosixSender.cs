using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using _86BoxManager.API;
using Mono.Unix.Native;

#pragma warning disable CA1416

namespace _86BoxManager.Unix
{
    public abstract class PosixSender : IMessageSender, IDisposable, IMessageLoop
    {
        private readonly IMessageReceiver _callback;
        private readonly PosixSignalRegistration _posix;

        protected PosixSender(IMessageReceiver callback)
        {
            _callback = callback;
            _posix = PosixSignalRegistration.Create(PosixSignal.SIGCONT, OnPosixStop);
        }

        public void Dispose()
        {
            _posix.Dispose();
        }

        ~PosixSender()
        {
            Dispose();
        }

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

        public IntPtr GetHandle()
        {
            var proc = Process.GetCurrentProcess();
            return new IntPtr(proc.Id);
        }

        public abstract void DoVmRequestStop(IntPtr hWnd);
        public abstract void DoVmForceStop(IntPtr hWnd);
        public abstract void DoVmPause(IntPtr hWnd);
        public abstract void DoVmResume(IntPtr hWnd);
        public abstract void DoVmCtrlAltDel(IntPtr hWnd);
        public abstract void DoVmHardReset(IntPtr hWnd);
        public abstract void DoVmConfigure(IntPtr hWnd);
    }
}