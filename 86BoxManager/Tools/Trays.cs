using Gdk;
using Gtk;

#pragma warning disable CS0612

namespace _86boxManager.Tools
{
    public static class Trays
    {
        public static void ApplyIcon(this StatusIcon trayIcon, Pixbuf pix)
        {
            trayIcon.Icon = pix;
        }

        public static void MakeVisible(this StatusIcon trayIcon, bool value)
        {
            trayIcon.Visible = value;
        }
    }
}