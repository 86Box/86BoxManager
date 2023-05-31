namespace _86BoxManager.API
{
    public interface IEnv
    {
        string MyComputer { get; }

        string UserProfile { get; }

        string[] ExeNames { get; }

        string MyDocuments { get; }

        string Desktop { get; }

        string[] GetProgramFiles(string appName);
    }
}