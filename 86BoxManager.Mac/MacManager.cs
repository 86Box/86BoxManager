using System;
using System.IO;
using System.Linq;
using _86BoxManager.API;
using _86BoxManager.Common;
using _86BoxManager.Unix;

namespace _86BoxManager.Mac
{
    public sealed class MacManager : UnixManager
    {
        public override IVerInfo GetBoxVersion(string exeDir)
        {
            var info = Path.Combine(exeDir, "..", "Info.plist");
            var text = File.ReadAllText(info);
            var bip = text.Split("CFBundleVersion", 2);
            var bit = bip.Last().Split("<string>", 2);
            var bi = bit.Last().Split("</", 2).First();
            var bv = Version.Parse(bi);
            return new CommonVerInfo
            {
                FileMajorPart = bv.Major,
                FileMinorPart = bv.Minor,
                FileBuildPart = bv.Build,
                FilePrivatePart = bv.Revision
            };
        }

        public override string FormatBoxArgs(string vmPath, string idString, string hWndHex)
        {
            return $@"--vmpath ""{vmPath}""";
        }
    }
}