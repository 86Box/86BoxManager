using System;
using System.Diagnostics;
using System.IO;
using _86BoxManager.API;
using IWshRuntimeLibrary;
using static _86BoxManager.Windows.Internal.Win32Imports;

namespace _86BoxManager.Windows
{
    public sealed class WinShell : IShell
    {
        public void CreateShortcut(string address, string name, string desc, string startup)
        {
            dynamic shell = new WshShell();
            dynamic shortcut = (IWshShortcut)shell.CreateShortcut(address);
            shortcut.Description = desc;
            shortcut.IconLocation = $"{Path.Combine(startup, "86manager.exe")},0";
            shortcut.TargetPath = Path.Combine(startup, "86manager.exe");
            shortcut.Arguments = $@"-S ""{name}""";
            shortcut.Save();
        }

        public void PushToForeground(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);
        }

        public void PrepareAppId(string appId)
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            SetCurrentProcessExplicitAppUserModelID(appId);
        }

        public void OpenFolder(string folder)
        {
            Process.Start(new ProcessStartInfo(folder)
            {
                UseShellExecute = true
            });
        }

        public void EditFile(string file)
        {
            Process.Start(new ProcessStartInfo(file)
            {
                UseShellExecute = true
            });
        }
    }
}