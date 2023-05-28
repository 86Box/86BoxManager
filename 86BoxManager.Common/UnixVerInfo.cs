using _86BoxManager.API;

namespace _86BoxManager.Common
{
    public sealed class UnixVerInfo : IVerInfo
    {
        public int FilePrivatePart { get; set; }

        public int FileMajorPart { get; set; }

        public int FileMinorPart { get; set; }

        public int FileBuildPart { get; set; }
    }
}