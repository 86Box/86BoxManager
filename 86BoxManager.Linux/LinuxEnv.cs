using System;
using System.IO;
using _86BoxManager.API;

namespace _86BoxManager.Linux
{
    public sealed class LinuxEnv : IEnv
    {
        public LinuxEnv()
        {
            MyComputer = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var fakeDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            MyDocuments = Path.Combine(fakeDoc, "Documents");

            ExeNames = new[] { "86Box" };
        }

        public string[] ExeNames { get; }
        public string MyComputer { get; }
        public string UserProfile { get; }
        public string MyDocuments { get; }
        public string Desktop { get; }

        public string[] GetProgramFiles(string appName)
        {
            var folders = new[]
            {
                Path.Combine(UserProfile, "Portable", appName),
                Path.Combine("/opt", appName),
                "/usr/local/bin",
                "/usr/bin"
            };
            return folders;
        }
    }
}