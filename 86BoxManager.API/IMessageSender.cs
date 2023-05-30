using System;

namespace _86BoxManager.API
{
    public interface IMessageSender
    {
        void DoVmRequestStop(IntPtr hWnd);
        void DoVmForceStop(IntPtr hWnd);

        void DoVmPause(IntPtr hWnd);
        void DoVmResume(IntPtr hWnd);

        void DoVmCtrlAltDel(IntPtr hWnd);
        void DoVmHardReset(IntPtr hWnd);

        void DoVmConfigure(IntPtr hWnd);

        void DoManagerStartVm(IntPtr hWnd, string vmName);
    }
}