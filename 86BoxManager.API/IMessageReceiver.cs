using System;

namespace _86BoxManager.API
{
    public interface IMessageReceiver
    {
        void OnEmulatorInit(IntPtr hWnd, uint vmId);
        void OnEmulatorShutdown(IntPtr hWnd);

        void OnVmPaused(IntPtr hWnd);
        void OnVmResumed(IntPtr hWnd);

        void OnDialogOpened(IntPtr hWnd);
        void OnDialogClosed(IntPtr hWnd);

        void OnManagerStartVm(string vmName);
    }
}