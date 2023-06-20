using System.IO;

namespace _86BoxManager.Xplat
{
    public static class Search
    {
        public static string Find(string[] folders, string[] exeNames)
        {
            foreach (var folder in folders)
            foreach (var exeName in exeNames)
            {
                var exePath = Path.Combine(folder, exeName);
                if (!File.Exists(exePath))
                    continue;
                return folder;
            }
            return null;
        }

        public static string CheckTrail(this string path)
        {
            //To make sure there's a trailing backslash at the end, as other code using these strings expects it!
            if (!path.EndsWith(Path.DirectorySeparatorChar))
            {
                path += Path.DirectorySeparatorChar;
            }
            return path;
        }
    }
}