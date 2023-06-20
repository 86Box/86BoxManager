using System.IO;
using Gdk;

namespace _86boxManager.Tools
{
    internal static class Resources
    {
        public static Pixbuf LoadImage(Stream stream, int? size = null)
        {
            var res = new Pixbuf(stream);
            if (size == null)
                return res;

            var px = size.Value;
            return res.ScaleSimple(px, px, InterpType.Bilinear);
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