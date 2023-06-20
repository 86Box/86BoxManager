using System;
using _86boxManager.Tools;
using _86boxManager.View;
using _86BoxManager.Xplat;
using Gdk;
using Gtk;
using static _86boxManager.Tools.Resources;

namespace _86boxManager
{
    internal static class Program
    {
        //Get command line arguments
        public static string[] Args;

        //For grouping windows together in Win7+ taskbar
        private static readonly string AppId = "86Box.86Box";

        internal static frmMain Root;
        
        [STAThread]
        private static void Main(string[] args)
        {
            Args = args;

            Platforms.Shell.PrepareAppId(AppId);
            Application.Init();

            //Check if it is the very first and only instance running.
            //If it's not, we need to restore and focus the existing window, 
            //as well as pass on any potential command line arguments
            if (CheckRunningManagerAndAbort(args))
                return;

            //Then check if any instances of 86Box are already running and warn the user
            if (CheckRunningEmulatorAndAbort())
                return;

            var app = new Application("org.86box.manager", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new frmMain();
            win.SetPosition(WindowPosition.Center);
            win.Icon = LoadIcon();

            app.AddWindow(Root = win);

            win.Show();
            Application.Run();
        }

        internal static Pixbuf LoadIcon(int? size = null)
            => LoadImage(FindResource("/86Box-gray.png"), size);

        private static bool CheckRunningManagerAndAbort(string[] args)
        {
            const string name = "86Box Manager";
            const string handleName = "86Box Manager Secret";

            var firstInstance = Platforms.Manager.IsFirstInstance(name);
            if (!firstInstance)
            {
                var hWnd = Platforms.Manager.RestoreAndFocus(name, handleName);

                // If this second instance comes from a VM shortcut, we need to pass on the
                // command line arguments so the VM will start in the existing instance.
                // NOTE: This code will have to be modified in case more
                // command line arguments are added in the future.
                if (GetVmArg(args, out var message))
                {
                    var sender = Platforms.Manager.GetSender();
                    sender.DoManagerStartVm(hWnd, message);
                }
                return true;
            }
            return false;
        }

        internal static bool GetVmArg(string[] args, out string vmName)
        {
            if (args.Length == 2 && args[0] == "-S" && args[1] != null)
            {
                vmName = args[1];
                return true;
            }
            vmName = default;
            return false;
        }

        private static bool CheckRunningEmulatorAndAbort()
        {
            var isRunning = Platforms.Manager.IsProcessRunning("86box") ||
                            Platforms.Manager.IsProcessRunning("86Box");
            if (isRunning)
            {
                var result = Dialogs.ShowMessageBox("At least one instance of 86Box is already running. " +
                                                    "It's not recommended that you run 86Box directly " +
                                                    "outside of Manager. Do you want to continue at your own risk?",
                    MessageType.Warning, ButtonsType.YesNo, "Warning");
                if (result == (int)ResponseType.No)
                {
                    return true;
                }
            }
            return false;
        }
    }
}