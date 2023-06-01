using System.IO;
using System.Linq;
using _86BoxManager.API;
using _86BoxManager.Common;
using _86BoxManager.Unix;

namespace _86BoxManager.Linux
{
    public sealed class LinuxManager : UnixManager
    {
        public LinuxManager() : base(GetTmpDir()) { }

        public override IVerInfo GetBoxVersion(string exeDir)
        {
            if (string.IsNullOrWhiteSpace(exeDir) || !Directory.Exists(exeDir))
            {
                // Not found!
                return null;
            }
            var info = new CommonVerInfo();
            var appImage = Directory.GetFiles(exeDir, "86Box-*.AppImage").FirstOrDefault();
            if (appImage != null)
            {
                var full = Path.GetFileNameWithoutExtension(appImage);
                var build = full.Split('-').LastOrDefault();

                // HACK: Set version because we can't read the ELF version
                if (build == "b4311")
                {
                    info.FilePrivatePart = int.Parse(build.TrimStart('b'));
                    info.FileMinorPart = 11;
                    info.FileMajorPart = 3;
                    info.FileBuildPart = 0;
                }
            }
            return info;
        }

        public static string GetTmpDir() => "/tmp";
    }
}