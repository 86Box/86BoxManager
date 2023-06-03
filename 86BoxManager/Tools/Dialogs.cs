using System;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.DTO;

namespace _86boxManager.Tools
{
    internal static class Dialogs
    {
        public static ButtonResult ShowMessageBox(string msg, Icon icon,
            ButtonEnum buttons = ButtonEnum.Ok, string title = "Attention")
        {
            var opts = new MessageBoxStandardParams
            {
                ButtonDefinitions = buttons,
                ContentTitle = title,
                ContentMessage = msg,
                Icon = icon,
                ShowInCenter = true,
                CanResize = false
            };
            var window = MessageBoxManager.GetMessageBoxStandardWindow(opts);
            var raw = window.Show();
            if (Application.Current is var app)
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Instance;
                var windowField = window.GetType().GetField("_window", flags)!;
                var windowObj = (Window)windowField.GetValue(window);
                app.Run(windowObj);
            }
            var res = raw.GetAwaiter().GetResult();
            return res;
        }

        public static string SelectFolder(string text, string initDir, Window parent)
        {
            throw new System.NotImplementedException();
        }

        public static string SaveFile(string title, string dir, string filter, Window parent)
        {
            throw new System.NotImplementedException();
        }

        public static void RunDialog(this Window parent, Window dialog, Action func = null)
        {
            var raw = dialog.ShowDialog(parent);
            func?.Invoke();
            raw.GetAwaiter().GetResult();
        }
    }
}