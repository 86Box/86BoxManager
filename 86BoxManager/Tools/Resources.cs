using System.IO;
using Avalonia.Media.Imaging;

namespace _86boxManager.Tools
{
    internal static class Resources
    {
        public static Bitmap LoadImage(Stream stream)
        {
            var bitmap = new Bitmap(stream);
            return bitmap;
        }

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