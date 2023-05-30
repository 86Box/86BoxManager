using System;
using System.IO;
using System.Linq;
using _86BoxManager.API;
using _86BoxManager.Common;

namespace _86BoxManager.Linux
{
    public sealed class LinuxManager : IManager
    {
        public bool IsFirstInstance(string name)
        {
            // TODO : Fix
            return true;
        }

        public IntPtr RestoreAndFocus(string title)
        {
            throw new NotImplementedException();
        }

        public void StartVmInside(string message, IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public bool IsProcessRunning(string name)
        {
            // TODO : Fix
            return false;
        }

        public string GetVmName(object raw)
        {
            throw new NotImplementedException();
        }

        public IVerInfo GetBoxVersion(string exeDir)
        {
            if (string.IsNullOrWhiteSpace(exeDir) || !Directory.Exists(exeDir))
                return null;
            var info = new UnixVerInfo();
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

        public string FormatBoxArgs(string vmPath, string idString, string hWndHex)
        {
            return $@"--vmpath ""{vmPath}""";
        }

        public IMessageLoop GetLoop(IMessageHandler callback)
        {
            throw new NotImplementedException();
        }
    }
}