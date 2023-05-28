using System;

namespace _86BoxManager.API
{
    public interface IManager
    {
        bool IsFirstInstance(string name);

        IntPtr RestoreAndFocus(string title);

        void StartVmInside(string message, IntPtr hWnd);

        bool IsProcessRunning(string name);

        string GetVmName(object raw);

        IVerInfo GetBoxVersion(string exeDir);
        
        string FormatBoxArgs(string vmPath, string idString, string hWndHex);
    }
}