using System;

namespace _86BoxManager.API
{
    public interface IShell
    {
        void CreateShortcut(string address, string name, string desc, string startup);

        void PushToForeground(IntPtr window);

        void PrepareAppId(string appId);

        void OpenFolder(string folder);

        void EditFile(string file);
    }
}