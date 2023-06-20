using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using _86BoxManager.API;
using _86BoxManager.Core;
using _86BoxManager.Model;
using _86BoxManager.Models;
using _86BoxManager.Registry;
using _86BoxManager.Tools;
using _86BoxManager.Xplat;
using Gdk;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using Window = Gtk.Window;
using IOPath = System.IO.Path;
using RegistryValueKind = _86BoxManager.Registry.ValueKind;
using SortType = Gtk.SortType;

namespace _86BoxManager.View
{
    internal sealed partial class frmMain : Window
    {
        public frmMain() : this(new Builder("frmMain.glade"))
        {
            InitializeComponent();
        }

        private frmMain(Builder builder) : base(builder.GetRawOwnedObject("frmMain"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            var cancelQuit = default(bool?);
            frmMain_FormClosing(a, ref cancelQuit);
            if (cancelQuit == true)
            {
                a.RetVal = true;
                return;
            }
            Application.Quit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.RunDialog(new dlgAddVM());
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.RunDialog(new dlgEditVM());
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            this.RunDialog(new dlgSettings(), () =>
            {
                //Reload the settings due to potential changes
                LoadSettings();
            });
        }
        
        private void btnStart_Click(object sender, EventArgs e)
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

        public VMRow GetFocusedVm()
        {
            var item = lstVMs.GetSelItems();
            return item.FirstOrDefault();
        }
        
        private void BringToFront()
        {
            Present();
        }
        
        private void open86BoxManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            BringToFront();
            trayIcon.MakeVisible(false);
        }
        
        private void trayIcon_MouseDoubleClick(object o, ButtonPressEventArgs args)
        {
            var e = args.Event;
            if (e.Type != EventType.TwoButtonPress)
                return;
            
            //Restore the window and hide the tray icon
            Show();
            BringToFront();
            trayIcon.MakeVisible(false);
        }
        
        private void OnTrayPopup(object o, PopupMenuArgs args)
        {
            cmsTrayIcon.ShowAll();
            cmsTrayIcon.Popup();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            BringToFront();
            trayIcon.MakeVisible(false);

            this.RunDialog(new dlgSettings(), () => LoadSettings());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
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
                var result = (ResponseType)Dialogs.ShowMessageBox("Some virtual machines are still running. " +
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
            Application.Quit();
        }
        
        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            VMCenter.Kill(selected, this);
        }
        
        private void wipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            VMCenter.Wipe(selected);
        }
        
        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            var vm = selected[0].Tag;

            this.RunDialog(new dlgCloneVM(vm.Path));
        }
        
        private void openConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            VMCenter.OpenConfig(selected);
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            VMCenter.Configure();
        }

        private void OnTreeButtonRelease(object o, ButtonReleaseEventArgs args)
        {
            var e = args.Event;
            if (e.Button != 3)
                return;

            var cancel = new CancelEventArgs();
            cmsVM_Opening(o, cancel);
            if (cancel.Cancel)
                return;

            var rect = new Rectangle((int)e.X, (int)e.Y + 24, 1, 1);
            lstVMpop.RelativeTo = lstVMs;
            lstVMpop.PointingTo = rect;
            lstVMpop.ShowAll();
            lstVMpop.Hide();
            lstVMpop.Popup();
        }

        public string CfgPath => cfgpath;

        private (WindowState ChangedMask, WindowState NewWindowState) LastState { get; set; }

        private void Window_StateChanged(object o, WindowStateEventArgs args)
        {
            var e = args.Event;
            LastState = (e.ChangedMask, e.NewWindowState);
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
                    Dialogs.ShowMessageBox("86Box Manager settings could not be loaded. This is normal" +
                                           " if you're running 86Box Manager for the first time. Default " +
                                           "values will be used.",
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

                    lstVMs.EnableGridLines = TreeViewGridLines.None;
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

                    lstVMs.EnableGridLines = gridlines ? TreeViewGridLines.Both : TreeViewGridLines.None;
                    VMCenter.Sort(sortColumn, sortOrder);
                }

                regkey.Close();
            }
            catch
            {
                Dialogs.ShowMessageBox("An error occured trying to load the 86Box Manager registry" +
                                       " keys and/or values. Make sure you have the required permissions" +
                                       " and try again.",
                    MessageType.Error, ButtonsType.Ok, "Error");
                Application.Quit();
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
                btnStart.Sensitive = false;
                btnPause.Sensitive = false;
                btnEdit.Sensitive = false;
                btnDelete.Sensitive = false;
                btnConfigure.Sensitive = false;
                btnCtrlAltDel.Sensitive = false;
                btnReset.Sensitive = false;

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
        
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
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
        
        private void hardResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMCenter.HardReset(lstVMs.GetSelItems());
        }
        
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMCenter.Remove(lstVMs.GetSelItems(),this);
        }
        
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RunDialog(new dlgEditVM());
        }
        
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = lstVMs.GetSelItems();
            VMCenter.OpenFolder(selected);
        }
        
        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMCenter.Configure();
        }
        
        private void resetCTRLALTDELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMCenter.CtrlAltDel(lstVMs.GetSelItems(), this);
        }
        
        // Start VM if it's stopped or stop it if it's running/paused
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void createADesktopShortcutToolStripMenuItem_Click(object sender, EventArgs e)
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
        
        // Starts/stops selected VM when enter is pressed
        private void lstVMs_KeyDown(object o, KeyReleaseEventArgs args)
        {
            var e = args.Event;
            var isEnter = e.Key is Gdk.Key.Return or Gdk.Key.KP_Enter;
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
            var isDelete = e.Key is Gdk.Key.Delete or Gdk.Key.KP_Delete;
            if (isDelete && lstVMs.GetSelItems().Count == 1)
            {
                VMCenter.Remove(lstVMs.GetSelItems(), this);
            }
        }
        
        private void btnCtrlAltDel_Click(object sender, EventArgs e)
        {
            VMCenter.CtrlAltDel(lstVMs.GetSelItems(), this);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            VMCenter.HardReset(lstVMs.GetSelItems());
        }

        private void btnPause_Click(object sender, EventArgs e)
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
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            VMCenter.Remove(lstVMs.GetSelItems(),this);
        }
        
        // For double clicking an item, do something based on VM status
        private void lstVMs_MouseDoubleClick(object o, ButtonPressEventArgs args)
        {
            var e = args.Event;
            if (e.Button == 1 && e.Type == EventType.TwoButtonPress)
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
        
        // Closing 86Box Manager before closing all the VMs can lead to weirdness if 86Box Manager is then restarted. 
        // So let's warn the user just in case and request confirmation.
        private void frmMain_FormClosing(DeleteEventArgs a, ref bool? cancel)
        {
            var e = a.Event;
            var vmCount = 0; //Number of running VMs

            //Close to tray
            if (e.Type == EventType.Delete && closeTray)
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
                    if (vm.Status != VM.STATUS_STOPPED && Visible)
                    {
                        vmCount++;
                    }
                }
            }

            //If there are running VMs, display the warning and stop the VMs if user says so
            if (vmCount > 0)
            {
                cancel = true;
                var result = (ResponseType)Dialogs.ShowMessageBox("Some virtual machines are still running. It's " +
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
                        startToolStripMenuItem.Text = "Stop";
                        startToolStripMenuItem.Sensitive = true;
                        startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                        editToolStripMenuItem.Sensitive = false;
                        deleteToolStripMenuItem.Sensitive = false;
                        hardResetToolStripMenuItem.Sensitive = true;
                        resetCTRLALTDELETEToolStripMenuItem.Sensitive = true;
                        pauseToolStripMenuItem.Sensitive = true;
                        pauseToolStripMenuItem.Text = "Pause";
                        configureToolStripMenuItem.Sensitive = true;
                        break;
                    case VM.STATUS_STOPPED:
                        startToolStripMenuItem.Text = "Start";
                        startToolStripMenuItem.Sensitive = true;
                        startToolStripMenuItem.SetToolTip("Start this virtual machine");
                        editToolStripMenuItem.Sensitive = true;
                        deleteToolStripMenuItem.Sensitive = true;
                        hardResetToolStripMenuItem.Sensitive = false;
                        resetCTRLALTDELETEToolStripMenuItem.Sensitive = false;
                        pauseToolStripMenuItem.Sensitive = false;
                        pauseToolStripMenuItem.Text = "Pause";
                        configureToolStripMenuItem.Sensitive = true;
                        break;
                    case VM.STATUS_WAITING:
                        startToolStripMenuItem.Sensitive = false;
                        startToolStripMenuItem.Text = "Stop";
                        startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                        editToolStripMenuItem.Sensitive = false;
                        deleteToolStripMenuItem.Sensitive = false;
                        hardResetToolStripMenuItem.Sensitive = false;
                        resetCTRLALTDELETEToolStripMenuItem.Sensitive = false;
                        pauseToolStripMenuItem.Sensitive = false;
                        pauseToolStripMenuItem.Text = "Pause";
                        pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                        configureToolStripMenuItem.Sensitive = false;
                        break;
                    case VM.STATUS_PAUSED:
                        startToolStripMenuItem.Sensitive = true;
                        startToolStripMenuItem.Text = "Stop";
                        startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                        editToolStripMenuItem.Sensitive = false;
                        deleteToolStripMenuItem.Sensitive = false;
                        hardResetToolStripMenuItem.Sensitive = true;
                        resetCTRLALTDELETEToolStripMenuItem.Sensitive = true;
                        pauseToolStripMenuItem.Sensitive = true;
                        pauseToolStripMenuItem.Text = "Resume";
                        pauseToolStripMenuItem.SetToolTip("Resume this virtual machine");
                        configureToolStripMenuItem.Sensitive = true;
                        break;
                }
                return;
            }

            // Multiple VMs selected => disable most options
            startToolStripMenuItem.Text = "Start";
            startToolStripMenuItem.Sensitive = false;
            startToolStripMenuItem.SetToolTip("Start this virtual machine");
            editToolStripMenuItem.Sensitive = false;
            deleteToolStripMenuItem.Sensitive = true;
            hardResetToolStripMenuItem.Sensitive = false;
            resetCTRLALTDELETEToolStripMenuItem.Sensitive = false;
            pauseToolStripMenuItem.Sensitive = false;
            pauseToolStripMenuItem.Text = "Pause";
            killToolStripMenuItem.Sensitive = true;
            configureToolStripMenuItem.Sensitive = false;
            cloneToolStripMenuItem.Sensitive = false;
        }

        private void lstVMs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var select = (TreeSelection)sender;
            var selected = select.GetSelItems();

            // Disable relevant buttons if no VM is selected
            if (selected.Count == 0)
            {
                btnConfigure.Sensitive = false;
                btnStart.Sensitive = false;
                btnEdit.Sensitive = false;
                btnDelete.Sensitive = false;
                btnReset.Sensitive = false;
                btnCtrlAltDel.Sensitive = false;
                btnPause.Sensitive = false;
                return;
            }

            if (selected.Count == 1)
            {
                //Disable relevant buttons if VM is running
                var vm = selected[0].Tag;
                if (vm.Status == VM.STATUS_RUNNING)
                {
                    btnStart.Sensitive = true;
                    btnStart.Label = "Stop";
                    btnStart.SetToolTip("Stop this virtual machine");
                    btnEdit.Sensitive = false;
                    btnDelete.Sensitive = false;
                    btnConfigure.Sensitive = true;
                    btnPause.Sensitive = true;
                    btnPause.Label = "Pause";
                    btnReset.Sensitive = true;
                    btnCtrlAltDel.Sensitive = true;
                }
                else if (vm.Status == VM.STATUS_STOPPED)
                {
                    btnStart.Sensitive = true;
                    btnStart.Label = "Start";
                    btnStart.SetToolTip("Start this virtual machine");
                    btnEdit.Sensitive = true;
                    btnDelete.Sensitive = true;
                    btnConfigure.Sensitive = true;
                    btnPause.Sensitive = false;
                    btnPause.Label = "Pause";
                    btnReset.Sensitive = false;
                    btnCtrlAltDel.Sensitive = false;
                }
                else if (vm.Status == VM.STATUS_PAUSED)
                {
                    btnStart.Sensitive = true;
                    btnStart.Label = "Stop";
                    btnStart.SetToolTip("Stop this virtual machine");
                    btnEdit.Sensitive = false;
                    btnDelete.Sensitive = false;
                    btnConfigure.Sensitive = true;
                    btnPause.Sensitive = true;
                    btnPause.Label = "Resume";
                    btnReset.Sensitive = true;
                    btnCtrlAltDel.Sensitive = true;
                }
                else if (vm.Status == VM.STATUS_WAITING)
                {
                    btnStart.Sensitive = false;
                    btnStart.Label = "Stop";
                    btnStart.SetToolTip("Stop this virtual machine");
                    btnEdit.Sensitive = false;
                    btnDelete.Sensitive = false;
                    btnReset.Sensitive = false;
                    btnCtrlAltDel.Sensitive = false;
                    btnPause.Sensitive = false;
                    btnPause.Label = "Pause";
                    btnConfigure.Sensitive = false;
                }
                return;
            }

            btnConfigure.Sensitive = false;
            btnStart.Sensitive = false;
            btnEdit.Sensitive = false;
            btnDelete.Sensitive = true;
            btnReset.Sensitive = false;
            btnCtrlAltDel.Sensitive = false;
            btnPause.Sensitive = false;
        }

        // Handles things when WindowState changes
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (LastState.NewWindowState == WindowState.Iconified && minimizeTray)
            {
                trayIcon.MakeVisible(true);
                Hide();
                return;
            }
            if (LastState.NewWindowState == default)
            {
                Show();
                trayIcon.MakeVisible(false);
            }
        }

        // Handles the click event for the listview column headers, allowing to sort the items by columns
        private void lstVMs_ColumnClick(object sender, EventArgs e)
        {
            var source = (TreeViewColumn)sender;
            var column = source.SortColumnId;
            var order = source.SortOrder;

            VMCenter.Sort(column, order);
        }
    }
}