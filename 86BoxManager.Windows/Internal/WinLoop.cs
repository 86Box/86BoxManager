using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;
using _86BoxManager.API;
using static _86BoxManager.Windows.Internal.Win32Imports;

namespace _86BoxManager.Windows.Internal
{
    internal sealed class WinLoop : Form, IMessageLoop
    {
        private readonly IMessageHandler _callback;

        public WinLoop(IMessageHandler callback)
        {
            _callback = callback;
        }

        // This function monitors received window messages
        protected override void WndProc(ref Message m)
        {
            // 0x8891 - Main window init complete, wparam = VM ID, lparam = VM window handle
            // 0x8895 - VM paused/resumed, wparam = 1: VM paused, wparam = 0: VM resumed
            // 0x8896 - Dialog opened/closed, wparam = 1: opened, wparam = 0: closed
            // 0x8897 - Shutdown confirmed

            if (m.Msg == 0x8891)
            {
                if (m.LParam != IntPtr.Zero && m.WParam.ToInt64() >= 0)
                {
                    var vmId = (uint)m.WParam.ToInt32();
                    var hWnd = m.LParam;
                    _callback.OnEmulatorInit(hWnd, vmId);
                }
            }

            if (m.Msg == 0x8895)
            {
                if (m.WParam.ToInt32() == 1) // VM was paused
                {
                    var hWnd = m.LParam;
                    _callback.OnVmPaused(hWnd);
                }
                else if (m.WParam.ToInt32() == 0) // VM was resumed
                {
                    var hWnd = m.LParam;
                    _callback.OnVmResumed(hWnd);
                }
            }

            if (m.Msg == 0x8896)
            {
                if (m.WParam.ToInt32() == 1) // A dialog was opened
                {
                    var hWnd = m.LParam;
                    _callback.OnDialogOpened(hWnd);
                }
                else if (m.WParam.ToInt32() == 0) // A dialog was closed
                {
                    var hWnd = m.LParam;
                    _callback.OnDialogClosed(hWnd);
                }
            }

            if (m.Msg == 0x8897) // Shutdown confirmed
            {
                var hWnd = m.LParam;
                _callback.OnEmulatorShutdown(hWnd);
            }

            // This is the WM_COPYDATA message, used here to pass command line args to an already running instance
            // NOTE: This code will have to be modified in case more command line arguments are added in the future.
            if (m.Msg == WM_COPYDATA)
            {
                // Get the VM name and find the associated LVI and VM object
                var ds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                var vmName = Marshal.PtrToStringAnsi(ds.lpData, ds.cbData);

                _callback.OnManagerStartVm(vmName);
            }

            base.WndProc(ref m);
        }

        public IntPtr GetHandle()
        {
            var native = Handle;
            return native;
        }
    }
}