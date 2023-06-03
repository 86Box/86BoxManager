using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using _86boxManager.Tools;
using _86boxManager.Models;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;

// ReSharper disable InconsistentNaming

namespace _86boxManager.Core
{
    public sealed class VMWatch
    {
        private readonly BackgroundWorker _bgw;

        public VMWatch(BackgroundWorker bgw)
        {
            _bgw = bgw;
            _bgw.DoWork += background_DoWork;
            _bgw.RunWorkerCompleted += background_RunCompleted;
        }

        public void Dispose()
        {
            _bgw.DoWork -= background_DoWork;
            _bgw.RunWorkerCompleted -= background_RunCompleted;
            _bgw.Dispose();
        }

        // Wait for the associated window of a VM to close
        private void background_DoWork(object sender, DoWorkEventArgs e)
        {
            var vm = e.Argument as VM;
            try
            {
                // Find the process associated with the VM
                var p = Process.GetProcessById(vm.Pid);

                // Wait for it to exit
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                Dialogs.ShowMessageBox("An error has occurred. Please provide the following details" +
                                       $" to the developer:\n{ex.Message}\n{ex.StackTrace}",
                    MessageType.Error, ButtonsType.Ok, "Error");
            }
            e.Result = vm;
        }

        // Update the UI once the VM's window is closed
        private void background_RunCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;
            var vm = e.Result as VM;

            var allItems = lstVMs.GetAllItems();
            var selected = lstVMs.GetSelItems();

            // Go through the listview, find the item representing the VM and update things accordingly
            foreach (var item in allItems)
            {
                if (item.Tag.Equals(vm))
                {
                    vm.Status = VM.STATUS_STOPPED;
                    vm.hWnd = IntPtr.Zero;
                    item.SetStatus(vm.GetStatusString());
                    item.SetIcon(vm.Status);

                    if (vm.OnExit != null)
                    {
                        vm.OnExit(vm);
                        vm.OnExit = null;
                    }

                    if (selected.Count > 0 && selected[0].Equals(item))
                    {
                        ui.btnEdit.IsEnabled = true;
                        ui.btnDelete.IsEnabled = true;
                        ui.btnStart.IsEnabled = true;
                        ui.btnStart.Content = "Start";
                        ui.btnStart.SetToolTip("Start this virtual machine");
                        ui.btnConfigure.IsEnabled = true;
                        ui.btnPause.IsEnabled = false;
                        ui.btnPause.Content = "Pause";
                        ui.btnCtrlAltDel.IsEnabled = false;
                        ui.btnReset.IsEnabled = false;
                    }
                }
            }

            VMCenter.CountRefresh();
        }

        public static bool TryWaitForInputIdle(Process process, int forceDelay)
        {
            try
            {
                return process.WaitForInputIdle();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(forceDelay);
                return false;
            }
        }

        public static uint GetTempId(VM vm)
        {
            /* This generates a VM ID on the fly from the VM path. The reason it's done this way is
                 * it doesn't break existing VMs and doesn't require extensive modifications to this
                 * legacy version for it to work with newer 86Box versions...
                 * IDs also have to be unsigned for 86Box, but GetHashCode() returns signed and result
                 * can be negative, so shift it up by int.MaxValue to ensure it's always positive. */

            var tempid = vm.Path.GetHashCode();
            uint id = 0;

            if (tempid < 0)
                id = (uint)(tempid + int.MaxValue);
            else
                id = (uint)tempid;

            return id;
        }
    }
}