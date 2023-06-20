using System;
using _86boxManager.Tools;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;

namespace _86boxManager.View
{
    partial class dlgCloneVM
    {
        private void InitializeComponent()
        {
            Shown += dlgCloneVM_Load;
            txtName.Changed += txtName_TextChanged;
            btnClone.Clicked += btnClone_Click;
        }

        [UI] private Label lblPath1 = null;
        [UI] private Label lblOldVM = null;
        [UI] private Entry txtName = null;
        [UI] private Entry txtDescription = null;
        [UI] private Button btnClone = null;
        [UI] private CheckButton cbxOpenCFG = null;
        [UI] private CheckButton cbxStartVM = null;
    }
}