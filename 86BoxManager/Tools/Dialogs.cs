using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.DTO;
using System.Linq;
using StartLoc = Avalonia.Controls.WindowStartupLocation;

namespace _86BoxManager.Tools
{
    internal static class Dialogs
    {
        public static ButtonResult ShowMessageBox(string msg, Icon icon,
            ButtonEnum buttons = ButtonEnum.Ok, string title = "Attention")
        {
            var parent = Program.Root;
            var loc = parent == null ? StartLoc.CenterScreen : StartLoc.CenterOwner;
            var opts = new MessageBoxStandardParams
            {
                ButtonDefinitions = buttons,
                ContentTitle = title,
                ContentMessage = msg,
                Icon = icon,
                CanResize = false,
                WindowStartupLocation = loc,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            var window = MessageBoxManager.GetMessageBoxStandardWindow(opts);
            var raw = parent != null ? window.ShowDialog(parent) : window.Show();
            if (Application.Current is var app)
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Instance;
                var windowField = window.GetType().GetField("_window", flags)!;
                var windowObj = (Window)windowField.GetValue(window)!;
                if (parent?.Icon is { } wi)
                    windowObj.Icon = wi;
                app.Run(windowObj);
            }
            var res = raw.GetAwaiter().GetResult();
            return res;
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static async Task RunDialog(this Window parent, Window dialog, Action func = null)
        {
            dialog.WindowStartupLocation = StartLoc.CenterOwner;
            dialog.Icon = parent.Icon;

            var raw = dialog.ShowDialog(parent);
            await raw;
            func?.Invoke();
            (dialog as IDisposable)?.Dispose();
        }

        public static async Task<string> SelectFolder(string title, string dir, Window parent)
        {
            var dialog = new OpenFolderDialog
            {
                Title = title, Directory = dir
            };

            string result = null;
            var raw = dialog.ShowAsync(parent);
            var res = await raw;

            if (!string.IsNullOrWhiteSpace(res))
            {
                result = res;
            }
            return result;
        }

        public static async Task<string> SaveFile(string title, string dir, string filter,
            Window parent, string ext = null)
        {
            var dialog = new SaveFileDialog
            {
                Title = title, Directory = dir, DefaultExtension = ext
            };

            if (filter != null)
            {
                var tmp = filter.Split('|', 2);
                dialog.Filters = new List<FileDialogFilter>
                {
                    new() { Name = tmp.First(), Extensions = new List<string> { tmp.Last() } }
                };
            }

            string result = null;
            var raw = dialog.ShowAsync(parent);
            var res = await raw;

            if (!string.IsNullOrWhiteSpace(res))
            {
                result = res;
            }
            return result;
        }
    }
}