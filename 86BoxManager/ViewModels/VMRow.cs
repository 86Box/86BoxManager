using System.Collections.Generic;
using _86BoxManager.Models;
using Avalonia.Media.Imaging;
using ReactiveUI;
using static _86BoxManager.Tools.Resources;

// ReSharper disable InconsistentNaming

namespace _86BoxManager.ViewModels
{
    internal sealed class VMRow : ReactiveObject
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

        public VMRow(VM real)
        {
            Tag = real;
        }

        internal VM Tag { get; set; }

        public bool Focused
        {
            set => Selected = value;
        }

        public bool Selected
        {
            set
            {
                var ui = Program.Root;
                if (value)
                    ui.lstVMs.SelectedItems.Add(this);
                else
                    ui.lstVMs.SelectedItems.Remove(this);
            }
        }

        public Bitmap Icon => _icons[Tag.Status];
        public string Name => Tag.Name;
        public string Status => Tag.GetStatusString();
        public string Desc => Tag.Desc;
        public string Path => Tag.Path;

        public void SetStatus(string _)
        {
            // NO OP
        }

        public void SetIcon(int status)
        {
            Tag.Status = status;
            this.RaisePropertyChanged(nameof(Icon));
            this.RaisePropertyChanged(nameof(Status));
            this.RaisePropertyChanged(nameof(Tag));
        }
    }
}