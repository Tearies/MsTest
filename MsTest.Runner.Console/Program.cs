using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace MsTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var parseResult = Parser.Default.ParseArguments<MstestOptions>(args);
            parseResult.WithParsed(TestWithOptions).WithNotParsed(p =>
            {
                Console.Error.WriteLine("MsTestEnd");
            });
        }

        private static void TestWithOptions(MstestOptions mstestOptions)
        {
            try
            {
                ManualResetEvent manualResetEvent = new ManualResetEvent(false);
                MsTestFactory<MstestOptions>.Default.Start(mstestOptions,
                    p =>
                    {
                        Console.Out.WriteLine($" test Process{p} exited");
                        manualResetEvent.Set();
                    }, p =>
                    {
                        Console.Out.WriteLine(p);
                    }, p =>
                    {
                        Console.Out.WriteLine(p);
                    });
                manualResetEvent.WaitOne();
                Console.Out.WriteLine("MsTestEnd");
                File.Copy(mstestOptions.TempResultsFile,mstestOptions.ResultsFile,true);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
            }
           
        }
    }
}
