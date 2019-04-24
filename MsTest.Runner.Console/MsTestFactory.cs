using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MsTest.Runner
{
    public class MsTestFactory<T> where T : IMsTest
    {
        private MsTestFactory()
        {
        }

        public static readonly MsTestFactory<T> Default = new Lazy<MsTestFactory<T>>(() => new MsTestFactory<T>()).Value;
        private const string ProgramFileName = @"\mstest.exe";
        private const string MsTestHome = "MsTestHome";
        public void Start(T mstestOptions, Action<int> testExited, Action<string> error, Action<string> output)
        {
            
                var dir = Environment.GetEnvironmentVariable(MsTestHome);
                var fileName = $"\"{dir}{ProgramFileName}\"";
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = mstestOptions.BuildWorkingArgments,
                    WorkingDirectory = mstestOptions.BuildWorkingDirectory,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                };
                output($"{processStartInfo.FileName}||{mstestOptions.BuildWorkingArgments}||{mstestOptions.BuildWorkingDirectory}");
                Process process = new Process { EnableRaisingEvents = true, StartInfo = processStartInfo };
                process.ErrorDataReceived += (s, e) => { error(e.Data); };
                process.OutputDataReceived += (s, e) => { output(e.Data); };
                process.Exited += (s, e) =>
                {
                    testExited(((Process)s).Id);
                };
                process.Start();
        }
    }
}