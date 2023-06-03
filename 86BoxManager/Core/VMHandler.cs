using System;
using _86BoxManager.API;
using _86boxManager.Models;
using _86boxManager.Tools;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;

// ReSharper disable InconsistentNaming
namespace _86boxManager.Core
{
    internal sealed class VMHandler : IMessageReceiver
    {
        private static uint GetTempId(VM vm)
        {
            // This can return negative integers, which is a no-no for 86Box,
            // hence the shift up by int.MaxValue
            var tempId = vm.Path.GetHashCode();
            uint id;
            if (tempId < 0)
                id = (uint)(tempId + int.MaxValue);
            else
                id = (uint)tempId;
            return id;
        }

        public void OnEmulatorInit(IntPtr hWnd, uint vmId)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;
            var items = lstVMs.GetAllItems();

            foreach (var lvi in items)
            {
                var vm = lvi.Tag;
                var id = GetTempId(vm);
                if (id != vmId)
                    continue;

                vm.hWnd = hWnd;
                break;
            }
        }

        public void OnEmulatorShutdown(IntPtr hWnd)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;
            var items = lstVMs.GetAllItems();

            foreach (var lvi in items)
            {
                var vm = lvi.Tag;
                if (!vm.hWnd.Equals(hWnd) || vm.Status == VM.STATUS_STOPPED)
                    continue;

                vm.Status = VM.STATUS_STOPPED;
                vm.hWnd = IntPtr.Zero;
                lvi.SetStatus(vm.GetStatusString());
                lvi.SetIcon(0);

                ui.btnStart.Content = "Start";
                // TODO ui.startToolStripMenuItem.Text = "Start";
                // TODO ui.startToolStripMenuItem.SetToolTip("Start this virtual machine");
                ui.btnStart.SetToolTip("Start this virtual machine");
                ui.btnPause.Content = "Pause";
                // TODO ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                // TODO ui.pauseToolStripMenuItem.Text = "Pause";
                ui.btnPause.SetToolTip("Pause this virtual machine");

                var selectedItems = lstVMs.GetSelItems();

                if (selectedItems.Count == 1)
                {
                    ui.btnEdit.IsEnabled = true;
                    ui.btnDelete.IsEnabled = true;
                    ui.btnStart.IsEnabled = true;
                    ui.btnConfigure.IsEnabled = true;
                    ui.btnPause.IsEnabled = false;
                    ui.btnReset.IsEnabled = false;
                    ui.btnCtrlAltDel.IsEnabled = false;
                }
                else if (selectedItems.Count == 0)
                {
                    ui.btnEdit.IsEnabled = false;
                    ui.btnDelete.IsEnabled = false;
                    ui.btnStart.IsEnabled = false;
                    ui.btnConfigure.IsEnabled = false;
                    ui.btnPause.IsEnabled = false;
                    ui.btnReset.IsEnabled = false;
                    ui.btnCtrlAltDel.IsEnabled = false;
                }
                else
                {
                    ui.btnEdit.IsEnabled = false;
                    ui.btnDelete.IsEnabled = true;
                    ui.btnStart.IsEnabled = false;
                    ui.btnConfigure.IsEnabled = false;
                    ui.btnPause.IsEnabled = false;
                    ui.btnReset.IsEnabled = false;
                    ui.btnCtrlAltDel.IsEnabled = false;
                }
            }
            VMCenter.CountRefresh();
        }

        public void OnVmPaused(IntPtr hWnd)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;
            var items = lstVMs.GetAllItems();

            foreach (var lvi in items)
            {
                var vm = lvi.Tag;
                if (!vm.hWnd.Equals(hWnd) || vm.Status == VM.STATUS_PAUSED)
                    continue;

                vm.Status = VM.STATUS_PAUSED;
                lvi.SetStatus(vm.GetStatusString());
                lvi.SetIcon(2);
                // TODO ui.pauseToolStripMenuItem.Text = "Resume";
                ui.btnPause.Content = "Resume";
                // TODO ui.pauseToolStripMenuItem.SetToolTip("Resume this virtual machine");
                ui.btnPause.SetToolTip("Resume this virtual machine");
                ui.btnStart.IsEnabled = true;
                ui.btnStart.Content = "Stop";
                // TODO ui.startToolStripMenuItem.Text = "Stop";
                // TODO ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                ui.btnStart.SetToolTip("Stop this virtual machine");
                ui.btnConfigure.IsEnabled = true;
            }
            VMCenter.CountRefresh();
        }

        public void OnVmResumed(IntPtr hWnd)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;
            var items = lstVMs.GetAllItems();

            foreach (var lvi in items)
            {
                var vm = lvi.Tag;
                if (!vm.hWnd.Equals(hWnd) || vm.Status == VM.STATUS_RUNNING)
                    continue;

                vm.Status = VM.STATUS_RUNNING;
                lvi.SetStatus(vm.GetStatusString());
                lvi.SetIcon(1);
                // TODO ui.pauseToolStripMenuItem.Text = "Pause";
                ui.btnPause.Content = "Pause";
                ui.btnPause.SetToolTip("Pause this virtual machine");
                // TODO ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                ui.btnStart.IsEnabled = true;
                ui.btnStart.Content = "Stop";
                ui.btnStart.SetToolTip("Stop this virtual machine");
                // TODO ui.startToolStripMenuItem.Text = "Stop";
                // TODO ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                ui.btnConfigure.IsEnabled = true;
            }
            VMCenter.CountRefresh();
        }

        public void OnDialogOpened(IntPtr hWnd)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;
            var items = lstVMs.GetAllItems();

            foreach (var lvi in items)
            {
                var vm = lvi.Tag;
                if (!vm.hWnd.Equals(hWnd) || vm.Status == VM.STATUS_WAITING)
                    continue;

                vm.Status = VM.STATUS_WAITING;
                lvi.SetStatus(vm.GetStatusString());
                lvi.SetIcon(2);
                ui.btnStart.IsEnabled = false;
                ui.btnStart.Content = "Stop";
                ui.btnStart.SetToolTip("Stop this virtual machine");
                // TODO ui.startToolStripMenuItem.Text = "Stop";
                // TODO ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                ui.btnEdit.IsEnabled = false;
                ui.btnDelete.IsEnabled = false;
                ui.btnConfigure.IsEnabled = false;
                ui.btnReset.IsEnabled = false;
                ui.btnPause.IsEnabled = false;
                ui.btnCtrlAltDel.IsEnabled = false;
            }
            VMCenter.CountRefresh();
        }

        public void OnDialogClosed(IntPtr hWnd)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;
            var items = lstVMs.GetAllItems();

            foreach (var lvi in items)
            {
                var vm = lvi.Tag;
                if (!vm.hWnd.Equals(hWnd) || vm.Status == VM.STATUS_RUNNING)
                    continue;

                vm.Status = VM.STATUS_RUNNING;
                lvi.SetStatus(vm.GetStatusString());
                lvi.SetIcon(1);
                ui.btnStart.IsEnabled = true;
                ui.btnStart.Content = "Stop";
                ui.btnStart.SetToolTip("Stop this virtual machine");
                // TODO ui.startToolStripMenuItem.Text = "Stop";
                // TODO ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                ui.btnEdit.IsEnabled = false;
                ui.btnDelete.IsEnabled = false;
                ui.btnConfigure.IsEnabled = true;
                ui.btnReset.IsEnabled = true;
                ui.btnPause.IsEnabled = true;
                ui.btnPause.Content = "Pause";
                // TODO ui.pauseToolStripMenuItem.Text = "Pause";
                // TODO ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                ui.btnPause.SetToolTip("Pause this virtual machine");
                ui.btnCtrlAltDel.IsEnabled = true;
            }
            VMCenter.CountRefresh();
        }

        public void OnManagerStartVm(string vmName)
        {
            var ui = Program.Root;
            var lstVMs = ui.lstVMs;

            var lvi = lstVMs.FindItemWithText(vmName);

            // This check is necessary in case the specified VM was already removed but the shortcut remains
            if (lvi != null)
            {
                var vm = lvi.Tag;

                // If the VM is already running, display a message, otherwise, start it
                if (vm.Status != VM.STATUS_STOPPED)
                {
                    Dialogs.ShowMessageBox($@"The virtual machine ""{vmName}"" is already running.",
                        MessageType.Info, ButtonsType.Ok, "Virtual machine already running");
                }
                else
                {
                    // This is needed so that we start the correct VM in case multiple items are selected
                    lstVMs.ClearSelect();

                    lvi.Focused = true;
                    lvi.Selected = true;
                    VMCenter.Start();
                }
                return;
            }

            Dialogs.ShowMessageBox($@"The virtual machine ""{vmName}"" could not be found. " +
                                   "It may have been removed or the specified name is incorrect.",
                MessageType.Error, ButtonsType.Ok, "Virtual machine not found");
        }
    }
}