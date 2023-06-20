using System;
using _86BoxManager.Tools;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;

namespace _86BoxManager.View
{
    partial class dlgSettings
    {
        private void InitializeComponent()
        {
            aboutImg.Pixbuf = Program.LoadIcon(64);
            Shown += dlgSettings_Load;
            Close += dlgSettings_FormClosing;
            
            btnApply.Clicked += btnApply_Click;
            btnOK.Clicked += btnOK_Click;
            btnBrowse1.Clicked += btnBrowse1_Click;
            btnBrowse2.Clicked += btnBrowse2_Click;
            btnBrowse3.Clicked += btnBrowse3_Click;
            btnDefaults.Clicked += btnDefaults_Click;

            lnkGithub2.Clicked += lnkGithub2_LinkClicked;
            lnkGithub.Clicked += lnkGithub_LinkClicked;

            txtEXEdir.Changed += txt_TextChanged;
            txtCFGdir.Changed += txt_TextChanged;

            cbxShowConsole.Toggled += cbx_CheckedChanged;
            cbxMinimizeTray.Toggled += cbx_CheckedChanged;
            cbxCloseTray.Toggled += cbx_CheckedChanged;
            cbxMinimize.Toggled += cbx_CheckedChanged;
            cbxGrid.Toggled += cbx_CheckedChanged;
            cbxLogging.Toggled += cbxLogging_CheckedChanged;
        }

        [UI] private Label lbl86BoxVer1 = null;
        [UI] private Label lblVersion1 = null;
        [UI] private Button btnApply = null;
        [UI] private Button btnDefaults = null;
        [UI] private Button btnOK = null;
        [UI] private Entry txtEXEdir = null;
        [UI] private Entry txtCFGdir = null;
        [UI] private Entry txtLogPath = null;
        [UI] private CheckButton cbxLogging = null;
        [UI] private CheckButton cbxMinimize = null;
        [UI] private CheckButton cbxShowConsole = null;
        [UI] private CheckButton cbxMinimizeTray = null;
        [UI] private CheckButton cbxGrid = null;
        [UI] private CheckButton cbxCloseTray = null;
        [UI] private Button btnBrowse3 = null;
        [UI] private Button btnBrowse2 = null;
        [UI] private Button btnBrowse1 = null;
        [UI] private LinkButton lnkGithub = null;
        [UI] private LinkButton lnkGithub2 = null;
        [UI] private Image aboutImg = null;
    }
}