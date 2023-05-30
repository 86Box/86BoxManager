using System;

namespace _86BoxManager.API
{
    public interface IManager
    {
        bool IsFirstInstance(string name);

        IntPtr RestoreAndFocus(string title, string handleTitle);

        bool IsProcessRunning(string name);

        IVerInfo GetBoxVersion(string exeDir);

        string FormatBoxArgs(string vmPath, string idString, string hWndHex);

        IMessageLoop GetLoop(IMessageReceiver callback);

        IMessageSender GetSender();
    }
}