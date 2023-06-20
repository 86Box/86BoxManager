using System.Diagnostics;
using System.IO;
using System.Text;
using _86BoxManager.Common;

namespace _86BoxManager.Mac
{
    public sealed class MacShell : CommonShell
    {
        public override void CreateShortcut(string address, string name, string desc, string startup)
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

        public override void OpenFolder(string folder)
        {
            var start = new ProcessStartInfo("open");
            start.ArgumentList.Add(folder);
            Process.Start(start);
        }
    }
}