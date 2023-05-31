using System;

namespace _86BoxManager.API
{
    public interface IManager
    {
        bool IsFirstInstance(string name);

        IntPtr RestoreAndFocus(string title, string handleTitle);

        bool IsProcessRunning(string name);

        IVerInfo GetBoxVersion(string exeDir);

        IMessageLoop GetLoop(IMessageReceiver callback);

        IMessageSender GetSender();

        IExecutor GetExecutor();
    }
}