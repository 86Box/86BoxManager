using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _86BoxManager.API;
using _86BoxManager.Common;
using _86BoxManager.Model;
using _86BoxManager.Models;
using _86BoxManager.Registry;
using _86BoxManager.Tools;
using _86BoxManager.View;
using _86BoxManager.Xplat;
using Gtk;
using IOPath = System.IO.Path;
using RegistryValueKind = _86BoxManager.Registry.ValueKind;
using SortType = Gtk.SortType;

// ReSharper disable InconsistentNaming
namespace _86BoxManager.Core
{
    internal static class VMCenter
    {
        private static VMWatch _watch;

        public static (string cfgpath, string exepath) FindPaths()
        {
            var cfgPath = IOPath.Combine(Platforms.Env.UserProfile, "86Box VMs").CheckTrail();
            var exeFolders = Platforms.Env.GetProgramFiles("86Box");
            var exeFound = Search.Find(exeFolders, Platforms.Env.ExeNames);
            if (exeFound == null) {
                // The old code did that, so... reproduce
                exeFound = exeFolders.First();
            }
            var exePath = exeFound.CheckTrail();
            return (cfgPath, exePath);
        }
        
        // Checks if a VM with this name already exists
        public static bool CheckIfExists(string name)
        {
            try
            {
                var regkey = Configs.Open86BoxVmKey(true);
                if (regkey == null) //Regkey doesn't exist yet
                {
                    regkey = Configs.Open86BoxKey(true);
                    Configs.Create86BoxVmKey();
                    return false;
                }

                //VM's registry value doesn't exist yet
                if (regkey.GetValue(name) == null)
                {
                    regkey.Close();
                    return false;
                }

                //It really exists
                regkey.Close();
                return true;
            }
            catch
            {
                Dialogs.ShowMessageBox("Could not load the virtual machine information from the " +
                                       "registry. Make sure you have the required permissions " +
                                       "and try again.",
                    MessageType.Error, ButtonsType.Ok, "Error");
                return false;
            }
        }

        // Imports existing VM files to a new VM
        public static void Import(string name, string desc, string importPath, bool openCFG, bool startVM, frmMain ui)
        {
            var cfgpath = Program.Root.cfgpath;

            var newVM = new VM(name, desc, Path.Combine(cfgpath, name));
            var newLvi = Program.Root.lstVMs.Insert(newVM.Name, newVM);
            Directory.CreateDirectory(newVM.Path);

            var importFailed = false;

            // Copy existing files to the new VM directory
            try
            {
                var allDirs = Directory.GetDirectories(importPath, "*", SearchOption.AllDirectories);
                foreach (var oldPath in allDirs)
                {
                    Directory.CreateDirectory(oldPath.Replace(importPath, newVM.Path));
                }
                var allFiles = Directory.GetFiles(importPath, "*.*", SearchOption.AllDirectories);
                foreach (var newPath in allFiles)
                {
                    var oldPath = newPath.Replace(importPath, newVM.Path);
                    File.Copy(newPath, oldPath, true);
                }
            }
            catch
            {
                // Set this flag so we can inform the user at the end
                importFailed = true;
            }

            var data = Serializer.Write(newVM);
            var regkey = Configs.Open86BoxVmKey(true);
            regkey.SetValue(newVM.Name, data, RegistryValueKind.Binary);
            regkey.Close();

            if (importFailed)
            {
                Dialogs.ShowMessageBox($@"Virtual machine ""{newVM.Name}"" was successfully created, but " +
                                       "files could not be imported. Make sure the path you selected was correct " +
                                       "and valid. If the VM is already located in your VMs folder, you don't " +
                                       "need to select the Import option, just add a new VM with the same name.",
                    MessageType.Warning, ButtonsType.Ok, "Import failed");
            }
            else
            {
                Dialogs.ShowMessageBox($@"Virtual machine ""{newVM.Name}"" was successfully created, files " +
                                       "were imported. Remember to update any paths pointing to disk images in " +
                                       "your config!",
                    MessageType.Info, ButtonsType.Ok, "Success");
            }

            // Select the newly created VM
            foreach (var lvi in ui.lstVMs.GetSelItems())
            {
                lvi.Selected = false;
            }
            newLvi.Focused = true;
            newLvi.Selected = true;

            // Start the VM and/or open settings window if the user chose this option
            if (startVM)
            {
                Start();
            }
            if (openCFG)
            {
                Configure();
            }

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }
        
        // Creates a new VM from the data received and adds it to the listview
        public static void Add(string name, string desc, bool openCFG, bool startVM)
        {
            var ui = Program.Root;
            var cfgpath = ui.cfgpath;

            var newVM = new VM(name, desc, Path.Combine(cfgpath, name));
            var newLvi = ui.lstVMs.Insert(newVM.Name, newVM);
            Directory.CreateDirectory(newVM.Path);

            var data = Serializer.Write(newVM);
            var regkey = Configs.Open86BoxVmKey(true);
            regkey.SetValue(newVM.Name, data, RegistryValueKind.Binary);
            regkey.Close();

            Dialogs.ShowMessageBox($@"Virtual machine ""{newVM.Name}"" was successfully created!",
                MessageType.Info, ButtonsType.Ok, "Success");

            // Select the newly created VM
            foreach (var lvi in ui.lstVMs.GetSelItems())
            {
                lvi.Selected = false;
            }
            newLvi.Focused = true;
            newLvi.Selected = true;

            // Start the VM and/or open settings window if the user chose this option
            if (startVM)
            {
                Start();
            }
            if (openCFG)
            {
                Configure();
            }

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        // Changes a VM's name and/or description
        public static void Edit(string name, string desc)
        {
            var ui = Program.Root;
            var cfgpath = ui.cfgpath;

            var selected = ui.lstVMs.GetSelItems();
            var vm = selected[0].Tag;
            var oldname = vm.Name;
            if (!vm.Name.Equals(name))
            {
                try
                {
                    // Move the actual VM files too. This will invalidate any paths inside the cfg,
                    // but the user is informed to update those manually.
                    var sourceDir = Path.Combine(cfgpath, vm.Name);
                    var destDir = Path.Combine(cfgpath, name);
                    Directory.Move(sourceDir, destDir);
                }
                catch
                {
                    Dialogs.ShowMessageBox("An error has occurred while trying to move the files " +
                                           "for this virtual machine. Please try to move them manually.",
                        MessageType.Error, ButtonsType.Ok, "Error");
                }
                vm.Name = name;
                vm.Path = cfgpath + vm.Name;
            }
            vm.Desc = desc;

            // Create a new registry value with new info, delete the old one
            var regkey = Configs.Open86BoxVmKey(true);
            regkey.DeleteValue(oldname);

            var data = Serializer.Write(vm);
            regkey = Configs.Open86BoxVmKey(true);
            regkey.SetValue(vm.Name, data, RegistryValueKind.Binary);
            regkey.Close();

            Dialogs.ShowMessageBox($@"Virtual machine ""{vm.Name}"" was successfully modified." +
                                   " Please update its configuration so that any absolute paths" +
                                   " (e.g. for hard disk images) point to the new folder.",
                MessageType.Info, ButtonsType.Ok, "Success");
            Sort(ui.sortColumn, ui.sortOrder);
            ui.LoadVMs();
        }

        // Sends a running/pause VM a request to stop without asking the user for confirmation
        public static void ForceStop(IList<VMRow> selected, frmMain ui)
        {
            var vm = selected[0].Tag;
            try
            {
                if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
                {
                    Platforms.Manager.GetSender().DoVmForceStop(vm);
                }
            }
            catch (Exception)
            {
                Dialogs.ShowMessageBox("An error occurred trying to stop the selected virtual machine.",
                    MessageType.Error, ButtonsType.Ok, "Error");
            }

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        // Kills the process associated with the selected VM
        public static void Kill(IEnumerable<VMRow> selected, frmMain ui)
        {
            foreach (var lvi in selected)
            {
                var vm = lvi.Tag;

                //Ask the user to confirm
                var result = (ResponseType)Dialogs.ShowMessageBox(
                    $@"Killing a virtual machine can cause data loss. " +
                    "Only do this if 86Box executable process gets stuck. Do you " +
                    @$"really wish to kill the virtual machine ""{vm.Name}""?",
                    MessageType.Warning, ButtonsType.YesNo, "Warning");
                if (result == ResponseType.Yes)
                {
                    try
                    {
                        var p = Process.GetProcessById(vm.Pid);
                        p.Kill();
                    }
                    catch
                    {
                        Dialogs.ShowMessageBox($@"Could not kill 86Box.exe process for virtual " +
                                               @"machine ""{vm.Name}"". The process may have already " +
                                               "ended on its own or access was denied.",
                            MessageType.Error, ButtonsType.Ok, "Could not kill process");
                        continue;
                    }

                    // We need to cleanup afterwards to make sure the VM is put back into a valid state
                    vm.Status = VM.STATUS_STOPPED;
                    vm.hWnd = IntPtr.Zero;

                    lvi.SetStatus(vm.GetStatusString());
                    lvi.SetIcon(vm.Status);

                    ui.btnStart.Label = "Start";
                    ui.btnStart.SetToolTip("Stop this virtual machine");
                    ui.btnPause.Label = "Pause";

                    if (ui.lstVMs.GetSelItems().Count > 0)
                    {
                        ui.btnEdit.Sensitive = true;
                        ui.btnDelete.Sensitive = true;
                        ui.btnStart.Sensitive = true;
                        ui.btnConfigure.Sensitive = true;
                        ui.btnPause.Sensitive = false;
                        ui.btnReset.Sensitive = false;
                        ui.btnCtrlAltDel.Sensitive = false;
                    }
                    else
                    {
                        ui.btnEdit.Sensitive = false;
                        ui.btnDelete.Sensitive = false;
                        ui.btnStart.Sensitive = false;
                        ui.btnConfigure.Sensitive = false;
                        ui.btnPause.Sensitive = false;
                        ui.btnReset.Sensitive = false;
                        ui.btnCtrlAltDel.Sensitive = false;
                    }
                }
            }

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        // Deletes the config and nvr of selected VM
        public static void Wipe(IEnumerable<VMRow> selected)
        {
            foreach (var lvi in selected)
            {
                var vm = lvi.Tag;

                var result = (ResponseType)Dialogs.ShowMessageBox(
                    "Wiping a virtual machine deletes its configuration" +
                    " and nvr files. You'll have to reconfigure the virtual " +
                    "machine (and the BIOS if applicable).\n\n Are you sure " +
                    @$"you wish to wipe the virtual machine ""{vm.Name}""?",
                    MessageType.Warning, ButtonsType.YesNo, "Warning");
                if (result == ResponseType.Yes)
                {
                    if (vm.Status != VM.STATUS_STOPPED)
                    {
                        Dialogs.ShowMessageBox($@"The virtual machine ""{vm.Name}"" is currently " +
                                               "running and cannot be wiped. Please stop virtual machines " +
                                               "before attempting to wipe them.",
                            MessageType.Error, ButtonsType.Ok, "Success");
                        continue;
                    }
                    try
                    {
                        File.Delete(Path.Combine(vm.Path, "86box.cfg"));
                        Directory.Delete(Path.Combine(vm.Path, "nvr"), true);
                        Dialogs.ShowMessageBox($@"The virtual machine ""{vm.Name}"" was successfully wiped.",
                            MessageType.Info, ButtonsType.Ok, "Success");
                    }
                    catch (Exception ex)
                    {
                        Dialogs.ShowMessageBox($@"An error occurred trying to wipe the virtual machine ""{vm.Name}"".",
                            MessageType.Error, ButtonsType.Ok, ex.GetType().Name);
                    }
                }
            }
        }

        public static void OpenConfig(IEnumerable<VMRow> selected)
        {
            foreach (var lvi in selected)
            {
                var vm = lvi.Tag;
                try
                {
                    var file = IOPath.Combine(vm.Path, "86box.cfg");
                    Platforms.Shell.EditFile(file);
                }
                catch
                {
                    Dialogs.ShowMessageBox($@"The config file for the virtual machine ""{vm.Name}"" could" +
                                           " not be opened. Make sure it still exists and that you have " +
                                           "sufficient privileges to access it.",
                        MessageType.Error, ButtonsType.Ok, "Error");
                }
            }
        }

        // Opens the settings window for the selected VM
        public static void Configure()
        {
            var ui = Program.Root;

            var selected = ui.lstVMs.GetSelItems();
            var row = selected[0];
            var vm = row.Tag;

            // If the VM is already running, only send the message to open the settings window. 
            // Otherwise, start the VM with the -S parameter
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                Platforms.Manager.GetSender().DoVmConfigure(vm);
                Platforms.Shell.PushToForeground(vm.hWnd);
            }
            else if (vm.Status == VM.STATUS_STOPPED)
            {
                try
                {
                    var exec = Platforms.Manager.GetExecutor();
                    var info = exec.BuildConfigInfo(GetExecArgs(ui, vm, null));
                    if (!ui.showConsole)
                    {
                        info.RedirectStandardOutput = true;
                        info.UseShellExecute = false;
                    }
                    var p = Process.Start(info);
                    if (p == null)
                        throw new InvalidOperationException($"Could not start: {info.FileName}");
                    VMWatch.TryWaitForInputIdle(p, 250);

                    vm.Status = VM.STATUS_WAITING;
                    vm.hWnd = p.MainWindowHandle;
                    vm.Pid = p.Id;

                    row.SetStatus(vm.GetStatusString());
                    row.SetIcon(vm.Status);

                    var bgw = new BackgroundWorker
                    {
                        WorkerReportsProgress = false,
                        WorkerSupportsCancellation = false
                    };
                    var watch = new VMWatch(bgw);
                    _watch?.Dispose();
                    _watch = watch;
                    bgw.RunWorkerAsync(vm);

                    ui.btnStart.Sensitive = false;
                    ui.btnStart.Label = "Stop";
                    ui.btnStart.SetToolTip("Stop this virtual machine");
                    ui.startToolStripMenuItem.Text = "Stop";
                    ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                    ui.btnEdit.Sensitive = false;
                    ui.btnDelete.Sensitive = false;
                    ui.btnConfigure.Sensitive = false;
                    ui.btnReset.Sensitive = false;
                    ui.btnPause.Sensitive = false;
                    ui.btnPause.Label = "Pause";
                    ui.btnPause.SetToolTip("Pause this virtual machine");
                    ui.pauseToolStripMenuItem.Text = "Pause";
                    ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                    ui.btnCtrlAltDel.Sensitive = false;
                }
                catch (Win32Exception)
                {
                    Dialogs.ShowMessageBox("Cannot find 86Box executable. Make sure your " +
                                           "settings are correct and try again.",
                        MessageType.Error, ButtonsType.Ok, "Error");
                }
                catch (Exception ex)
                {
                    // Revert to stopped status and alert the user
                    vm.Status = VM.STATUS_STOPPED;
                    vm.hWnd = IntPtr.Zero;
                    vm.Pid = -1;
                    Dialogs.ShowMessageBox("This virtual machine could not be configured. Please " +
                                           "provide the following information to the developer:\n" +
                                           $"{ex.Message}\n{ex.StackTrace}",
                        MessageType.Error, ButtonsType.Ok, "Error");
                }
            }

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        // Sends a running/paused VM a request to stop and asking the user for confirmation
        public static void RequestStop(IList<VMRow> selected, frmMain ui)
        {
            var vm = selected[0].Tag;
            try
            {
                if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
                {
                    Platforms.Manager.GetSender().DoVmRequestStop(vm);
                    Platforms.Shell.PushToForeground(vm.hWnd);
                }
            }
            catch (Exception)
            {
                Dialogs.ShowMessageBox("An error occurred trying to stop the selected virtual machine.",
                    MessageType.Error, ButtonsType.Ok, "Error");
            }

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        // Starts the selected VM
        public static void Start()
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;

            try
            {
                var selected = lstVMs.GetSelItems();
                var row = selected[0];
                var vm = row.Tag;

                var id = VMWatch.GetTempId(vm);
                var idString = $"{id:X}".PadLeft(16, '0');

                if (vm.Status == VM.STATUS_STOPPED)
                {
                    var exec = Platforms.Manager.GetExecutor();
                    var info = exec.BuildStartInfo(GetExecArgs(ui, vm, idString));
                    if (!ui.showConsole)
                    {
                        info.RedirectStandardOutput = true;
                        info.UseShellExecute = false;
                    }
                    var p = Process.Start(info);
                    if (p == null)
                        throw new InvalidOperationException($"Could not start: {info.FileName}");

                    vm.Status = VM.STATUS_RUNNING;
                    vm.Pid = p.Id;

                    row.SetStatus(vm.GetStatusString());
                    row.SetIcon(1);

                    // Minimize the main window if the user wants this
                    if (ui.minimize)
                    {
                        ui.Iconify();
                    }

                    // Create a new background worker which will wait for the VM's window to
                    // close, so it can update the UI accordingly
                    var bgw = new BackgroundWorker
                    {
                        WorkerReportsProgress = false,
                        WorkerSupportsCancellation = false
                    };
                    var watch = new VMWatch(bgw);
                    _watch?.Dispose();
                    _watch = watch;
                    bgw.RunWorkerAsync(vm);

                    ui.btnStart.Sensitive = true;
                    ui.btnStart.Label = "Stop";
                    ui.btnStart.SetToolTip("Stop this virtual machine");
                    ui.btnEdit.Sensitive = false;
                    ui.btnDelete.Sensitive = false;
                    ui.btnPause.Sensitive = true;
                    ui.btnPause.Label = "Pause";
                    ui.btnReset.Sensitive = true;
                    ui.btnCtrlAltDel.Sensitive = true;
                    ui.btnConfigure.Sensitive = true;

                    CountRefresh();
                }
            }
            catch (InvalidOperationException)
            {
                Dialogs.ShowMessageBox("The process failed to initialize or its window " +
                                       "handle could not be obtained.",
                    MessageType.Error, ButtonsType.Ok, "Error");
            }
            catch (Win32Exception)
            {
                Dialogs.ShowMessageBox("Cannot find 86Box executable. Make sure your settings " +
                                       "are correct and try again.",
                    MessageType.Error, ButtonsType.Ok, "Error");
            }
            catch (Exception ex)
            {
                Dialogs.ShowMessageBox("An error has occurred. Please provide the following " +
                                       $"information to the developer:\n{ex.Message}\n{ex.StackTrace}",
                    MessageType.Error, ButtonsType.Ok, "Error");
            }

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        private static IExecVars GetExecArgs(frmMain ui, VM vm, string idString)
        {
            var hWndHex = ui.hWndHex;
            var vmPath = vm.Path;
            var exePath = ui.exepath;
            var exeName = Platforms.Env.ExeNames.First();

            var vars = new CommonExecVars
            {
                FileName = IOPath.Combine(exePath, exeName),
                VmPath = vmPath,
                Vm = vm,
                LogFile = ui.logging ? ui.logpath : null,
                Handle = idString != null ? (idString, hWndHex) : null
            };
            return vars;
        }

        // Sort the VM list by specified column and order
        public static void Sort(int column, SortType order)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;

            // const string ascArrow = " ▲";
            // const string descArrow = " ▼";

            if (lstVMs.GetSelItems().Count > 1)
            {
                // Just in case so we don't end up with weird selection glitches
                lstVMs.ClearSelect();
            }

            ui.sortColumn = column;
            ui.sortOrder = order;

            // Save the new column and order to the registry
            try
            {
                var regkey = Configs.Open86BoxKey(true);
                regkey.SetValue("SortColumn", ui.sortColumn, RegistryValueKind.DWord);
                regkey.SetValue("SortOrder", ui.sortOrder, RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {
                Dialogs.ShowMessageBox("Could not save the column sorting state to the " +
                                       "registry. Make sure you have the required permissions" +
                                       " and try again.",
                    MessageType.Error, ButtonsType.Ok, "Error");
            }
        }

        // Resumes the selected VM
        public static void Resume(IList<VMRow> selected, frmMain ui)
        {
            var row = selected[0];
            var vm = row.Tag;
            Platforms.Manager.GetSender().DoVmResume(vm);
            vm.Status = VM.STATUS_RUNNING;
            row.SetStatus(vm.GetStatusString());
            row.SetIcon(1);
            ui.pauseToolStripMenuItem.Text = "Pause";
            ui.btnPause.Label = "Pause";
            ui.btnStart.Sensitive = true;
            ui.startToolStripMenuItem.Text = "Stop";
            ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
            ui.btnConfigure.Sensitive = true;
            ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
            ui.btnStart.SetToolTip("Stop this virtual machine");
            ui.btnPause.SetToolTip("Pause this virtual machine");

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        // Pauses the selected VM
        public static void Pause(IList<VMRow> selected, frmMain ui)
        {
            var row = selected[0];
            var vm = row.Tag;
            Platforms.Manager.GetSender().DoVmPause(vm);
            row.SetStatus(vm.GetStatusString());
            row.SetIcon(2);
            ui.pauseToolStripMenuItem.Text = "Resume";
            ui.btnPause.Label = "Resume";
            ui.btnStart.SetToolTip("Stop this virtual machine");
            ui.btnStart.Sensitive = true;
            ui.btnStart.Label = "Stop";
            ui.startToolStripMenuItem.Text = "Stop";
            ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
            ui.btnConfigure.Sensitive = true;
            ui.pauseToolStripMenuItem.SetToolTip("Resume this virtual machine");
            ui.btnPause.SetToolTip("Resume this virtual machine");

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        // Performs a hard reset for the selected VM
        public static void HardReset(IList<VMRow> selected)
        {
            var vm = selected[0].Tag;
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                Platforms.Manager.GetSender().DoVmHardReset(vm);
                Platforms.Shell.PushToForeground(vm.hWnd);
            }
            CountRefresh();
        }

        // Removes the selected VM. Confirmations for maximum safety
        public static void Remove(IEnumerable<VMRow> selected, frmMain ui)
        {
            foreach (var lvi in selected)
            {
                var vm = lvi.Tag;
                var result1 = (ResponseType)Dialogs.ShowMessageBox($@"Are you sure you want to remove the" +
                                                                   @$" virtual machine ""{vm.Name}""?",
                    MessageType.Warning, ButtonsType.YesNo, "Remove virtual machine");

                if (result1 == ResponseType.Yes)
                {
                    if (vm.Status != VM.STATUS_STOPPED)
                    {
                        Dialogs.ShowMessageBox($@"Virtual machine ""{vm.Name}"" is currently " +
                                               "running and cannot be removed. Please stop virtual machines" +
                                               " before attempting to remove them.",
                            MessageType.Error, ButtonsType.Ok, "Error");
                        continue;
                    }
                    try
                    {
                        ui.lstVMs.RemoveItem(lvi);
                        var regkey = Configs.Open86BoxVmKey(true);
                        regkey.DeleteValue(vm.Name);
                        regkey.Close();
                    }
                    catch (Exception ex) // Catches "regkey doesn't exist" exceptions and such
                    {
                        Dialogs.ShowMessageBox(@$"Virtual machine ""{vm.Name}"" could not be removed due to " +
                                               $"the following error:\n\n{ex.Message}",
                            MessageType.Error, ButtonsType.Ok, "Error");
                        continue;
                    }

                    var result2 = Dialogs.ShowMessageBox($@"Virtual machine ""{vm.Name}"" was " +
                                                         "successfully removed. Would you like to delete" +
                                                         " its files as well?",
                        MessageType.Question, ButtonsType.YesNo, "Virtual machine removed");
                    if (result2 == (int)ResponseType.Yes)
                    {
                        try
                        {
                            Directory.Delete(vm.Path, true);
                        }
                        catch (UnauthorizedAccessException) //Files are read-only or protected by privileges
                        {
                            Dialogs.ShowMessageBox("86Box Manager was unable to delete the files of this " +
                                                   "virtual machine because they are read-only or you don't " +
                                                   "have sufficient privileges to delete them.\n\nMake sure " +
                                                   "the files are free for deletion, then remove them manually.",
                                MessageType.Error, ButtonsType.Ok, "Error");
                            continue;
                        }
                        catch (DirectoryNotFoundException) //Directory not found
                        {
                            Dialogs.ShowMessageBox("86Box Manager was unable to delete the files of this " +
                                                   "virtual machine because they no longer exist.",
                                MessageType.Error, ButtonsType.Ok, "Error");
                            continue;
                        }
                        catch (IOException) //Files are in use by another process
                        {
                            Dialogs.ShowMessageBox("86Box Manager was unable to delete some files of this " +
                                                   "virtual machine because they are currently in use by " +
                                                   "another process.\n\nMake sure the files are free for " +
                                                   "deletion, then remove them manually.",
                                MessageType.Error, ButtonsType.Ok, "Error");
                            continue;
                        }
                        catch (Exception ex) //Other exceptions
                        {
                            Dialogs.ShowMessageBox($"The following error occurred while trying to remove" +
                                                   $" the files of this virtual machine:\n\n{ex.Message}",
                                MessageType.Error, ButtonsType.Ok, "Error");
                            continue;
                        }
                        Dialogs.ShowMessageBox($@"Files of virtual machine ""{vm.Name}"" were successfully deleted.",
                            MessageType.Info, ButtonsType.Ok, "Virtual machine files removed");
                    }
                }
            }

            Sort(ui.sortColumn, ui.sortOrder);
            CountRefresh();
        }

        // Opens the folder containing the selected VM
        public static void OpenFolder(IEnumerable<VMRow> selected)
        {
            foreach (var lvi in selected)
            {
                var vm = lvi.Tag;
                try
                {
                    Platforms.Shell.OpenFolder(vm.Path);
                }
                catch
                {
                    Dialogs.ShowMessageBox($@"The folder for the virtual machine ""{vm.Name}"" could" +
                                           " not be opened. Make sure it still exists and that you have " +
                                           "sufficient privileges to access it.",
                        MessageType.Error, ButtonsType.Ok, "Error");
                }
            }
        }

        // Sends the CTRL+ALT+DEL keystroke to the VM, result depends on the guest OS
        public static void CtrlAltDel(IList<VMRow> selected, frmMain ui)
        {
            var row = selected[0];
            var vm = row.Tag;
            if (vm.Status == VM.STATUS_RUNNING || vm.Status == VM.STATUS_PAUSED)
            {
                Platforms.Manager.GetSender().DoVmCtrlAltDel(vm);
                vm.Status = VM.STATUS_RUNNING;
                row.SetStatus(vm.GetStatusString());
                ui.btnPause.Label = "Pause";
                ui.btnPause.SetToolTip("Pause this virtual machine");
                ui.pauseToolStripMenuItem.Text = "Pause";
                ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
            }
            CountRefresh();
        }

        // Refreshes the VM counter in the status bar
        public static void CountRefresh()
        {
            var ui = Program.Root;

            var runningVMs = 0;
            var pausedVMs = 0;
            var waitingVMs = 0;
            var stoppedVMs = 0;

            var vms = ui.lstVMs.GetAllItems();
            foreach (var item in vms)
            {
                var vm = item.Tag;
                switch (vm.Status)
                {
                    case VM.STATUS_PAUSED:
                        pausedVMs++;
                        break;
                    case VM.STATUS_RUNNING:
                        runningVMs++;
                        break;
                    case VM.STATUS_STOPPED:
                        stoppedVMs++;
                        break;
                    case VM.STATUS_WAITING:
                        waitingVMs++;
                        break;
                }
            }

            ui.lblVMCount.Text = "All VMs: " + vms.Count + " | Running: " + runningVMs + " | Paused: " + pausedVMs +
                                 " | Waiting: " + waitingVMs + " | Stopped: " + stoppedVMs;
        }
    }
}