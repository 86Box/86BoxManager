using System;
using _86boxManager.Models;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using _86BoxManager.API;
using _86boxManager.Core;
using _86boxManager.Registry;
using _86boxManager.Tools;
using _86boxManager.Xplat;
using IOPath = System.IO.Path;
using RegistryValueKind = _86boxManager.Registry.ValueKind;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;

namespace _86boxManager.Views
{
    public partial class frmMain : Window
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Main_OnOpened(object sender, EventArgs e)
        {
            if (Program.Root == null)
            {
                Program.Root = this;
                frmMain_Load(sender, e);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadSettings();
            LoadVMs();

            msgHandler = new VMHandler();
            msgSink = Platforms.Manager.GetLoop(msgHandler);
            var handle = msgSink.GetHandle();

            //Convert the current window handle to a form that's expected by 86Box
            hWndHex = $"{handle.ToInt64():X}";
            hWndHex = hWndHex.PadLeft(16, '0');

            //Check if command line arguments for starting a VM are OK
            if (Program.GetVmArg(Program.Args, out var invVmName))
            {
                //Find the VM with given name
                var lvi = Cache.FindItemWithText(invVmName);

                //Then select and start it if it's found
                if (lvi != null)
                {
                    lvi.Focused = true;
                    lvi.Selected = true;
                    VMCenter.Start();
                    return;
                }

                Dialogs.ShowMessageBox($@"The virtual machine ""{invVmName}"" could not be found. " +
                                       "It may have been removed or the specified name is incorrect.",
                    MessageType.Error, ButtonsType.Ok, "Virtual machine not found");
            }
        }

        public string CfgPath { get; }

        internal VMRow GetFocusedVm()
        {
            var item = lstVMs.GetSelItems();
            return item.FirstOrDefault();
        }

        private void BringToFront()
        {
            this.BringIntoView();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            await this.RunDialog(new dlgAddVM());
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            await this.RunDialog(new dlgEditVM());
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            var row = lstVMs.GetSelItems();
            var vm = row[0].Tag;
            if (vm.Status == VM.STATUS_STOPPED)
            {
                VMCenter.Start();
            }
            else if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                VMCenter.RequestStop(lstVMs.GetSelItems(), this);
            }
        }

        private async void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            await this.RunDialog(new dlgSettings(), () =>
            {
                //Reload the settings due to potential changes
                LoadSettings();
            });
        }

        //Load the settings from the registry
        private void LoadSettings()
        {
            try
            {
                //Try to load the settings from registry, if it fails fallback to default values
                var regkey = Configs.Open86BoxKey(true);

                if (regkey == null)
                {
                    Dialogs.ShowMessageBox("86Box Manager settings could not be loaded.\n" +
                                           "This is normal if you're running 86Box Manager\n" +
                                           "for the first time. Default values will be used.",
                        MessageType.Warning, ButtonsType.Ok, "Warning");

                    //Create the key and reopen it for write access
                    Configs.Create86BoxKey();
                    regkey = Configs.Open86BoxKey(true);
                    Configs.Create86BoxVmKey();

                    (cfgpath, exepath) = VMCenter.FindPaths();
                    minimize = false;
                    showConsole = true;
                    minimizeTray = false;
                    closeTray = false;
                    logging = false;
                    logpath = "";
                    gridlines = false;
                    sortColumn = 0;
                    sortOrder = SortType.Ascending;

                    // TODO lstVMs.EnableGridLines = TreeViewGridLines.None;
                    VMCenter.Sort(sortColumn, sortOrder);

                    //Defaults must also be written to the registry
                    regkey = Configs.Open86BoxKey(true);
                    regkey.SetValue("EXEdir", exepath, RegistryValueKind.String);
                    regkey.SetValue("CFGdir", cfgpath, RegistryValueKind.String);
                    regkey.SetValue("MinimizeOnVMStart", minimize, RegistryValueKind.DWord);
                    regkey.SetValue("ShowConsole", showConsole, RegistryValueKind.DWord);
                    regkey.SetValue("MinimizeToTray", minimizeTray, RegistryValueKind.DWord);
                    regkey.SetValue("CloseToTray", closeTray, RegistryValueKind.DWord);
                    regkey.SetValue("EnableLogging", logging, RegistryValueKind.DWord);
                    regkey.SetValue("LogPath", logpath, RegistryValueKind.String);
                    regkey.SetValue("EnableGridLines", gridlines, RegistryValueKind.DWord);
                    regkey.SetValue("SortColumn", sortColumn, RegistryValueKind.DWord);
                    regkey.SetValue("SortOrder", sortOrder, RegistryValueKind.DWord);
                }
                else
                {
                    exepath = regkey.GetValue("EXEdir").ToString().CheckTrail();
                    cfgpath = regkey.GetValue("CFGdir").ToString().CheckTrail();
                    minimize = Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart"));
                    showConsole = Convert.ToBoolean(regkey.GetValue("ShowConsole"));
                    minimizeTray = Convert.ToBoolean(regkey.GetValue("MinimizeToTray"));
                    closeTray = Convert.ToBoolean(regkey.GetValue("CloseToTray"));
                    logpath = regkey.GetValue("LogPath").ToString();
                    logging = Convert.ToBoolean(regkey.GetValue("EnableLogging"));
                    gridlines = Convert.ToBoolean(regkey.GetValue("EnableGridLines"));
                    sortColumn = (int)regkey.GetValue("SortColumn");
                    sortOrder = (SortType)regkey.GetValue("SortOrder");

                    // TODO lstVMs.EnableGridLines = gridlines ? TreeViewGridLines.Both : TreeViewGridLines.None;
                    VMCenter.Sort(sortColumn, sortOrder);
                }

                regkey.Close();
            }
            catch
            {
                Dialogs.ShowMessageBox(
                    "An error occured trying to load the 86Box Manager registry keys and/or values.\n" +
                    "Make sure you have the required permissions and try again.",
                    MessageType.Error, ButtonsType.Ok, "Error");
                Quit();
            }
        }

        // Load the VMs from the registry
        internal void LoadVMs()
        {
            lstVMs.ClearAll();
            VMCenter.CountRefresh();

            try
            {
                var regkey = Configs.Open86BoxVmKey();

                foreach (var value in regkey.GetValueNames())
                {
                    var vm = Serializer.Read<VM>(regkey.GetValue<byte[]>(value));
                    lstVMs.Insert(vm.Name, vm);
                }

                lstVMs.ClearSelect();
                btnStart.IsEnabled = false;
                btnPause.IsEnabled = false;
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnConfigure.IsEnabled = false;
                btnCtrlAltDel.IsEnabled = false;
                btnReset.IsEnabled = false;

                VMCenter.CountRefresh();
            }
            catch
            {
                Dialogs.ShowMessageBox("The Virtual Machines registry key could not be opened, so no " +
                                       "stored virtual machines can be used. Make sure you have the " +
                                       "required permissions and try again.",
                    MessageType.Error, ButtonsType.Ok, "Error");
            }
        }

        public string exepath = ""; //Path to 86box.exe and the romset
        public string cfgpath = ""; //Path to the virtual machines folder (configs, nvrs, etc.)
        internal bool minimize = false; //Minimize the main window when a VM is started?
        internal bool showConsole = true; //Show the console window when a VM is started?
        private bool minimizeTray = false; //Minimize the Manager window to tray icon?
        private bool closeTray = false; //Close the Manager Window to tray icon?
        internal string hWndHex = "";  //Window handle of this window
        internal int sortColumn = 0; //The column for sorting
        internal SortType sortOrder = SortType.Ascending; //Sorting order
        internal bool logging = false; //Logging enabled for 86Box.exe (-L parameter)?
        internal string logpath = ""; //Path to log file
        private bool gridlines = false; //Are grid lines enabled for VM list?

        private IMessageReceiver msgHandler;
        private IMessageLoop msgSink;

        private void btnConfigure_Click(object sender, RoutedEventArgs e)
        {
            VMCenter.Configure();
        }

        private void btnCtrlAltDel_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Window_OnClosing(object sender, CancelEventArgs e)
        {
            var cancelQuit = default(bool?);
            frmMain_FormClosing(e, ref cancelQuit);
            if (cancelQuit == true)
            {
                e.Cancel = true;
                return;
            }
            Quit();
        }

        private void Quit()
        {
            Environment.Exit(0);
        }

        // Closing 86Box Manager before closing all the VMs can lead to weirdness if 86Box Manager is then restarted. 
        // So let's warn the user just in case and request confirmation.
        private void frmMain_FormClosing(CancelEventArgs a, ref bool? cancel)
        {
            var vmCount = 0; //Number of running VMs

            //Close to tray
            if (closeTray)
            {
                cancel = true;
                // TODO trayIcon.MakeVisible(true);
                Hide();
            }
            else
            {
                foreach (var item in lstVMs.GetAllItems())
                {
                    var vm = item.Tag;
                    if (vm.Status != VM.STATUS_STOPPED && IsVisible)
                    {
                        vmCount++;
                    }
                }
            }

            //If there are running VMs, display the warning and stop the VMs if user says so
            if (vmCount > 0)
            {
                cancel = true;
                var result = Dialogs.ShowMessageBox("Some virtual machines are still running. It's " +
                                                    "recommended you stop them first before closing " +
                                                    "86Box Manager. Do you want to stop them now?",
                    MessageType.Warning, ButtonsType.YesNo, "Virtual machines are still running");
                if (result == ResponseType.Yes)
                {
                    foreach (var lvi in lstVMs.GetAllItems())
                    {
                        lstVMs.ClearSelect(); //To prevent weird stuff
                        var vm = lvi.Tag;
                        if (vm.Status != VM.STATUS_STOPPED)
                        {
                            lvi.Focused = true;
                            lvi.Selected = true;
                            // Tell the VM to shut down without confirmation
                            VMCenter.ForceStop(lstVMs.GetSelItems(), this);
                            var p = Process.GetProcessById(vm.Pid);
                            // Wait 500 milliseconds for each VM to close
                            p.WaitForExit(500);
                        }
                    }
                }
                else if (result == ResponseType.Cancel)
                {
                    return;
                }

                cancel = false;
            }
        }
    }
}