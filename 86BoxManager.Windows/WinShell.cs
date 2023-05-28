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

        public void ForceStop(IntPtr hWnd)
        {
            PostMessage(hWnd, 0x8893, new IntPtr(1), IntPtr.Zero);
        }

        public void RequestStop(IntPtr hWnd)
        {
            PostMessage(hWnd, 0x8893, IntPtr.Zero, IntPtr.Zero);
        }

        public void PushToForeground(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);
        }

        public void Resume(IntPtr hWnd)
        {
            PostMessage(hWnd, 0x8890, IntPtr.Zero, IntPtr.Zero);
        }

        public void Pause(IntPtr hWnd)
        {
            PostMessage(hWnd, 0x8890, IntPtr.Zero, IntPtr.Zero);
        }

        public void Configure(IntPtr hWnd)
        {
            PostMessage(hWnd, 0x8889, IntPtr.Zero, IntPtr.Zero);
        }

        public void HardReset(IntPtr hWnd)
        {
            PostMessage(hWnd, 0x8892, IntPtr.Zero, IntPtr.Zero);
        }

        public void CtrlAltDel(IntPtr hWnd)
        {
            PostMessage(hWnd, 0x8894, IntPtr.Zero, IntPtr.Zero);
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