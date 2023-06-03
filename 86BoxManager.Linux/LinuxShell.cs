using System.IO;
using System.Text;
using _86BoxManager.Common;

namespace _86BoxManager.Linux
{
    public sealed class LinuxShell : CommonShell
    {
        public override void CreateShortcut(string address, string name, string desc, string startup)
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
    }
}