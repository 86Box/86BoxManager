using System;
using _86boxManager.Models;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using _86BoxManager.API;
using _86boxManager.Core;
using _86boxManager.Tools;
using _86boxManager.ViewModels;
using IOPath = System.IO.Path;
using RegistryValueKind = _86BoxManager.Registry.ValueKind;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;
using System.Threading;
using _86BoxManager.Core;
using _86BoxManager.Model;
using _86BoxManager.Registry;
using _86BoxManager.Xplat;
using Avalonia;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace _86boxManager.Views
{
    public partial class frmMain : Window
    {
        public frmMain()
        {
            InitializeComponent();
        }

        internal MainModel Model => (MainModel)DataContext;

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
            PrepareUi();
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
                var lvi = lstVMs.FindItemWithText(invVmName);

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

        public string CfgPath => cfgpath;

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

                    lstVMs.EnableGridLines(false);
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

                    lstVMs.EnableGridLines(gridlines);
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
        internal string hWndHex = ""; //Window handle of this window
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
                trayIcon.MakeVisible(true);
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

        private void btnCtrlAltDel_Click(object sender, RoutedEventArgs e)
        {
            VMCenter.CtrlAltDel(lstVMs.GetSelItems(), this);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            VMCenter.HardReset(lstVMs.GetSelItems());
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            var vm = selected[0].Tag;
            if (vm.Status == VM.STATUS_PAUSED)
            {
                VMCenter.Resume(lstVMs.GetSelItems(), this);
            }
            else if (vm.Status == VM.STATUS_RUNNING)
            {
                VMCenter.Pause(lstVMs.GetSelItems(), this);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            VMCenter.Remove(lstVMs.GetSelItems(), this);
        }

        private void openConfigFileToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            VMCenter.OpenConfig(selected);
        }

        private void killToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            VMCenter.Kill(selected, this);
        }

        private void wipeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            VMCenter.Wipe(selected);
        }

        private async void cloneToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            var vm = selected[0].Tag;

            await this.RunDialog(new dlgCloneVM(vm.Path));
        }

        private void pauseToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var vm = lstVMs.GetSelItems()[0].Tag;
            if (vm.Status == VM.STATUS_PAUSED)
            {
                VMCenter.Resume(lstVMs.GetSelItems(), this);
            }
            else if (vm.Status == VM.STATUS_RUNNING)
            {
                VMCenter.Pause(lstVMs.GetSelItems(), this);
            }
        }

        private void hardResetToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            VMCenter.HardReset(lstVMs.GetSelItems());
        }

        private void deleteToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            VMCenter.Remove(lstVMs.GetSelItems(), this);
        }

        private async void editToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await this.RunDialog(new dlgEditVM());
        }

        private void openFolderToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            VMCenter.OpenFolder(selected);
        }

        private void configureToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            VMCenter.Configure();
        }

        private void resetCTRLALTDELETEToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            VMCenter.CtrlAltDel(lstVMs.GetSelItems(), this);
        }

        // Start VM if it's stopped or stop it if it's running/paused
        private void startToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            var vm = selected[0].Tag;
            if (vm.Status == VM.STATUS_STOPPED)
            {
                VMCenter.Start();
            }
            else if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                VMCenter.RequestStop(lstVMs.GetSelItems(), this);
            }
        }

        private void createADesktopShortcutToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            foreach (var lvi in selected)
            {
                var vm = lvi.Tag;
                try
                {
                    var desktop = Platforms.Env.Desktop;
                    var shortcutAddress = IOPath.Combine(desktop, $"{vm.Name}.lnk");
                    var shortcutDesc = vm.Desc;
                    var vmName = vm.Name;
                    var startupPath = CurrentApp.StartupPath;

                    Platforms.Shell.CreateShortcut(shortcutAddress, vmName, shortcutDesc, startupPath);

                    Dialogs.ShowMessageBox($@"A desktop shortcut for the virtual machine ""{vm.Name}"" " +
                                           "was successfully created.",
                        MessageType.Info, ButtonsType.Ok, "Success");
                }
                catch
                {
                    Dialogs.ShowMessageBox($@"A desktop shortcut for the virtual machine ""{vm.Name}"" could" +
                                           " not be created.",
                        MessageType.Error, ButtonsType.Ok, "Error");
                }
            }
        }

        internal void open86BoxManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            BringToFront();
            trayIcon.MakeVisible(false);
        }

        internal async void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            BringToFront();
            trayIcon.MakeVisible(false);

            await this.RunDialog(new dlgSettings(), () => LoadSettings());
        }

        internal void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var vmCount = 0;
            foreach (var item in lstVMs.GetAllItems())
            {
                var vm = item.Tag;
                if (vm.Status != VM.STATUS_STOPPED)
                {
                    vmCount++;
                }
            }

            // If there are running VMs, display the warning and stop the VMs if user says so
            if (vmCount > 0)
            {
                var result = Dialogs.ShowMessageBox("Some virtual machines are still running. " +
                                                    "It's recommended you stop them first before " +
                                                    "closing 86Box Manager. Do you want to stop them now?",
                    MessageType.Warning, ButtonsType.YesNo, "Virtual machines are still running");
                if (result == ResponseType.Yes)
                {
                    foreach (var lvi in lstVMs.GetAllItems())
                    {
                        var vm = lvi.Tag;
                        lstVMs.ClearSelect();
                        if (vm.Status != VM.STATUS_STOPPED)
                        {
                            lvi.Focused = true;
                            lvi.Selected = true;

                            // Tell the VMs to stop without asking for user confirmation
                            VMCenter.ForceStop(lstVMs.GetSelItems(), this);
                        }
                    }

                    // Wait just a bit to make sure everything goes as planned
                    Thread.Sleep(vmCount * 500);
                }
                else if (result == ResponseType.Cancel)
                {
                    return;
                }
            }
            Quit();
        }

        // Handles things when WindowState changes
        protected override void HandleWindowStateChanged(WindowState state)
        {
            base.HandleWindowStateChanged(state);

            if (state == WindowState.Minimized && minimizeTray)
            {
                trayIcon.MakeVisible(true);
                Hide();
                return;
            }

            if (state == WindowState.Normal)
            {
                Show();
                trayIcon.MakeVisible(false);
            }
        }

        private DataGridColumn clmIcon;
        private DataGridColumn clmName;
        private DataGridColumn clmStatus;
        private DataGridColumn clmDesc;
        private DataGridColumn clmPath;
        private TrayIcon trayIcon;
        private NativeMenu cmsTrayIcon;

        private void PrepareUi()
        {
            var columns = lstVMs.Columns;
            clmIcon = columns[0];
            clmName = columns[1];
            clmStatus = columns[2];
            clmDesc = columns[3];
            clmPath = columns[4];

            clmIcon.PropertyChanged += lstVMs_ColumnClick;
            clmName.PropertyChanged += lstVMs_ColumnClick;
            clmStatus.PropertyChanged += lstVMs_ColumnClick;
            clmDesc.PropertyChanged += lstVMs_ColumnClick;
            clmPath.PropertyChanged += lstVMs_ColumnClick;

            var app = Application.Current;
            trayIcon = app?.GetValue(TrayIcon.IconsProperty).FirstOrDefault();
            if (trayIcon is { Menu: { } })
            {
                cmsTrayIcon = trayIcon.Menu;
            }
        }

        // Handles the click event for the listview column headers, allowing to sort the items by columns
        private void lstVMs_ColumnClick(object sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            // TODO Sorting?!
            // var source = (TreeViewColumn)sender;
            // var column = source.SortColumnId;
            // var order = source.SortOrder;
            // VMCenter.Sort(column, order);
        }

        internal void trayIcon_MouseClick(object sender, EventArgs e)
        {
            if (IsVisible)
                return;

            //Restore the window and hide the tray icon
            Show();
            BringToFront();
            trayIcon.MakeVisible(false);
        }

        private void lstVMs_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            var select = (DataGrid)sender;
            var selected = select.GetSelItems();

            // Disable relevant buttons if no VM is selected
            if (selected.Count == 0)
            {
                btnConfigure.IsEnabled = false;
                btnStart.IsEnabled = false;
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnReset.IsEnabled = false;
                btnCtrlAltDel.IsEnabled = false;
                btnPause.IsEnabled = false;
                return;
            }

            if (selected.Count == 1)
            {
                //Disable relevant buttons if VM is running
                var vm = selected[0].Tag;
                if (vm.Status == VM.STATUS_RUNNING)
                {
                    btnStart.IsEnabled = true;
                    btnStart.Content = "Stop";
                    btnStart.SetToolTip("Stop this virtual machine");
                    btnEdit.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnConfigure.IsEnabled = true;
                    btnPause.IsEnabled = true;
                    btnPause.Content = "Pause";
                    btnReset.IsEnabled = true;
                    btnCtrlAltDel.IsEnabled = true;
                }
                else if (vm.Status == VM.STATUS_STOPPED)
                {
                    btnStart.IsEnabled = true;
                    btnStart.Content = "Start";
                    btnStart.SetToolTip("Start this virtual machine");
                    btnEdit.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                    btnConfigure.IsEnabled = true;
                    btnPause.IsEnabled = false;
                    btnPause.Content = "Pause";
                    btnReset.IsEnabled = false;
                    btnCtrlAltDel.IsEnabled = false;
                }
                else if (vm.Status == VM.STATUS_PAUSED)
                {
                    btnStart.IsEnabled = true;
                    btnStart.Content = "Stop";
                    btnStart.SetToolTip("Stop this virtual machine");
                    btnEdit.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnConfigure.IsEnabled = true;
                    btnPause.IsEnabled = true;
                    btnPause.Content = "Resume";
                    btnReset.IsEnabled = true;
                    btnCtrlAltDel.IsEnabled = true;
                }
                else if (vm.Status == VM.STATUS_WAITING)
                {
                    btnStart.IsEnabled = false;
                    btnStart.Content = "Stop";
                    btnStart.SetToolTip("Stop this virtual machine");
                    btnEdit.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnReset.IsEnabled = false;
                    btnCtrlAltDel.IsEnabled = false;
                    btnPause.IsEnabled = false;
                    btnPause.Content = "Pause";
                    btnConfigure.IsEnabled = false;
                }
                return;
            }

            btnConfigure.IsEnabled = false;
            btnStart.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = true;
            btnReset.IsEnabled = false;
            btnCtrlAltDel.IsEnabled = false;
            btnPause.IsEnabled = false;
        }

        // Starts/stops selected VM when enter is pressed
        private void lstVMs_KeyDown(object o, KeyEventArgs e)
        {
            var isEnter = e.Key is Key.Return or Key.Enter;
            if (isEnter && lstVMs.GetSelItems().Count == 1)
            {
                var vm = lstVMs.GetSelItems()[0].Tag;
                if (vm.Status == VM.STATUS_RUNNING)
                {
                    VMCenter.RequestStop(lstVMs.GetSelItems(), this);
                }
                else if (vm.Status == VM.STATUS_STOPPED)
                {
                    VMCenter.Start();
                }
            }
            var isDelete = e.Key is Key.Delete;
            if (isDelete && lstVMs.GetSelItems().Count == 1)
            {
                VMCenter.Remove(lstVMs.GetSelItems(), this);
            }
        }

        // For double clicking an item, do something based on VM status
        private void lstVMs_MouseDoubleClick(object o, DataGridCellPointerPressedEventArgs args)
        {
            var e = args.PointerPressedEventArgs;
            var point = e.GetCurrentPoint((IVisual)o);
            if (point.Properties.IsLeftButtonPressed && e.ClickCount == 2)
            {
                var item = lstVMs.GetSelItems()[0];
                if (item != null)
                {
                    var vm = lstVMs.GetSelItems()[0].Tag;
                    if (vm.Status == VM.STATUS_STOPPED)
                    {
                        VMCenter.Start();
                    }
                    else if (vm.Status == VM.STATUS_RUNNING)
                    {
                        VMCenter.RequestStop(lstVMs.GetSelItems(), this);
                    }
                    else if (vm.Status == VM.STATUS_PAUSED)
                    {
                        VMCenter.Resume(lstVMs.GetSelItems(), this);
                    }
                }
            }
        }

        private void OnTreeButtonRelease(object sender, PointerReleasedEventArgs args)
        {
            var point = args.GetCurrentPoint((IVisual)sender);
            if (point.Properties.IsRightButtonPressed)
                return;

            var cancel = new CancelEventArgs();
            cmsVM_Opening(sender, cancel);
            if (cancel.Cancel)
                return;
        }

        // Enable/disable relevant menu items depending on selected VM's status
        private void cmsVM_Opening(object sender, CancelEventArgs e)
        {
            var selected = lstVMs.GetSelItems();

            // Available menu option differs based on the number of selected VMs
            if (selected.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            if (selected.Count == 1)
            {
                var vm = selected[0].Tag;
                switch (vm.Status)
                {
                    case VM.STATUS_RUNNING:
                        startToolStripMenuItem.Header = "Stop";
                        startToolStripMenuItem.IsEnabled = true;
                        startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                        editToolStripMenuItem.IsEnabled = false;
                        deleteToolStripMenuItem.IsEnabled = false;
                        hardResetToolStripMenuItem.IsEnabled = true;
                        resetCTRLALTDELETEToolStripMenuItem.IsEnabled = true;
                        pauseToolStripMenuItem.IsEnabled = true;
                        pauseToolStripMenuItem.Header = "Pause";
                        configureToolStripMenuItem.IsEnabled = true;
                        break;
                    case VM.STATUS_STOPPED:
                        startToolStripMenuItem.Header = "Start";
                        startToolStripMenuItem.IsEnabled = true;
                        startToolStripMenuItem.SetToolTip("Start this virtual machine");
                        editToolStripMenuItem.IsEnabled = true;
                        deleteToolStripMenuItem.IsEnabled = true;
                        hardResetToolStripMenuItem.IsEnabled = false;
                        resetCTRLALTDELETEToolStripMenuItem.IsEnabled = false;
                        pauseToolStripMenuItem.IsEnabled = false;
                        pauseToolStripMenuItem.Header = "Pause";
                        configureToolStripMenuItem.IsEnabled = true;
                        break;
                    case VM.STATUS_WAITING:
                        startToolStripMenuItem.IsEnabled = false;
                        startToolStripMenuItem.Header = "Stop";
                        startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                        editToolStripMenuItem.IsEnabled = false;
                        deleteToolStripMenuItem.IsEnabled = false;
                        hardResetToolStripMenuItem.IsEnabled = false;
                        resetCTRLALTDELETEToolStripMenuItem.IsEnabled = false;
                        pauseToolStripMenuItem.IsEnabled = false;
                        pauseToolStripMenuItem.Header = "Pause";
                        pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                        configureToolStripMenuItem.IsEnabled = false;
                        break;
                    case VM.STATUS_PAUSED:
                        startToolStripMenuItem.IsEnabled = true;
                        startToolStripMenuItem.Header = "Stop";
                        startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                        editToolStripMenuItem.IsEnabled = false;
                        deleteToolStripMenuItem.IsEnabled = false;
                        hardResetToolStripMenuItem.IsEnabled = true;
                        resetCTRLALTDELETEToolStripMenuItem.IsEnabled = true;
                        pauseToolStripMenuItem.IsEnabled = true;
                        pauseToolStripMenuItem.Header = "Resume";
                        pauseToolStripMenuItem.SetToolTip("Resume this virtual machine");
                        configureToolStripMenuItem.IsEnabled = true;
                        break;
                }
                return;
            }

            // Multiple VMs selected => disable most options
            startToolStripMenuItem.Header = "Start";
            startToolStripMenuItem.IsEnabled = false;
            startToolStripMenuItem.SetToolTip("Start this virtual machine");
            editToolStripMenuItem.IsEnabled = false;
            deleteToolStripMenuItem.IsEnabled = true;
            hardResetToolStripMenuItem.IsEnabled = false;
            resetCTRLALTDELETEToolStripMenuItem.IsEnabled = false;
            pauseToolStripMenuItem.IsEnabled = false;
            pauseToolStripMenuItem.Header = "Pause";
            killToolStripMenuItem.IsEnabled = true;
            configureToolStripMenuItem.IsEnabled = false;
            cloneToolStripMenuItem.IsEnabled = false;
        }
    }
}