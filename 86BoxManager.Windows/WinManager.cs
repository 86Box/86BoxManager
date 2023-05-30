using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using _86BoxManager.API;
using _86BoxManager.Windows.Internal;
using static _86BoxManager.Windows.Internal.Win32Imports;

namespace _86BoxManager.Windows
{
    public sealed class WinManager : IManager
    {
        private static Mutex mutex = null;

        public bool IsFirstInstance(string name)
        {
            //Use a mutex to check if this is the first instance of Manager
            mutex = new Mutex(true, name, out var firstInstance);
            return firstInstance;
        }

        public IntPtr RestoreAndFocus(string windowTitle, string handleTitle)
        {
            //Finds the existing window, unhides it, restores it and sets focus to it
            var hWnd = FindWindow(null, windowTitle);
            ShowWindow(hWnd, ShowWindowEnum.Show);
            ShowWindow(hWnd, ShowWindowEnum.Restore);
            SetForegroundWindow(hWnd);

            hWnd = FindWindow(null, handleTitle);
            return hWnd;
        }

        public bool IsProcessRunning(string name)
        {
            var pname = Process.GetProcessesByName(name);
            return pname.Length > 0;
        }

        public string GetVmName(object raw)
        {
            dynamic m = raw;
            var ds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
            var vmName = Marshal.PtrToStringAnsi(ds.lpData, ds.cbData);
            return vmName;
        }

        public IVerInfo GetBoxVersion(string exeDir)
        {
            var exePath = Path.Combine(exeDir, "86Box.exe");
            var vi = FileVersionInfo.GetVersionInfo(exePath);
            return new WinVerInfo(vi);
        }

        public string FormatBoxArgs(string vmPath, string idString, string hWndHex)
        {
            return $@"--vmpath ""{vmPath}"" --hwnd {idString},{hWndHex}";
        }

        public IMessageLoop GetLoop(IMessageReceiver callback)
        {
            var loop = new WinLoop(callback);
            return loop;
        }

        public IMessageSender GetSender()
        {
            var loop = new WinLoop(null);
            return loop;
        }
    }
}