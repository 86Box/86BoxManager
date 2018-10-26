using System;
using System.Windows.Forms;

namespace _86boxManager
{
    public partial class dlgAbout : Form
    {
        public dlgAbout()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Open the link in the user's default web browser
        private void lnkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkGithub.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/86Box/86BoxManager");
        }

        private void lnkGuthub2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkGithub2.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/86Box/86Box");
        }

        private void dlgAbout_Load(object sender, EventArgs e)
        {
            lblVersion1.Text = Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 2);
            if (Program.PRERELEASE)
            {
                lblVersion1.Text += " Pre-release";
            }
        }
    }
}