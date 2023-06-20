using Gtk;

namespace _86BoxManager.Tools
{
    public static class Dialogs
    {
        public static string SaveFile(string title, string dir, string filter, Window parent)
        {
            var dialog = new FileChooserDialog(title, parent, FileChooserAction.Save,
                "Cancel", ResponseType.Cancel,
                "Save", ResponseType.Accept);
            dialog.SetCurrentFolder(dir);

            if (filter != null)
                dialog.Filter = new FileFilter { Name = filter };

            string result = null;
            if (dialog.Run() == (int)ResponseType.Accept)
            {
                result = dialog.Filename;
            }
            dialog.Destroy();

            return result;
        }

        public static string SelectFolder(string title, string dir, Window parent)
        {
            var dialog = new FileChooserDialog(title, parent, FileChooserAction.SelectFolder,
                "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept);
            dialog.SetCurrentFolder(dir);

            string result = null;
            if (dialog.Run() == (int)ResponseType.Accept)
            {
                result = dialog.Filename;
            }
            dialog.Destroy();

            return result;
        }

        public static int ShowMessageBox(string text, MessageType type,
            ButtonsType btn = ButtonsType.Ok, string title = null,
            DialogFlags flags = DialogFlags.Modal, Window parent = null)
        {
            if (parent == null)
                parent = Program.Root;
            var dialog = new MessageDialog(parent, flags, type, btn, text);
            if (title != null)
                dialog.Title = title;
            var res = dialog.Run();
            dialog.Destroy();
            return res;
        }

        public static void RunDialog(this Window parent, Dialog dlg, System.Action after = null)
        {
            using (dlg)
            {
                dlg.TransientFor = parent;
                dlg.ShowAll();
                dlg.Run();
                after?.Invoke();
                dlg.Destroy();
            }
        }
    }
}