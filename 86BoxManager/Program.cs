using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace _86boxManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Check if 86box.exe is already running
            Process[] pname = Process.GetProcessesByName("86box");
            if (pname.Length > 0)
            {
                MessageBox.Show("At least one instance of 86box is already running. Please close all instances of 86box and restart this program.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //Check if 86manager.exe is already running
                pname = Process.GetProcessesByName("86manager");
                if (pname.Length > 1)
                {
                    MessageBox.Show("86Box Manager is already running. You can only run one instance at a time.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmMain());
                }
            }
        }
    }
}
