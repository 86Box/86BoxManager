using System;
using Avalonia.Controls;

namespace _86boxManager.Views
{
    public partial class frmMain : Window
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Main_OnOpened(object sender, EventArgs e)
        {
            if (Program.Root == null)
            {
                Program.Root = this;
                Main_OnOpened_FirstBoot();
            }
        }

        private void Main_OnOpened_FirstBoot()
        {
            // TODO
        }
    }
}