﻿using System;
using System.IO;
using System.Linq;
using _86BoxManager.API;
using _86BoxManager.Common;

namespace _86BoxManager.Mac
{
    public class MacManager : IManager
    {
        public IVerInfo GetBoxVersion(string exeDir)
        {
            var info = Path.Combine(exeDir, "..", "Info.plist");
            var text = File.ReadAllText(info);
            var bip = text.Split("CFBundleVersion", 2);
            var bit = bip.Last().Split("<string>", 2);
            var bi = bit.Last().Split("</", 2).First();
            var bv = Version.Parse(bi);
            return new CommonVerInfo
            {
                FileMajorPart = bv.Major,
                FileMinorPart = bv.Minor,
                FileBuildPart = bv.Build,
                FilePrivatePart = bv.Revision
            };
        }

        public string FormatBoxArgs(string vmPath, string idString, string hWndHex)
        {
            return $@"--vmpath ""{vmPath}""";
        }

        public IMessageLoop GetLoop(IMessageReceiver callback)
        {
            throw new NotImplementedException();
        }

        public IMessageSender GetSender()
        {
            throw new NotImplementedException();
        }

        public bool IsFirstInstance(string name)
        {
            // TODO : Fix
            return true;
        }

        public bool IsProcessRunning(string name)
        {
            // TODO : Fix
            return false;
        }

        public IntPtr RestoreAndFocus(string windowTitle, string handleTitle)
        {
            throw new NotImplementedException();
        }
    }
}