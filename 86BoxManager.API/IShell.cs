using System;

namespace _86BoxManager.API
{
    public interface IShell
    {
        void CreateShortcut(string address, string name, string desc, string startup);

        void ForceStop(IntPtr window);

        void RequestStop(IntPtr window);

        void PushToForeground(IntPtr window);

        void Resume(IntPtr window);

        void Pause(IntPtr window);

        void Configure(IntPtr window);

        void HardReset(IntPtr window);

        void CtrlAltDel(IntPtr window);

        void PrepareAppId(string appId);

        void OpenFolder(string folder);

        void EditFile(string file);
    }
}