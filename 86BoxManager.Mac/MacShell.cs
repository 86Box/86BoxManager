using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using _86BoxManager.API;

namespace _86BoxManager.Mac
{
    public class MacShell : IShell
    {
        public void Configure(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void CreateShortcut(string address, string name, string desc, string startup)
        {
            var fileName = address.Replace(".lnk", ".sh");
            var myExe = Path.Combine(startup, "86Manager");
            var lines = new[]
            {
                "#!/bin/sh",
                @$"echo ""Name    : {name}""",
                @$"echo ""Comment : {desc}""",
                @$"""{myExe}"" -S ""{name}"" &"
            };
            var bom = new UTF8Encoding(false);
            File.WriteAllLines(fileName, lines, bom);
            Process.Start(new ProcessStartInfo("chmod", @$"+x ""{fileName}"""));
        }

        public void CtrlAltDel(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void ForceStop(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void HardReset(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void Pause(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void PrepareAppId(string appId)
        {
            // NO-OP
        }

        public void OpenFolder(string folder)
        {
            var start = new ProcessStartInfo("open");
            start.ArgumentList.Add(folder);
            Process.Start(start);
        }

        public void EditFile(string file)
        {
            Process.Start(new ProcessStartInfo(file)
            {
                UseShellExecute = true
            });
        }

        public void PushToForeground(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void RequestStop(IntPtr window)
        {
            throw new NotImplementedException();
        }

        public void Resume(IntPtr window)
        {
            throw new NotImplementedException();
        }
    }
}