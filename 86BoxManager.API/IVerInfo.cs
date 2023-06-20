namespace _86BoxManager.API
{
    public interface IVerInfo
    {
        int FilePrivatePart { get; }

        int FileMajorPart { get; }

        int FileMinorPart { get; }

        int FileBuildPart { get; }
    }
}