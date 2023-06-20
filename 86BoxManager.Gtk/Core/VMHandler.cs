using System;
using _86BoxManager.API;
using _86BoxManager.Model;
using _86BoxManager.Models;
using _86BoxManager.Tools;
using Gtk;

// ReSharper disable InconsistentNaming
namespace _86BoxManager.Core
{
    internal sealed class VMHandler : IMessageReceiver
    {
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

                ui.btnStart.Label = "Start";
                ui.startToolStripMenuItem.Text = "Start";
                ui.startToolStripMenuItem.SetToolTip("Start this virtual machine");
                ui.btnStart.SetToolTip("Start this virtual machine");
                ui.btnPause.Label = "Pause";
                ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                ui.pauseToolStripMenuItem.Text = "Pause";
                ui.btnPause.SetToolTip("Pause this virtual machine");

                var selectedItems = lstVMs.GetSelItems();

                if (selectedItems.Count == 1)
                {
                    ui.btnEdit.Sensitive = true;
                    ui.btnDelete.Sensitive = true;
                    ui.btnStart.Sensitive = true;
                    ui.btnConfigure.Sensitive = true;
                    ui.btnPause.Sensitive = false;
                    ui.btnReset.Sensitive = false;
                    ui.btnCtrlAltDel.Sensitive = false;
                }
                else if (selectedItems.Count == 0)
                {
                    ui.btnEdit.Sensitive = false;
                    ui.btnDelete.Sensitive = false;
                    ui.btnStart.Sensitive = false;
                    ui.btnConfigure.Sensitive = false;
                    ui.btnPause.Sensitive = false;
                    ui.btnReset.Sensitive = false;
                    ui.btnCtrlAltDel.Sensitive = false;
                }
                else
                {
                    ui.btnEdit.Sensitive = false;
                    ui.btnDelete.Sensitive = true;
                    ui.btnStart.Sensitive = false;
                    ui.btnConfigure.Sensitive = false;
                    ui.btnPause.Sensitive = false;
                    ui.btnReset.Sensitive = false;
                    ui.btnCtrlAltDel.Sensitive = false;
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
                ui.pauseToolStripMenuItem.Text = "Resume";
                ui.btnPause.Label = "Resume";
                ui.pauseToolStripMenuItem.SetToolTip("Resume this virtual machine");
                ui.btnPause.SetToolTip("Resume this virtual machine");
                ui.btnStart.Sensitive = true;
                ui.btnStart.Label = "Stop";
                ui.startToolStripMenuItem.Text = "Stop";
                ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                ui.btnStart.SetToolTip("Stop this virtual machine");
                ui.btnConfigure.Sensitive = true;
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
                ui.pauseToolStripMenuItem.Text = "Pause";
                ui.btnPause.Label = "Pause";
                ui.btnPause.SetToolTip("Pause this virtual machine");
                ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                ui.btnStart.Sensitive = true;
                ui.btnStart.Label = "Stop";
                ui.btnStart.SetToolTip("Stop this virtual machine");
                ui.startToolStripMenuItem.Text = "Stop";
                ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                ui.btnConfigure.Sensitive = true;
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
                ui.btnCtrlAltDel.Sensitive = false;
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
                ui.btnStart.Sensitive = true;
                ui.btnStart.Label = "Stop";
                ui.btnStart.SetToolTip("Stop this virtual machine");
                ui.startToolStripMenuItem.Text = "Stop";
                ui.startToolStripMenuItem.SetToolTip("Stop this virtual machine");
                ui.btnEdit.Sensitive = false;
                ui.btnDelete.Sensitive = false;
                ui.btnConfigure.Sensitive = true;
                ui.btnReset.Sensitive = true;
                ui.btnPause.Sensitive = true;
                ui.btnPause.Label = "Pause";
                ui.pauseToolStripMenuItem.Text = "Pause";
                ui.pauseToolStripMenuItem.SetToolTip("Pause this virtual machine");
                ui.btnPause.SetToolTip("Pause this virtual machine");
                ui.btnCtrlAltDel.Sensitive = true;
            }
            VMCenter.CountRefresh();
        }

        public void OnManagerStartVm(string vmName)
        {
            Application.Invoke(delegate { OnManagerStartVmGtk(vmName); });
        }

        private void OnManagerStartVmGtk(string vmName)
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