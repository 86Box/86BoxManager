using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using _86BoxManager.API;

namespace _86BoxManager.Linux
{
    public sealed class LinuxShell : IShell
    {
        public void CreateShortcut(string address, string name, string desc, string startup)
        {
            var fileName = address.Replace(".lnk", ".desktop");
            var myExe = Path.Combine(startup, "86Manager");
            var myIcon = Path.Combine(startup, "Resources", "86Box-gray.svg");
            var lines = new[]
            {
                "[Desktop Entry]",
                "Version=1.0",
                "Type=Application",
                $"Name={name}",
                @$"Exec=""{myExe}"" -S ""{name}""",
                $"Icon={myIcon}",
                $"Comment={desc}",
                "Terminal=false",
                "Categories=Game;Emulator;",
                "StartupWMClass=86box-vm",
                "StartupNotify=true"
            };
            var bom = new UTF8Encoding(false);
            File.WriteAllLines(fileName, lines, bom);
        }

        public void ForceStop(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void RequestStop(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void PushToForeground(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void Resume(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void Pause(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void Configure(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void HardReset(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void CtrlAltDel(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void PrepareAppId(string appId)
        {
            // NO-OP
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