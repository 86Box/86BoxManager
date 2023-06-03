using System.IO;

namespace _86boxManager.Tools
{
    internal static class Resources
    {
        public static Stream FindResource(string path)
        {
            const string n = $".{nameof(Resources)}";
            var type = typeof(Program);
            var dll = type.Assembly;
            var prefix = type.FullName?.Replace(".Program", n);
            var fullName = prefix + path.Replace('/', '.');
            var resource = dll.GetManifestResourceStream(fullName);
            return resource;
        }
    }
}