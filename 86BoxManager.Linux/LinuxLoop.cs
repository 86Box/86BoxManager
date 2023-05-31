using System;
using _86BoxManager.API;
using _86BoxManager.Unix;

#pragma warning disable CA1416

namespace _86BoxManager.Linux
{
    public sealed class LinuxLoop : PosixSender
    {
        public LinuxLoop(IMessageReceiver callback) : base(callback)
        {
        }

        public override void DoVmRequestStop(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public override void DoVmForceStop(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public override void DoVmPause(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public override void DoVmResume(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public override void DoVmCtrlAltDel(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public override void DoVmHardReset(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public override void DoVmConfigure(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }
    }
}