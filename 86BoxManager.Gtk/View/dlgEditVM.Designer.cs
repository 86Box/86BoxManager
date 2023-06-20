using System;
using _86BoxManager.Tools;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;

namespace _86BoxManager.View
{
    partial class dlgEditVM
    {
        private void InitializeComponent()
        {
            Shown += dlgEditVM_Load;
            txtName.Changed += txtName_TextChanged;
            btnApply.Clicked += btnApply_Click;
        }

        [UI] private Entry txtName = null;
        [UI] private Entry txtDesc = null;
        [UI] private Label lblPath1 = null;
        [UI] private Button btnApply = null;
    }
}