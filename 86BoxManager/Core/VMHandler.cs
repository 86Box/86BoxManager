using System;
using _86BoxManager.API;
using _86boxManager.Models;

// ReSharper disable InconsistentNaming
namespace _86boxManager.Core
{
    internal sealed class VMHandler : IMessageReceiver
    {
        private static uint GetTempId(VM vm)
        {
            // This can return negative integers, which is a no-no for 86Box,
            // hence the shift up by int.MaxValue
            var tempId = vm.Path.GetHashCode();
            uint id;
            if (tempId < 0)
                id = (uint)(tempId + int.MaxValue);
            else
                id = (uint)tempId;
            return id;
        }

        public void OnEmulatorInit(IntPtr hWnd, uint vmId)
        {
            throw new NotImplementedException();
        }

        public void OnEmulatorShutdown(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public void OnVmPaused(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public void OnVmResumed(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public void OnDialogOpened(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public void OnDialogClosed(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public void OnManagerStartVm(string vmName)
        {
            throw new NotImplementedException();
        }
    }
}