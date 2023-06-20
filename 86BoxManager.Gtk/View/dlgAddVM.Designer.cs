using System;
using _86boxManager.Tools;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;

namespace _86boxManager.View
{
    partial class dlgAddVM
    {
        private void InitializeComponent()
        {
            Shown += dlgAddVM_Load;
            cbxImport.Toggled += cbxImport_CheckedChanged;
            btnBrowse.Clicked += btnBrowse_Click;
            txtName.Changed += txtName_TextChanged;
            btnAdd.Clicked += btnAdd_Click;
        }

        [UI] private Label lblPath1 = null;
        [UI] private Entry txtName = null;
        [UI] private Entry txtDescription = null;
        [UI] private Entry txtImportPath = null;
        [UI] private CheckButton cbxOpenCFG = null;
        [UI] private CheckButton cbxStartVM = null;
        [UI] private CheckButton cbxImport = null;
        [UI] private Button btnBrowse = null;
        [UI] private Button btnAdd = null;
    }
}