using _86BoxManager.API;
using System;
using System.IO;

namespace _86BoxManager.Mac
{
    public sealed class MacEnv : IEnv
    {
        public MacEnv()
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

        public string[] GetProgramFiles(string a)
        {
            var folders = new[]
            {
                Path.Combine(UserProfile, "Portable", a, a + ".app", "Contents", "MacOS"),
                Path.Combine(UserProfile, "Applications", a + ".app", "Contents", "MacOS"),
                Path.Combine("/Applications", a + ".app", "Contents", "MacOS"),
                Path.Combine("/opt", a),
                "/usr/local/bin",
                "/usr/bin"
            };
            return folders;
        }
    }
}
