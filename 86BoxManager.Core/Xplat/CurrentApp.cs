using System.IO;
using System.Reflection;

namespace _86BoxManager.Xplat
{
    public static class CurrentApp
    {
        public static string ProductVersion { get; } = ReadVersion();

        public static string StartupPath { get; } = ReadStartup();

        private static Assembly GetEntryAss()
        {
            return Assembly.GetEntryAssembly() ?? typeof(CurrentApp).Assembly;
        }

        private static string ReadStartup()
        {
            var ass = GetEntryAss();
            var path = Path.GetFullPath(ass.Location);
            var dir = Path.GetDirectoryName(path);
            return dir;
        }

        private static string ReadVersion()
        {
            var ass = GetEntryAss();
            var ver = ass.GetName().Version;
            return ver?.ToString();
        }
    }
}