using System;
using System.Runtime.InteropServices;
using _86BoxManager.API;
using _86BoxManager.Linux;
using _86BoxManager.Mac;
using _86BoxManager.Windows;

namespace _86boxManager.Xplat
{
    internal static class Platforms
    {
        public static readonly IShell Shell;
        public static readonly IManager Manager;
        public static readonly IEnv Env;

        static Platforms()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Shell = new LinuxShell();
                Manager = new LinuxManager();
                Env = new LinuxEnv();
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Shell = new MacShell();
                Manager = new MacManager();
                Env = new MacEnv();
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Shell = new WinShell();
                Manager = new WinManager();
                Env = new WinEnv();
                return;
            }

            throw new InvalidOperationException("Not supported OS! Sorry!");
        }
    }
}