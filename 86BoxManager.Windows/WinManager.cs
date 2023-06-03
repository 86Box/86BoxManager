using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using _86BoxManager.API;
using _86BoxManager.Common;
using _86BoxManager.Windows.Internal;
using static _86BoxManager.Windows.Internal.Win32Imports;

namespace _86BoxManager.Windows
{
    public sealed class WinManager : CommonManager, IManager
    {
        private static Mutex mutex = null;

        public override bool IsFirstInstance(string name)
        {
            //Use a mutex to check if this is the first instance of Manager
            mutex = new Mutex(true, name, out var firstInstance);
            return firstInstance;
        }

        public override IntPtr RestoreAndFocus(string windowTitle, string handleTitle)
        {
            //Finds the existing window, unhides it, restores it and sets focus to it
            var hWnd = FindWindow(null, windowTitle);
            ShowWindow(hWnd, ShowWindowEnum.Show);
            ShowWindow(hWnd, ShowWindowEnum.Restore);
            SetForegroundWindow(hWnd);

            hWnd = FindWindow(null, handleTitle);
            return hWnd;
        }

        public override IVerInfo GetBoxVersion(string exeDir)
        {
            var exePath = Path.Combine(exeDir, "86Box.exe");
            if (!File.Exists(exePath))
            {
                // Not found!
                return null;
            }
            var vi = FileVersionInfo.GetVersionInfo(exePath);
            return new WinVerInfo(vi);
        }

        public override IMessageLoop GetLoop(IMessageReceiver callback)
        {
            var loop = new WinLoop(callback);
            return loop;
        }

        public override IMessageSender GetSender()
        {
            var loop = new WinLoop(null);
            return loop;
        }

        public override IExecutor GetExecutor()
        {
            var exec = new WinExecutor();
            return exec;
        }
    }
}