using System;

namespace _86BoxManager.API
{
    public interface IMessageSender
    {
        void DoVmRequestStop(IVm vm);
        void DoVmForceStop(IVm vm);

        void DoVmPause(IVm vm);
        void DoVmResume(IVm vm);

        void DoVmCtrlAltDel(IVm vm);
        void DoVmHardReset(IVm vm);

        void DoVmConfigure(IVm vm);

        void DoManagerStartVm(IntPtr hWnd, string vmName);
    }
}