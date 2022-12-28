using _86boxManager.Properties;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace _86boxManager
{
    public partial class frmMain : Form
    {
        //Win32 API imports
        //Posts a message to the window with specified handle - DOES NOT WAIT FOR THE RECIPIENT TO PROCESS THE MESSAGE!!!
        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        //Focus a window
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        //private static RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true); //Registry key for accessing the settings and VM list
        public string exepath = ""; //Path to 86box.exe and the romset
        public string cfgpath = ""; //Path to the virtual machines folder (configs, nvrs, etc.)
        private bool minimize = false; //Minimize the main window when a VM is started?
        private bool showConsole = true; //Show the console window when a VM is started?
        private bool minimizeTray = false; //Minimize the Manager window to tray icon?
        private bool closeTray = false; //Close the Manager Window to tray icon?
        private string hWndHex = "";  //Window handle of this window  
        private int sortColumn = 0; //The column for sorting
        private SortOrder sortOrder = SortOrder.Ascending; //Sorting order
        private bool logging = false; //Logging enabled for 86Box.exe (-L parameter)?
        private string logpath = ""; //Path to log file
        private bool gridlines = false; //Are grid lines enabled for VM list?

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadSettings();
            LoadVMs();

            //Load main window's state, size and position
            //BUGBUG: Restoring these causes anchor problems, so we're not doing it anymore...
            /*WindowState = Settings.Default.WindowState;
            Size = Settings.Default.WindowSize;
            Location = Settings.Default.WindowPosition;*/

            //Load listview column widths
            clmName.Width = Settings.Default.NameColWidth;
            clmStatus.Width = Settings.Default.StatusColWidth;
            clmDesc.Width = Settings.Default.DescColWidth;
            clmPath.Width = Settings.Default.PathColWidth;

            //Convert the current window handle to a form that's expected by 86Box
            hWndHex = string.Format("{0:X}", Handle.ToInt64());
            hWndHex = hWndHex.PadLeft(16, '0');

            //Check if command line arguments for starting a VM are OK
            if (Program.args.Length == 3 && Program.args[1] == "-S" && Program.args[2] != null)
            {
                //Find the VM with given name
                ListViewItem lvi = lstVMs.FindItemWithText(Program.args[2], false, 0, false);

                //Then select and start it if it's found
                if (lvi != null)
                {
                    lvi.Focused = true;
                    lvi.Selected = true;
                    VMStart();
                }
                else
                {
                    MessageBox.Show("The virtual machine \"" + Program.args[2] + "\" could not be found. It may have been removed or the specified name is incorrect.", "Virtual machine not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            if (vm.Status == VM.STATUS_STOPPED)
            {
                VMStart();
            }
            else if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                VMRequestStop();
            }
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            VMConfigure();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            dlgSettings dlg = new dlgSettings();
            dlg.ShowDialog();
            LoadSettings(); //Reload the settings due to potential changes    
            dlg.Dispose();
        }

        private void lstVMs_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Disable relevant buttons if no VM is selected
            if (lstVMs.SelectedItems.Count == 0)
            {
                btnConfigure.Enabled = false;
                btnStart.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnReset.Enabled = false;
                btnCtrlAltDel.Enabled = false;
                btnPause.Enabled = false;
            }
            else if (lstVMs.SelectedItems.Count == 1)
            {
                //Disable relevant buttons if VM is running
                VM vm = (VM)lstVMs.SelectedItems[0].Tag;
                if (vm.Status == VM.STATUS_RUNNING)
                {
                    btnStart.Enabled = true;
                    btnStart.Text = "Stop";
                    toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnConfigure.Enabled = true;
                    btnPause.Enabled = true;
                    btnPause.Text = "Pause";
                    btnReset.Enabled = true;
                    btnCtrlAltDel.Enabled = true;
                }
                else if (vm.Status == VM.STATUS_STOPPED)
                {
                    btnStart.Enabled = true;
                    btnStart.Text = "Start";
                    toolTip.SetToolTip(btnStart, "Start this virtual machine");
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnConfigure.Enabled = true;
                    btnPause.Enabled = false;
                    btnPause.Text = "Pause";
                    btnReset.Enabled = false;
                    btnCtrlAltDel.Enabled = false;
                }
                else if (vm.Status == VM.STATUS_PAUSED)
                {
                    btnStart.Enabled = true;
                    btnStart.Text = "Stop";
                    toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnConfigure.Enabled = true;
                    btnPause.Enabled = true;
                    btnPause.Text = "Resume";
                    btnReset.Enabled = true;
                    btnCtrlAltDel.Enabled = true;
                }
                else if (vm.Status == VM.STATUS_WAITING)
                {
                    btnStart.Enabled = false;
                    btnStart.Text = "Stop";
                    toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnReset.Enabled = false;
                    btnCtrlAltDel.Enabled = false;
                    btnPause.Enabled = false;
                    btnPause.Text = "Pause";
                    btnConfigure.Enabled = false;
                }
            }
            else
            {
                btnConfigure.Enabled = false;
                btnStart.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = true;
                btnReset.Enabled = false;
                btnCtrlAltDel.Enabled = false;
                btnPause.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dlgAddVM dlg = new dlgAddVM();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dlgEditVM dlg = new dlgEditVM();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        //Load the settings from the registry
        private void LoadSettings()
        {
            try
            {
                //Try to load the settings from registry, if it fails fallback to default values
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);

                if (regkey == null)
                {
                    MessageBox.Show("86Box Manager settings could not be loaded. This is normal if you're running 86Box Manager for the first time. Default values will be used.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //Create the key and reopen it for write access
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\86Box");
                    regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
                    regkey.CreateSubKey("Virtual Machines");

                    cfgpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\86Box VMs\";
                    exepath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\86Box\";
                    minimize = false;
                    showConsole = true;
                    minimizeTray = false;
                    closeTray = false;
                    logging = false;
                    logpath = "";
                    gridlines = false;
                    sortColumn = 0;
                    sortOrder = SortOrder.Ascending;

                    lstVMs.GridLines = false;
                    VMSort(sortColumn, sortOrder);

                    //Defaults must also be written to the registry
                    regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
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
                    exepath = regkey.GetValue("EXEdir").ToString();
                    cfgpath = regkey.GetValue("CFGdir").ToString();
                    minimize = Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart"));
                    showConsole = Convert.ToBoolean(regkey.GetValue("ShowConsole"));
                    minimizeTray = Convert.ToBoolean(regkey.GetValue("MinimizeToTray"));
                    closeTray = Convert.ToBoolean(regkey.GetValue("CloseToTray"));
                    logpath = regkey.GetValue("LogPath").ToString();
                    logging = Convert.ToBoolean(regkey.GetValue("EnableLogging"));
                    gridlines = Convert.ToBoolean(regkey.GetValue("EnableGridLines"));
                    sortColumn = (int)regkey.GetValue("SortColumn");
                    sortOrder = (SortOrder)regkey.GetValue("SortOrder");

                    lstVMs.GridLines = gridlines;
                    VMSort(sortColumn, sortOrder);
                }

                regkey.Close();

                //To make sure there's a trailing backslash at the end, as other code using these strings expects it!
                if (!exepath.EndsWith(@"\"))
                {
                    exepath += @"\";
                }
                if (!cfgpath.EndsWith(@"\"))
                {
                    cfgpath += @"\";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured trying to load the 86Box Manager registry keys and/or values. Make sure you have the required permissions and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                
                Application.Exit();
            }
        }

        //TODO: Rewrite
        //Load the VMs from the registry
        private void LoadVMs()
        {
            lstVMs.Items.Clear();
            VMCountRefresh();
            try
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines");
                VM vm = new VM();

                foreach (var value in regkey.GetValueNames())
                {
                    MemoryStream ms = new MemoryStream((byte[])regkey.GetValue(value));
                    BinaryFormatter bf = new BinaryFormatter();
                    vm = (VM)bf.Deserialize(ms);
                    ms.Close();

                    ListViewItem newLvi = new ListViewItem(vm.Name)
                    {
                        Tag = vm,
                        ImageIndex = 0
                    };
                    newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, vm.GetStatusString()));
                    newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, vm.Desc));
                    newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, vm.Path));
                    lstVMs.Items.Add(newLvi);
                }

                lstVMs.SelectedItems.Clear();
                btnStart.Enabled = false;
                btnPause.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnConfigure.Enabled = false;
                btnCtrlAltDel.Enabled = false;
                btnReset.Enabled = false;

                VMCountRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Virtual Machines registry key could not be opened, so no stored virtual machines can be used. Make sure you have the required permissions and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Wait for the associated window of a VM to close
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            VM vm = e.Argument as VM;
            try
            {
                Process p = Process.GetProcessById(vm.Pid); //Find the process associated with the VM
                p.WaitForExit(); //Wait for it to exit
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred. Please provide the following details to the developer:\n" + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            e.Result = vm;
        }

        //Update the UI once the VM's window is closed
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            VM vm = e.Result as VM;

            //Go through the listview, find the item representing the VM and update things accordingly
            foreach (ListViewItem item in lstVMs.Items)
            {
                if (item.Tag.Equals(vm))
                {
                    vm.Status = VM.STATUS_STOPPED;
                    vm.hWnd = IntPtr.Zero;
                    item.SubItems[1].Text = vm.GetStatusString();
                    item.ImageIndex = 0;
                    if (lstVMs.SelectedItems.Count > 0 && lstVMs.SelectedItems[0].Equals(item))
                    {
                        btnEdit.Enabled = true;
                        btnDelete.Enabled = true;
                        btnStart.Enabled = true;
                        btnStart.Text = "Start";
                        toolTip.SetToolTip(btnStart, "Start this virtual machine");
                        btnConfigure.Enabled = true;
                        btnPause.Enabled = false;
                        btnPause.Text = "Pause";
                        btnCtrlAltDel.Enabled = false;
                        btnReset.Enabled = false;
                    }
                }
            }

            VMCountRefresh();
        }

        //Enable/disable relevant menu items depending on selected VM's status
        private void cmsVM_Opening(object sender, CancelEventArgs e)
        {
            //Available menu option differs based on the number of selected VMs
            if (lstVMs.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
            else if (lstVMs.SelectedItems.Count == 1)
            {
                VM vm = (VM)lstVMs.SelectedItems[0].Tag;
                switch (vm.Status)
                {
                    case VM.STATUS_RUNNING:
                        {
                            startToolStripMenuItem.Text = "Stop";
                            startToolStripMenuItem.Enabled = true;
                            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
                            editToolStripMenuItem.Enabled = false;
                            deleteToolStripMenuItem.Enabled = false;
                            hardResetToolStripMenuItem.Enabled = true;
                            resetCTRLALTDELETEToolStripMenuItem.Enabled = true;
                            pauseToolStripMenuItem.Enabled = true;
                            pauseToolStripMenuItem.Text = "Pause";
                            configureToolStripMenuItem.Enabled = true;
                        }
                        break;
                    case VM.STATUS_STOPPED:
                        {
                            startToolStripMenuItem.Text = "Start";
                            startToolStripMenuItem.Enabled = true;
                            startToolStripMenuItem.ToolTipText = "Start this virtual machine";
                            editToolStripMenuItem.Enabled = true;
                            deleteToolStripMenuItem.Enabled = true;
                            hardResetToolStripMenuItem.Enabled = false;
                            resetCTRLALTDELETEToolStripMenuItem.Enabled = false;
                            pauseToolStripMenuItem.Enabled = false;
                            pauseToolStripMenuItem.Text = "Pause";
                            configureToolStripMenuItem.Enabled = true;
                        }
                        break;
                    case VM.STATUS_WAITING:
                        {
                            startToolStripMenuItem.Enabled = false;
                            startToolStripMenuItem.Text = "Stop";
                            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
                            editToolStripMenuItem.Enabled = false;
                            deleteToolStripMenuItem.Enabled = false;
                            hardResetToolStripMenuItem.Enabled = false;
                            resetCTRLALTDELETEToolStripMenuItem.Enabled = false;
                            pauseToolStripMenuItem.Enabled = false;
                            pauseToolStripMenuItem.Text = "Pause";
                            pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
                            configureToolStripMenuItem.Enabled = false;
                        }
                        break;
                    case VM.STATUS_PAUSED:
                        {
                            startToolStripMenuItem.Enabled = true;
                            startToolStripMenuItem.Text = "Stop";
                            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
                            editToolStripMenuItem.Enabled = false;
                            deleteToolStripMenuItem.Enabled = false;
                            hardResetToolStripMenuItem.Enabled = true;
                            resetCTRLALTDELETEToolStripMenuItem.Enabled = true;
                            pauseToolStripMenuItem.Enabled = true;
                            pauseToolStripMenuItem.Text = "Resume";
                            pauseToolStripMenuItem.ToolTipText = "Resume this virtual machine";
                            configureToolStripMenuItem.Enabled = true;
                        }
                        break;
                };
            }
            //Multiple VMs selected => disable most options
            else
            {
                startToolStripMenuItem.Text = "Start";
                startToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.ToolTipText = "Start this virtual machine";
                editToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = true;
                hardResetToolStripMenuItem.Enabled = false;
                resetCTRLALTDELETEToolStripMenuItem.Enabled = false;
                pauseToolStripMenuItem.Enabled = false;
                pauseToolStripMenuItem.Text = "Pause";
                killToolStripMenuItem.Enabled = true;
                configureToolStripMenuItem.Enabled = false;
                cloneToolStripMenuItem.Enabled = false;
            }
        }

        //Closing 86Box Manager before closing all the VMs can lead to weirdness if 86Box Manager is then restarted. So let's warn the user just in case and request confirmation.
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            int vmCount = 0; //Number of running VMs

            //Close to tray
            if (e.CloseReason == CloseReason.UserClosing && closeTray)
            {
                e.Cancel = true;
                trayIcon.Visible = true;
                WindowState = FormWindowState.Minimized;
                Hide();
            }
            else
            {
                foreach (ListViewItem item in lstVMs.Items)
                {
                    VM vm = (VM)item.Tag;
                    if (vm.Status != VM.STATUS_STOPPED && Visible)
                    {
                        vmCount++;
                    }
                }
            }

            //If there are running VMs, display the warning and stop the VMs if user says so
            if (vmCount > 0)
            {
                e.Cancel = true;
                DialogResult = MessageBox.Show("Some virtual machines are still running. It's recommended you stop them first before closing 86Box Manager. Do you want to stop them now?", "Virtual machines are still running", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (DialogResult == DialogResult.Yes)
                {
                    foreach (ListViewItem lvi in lstVMs.Items)
                    {
                        lstVMs.SelectedItems.Clear(); //To prevent weird stuff
                        VM vm = (VM)lvi.Tag;
                        if (vm.Status != VM.STATUS_STOPPED)
                        {
                            lvi.Focused = true;
                            lvi.Selected = true;
                            VMForceStop(); //Tell the VM to shut down without confirmation
                            Process p = Process.GetProcessById(vm.Pid);
                            p.WaitForExit(500); //Wait 500 milliseconds for each VM to close
                        }
                    }

                }
                else if (DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                e.Cancel = false;
            }

            //Save main window's state, size and position
            //BUGBUG: Restoring these on startup causes anchor problems, so we're not doing it anymore...
            /*Settings.Default.WindowState = WindowState;
            Settings.Default.WindowSize = Size;
            Settings.Default.WindowPosition = Location;*/

            //Save listview column widths
            Settings.Default.NameColWidth = clmName.Width;
            Settings.Default.StatusColWidth = clmStatus.Width;
            Settings.Default.DescColWidth = clmDesc.Width;
            Settings.Default.PathColWidth = clmPath.Width;

            Settings.Default.Save();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            if (vm.Status == VM.STATUS_PAUSED)
            {
                VMResume();
            }
            else if (vm.Status == VM.STATUS_RUNNING)
            {
                VMPause();
            }
        }

        //Pauses the selected VM
        private void VMPause()
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            PostMessage(vm.hWnd, 0x8890, IntPtr.Zero, IntPtr.Zero);
            lstVMs.SelectedItems[0].SubItems[1].Text = vm.GetStatusString();
            lstVMs.SelectedItems[0].ImageIndex = 2;
            pauseToolStripMenuItem.Text = "Resume";
            btnPause.Text = "Resume";
            toolTip.SetToolTip(btnStart, "Stop this virtual machine");
            btnStart.Enabled = true;
            btnStart.Text = "Stop";
            startToolStripMenuItem.Text = "Stop";
            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
            btnConfigure.Enabled = true;
            pauseToolStripMenuItem.ToolTipText = "Resume this virtual machine";
            toolTip.SetToolTip(btnPause, "Resume this virtual machine");

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        //Resumes the selected VM
        private void VMResume()
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            PostMessage(vm.hWnd, 0x8890, IntPtr.Zero, IntPtr.Zero);
            vm.Status = VM.STATUS_RUNNING;
            lstVMs.SelectedItems[0].SubItems[1].Text = vm.GetStatusString();
            lstVMs.SelectedItems[0].ImageIndex = 1;
            pauseToolStripMenuItem.Text = "Pause";
            btnPause.Text = "Pause";
            btnStart.Enabled = true;
            startToolStripMenuItem.Text = "Stop";
            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
            btnConfigure.Enabled = true;
            pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
            toolTip.SetToolTip(btnStart, "Stop this virtual machine");
            toolTip.SetToolTip(btnPause, "Pause this virtual machine");

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        //Starts the selected VM
        private void VMStart()
        {
            try
            {
                VM vm = (VM)lstVMs.SelectedItems[0].Tag;

                /* This generates a VM ID on the fly from the VM path. The reason it's done this way is it doesn't break existing VMs and doesn't require
                 * extensive modifications to this legacy version for it to work with newer 86Box versions...
                 * 
                 * IDs also have to be unsigned for 86Box, but GetHashCode() returns signed and result can be negative, so shift it up by int.MaxValue to
                 * ensure it's always positive. */
                int tempid = vm.Path.GetHashCode();
                uint id = 0;

                if (tempid < 0)
                    id = (uint)(tempid + int.MaxValue);
                else
                    id = (uint)tempid;

                string idString = string.Format("{0:X}", id).PadLeft(16, '0');

                if (vm.Status == VM.STATUS_STOPPED)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = exepath + "86Box.exe";
                    p.StartInfo.Arguments = "--vmpath \"" + lstVMs.SelectedItems[0].SubItems[3].Text + "\" --hwnd " + idString + "," + hWndHex;

                    if (logging)
                    {
                        p.StartInfo.Arguments += " --logfile \"" + logpath + "\"";
                    }
                    if (!showConsole)
                    {
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.UseShellExecute = false;
                    }

                    p.Start();
                    vm.Pid = p.Id;
                    vm.Status = VM.STATUS_RUNNING;
                    lstVMs.SelectedItems[0].SubItems[1].Text = vm.GetStatusString();
                    lstVMs.SelectedItems[0].ImageIndex = 1;

                    //Minimize the main window if the user wants this
                    if (minimize)
                    {
                        WindowState = FormWindowState.Minimized;
                    }

                    //Create a new background worker which will wait for the VM's window to close, so it can update the UI accordingly
                    BackgroundWorker bgw = new BackgroundWorker
                    {
                        WorkerReportsProgress = false,
                        WorkerSupportsCancellation = false
                    };
                    bgw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                    bgw.RunWorkerAsync(vm);

                    btnStart.Enabled = true;
                    btnStart.Text = "Stop";
                    toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnPause.Enabled = true;
                    btnPause.Text = "Pause";
                    btnReset.Enabled = true;
                    btnCtrlAltDel.Enabled = true;
                    btnConfigure.Enabled = true;

                    VMCountRefresh();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("The process failed to initialize or its window handle could not be obtained.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Cannot find 86Box.exe. Make sure your settings are correct and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred. Please provide the following information to the developer:\n" + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        //Sends a running/pause VM a request to stop without asking the user for confirmation
        private void VMForceStop()
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            try
            {
                if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
                {
                    PostMessage(vm.hWnd, 0x8893, new IntPtr(1), IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred trying to stop the selected virtual machine.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        //Sends a running/paused VM a request to stop and asking the user for confirmation
        private void VMRequestStop()
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            try
            {
                if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
                {
                    PostMessage(vm.hWnd, 0x8893, IntPtr.Zero, IntPtr.Zero);
                    SetForegroundWindow(vm.hWnd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred trying to stop the selected virtual machine.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        //Start VM if it's stopped or stop it if it's running/paused
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            if (vm.Status == VM.STATUS_STOPPED)
            {
                VMStart();
            }
            else if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                VMRequestStop();
            }
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMConfigure();
        }

        //Opens the settings window for the selected VM
        private void VMConfigure()
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;

            //If the VM is already running, only send the message to open the settings window. Otherwise, start the VM with the -S parameter
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                PostMessage(vm.hWnd, 0x8889, IntPtr.Zero, IntPtr.Zero);
                SetForegroundWindow(vm.hWnd);
            }
            else if (vm.Status == VM.STATUS_STOPPED)
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = exepath + "86Box.exe";
                    p.StartInfo.Arguments = "--settings --vmpath \"" + lstVMs.SelectedItems[0].SubItems[3].Text + "\"";
                    if (!showConsole)
                    {
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.UseShellExecute = false;
                    }
                    p.Start();
                    p.WaitForInputIdle();

                    vm.Status = VM.STATUS_WAITING;
                    vm.hWnd = p.MainWindowHandle;
                    vm.Pid = p.Id;
                    lstVMs.SelectedItems[0].SubItems[1].Text = vm.GetStatusString();
                    lstVMs.SelectedItems[0].ImageIndex = 2;

                    BackgroundWorker bgw = new BackgroundWorker
                    {
                        WorkerReportsProgress = false,
                        WorkerSupportsCancellation = false
                    };
                    bgw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                    bgw.RunWorkerAsync(vm);

                    btnStart.Enabled = false;
                    btnStart.Text = "Stop";
                    toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                    startToolStripMenuItem.Text = "Stop";
                    startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnConfigure.Enabled = false;
                    btnReset.Enabled = false;
                    btnPause.Enabled = false;
                    btnPause.Text = "Pause";
                    toolTip.SetToolTip(btnPause, "Pause this virtual machine");
                    pauseToolStripMenuItem.Text = "Pause";
                    pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
                    btnCtrlAltDel.Enabled = false;
                }
                catch (Win32Exception ex)
                {
                    MessageBox.Show("Cannot find 86Box.exe. Make sure your settings are correct and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    //Revert to stopped status and alert the user
                    vm.Status = VM.STATUS_STOPPED;
                    vm.hWnd = IntPtr.Zero;
                    vm.Pid = -1;
                    MessageBox.Show("This virtual machine could not be started. Please provide the following information to the developer:\n" + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        private void resetCTRLALTDELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMCtrlAltDel();
        }

        //Sends the CTRL+ALT+DEL keystroke to the VM, result depends on the guest OS
        private void VMCtrlAltDel()
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                PostMessage(vm.hWnd, 0x8894, IntPtr.Zero, IntPtr.Zero);
                vm.Status = VM.STATUS_RUNNING;
                lstVMs.SelectedItems[0].SubItems[1].Text = vm.GetStatusString();
                btnPause.Text = "Pause";
                toolTip.SetToolTip(btnPause, "Pause this virtual machine");
                pauseToolStripMenuItem.Text = "Pause";
                pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
            }
            VMCountRefresh();
        }

        private void hardResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMHardReset();
        }

        //Performs a hard reset for the selected VM
        private void VMHardReset()
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                PostMessage(vm.hWnd, 0x8892, IntPtr.Zero, IntPtr.Zero);
                SetForegroundWindow(vm.hWnd);
            }
            VMCountRefresh();
        }

        //For double clicking an item, do something based on VM status
        private void lstVMs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (lstVMs.SelectedItems[0].Bounds.Contains(e.Location))
                {
                    VM vm = (VM)lstVMs.SelectedItems[0].Tag;
                    if (vm.Status == VM.STATUS_STOPPED)
                    {
                        VMStart();
                    }
                    else if (vm.Status == VM.STATUS_RUNNING)
                    {
                        VMRequestStop();
                    }
                    else if (vm.Status == VM.STATUS_PAUSED)
                    {
                        VMResume();
                    }
                }
            }
        }

        //Creates a new VM from the data recieved and adds it to the listview
        public void VMAdd(string name, string desc, bool openCFG, bool startVM)
        {
            VM newVM = new VM(name, desc, cfgpath + name);
            ListViewItem newLvi = new ListViewItem(newVM.Name)
            {
                Tag = newVM,
                ImageIndex = 0
            };
            newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, newVM.GetStatusString()));
            newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, newVM.Desc));
            newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, newVM.Path));
            lstVMs.Items.Add(newLvi);
            Directory.CreateDirectory(cfgpath + newVM.Name);

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, newVM);
                var data = ms.ToArray();

                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
                regkey.SetValue(newVM.Name, data, RegistryValueKind.Binary);
            }

            MessageBox.Show("Virtual machine \"" + newVM.Name + "\" was successfully created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Select the newly created VM
            foreach (ListViewItem lvi in lstVMs.SelectedItems)
            {
                lvi.Selected = false;
            }
            newLvi.Focused = true;
            newLvi.Selected = true;

            //Start the VM and/or open settings window if the user chose this option
            if (startVM)
            {
                VMStart();
            }
            if (openCFG)
            {
                VMConfigure();
            }

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        //Checks if a VM with this name already exists
        public bool VMCheckIfExists(string name)
        {
            try
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
                if (regkey == null) //Regkey doesn't exist yet
                {
                    regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
                    regkey.CreateSubKey(@"Virtual Machines");
                    return false;
                }

                //VM's registry value doesn't exist yet
                if (regkey.GetValue(name) == null)
                {
                    regkey.Close();
                    return false;
                }
                else
                {
                    regkey.Close();
                    return true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not load the virtual machine information from the registry. Make sure you have the required permissions and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //Changes a VM's name and/or description
        public void VMEdit(string name, string desc)
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            string oldname = vm.Name;
            if (!vm.Name.Equals(name))
            {
                try
                { //Move the actual VM files too. This will invalidate any paths inside the cfg, but the user is informed to update those manually.
                    Directory.Move(cfgpath + vm.Name, cfgpath + name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred while trying to move the files for this virtual machine. Please try to move them manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                vm.Name = name;
                vm.Path = cfgpath + vm.Name;
            }
            vm.Desc = desc;

            //Create a new registry value with new info, delete the old one
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
            using (var ms = new MemoryStream())
            {
                regkey.DeleteValue(oldname);
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, vm);
                var data = ms.ToArray();
                regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
                regkey.SetValue(vm.Name, data, RegistryValueKind.Binary);
            }
            regkey.Close();

            MessageBox.Show("Virtual machine \"" + vm.Name + "\" was successfully modified. Please update its configuration so that any absolute paths (e.g. for hard disk images) point to the new folder.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            VMSort(sortColumn, sortOrder);
            LoadVMs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            VMRemove();
        }

        //Removes the selected VM. Confirmations for maximum safety
        private void VMRemove()
        {
            foreach (ListViewItem lvi in lstVMs.SelectedItems)
            {
                VM vm = (VM)lvi.Tag;//(VM)lstVMs.SelectedItems[0].Tag;
                DialogResult result1 = MessageBox.Show("Are you sure you want to remove the virtual machine \"" + vm.Name + "\"?", "Remove virtual machine", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result1 == DialogResult.Yes)
                {
                    if (vm.Status != VM.STATUS_STOPPED)
                    {
                        MessageBox.Show("Virtual machine \"" + vm.Name + "\" is currently running and cannot be removed. Please stop virtual machines before attempting to remove them.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                    try
                    {
                        lstVMs.Items.Remove(lvi);
                        RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
                        regkey.DeleteValue(vm.Name);
                        regkey.Close();
                    }
                    catch (Exception ex) //Catches "regkey doesn't exist" exceptions and such
                    {
                        MessageBox.Show("Virtual machine \"" + vm.Name + "\" could not be removed due to the following error:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    DialogResult result2 = MessageBox.Show("Virtual machine \"" + vm.Name + "\" was successfully removed. Would you like to delete its files as well?", "Virtual machine removed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result2 == DialogResult.Yes)
                    {
                        try
                        {
                            Directory.Delete(vm.Path, true);
                        }
                        catch (UnauthorizedAccessException) //Files are read-only or protected by privileges
                        {
                            MessageBox.Show("86Box Manager was unable to delete the files of this virtual machine because they are read-only or you don't have sufficient privileges to delete them.\n\nMake sure the files are free for deletion, then remove them manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        catch (DirectoryNotFoundException) //Directory not found
                        {
                            MessageBox.Show("86Box Manager was unable to delete the files of this virtual machine because they no longer exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        catch (IOException) //Files are in use by another process
                        {
                            MessageBox.Show("86Box Manager was unable to delete some files of this virtual machine because they are currently in use by another process.\n\nMake sure the files are free for deletion, then remove them manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        catch (Exception ex)
                        { //Other exceptions
                            MessageBox.Show("The following error occurred while trying to remove the files of this virtual machine:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        MessageBox.Show("Files of virtual machine \"" + vm.Name + "\" were successfully deleted.", "Virtual machine files removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMRemove();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgEditVM dlg = new dlgEditVM();
            dlg.ShowDialog();
            dlg.Dispose();
        }

        private void btnCtrlAltDel_Click(object sender, EventArgs e)
        {
            VMCtrlAltDel();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            VMHardReset();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            if (vm.Status == VM.STATUS_PAUSED)
            {
                VMResume();
            }
            else if (vm.Status == VM.STATUS_RUNNING)
            {
                VMPause();
            }
        }

        //This function monitors recieved window messages
        protected override void WndProc(ref Message m)
        {
            // 0x8891 - Main window init complete, wparam = VM ID, lparam = VM window handle
            // 0x8895 - VM paused/resumed, wparam = 1: VM paused, wparam = 0: VM resumed
            // 0x8896 - Dialog opened/closed, wparam = 1: opened, wparam = 0: closed
            // 0x8897 - Shutdown confirmed
            if (m.Msg == 0x8891)
            {
                if (m.LParam != IntPtr.Zero && m.WParam.ToInt64() >= 0)
                {
                    foreach (ListViewItem lvi in lstVMs.Items)
                    {
                        VM vm = (VM)lvi.Tag;
                        int tempid = vm.Path.GetHashCode(); //This can return negative integers, which is a no-no for 86Box, hence the shift up by int.MaxValue
                        uint id = 0;
                        if (tempid < 0)
                            id = (uint)(tempid + int.MaxValue);
                        else
                            id = (uint)tempid;

                        if (id == (uint)m.WParam.ToInt32())
                        {
                            vm.hWnd = m.LParam;
                            break;
                        }
                    }
                }
            }
            if (m.Msg == 0x8895)
            {
                if (m.WParam.ToInt32() == 1) //VM was paused
                {
                    foreach (ListViewItem lvi in lstVMs.Items)
                    {
                        VM vm = (VM)lvi.Tag;
                        if (vm.hWnd.Equals(m.LParam) && vm.Status != VM.STATUS_PAUSED)
                        {
                            vm.Status = VM.STATUS_PAUSED;
                            lvi.SubItems[1].Text = vm.GetStatusString();
                            lvi.ImageIndex = 2;
                            pauseToolStripMenuItem.Text = "Resume";
                            btnPause.Text = "Resume";
                            pauseToolStripMenuItem.ToolTipText = "Resume this virtual machine";
                            toolTip.SetToolTip(btnPause, "Resume this virtual machine");
                            btnStart.Enabled = true;
                            btnStart.Text = "Stop";
                            startToolStripMenuItem.Text = "Stop";
                            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
                            toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                            btnConfigure.Enabled = true;
                        }
                    }
                    VMCountRefresh();
                }
                else if (m.WParam.ToInt32() == 0) //VM was resumed
                {
                    foreach (ListViewItem lvi in lstVMs.Items)
                    {
                        VM vm = (VM)lvi.Tag;
                        if (vm.hWnd == m.LParam && vm.Status != VM.STATUS_RUNNING)
                        {
                            vm.Status = VM.STATUS_RUNNING;
                            lvi.SubItems[1].Text = vm.GetStatusString();
                            lvi.ImageIndex = 1;
                            pauseToolStripMenuItem.Text = "Pause";
                            btnPause.Text = "Pause";
                            toolTip.SetToolTip(btnPause, "Pause this virtual machine");
                            pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
                            btnStart.Enabled = true;
                            btnStart.Text = "Stop";
                            toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                            startToolStripMenuItem.Text = "Stop";
                            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
                            btnConfigure.Enabled = true;
                        }
                    }
                    VMCountRefresh();
                }
            }
            if (m.Msg == 0x8896)
            {
                if (m.WParam.ToInt32() == 1)  //A dialog was opened
                {
                    foreach (ListViewItem lvi in lstVMs.Items)
                    {
                        VM vm = (VM)lvi.Tag;
                        if (vm.hWnd == m.LParam && vm.Status != VM.STATUS_WAITING)
                        {
                            vm.Status = VM.STATUS_WAITING;
                            lvi.SubItems[1].Text = vm.GetStatusString();
                            lvi.ImageIndex = 2;
                            btnStart.Enabled = false;
                            btnStart.Text = "Stop";
                            toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                            startToolStripMenuItem.Text = "Stop";
                            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                            btnConfigure.Enabled = false;
                            btnReset.Enabled = false;
                            btnPause.Enabled = false;
                            btnCtrlAltDel.Enabled = false;
                        }
                    }
                    VMCountRefresh();
                }
                else if (m.WParam.ToInt32() == 0) //A dialog was closed
                {
                    foreach (ListViewItem lvi in lstVMs.Items)
                    {
                        VM vm = (VM)lvi.Tag;
                        if (vm.hWnd == m.LParam && vm.Status != VM.STATUS_RUNNING)
                        {
                            vm.Status = VM.STATUS_RUNNING;
                            lvi.SubItems[1].Text = vm.GetStatusString();
                            lvi.ImageIndex = 1;
                            btnStart.Enabled = true;
                            btnStart.Text = "Stop";
                            toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                            startToolStripMenuItem.Text = "Stop";
                            startToolStripMenuItem.ToolTipText = "Stop this virtual machine";
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                            btnConfigure.Enabled = true;
                            btnReset.Enabled = true;
                            btnPause.Enabled = true;
                            btnPause.Text = "Pause";
                            pauseToolStripMenuItem.Text = "Pause";
                            pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
                            toolTip.SetToolTip(btnPause, "Pause this virtual machine");
                            btnCtrlAltDel.Enabled = true;
                        }
                    }
                    VMCountRefresh();
                }
            }

            if (m.Msg == 0x8897) //Shutdown confirmed
            {
                foreach (ListViewItem lvi in lstVMs.Items)
                {
                    VM vm = (VM)lvi.Tag;
                    if (vm.hWnd.Equals(m.LParam) && vm.Status != VM.STATUS_STOPPED)
                    {
                        vm.Status = VM.STATUS_STOPPED;
                        vm.hWnd = IntPtr.Zero;
                        lvi.SubItems[1].Text = vm.GetStatusString();
                        lvi.ImageIndex = 0;

                        btnStart.Text = "Start";
                        startToolStripMenuItem.Text = "Start";
                        startToolStripMenuItem.ToolTipText = "Start this virtual machine";
                        toolTip.SetToolTip(btnStart, "Start this virtual machine");
                        btnPause.Text = "Pause";
                        pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
                        pauseToolStripMenuItem.Text = "Pause";
                        toolTip.SetToolTip(btnPause, "Pause this virtual machine");
                        if (lstVMs.SelectedItems.Count == 1)
                        {
                            btnEdit.Enabled = true;
                            btnDelete.Enabled = true;
                            btnStart.Enabled = true;
                            btnConfigure.Enabled = true;
                            btnPause.Enabled = false;
                            btnReset.Enabled = false;
                            btnCtrlAltDel.Enabled = false;
                        }
                        else if (lstVMs.SelectedItems.Count == 0)
                        {
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                            btnStart.Enabled = false;
                            btnConfigure.Enabled = false;
                            btnPause.Enabled = false;
                            btnReset.Enabled = false;
                            btnCtrlAltDel.Enabled = false;
                        }
                        else
                        {
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = true;
                            btnStart.Enabled = false;
                            btnConfigure.Enabled = false;
                            btnPause.Enabled = false;
                            btnReset.Enabled = false;
                            btnCtrlAltDel.Enabled = false;
                        }
                    }
                }
                VMCountRefresh();
            }
            //This is the WM_COPYDATA message, used here to pass command line args to an already running instance
            //NOTE: This code will have to be modified in case more command line arguments are added in the future.
            if (m.Msg == 0x004A)
            {
                //Get the VM name and find the associated LVI and VM object
                COPYDATASTRUCT ds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                string vmName = Marshal.PtrToStringAnsi(ds.lpData, ds.cbData);
                ListViewItem lvi = lstVMs.FindItemWithText(vmName);

                //This check is necessary in case the specified VM was already removed but the shortcut remains
                if (lvi != null)
                {
                    VM vm = (VM)lvi.Tag;

                    //If the VM is already running, display a message, otherwise, start it
                    if (vm.Status != VM.STATUS_STOPPED)
                    {
                        MessageBox.Show("The virtual machine \"" + vmName + "\" is already running.", "Virtual machine already running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //This is needed so that we start the correct VM in case multiple items are selected
                        lstVMs.SelectedItems.Clear();

                        lvi.Focused = true;
                        lvi.Selected = true;
                        VMStart();
                    }
                }
                else
                {
                    MessageBox.Show("The virtual machine \"" + vmName + "\" could not be found. It may have been removed or the specified name is incorrect.", "Virtual machine not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            base.WndProc(ref m);
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMOpenFolder();
        }

        //Opens the folder containg the selected VM
        private void VMOpenFolder()
        {
            foreach (ListViewItem lvi in lstVMs.SelectedItems)
            {
                VM vm = (VM)lvi.Tag;
                try
                {
                    Process.Start(vm.Path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The folder for the virtual machine \"" + vm.Name + "\" could not be opened. Make sure it still exists and that you have sufficient privileges to access it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void createADesktopShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstVMs.SelectedItems)
            {
                VM vm = (VM)lvi.Tag;
                try
                {
                    WshShell shell = new WshShell();
                    string shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + vm.Name + ".lnk";
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                    shortcut.Description = vm.Desc;
                    shortcut.IconLocation = Application.StartupPath + @"\86manager.exe,0";
                    shortcut.TargetPath = Application.StartupPath + @"\86manager.exe";
                    shortcut.Arguments = "-S \"" + vm.Name + "\"";
                    shortcut.Save();

                    MessageBox.Show("A desktop shortcut for the virtual machine \"" + vm.Name + "\" was successfully created.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A desktop shortcut for the virtual machine \"" + vm.Name + "\" could not be created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Starts/stops selected VM when enter is pressed
        private void lstVMs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstVMs.SelectedItems.Count == 1)
            {
                VM vm = (VM)lstVMs.SelectedItems[0].Tag;
                if (vm.Status == VM.STATUS_RUNNING)
                {
                    VMRequestStop();
                }
                else if (vm.Status == VM.STATUS_STOPPED)
                {
                    VMStart();
                }
            }
            if (e.KeyCode == Keys.Delete && lstVMs.SelectedItems.Count == 1)
            {
                VMRemove();
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Restore the window and hide the tray icon
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            trayIcon.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int vmCount = 0;
            foreach (ListViewItem item in lstVMs.Items)
            {
                VM vm = (VM)item.Tag;
                if (vm.Status != VM.STATUS_STOPPED)
                {
                    vmCount++;
                }
            }

            //If there are running VMs, display the warning and stop the VMs if user says so
            if (vmCount > 0)
            {
                DialogResult = MessageBox.Show("Some virtual machines are still running. It's recommended you stop them first before closing 86Box Manager. Do you want to stop them now?", "Virtual machines are still running", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (DialogResult == DialogResult.Yes)
                {
                    foreach (ListViewItem lvi in lstVMs.Items)
                    {
                        VM vm = (VM)lvi.Tag;
                        lstVMs.SelectedItems.Clear();
                        if (vm.Status != VM.STATUS_STOPPED)
                        {
                            lvi.Focused = true;
                            lvi.Selected = true;
                            VMForceStop(); //Tell the VMs to stop without asking for user confirmation
                        }
                    }

                    Thread.Sleep(vmCount * 500); //Wait just a bit to make sure everything goes as planned
                }
                else if (DialogResult == DialogResult.Cancel)
                {
                    return;
                }
            }
            Application.Exit();
        }

        //Handles things when WindowState changes
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && minimizeTray)
            {
                trayIcon.Visible = true;
                Hide();
            }
            if (WindowState == FormWindowState.Normal)
            {
                Show();
                trayIcon.Visible = false;
            }
        }

        private void open86BoxManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            trayIcon.Visible = false;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            trayIcon.Visible = false;
            dlgSettings ds = new dlgSettings();
            ds.ShowDialog();
            LoadSettings();
            ds.Dispose();
        }

        //Kills the process associated with the selected VM
        private void VMKill()
        {
            foreach (ListViewItem lvi in lstVMs.SelectedItems)
            {
                VM vm = (VM)lvi.Tag;

                //Ask the user to confirm
                DialogResult = MessageBox.Show("Killing a virtual machine can cause data loss. Only do this if 86Box.exe process gets stuck.\n\nDo you really wish to kill the virtual machine \"" + vm.Name + "\"?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Process p = Process.GetProcessById(vm.Pid);
                        p.Kill();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not kill 86Box.exe process for virtual machine \"" + vm.Name + "\". The process may have already ended on its own or access was denied.", "Could not kill process", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    //We need to cleanup afterwards to make sure the VM is put back into a valid state
                    vm.Status = VM.STATUS_STOPPED;
                    vm.hWnd = IntPtr.Zero;
                    lstVMs.SelectedItems[0].SubItems[1].Text = vm.GetStatusString();
                    lstVMs.SelectedItems[0].ImageIndex = 0;

                    btnStart.Text = "Start";
                    toolTip.SetToolTip(btnStart, "Stop this virtual machine");
                    btnPause.Text = "Pause";
                    if (lstVMs.SelectedItems.Count > 0)
                    {
                        btnEdit.Enabled = true;
                        btnDelete.Enabled = true;
                        btnStart.Enabled = true;
                        btnConfigure.Enabled = true;
                        btnPause.Enabled = false;
                        btnReset.Enabled = false;
                        btnCtrlAltDel.Enabled = false;
                    }
                    else
                    {
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                        btnStart.Enabled = false;
                        btnConfigure.Enabled = false;
                        btnPause.Enabled = false;
                        btnReset.Enabled = false;
                        btnCtrlAltDel.Enabled = false;
                    }
                }
            }

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMKill();
        }

        //Sort the VM list by specified column and order
        private void VMSort(int column, SortOrder order)
        {
            const string ascArrow = " ▲";
            const string descArrow = " ▼";

            if (lstVMs.SelectedItems.Count > 1)
            {
                lstVMs.SelectedItems.Clear(); //Just in case so we don't end up with weird selection glitches
            }

            //Remove the arrows from the current column text if they exist
            if (sortColumn > -1 && (lstVMs.Columns[sortColumn].Text.EndsWith(ascArrow) || lstVMs.Columns[sortColumn].Text.EndsWith(descArrow)))
            {
                lstVMs.Columns[sortColumn].Text = lstVMs.Columns[sortColumn].Text.Substring(0, lstVMs.Columns[sortColumn].Text.Length - 2);
            }

            //Then append the appropriate arrow to the new column text
            if (order == SortOrder.Ascending)
            {
                lstVMs.Columns[column].Text += ascArrow;
            }
            else if (order == SortOrder.Descending)
            {
                lstVMs.Columns[column].Text += descArrow;
            }

            sortColumn = column;
            sortOrder = order;
            lstVMs.Sorting = sortOrder;
            lstVMs.Sort();
            lstVMs.ListViewItemSorter = new ListViewItemComparer(sortColumn, sortOrder);

            //Save the new column and order to the registry
            try
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
                regkey.SetValue("SortColumn", sortColumn, RegistryValueKind.DWord);
                regkey.SetValue("SortOrder", sortOrder, RegistryValueKind.DWord);
                regkey.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not save the column sorting state to the registry. Make sure you have the required permissions and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Handles the click event for the listview column headers, allowing to sort the items by columns
        private void lstVMs_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lstVMs.Sorting == SortOrder.Ascending)
            {
                VMSort(e.Column, SortOrder.Descending);
            }
            else if (lstVMs.Sorting == SortOrder.Descending || lstVMs.Sorting == SortOrder.None)
            {
                VMSort(e.Column, SortOrder.Ascending);
            }
        }

        private void wipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMWipe();
        }

        //Deletes the config and nvr of selected VM
        private void VMWipe()
        {
            foreach (ListViewItem lvi in lstVMs.SelectedItems)
            {
                VM vm = (VM)lvi.Tag;

                DialogResult = MessageBox.Show("Wiping a virtual machine deletes its configuration and nvr files. You'll have to reconfigure the virtual machine (and the BIOS if applicable).\n\n Are you sure you wish to wipe the virtual machine \"" + vm.Name + "\"?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult == DialogResult.Yes)
                {
                    if (vm.Status != VM.STATUS_STOPPED)
                    {
                        MessageBox.Show("The virtual machine \"" + vm.Name + "\" is currently running and cannot be wiped. Please stop virtual machines before attempting to wipe them.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                    try
                    {
                        System.IO.File.Delete(vm.Path + @"\86box.cfg");
                        Directory.Delete(vm.Path + @"\nvr", true);
                        MessageBox.Show("The virtual machine \"" + vm.Name + "\" was successfully wiped.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred trying to wipe the virtual machine \"" + vm.Name + "\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }
            }
        }

        //Imports existing VM files to a new VM
        public void VMImport(string name, string desc, string importPath, bool openCFG, bool startVM)
        {
            VM newVM = new VM(name, desc, cfgpath + name);
            ListViewItem newLvi = new ListViewItem(newVM.Name)
            {
                Tag = newVM,
                ImageIndex = 0
            };
            newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, newVM.GetStatusString()));
            newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, newVM.Desc));
            newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, newVM.Path));
            lstVMs.Items.Add(newLvi);
            Directory.CreateDirectory(cfgpath + newVM.Name);

            bool importFailed = false;

            //Copy existing files to the new VM directory
            try
            {
                foreach (string oldPath in Directory.GetDirectories(importPath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(oldPath.Replace(importPath, newVM.Path));
                }
                foreach (string newPath in Directory.GetFiles(importPath, "*.*", SearchOption.AllDirectories))
                {
                    System.IO.File.Copy(newPath, newPath.Replace(importPath, newVM.Path), true);
                }
            }
            catch (Exception ex)
            {
                importFailed = true; //Set this flag so we can inform the user at the end
            }

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, newVM);
                var data = ms.ToArray();
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
                regkey.SetValue(newVM.Name, data, RegistryValueKind.Binary);
            }

            if (importFailed)
            {
                MessageBox.Show("Virtual machine \"" + newVM.Name + "\" was successfully created, but files could not be imported. Make sure the path you selected was correct and valid.\n\nIf the VM is already located in your VMs folder, you don't need to select the Import option, just add a new VM with the same name.", "Import failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Virtual machine \"" + newVM.Name + "\" was successfully created, files were imported. Remember to update any paths pointing to disk images in your config!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Select the newly created VM
            foreach (ListViewItem lvi in lstVMs.SelectedItems)
            {
                lvi.Selected = false;
            }
            newLvi.Focused = true;
            newLvi.Selected = true;

            //Start the VM and/or open settings window if the user chose this option
            if (startVM)
            {
                VMStart();
            }
            if (openCFG)
            {
                VMConfigure();
            }

            VMSort(sortColumn, sortOrder);
            VMCountRefresh();
        }

        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VM vm = (VM)lstVMs.SelectedItems[0].Tag;
            dlgCloneVM dc = new dlgCloneVM(vm.Path);
            dc.ShowDialog();
            dc.Dispose();
        }

        //Refreshes the VM counter in the status bar
        private void VMCountRefresh()
        {
            int runningVMs = 0;
            int pausedVMs = 0;
            int waitingVMs = 0;
            int stoppedVMs = 0;

            foreach (ListViewItem lvi in lstVMs.Items)
            {
                VM vm = (VM)lvi.Tag;
                switch (vm.Status)
                {
                    case VM.STATUS_PAUSED: pausedVMs++; break;
                    case VM.STATUS_RUNNING: runningVMs++; break;
                    case VM.STATUS_STOPPED: stoppedVMs++; break;
                    case VM.STATUS_WAITING: waitingVMs++; break;
                }
            }

            lblVMCount.Text = "All VMs: " + lstVMs.Items.Count + " | Running: " + runningVMs + " | Paused: " + pausedVMs + " | Waiting: " + waitingVMs + " | Stopped: " + stoppedVMs;
        }

        private void openConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMOpenConfig();
        }

        private void VMOpenConfig()
        {
            foreach (ListViewItem lvi in lstVMs.SelectedItems)
            {
                VM vm = (VM)lvi.Tag;
                try
                {
                    Process.Start(vm.Path + Path.DirectorySeparatorChar + "86box.cfg");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The config file for the virtual machine \"" + vm.Name + "\" could not be opened. Make sure it still exists and that you have sufficient privileges to access it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}