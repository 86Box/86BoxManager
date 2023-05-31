using System;

// ReSharper disable InconsistentNaming

namespace _86BoxManager.API
{
    public interface IVm
    {
        /// <summary>
        /// Name of the virtual machine
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Window handle for the VM once it's started
        /// </summary>
        IntPtr hWnd { get; }
        
        /// <summary>
        /// Callback to invoke when VM is gone
        /// </summary>
        Action<IVm> OnExit { set; }
    }
}