using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace MsTest.Runner
{
    public class MstestOptions: IMsTest
    {
        

        [Option("testcontainer", Required =true)]
        public string TestContainer { get; set; }

        [Option("testmetadata")]
        public string TestMetadata { get; set; }

        [Option("testlist")]
        public string TestList { get; set; }
         
        [Option("category")]
        public string Category { get; set; }

        [Option("noisolation")]
        public bool Noisolation { get; set; }
         
        [Option("testsettings")]
        public string TestSettings { get; set; }

        [Option("runconfig")]
        public string RunConfig { get; set; }

        [Option("resultsfile",Required =true)]
        public string ResultsFile { get; set; }

        [Option("nologo")]
        public bool NoLogo { get; set; }

        [Option("usestderr")]
        public bool UseStdErr { get; set; }

        [Usage(ApplicationAlias = "MsTest.Runner.Console")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("请参考：https://docs.microsoft.com/en-us/previous-versions/ms182489%28v%3dvs.140%29", new MstestOptions { TestContainer="a.dll", ResultsFile="a.trx" });
            }
        }

        public string BuildWorkingDirectory 
        {
            get { return Path.GetDirectoryName(Path.GetFullPath(TestContainer)); }
            
        }

        public string BuildWorkingArgments => this.InternalBuildMstestArgments();

        private string InternalBuildMstestArgments()
        {
            StringBuilder builder = new StringBuilder();
            if (Noisolation)
            {
                builder.Append($" /{nameof(Noisolation).ToLower()}");
            }

            if (NoLogo)
            {
                builder.Append($" /{nameof(NoLogo).ToLower()}");
            }

            if (UseStdErr)
            {
                builder.Append($" /{nameof(UseStdErr).ToLower()}");
            }
             
            builder.Append($" /{nameof(TestContainer).ToLower()}:{Path.GetFileName(TestContainer)}");
            builder.Append($" /{nameof(ResultsFile).ToLower()}:{Path.GetFileName(ResultsFile)}");

            if(!string.IsNullOrEmpty(TestMetadata))
                builder.Append($" /{nameof(TestMetadata).ToLower()}:{TestMetadata}");

            if (!string.IsNullOrEmpty(TestList))
                builder.Append($" /{nameof(TestList).ToLower()}:{TestList}");


            if (!string.IsNullOrEmpty(Category))
                builder.Append($" /{nameof(Category).ToLower()}:{Category}");

            if (!string.IsNullOrEmpty(TestSettings))
                builder.Append($" /{nameof(TestSettings).ToLower()}:{TestSettings}");

            if (!string.IsNullOrEmpty(RunConfig))
                builder.Append($" /{nameof(RunConfig).ToLower()}:{RunConfig}");
            

            return builder.ToString();
        }
    }
}