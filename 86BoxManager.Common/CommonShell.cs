using System;
using System.Diagnostics;
using _86BoxManager.API;

namespace _86BoxManager.Common
{
    public abstract class CommonShell : IShell
    {
        public abstract void CreateShortcut(string address, string name, string desc, string startup);

        public virtual void PushToForeground(IntPtr window)
        {
            // NO-OP
        }

        public virtual void PrepareAppId(string appId)
        {
            // NO-OP
        }

        public virtual void OpenFolder(string folder)
        {
            Process.Start(new ProcessStartInfo(folder)
            {
                UseShellExecute = true
            });
        }

        public virtual void EditFile(string file)
        {
            Process.Start(new ProcessStartInfo(file)
            {
                UseShellExecute = true
            });
        }
    }
}