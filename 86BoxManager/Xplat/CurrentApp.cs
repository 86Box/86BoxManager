using System.IO;

namespace _86boxManager.Xplat
{
    internal static class CurrentApp
    {
        public static string ProductVersion { get; } = ReadVersion();

        public static string StartupPath { get; } = ReadStartup();

        private static string ReadStartup()
        {
            var ass = typeof(CurrentApp).Assembly;
            var path = Path.GetFullPath(ass.Location);
            var dir = Path.GetDirectoryName(path);
            return dir;
        }

        private static string ReadVersion()
        {
            var ass = typeof(CurrentApp).Assembly;
            var ver = ass.GetName().Version;
            return ver?.ToString();
        }
    }
}