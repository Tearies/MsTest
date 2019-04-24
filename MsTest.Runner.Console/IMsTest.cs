namespace MsTest.Runner
{
    public interface IMsTest
    {
        string BuildWorkingDirectory { get; }

        string BuildWorkingArgments { get; }
    }
}