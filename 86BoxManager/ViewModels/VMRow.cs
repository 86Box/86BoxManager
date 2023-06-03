using System.Collections.Generic;
using _86boxManager.Models;
using Avalonia.Media.Imaging;
using static _86boxManager.Tools.Resources;

// ReSharper disable InconsistentNaming

namespace _86boxManager.ViewModels
{
    internal sealed class VMRow
    {
        private static readonly IDictionary<int, Bitmap> _icons;

        static VMRow()
        {
            var pause = LoadImage(FindResource("/vm-paused.png"));
            var wait = LoadImage(FindResource("/vm-waiting.png"));
            var run = LoadImage(FindResource("/vm-running.png"));
            var stop = LoadImage(FindResource("/vm-stopped.png"));

            _icons = new Dictionary<int, Bitmap>
            {
                { VM.STATUS_PAUSED, pause },
                { VM.STATUS_WAITING, wait },
                { VM.STATUS_RUNNING, run },
                { VM.STATUS_STOPPED, stop }
            };
        }

        internal VM Tag { get; set; }

        internal bool Focused { get; set; }
        internal bool Selected { get; set; }

        public Bitmap Icon => _icons[Tag.Status];
        public string Name => Tag.Name;
        public string Status => Tag.GetStatusString();
        public string Desc => Tag.Desc;
        public string Path => Tag.Path;
    }
}