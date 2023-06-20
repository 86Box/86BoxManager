using System.Collections.Generic;
using _86BoxManager.Models;
using Gdk;
using Gtk;
using static _86BoxManager.Tools.Resources;

// ReSharper disable InconsistentNaming

namespace _86BoxManager.Model
{
    internal sealed class VMRow
    {
        private static readonly IDictionary<int, Pixbuf> _icons;

        static VMRow()
        {
            var pause = LoadImage(FindResource("/vm-paused.png"));
            var wait = LoadImage(FindResource("/vm-waiting.png"));
            var run = LoadImage(FindResource("/vm-running.png"));
            var stop = LoadImage(FindResource("/vm-stopped.png"));

            _icons = new Dictionary<int, Pixbuf>
            {
                { VM.STATUS_PAUSED, pause },
                { VM.STATUS_WAITING, wait },
                { VM.STATUS_RUNNING, run },
                { VM.STATUS_STOPPED, stop }
            };
        }

        private readonly TreeIter _it;
        private readonly ListStore _store;

        public VMRow(VM real, TreeIter it, ListStore store)
        {
            Tag = real;
            _it = it;
            _store = store;
        }

        public VM Tag { get; }

        public static Pixbuf GetIcon(int status)
        {
            var pix = _icons[status];
            return pix;
        }

        public void SetStatus(string text)
        {
            _store.SetValue(_it, 2, text);
        }

        public void SetIcon(int status)
        {
            var icon = GetIcon(status);
            _store.SetValue(_it, 0, icon);
        }

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
                    ui.lstVMs.Selection.SelectIter(_it);
                else
                    ui.lstVMs.Selection.UnselectIter(_it);
            }
        }
    }
}