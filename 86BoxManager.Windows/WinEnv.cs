using System;
using System.IO;
using _86BoxManager.API;

namespace _86BoxManager.Windows
{
    public sealed class WinEnv : IEnv
    {
        public WinEnv()
        {
            MyComputer = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            ExeNames = new[] { "86Box.exe" };
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
                Path.Combine("C:\\Portable", appName),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), appName),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), appName)
            };
            return folders;
        }
    }
}