using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _86boxManager
{
    public partial class frmMain : Form
    {
        //Win32 API imports
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd); //Checks if hWnd belongs to an existing window
        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam); //Sends a message to the window of hWnd

        private static RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true); //Registry key for accessing the settings and VM list
        public string exepath = ""; //Path to 86box.exe and the romset
        public string cfgpath = ""; //Path to the virtual machines folder (configs, nvrs, etc.)
        private bool minimize = false; //Minimize the main window when a vm is started?

        public frmMain()
        {
            InitializeComponent();

            //Load the relevant settings and VM list from the registry
            LoadSettings();
            LoadVMs();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            if (vm.Status == VM.STATUS_STOPPED)
            {
                VMStart();
            }
            else if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                VMStop();
            }
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            VMConfigure();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            dlgAbout dlg = new dlgAbout();
            dlg.ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            dlgSettings dlg = new dlgSettings();
            dlg.ShowDialog();
            LoadSettings(); //Reload the settings due to potential changes    
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
            else
            {
                //Disable relevant buttons if VM is running
                VM vm = (VM)lstVMs.FocusedItem.Tag;
                if (vm.Status == VM.STATUS_RUNNING)
                {
                    //btnConfigure.Enabled = false;
                    btnStart.Enabled = true;
                    btnStart.Text = "Stop";
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
                    btnStart.Enabled = false;
                    btnStart.Text = "Stop";
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnConfigure.Enabled = false;
                    btnPause.Enabled = true;
                    btnPause.Text = "Resume";
                    btnReset.Enabled = true;
                    btnCtrlAltDel.Enabled = true;
                }
                else if (vm.Status == VM.STATUS_IN_SETTINGS)
                {
                    btnStart.Enabled = false;
                    btnStart.Text = "Start";
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnReset.Enabled = false;
                    btnCtrlAltDel.Enabled = false;
                    btnPause.Enabled = false;
                    btnPause.Text = "Pause";
                    btnConfigure.Enabled = false;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dlgAddVM dlg = new dlgAddVM();
            dlg.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dlgEditVM dlg = new dlgEditVM();
            dlg.ShowDialog();
        }

        //Load the settings from the registry
        private void LoadSettings()
        {
            try
            {
                regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box");
                exepath = regkey.GetValue("EXEdir").ToString();
                cfgpath = regkey.GetValue("CFGdir").ToString();

                //This check is necessary in case the tailing backslash is not present!
                if (!exepath.EndsWith(@"\"))
                {
                    exepath += @"\";
                }

                if (!cfgpath.EndsWith(@"\"))
                {
                    cfgpath += @"\";
                }

                minimize = Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart"));
            }
            catch (Exception ex) //Bad settings, retry
            {
                MessageBox.Show("86Box Manager settings are missing or corrupted. This is normal if you're running 86Box Manager for the first time. Please (re)configure the settings now.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dlgSettings dlg = new dlgSettings();
                dlg.ShowDialog();
                LoadSettings();
            }
        }

        //Load the VMs from the registry
        private void LoadVMs()
        {
            try
            {
                regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines");
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
                        ToolTipText = vm.Desc,
                        ImageIndex = 0
                    };
                    newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, vm.GetStatusString()));
                    newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, vm.Path));
                    lstVMs.Items.Add(newLvi);
                }
            }
            catch (Exception ex)
            {
                //Ignore for now
            }
        }

        //Wait for the associated window of a VM to close
        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            VM vm = e.Argument as VM;
            try
            {
                Process p = Process.GetProcessById(vm.Pid); //Find the process associated with the VM
                p.WaitForExit(); //Wait for it to exit
            }
            catch(Exception ex)
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
                    if (lstVMs.SelectedItems.Count > 0 && lstVMs.FocusedItem.Equals(item))
                    {
                        btnEdit.Enabled = true;
                        btnDelete.Enabled = true;
                        btnStart.Enabled = true;
                        btnStart.Text = "Start";
                        btnConfigure.Enabled = true;
                        btnPause.Enabled = false;
                        btnPause.Text = "Pause";
                        btnCtrlAltDel.Enabled = false;
                        btnReset.Enabled = false;
                    }
                }
            }
        }

        //This is required so that the context menu only shows up when the user right-clicks on a listviewitem
        private void lstVMs_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lstVMs.FocusedItem.Bounds.Contains(e.Location))
                {
                    cmsVM.Show(Cursor.Position);
                }
            }
        }

        //Enable/disable relevant menu items depending on selected VM's status
        private void cmsVM_Opening(object sender, CancelEventArgs e)
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            switch (vm.Status)
            {
                case VM.STATUS_RUNNING:
                    {
                        startToolStripMenuItem.Text = "Shutdown";
                        startToolStripMenuItem.Enabled = true;
                        startToolStripMenuItem.ToolTipText = "Shutdown this virtual machine";
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
                case VM.STATUS_IN_SETTINGS:
                    {
                        startToolStripMenuItem.Enabled = false;
                        startToolStripMenuItem.Text = "Start";
                        startToolStripMenuItem.ToolTipText = "Start this virtual machine";
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
                        startToolStripMenuItem.Text = "Shutdown";
                        startToolStripMenuItem.ToolTipText = "Shutdown this virtual machine";
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

        //Closing 86Box Manager before closing all the VMs can lead to weirdness if 86Box Manager is then restarted. So let's warn the user just in case and request confirmation.
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ListViewItem item in lstVMs.Items)
            {
                VM vm = (VM)item.Tag;
                if (vm.Status != VM.STATUS_STOPPED)
                {
                    e.Cancel = true;
                    DialogResult = MessageBox.Show("It appears some virtual machines are still running. It's recommended you close those first before closing 86Box Manager. Do you want to exit 86Box Manager anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (DialogResult == DialogResult.Yes)
                    {
                        e.Cancel = false;
                        return; //We only need to display the warning once...
                    }
                    else if(DialogResult == DialogResult.No)
                    {
                        break;
                    }
                }
            }
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
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
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            PostMessage(vm.hWnd, 0x8890, IntPtr.Zero, IntPtr.Zero);
            vm.Status = VM.STATUS_PAUSED;
            lstVMs.FocusedItem.SubItems[1].Text = vm.GetStatusString();
            lstVMs.FocusedItem.ImageIndex = 2;
            pauseToolStripMenuItem.Text = "Resume";
            btnPause.Text = "Resume";
            pauseToolStripMenuItem.ToolTipText = "Resume this virtual machine";
        }

        //Resumes the selected VM
        private void VMResume()
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            PostMessage(vm.hWnd, 0x8890, IntPtr.Zero, IntPtr.Zero);
            vm.Status = VM.STATUS_RUNNING;
            lstVMs.FocusedItem.SubItems[1].Text = vm.GetStatusString();
            lstVMs.FocusedItem.ImageIndex = 1;
            pauseToolStripMenuItem.Text = "Pause";
            btnPause.Text = "Pause";
            pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
        }

        //Starts the selected VM
        private void VMStart()
        {
            try
            {
                VM vm = (VM)lstVMs.FocusedItem.Tag;
                if (vm.Status == VM.STATUS_STOPPED)
                {
                    Process p = Process.Start(exepath + "86Box.exe", "-P \"" + lstVMs.FocusedItem.SubItems[2].Text + "\""); //Start the process with appropriate arguments
                    p.WaitForInputIdle(); //Wait a bit so hWnd can be obtained

                    if (!p.MainWindowHandle.Equals(IntPtr.Zero))
                    {
                        vm.hWnd = p.MainWindowHandle; //Get the window handle of the newly created process
                        vm.Pid = p.Id; //Assign the pid to the VM
                        vm.Status = VM.STATUS_RUNNING;
                        lstVMs.FocusedItem.SubItems[1].Text = vm.GetStatusString();
                        lstVMs.FocusedItem.ImageIndex = 1;

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
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                        btnPause.Enabled = true;
                        btnPause.Text = "Pause";
                        btnReset.Enabled = true;
                        btnCtrlAltDel.Enabled = true;
                        btnConfigure.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred. Please provide the following information to the developer:\n" + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Stops a running/paused VM
        private void VMStop()
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                PostMessage(vm.hWnd, 0x8893, IntPtr.Zero, IntPtr.Zero);
                vm.Status = VM.STATUS_STOPPED;
                vm.hWnd = IntPtr.Zero;
                lstVMs.FocusedItem.SubItems[1].Text = vm.GetStatusString();
                lstVMs.FocusedItem.ImageIndex = 0;

                btnStart.Text = "Start";
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

        //Start VM if it's stopped or stop it if it's running/paused
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            if (vm.Status == VM.STATUS_STOPPED)
            {
                VMStart();
            }
            else if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                VMStop();
            }
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMConfigure();
        }

        //Opens the settings window for the selected VM
        private void VMConfigure()
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;

            //If the VM is already running, only send the message to open the settings window. Otherwise, start the VM with the -S parameter
            if (vm.Status == VM.STATUS_RUNNING)
            {
                PostMessage(vm.hWnd, 0x8889, IntPtr.Zero, IntPtr.Zero);
            }
            else if (vm.Status == VM.STATUS_STOPPED)
            {
                try
                {
                    Process p = Process.Start(exepath + "86Box.exe", "-S -P \"" + lstVMs.FocusedItem.SubItems[2].Text + "\"");
                    p.WaitForInputIdle();
                    vm.Status = VM.STATUS_IN_SETTINGS;
                    vm.hWnd = p.MainWindowHandle;
                    vm.Pid = p.Id;
                    lstVMs.FocusedItem.SubItems[1].Text = vm.GetStatusString();
                    lstVMs.FocusedItem.ImageIndex = 2;

                    BackgroundWorker bgw = new BackgroundWorker
                    {
                        WorkerReportsProgress = false,
                        WorkerSupportsCancellation = false
                    };
                    bgw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                    bgw.RunWorkerAsync(vm);

                    btnStart.Enabled = false;
                    btnStart.Text = "Start";
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnConfigure.Enabled = false;
                    btnReset.Enabled = false;
                    btnPause.Enabled = false;
                    btnPause.Text = "Pause";
                    btnCtrlAltDel.Enabled = false;
                }
                catch(Exception ex)
                {
                    //Revert to stopped status and alert the user
                    vm.Status = VM.STATUS_STOPPED;
                    vm.hWnd = IntPtr.Zero;
                    vm.Pid = -1;
                    MessageBox.Show("This virtual machine could not be started. Please provide the following information to the developer:\n" + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void resetCTRLALTDELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMCtrlAltDel();
        }

        //Sends the CTRL+ALT+DEL keystroke to the VM, result depends on the guest OS
        private void VMCtrlAltDel()
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                PostMessage(vm.hWnd, 0x8894, IntPtr.Zero, IntPtr.Zero);
                vm.Status = VM.STATUS_RUNNING;
                lstVMs.FocusedItem.SubItems[1].Text = vm.GetStatusString();
                btnPause.Text = "Pause";
            }
        }

        private void hardResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMHardReset();
        }

        //Performs a hard reset for the selected VM
        private void VMHardReset()
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                PostMessage(vm.hWnd, 0x8892, IntPtr.Zero, IntPtr.Zero);
                vm.Status = VM.STATUS_RUNNING;
                lstVMs.FocusedItem.SubItems[1].Text = vm.GetStatusString();
                btnPause.Text = "Pause";
            }
        }

        //For double clicking an item, do something based on VM status
        private void lstVMs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (lstVMs.FocusedItem.Bounds.Contains(e.Location))
                {
                    VM vm = (VM)lstVMs.FocusedItem.Tag;
                    if (vm.Status == VM.STATUS_STOPPED)
                    {
                        VMStart();
                    }
                    else if (vm.Status == VM.STATUS_RUNNING)
                    {
                        VMStop();
                    }
                    else if (vm.Status == VM.STATUS_PAUSED)
                    {
                        VMResume();
                    }
                }
            }
        }

        //Creates a new VM from the data recieved and adds it to the listview
        public void VMAdd(string name, string desc, bool openCFG)
        {
            VM newVM = new VM(name, desc, cfgpath + name);
            ListViewItem newLvi = new ListViewItem(newVM.Name)
            {
                Tag = newVM,
                ToolTipText = newVM.Desc,
                ImageIndex = 0
            };
            newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, newVM.GetStatusString()));
            newLvi.SubItems.Add(new ListViewItem.ListViewSubItem(newLvi, newVM.Path));
            lstVMs.Items.Add(newLvi);
            Directory.CreateDirectory(cfgpath + newVM.Name);

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, newVM);
                var data = ms.ToArray();
                regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
                regkey.SetValue(newVM.Name, data, RegistryValueKind.Binary);
            }

            MessageBox.Show("Virtual machine \"" + newVM.Name + "\" was successfully created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (openCFG)
            {
                lstVMs.FocusedItem = newLvi; //Select the newly created VM so VMConfigure can do its thing
                VMConfigure();
            }
        }

        //Checks if a VM with this name already exists
        public bool VMCheckIfExists(string name)
        {
            regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
            if(regkey == null) //Regkey doesn't exist yet
            {
                regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
                regkey.CreateSubKey(@"Virtual Machines");
                return false; 
            }
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

        //Changes a VM's name and/or description
        public void VMEdit(string name, string desc)
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
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
                lstVMs.FocusedItem.Text = name;
                lstVMs.FocusedItem.SubItems[2].Text = vm.Path;
            }
            vm.Desc = desc;
            lstVMs.FocusedItem.ToolTipText = desc;

            //Create a new registry value with new info, delete the old one
            regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
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

            MessageBox.Show("Virtual machine \"" + vm.Name + "\" was successfully modified. Please update its configuration so that any folder paths (e.g. for hard disk images) point to the new folder.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            VMRemove();
        }

        //Removes the selected VM. Confirmations for maximum safety
        private void VMRemove()
        {
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            DialogResult result1 = MessageBox.Show("Are you sure you want to remove this virtual machine?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result1 == DialogResult.Yes)
            {
                lstVMs.Items.Remove(lstVMs.FocusedItem);
                regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box\Virtual Machines", true);
                regkey.DeleteValue(vm.Name);
                regkey.Close();

                DialogResult result2 = MessageBox.Show("Would you like to delete the files of this virtual machine as well?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result2 == DialogResult.No)
                {
                    MessageBox.Show("Virtual machine \"" + vm.Name + "\" was successfully removed. Its files are still there if you want to re-add it later.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result2 == DialogResult.Yes)
                {
                    try
                    {
                        Directory.Delete(cfgpath + vm.Name, true);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        //Just ignore this for now
                    }
                    MessageBox.Show("Virtual machine \"" + vm.Name + "\" was successfully removed, along with its files.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VMRemove();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgEditVM dlg = new dlgEditVM();
            dlg.ShowDialog();
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
            VM vm = (VM)lstVMs.FocusedItem.Tag;
            if (vm.Status == VM.STATUS_PAUSED)
            {
                VMResume();
            }
            else if (vm.Status == VM.STATUS_RUNNING)
            {
                VMPause();
            }
        }
    }
}