using System;
using _86boxManager.Tools;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;

namespace _86boxManager.View
{
    partial class frmMain
    {
        private void InitializeComponent()
        {
            Shown += frmMain_Load;
            WindowStateEvent += Window_StateChanged;
            ResizeChecked += frmMain_Resize;

            btnAdd.Clicked += btnAdd_Click;
            btnEdit.Clicked += btnEdit_Click;
            btnStart.Clicked += btnStart_Click;
            btnSettings.Clicked += btnSettings_Click;
            btnConfigure.Clicked += btnConfigure_Click;
            btnCtrlAltDel.Clicked += btnCtrlAltDel_Click;
            btnReset.Clicked += btnReset_Click;
            btnPause.Clicked += btnPause_Click;
            btnDelete.Clicked += btnDelete_Click;

            lstVMs.Selection.Changed += lstVMs_SelectedIndexChanged;
            lstVMs.ButtonReleaseEvent += OnTreeButtonRelease;
            lstVMs.ButtonPressEvent += lstVMs_MouseDoubleClick;
            lstVMs.KeyReleaseEvent += lstVMs_KeyDown;

            clmName.Clicked += lstVMs_ColumnClick;
            clmStatus.Clicked += lstVMs_ColumnClick;
            clmDesc.Clicked += lstVMs_ColumnClick;
            clmPath.Clicked += lstVMs_ColumnClick;

            openConfigFileToolStripMenuItem.Clicked += openConfigFileToolStripMenuItem_Click;
            openConfigFileToolStripMenuItem.Sensitive = true;
            killToolStripMenuItem.Clicked += killToolStripMenuItem_Click;
            killToolStripMenuItem.Sensitive = true;
            wipeToolStripMenuItem.Clicked += wipeToolStripMenuItem_Click;
            wipeToolStripMenuItem.Sensitive = true;
            cloneToolStripMenuItem.Clicked += cloneToolStripMenuItem_Click;
            cloneToolStripMenuItem.Sensitive = true;
            pauseToolStripMenuItem.Clicked += pauseToolStripMenuItem_Click;
            pauseToolStripMenuItem.Sensitive = true;
            hardResetToolStripMenuItem.Clicked += hardResetToolStripMenuItem_Click;
            hardResetToolStripMenuItem.Sensitive = true;
            deleteToolStripMenuItem.Clicked += deleteToolStripMenuItem_Click;
            deleteToolStripMenuItem.Sensitive = true;
            editToolStripMenuItem.Clicked += editToolStripMenuItem_Click;
            editToolStripMenuItem.Sensitive = true;
            openFolderToolStripMenuItem.Clicked += openFolderToolStripMenuItem_Click;
            openFolderToolStripMenuItem.Sensitive = true;
            configureToolStripMenuItem.Clicked += configureToolStripMenuItem_Click;
            configureToolStripMenuItem.Sensitive = true;
            resetCTRLALTDELETEToolStripMenuItem.Clicked += resetCTRLALTDELETEToolStripMenuItem_Click;
            resetCTRLALTDELETEToolStripMenuItem.Sensitive = true;
            startToolStripMenuItem.Clicked += startToolStripMenuItem_Click;
            startToolStripMenuItem.Sensitive = true;
            createADesktopShortcutToolStripMenuItem.Clicked += createADesktopShortcutToolStripMenuItem_Click;
            createADesktopShortcutToolStripMenuItem.Sensitive = true;

            trayIcon.ApplyIcon(Program.LoadIcon());
            trayIcon.PopupMenu += OnTrayPopup;
            trayIcon.ButtonPressEvent += trayIcon_MouseDoubleClick;
            open86BoxManagerToolStripMenuItem.Activated += open86BoxManagerToolStripMenuItem_Click;
            settingsToolStripMenuItem.Activated += settingsToolStripMenuItem_Click;
            exitToolStripMenuItem.Activated += exitToolStripMenuItem_Click;
        }

        [UI] private Button btnAdd = null;
        [UI] internal Button btnEdit = null;
        [UI] private Button btnSettings = null;
        [UI] internal Button btnPause = null;
        [UI] internal Button btnCtrlAltDel = null;
        [UI] internal Button btnReset = null;
        [UI] internal Button btnDelete = null;
        [UI] internal Button btnStart = null;
        [UI] internal Button btnConfigure = null;
        [UI] internal TreeView lstVMs = null;
        [UI] private PopoverMenu lstVMpop = null;
        [UI] internal Label lblVMCount = null;
        [UI] private TreeViewColumn clmName = null;
        [UI] private TreeViewColumn clmStatus = null;
        [UI] private TreeViewColumn clmDesc = null;
        [UI] private TreeViewColumn clmPath = null;

        [UI] private ModelButton configureToolStripMenuItem = null;
        [UI] internal ModelButton pauseToolStripMenuItem = null;
        [UI] internal ModelButton startToolStripMenuItem = null;
        [UI] private ModelButton resetCTRLALTDELETEToolStripMenuItem = null;
        [UI] private ModelButton wipeToolStripMenuItem = null;
        [UI] private ModelButton deleteToolStripMenuItem = null;
        [UI] private ModelButton killToolStripMenuItem = null;
        [UI] private ModelButton hardResetToolStripMenuItem = null;
        [UI] private ModelButton cloneToolStripMenuItem = null;
        [UI] private ModelButton editToolStripMenuItem = null;
        [UI] private ModelButton openFolderToolStripMenuItem = null;
        [UI] private ModelButton openConfigFileToolStripMenuItem = null;
        [UI] private ModelButton createADesktopShortcutToolStripMenuItem = null;

        [UI] private StatusIcon trayIcon = null;
        [UI] private Menu cmsTrayIcon = null;
        [UI] private MenuItem exitToolStripMenuItem = null;
        [UI] private MenuItem settingsToolStripMenuItem = null;
        [UI] private MenuItem open86BoxManagerToolStripMenuItem = null;
    }
}