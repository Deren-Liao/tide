using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace GitCommand
{
    class Program
    {
        private const string projectRoot = @"c:\tide\1-hello-world-exceptional-logging\1-hello-world.csproj";
        private const string build_time_file = @"h:\myanotherroot\testgit\1-hello-world-exceptional-logging\App_Start\WebApiConfig.cs";
        private const string file = @"1-hello-world-exceptional-logging\App_Start\WebApiConfig.cs";
        private const string remote_name = @"https://github.com/Deren-Liao/tide.git";
        private const string sha = "8babece202d55d9fca22a884acbe9c0fcffab765";

        static void Main(string[] args)
        {
            RepositoryInformation repo = RepositoryInformation.GetRepositoryInformationForPath(@"c:\tide");
            WriteLine($"{repo.BranchName}");
            repo.DoesCommitExists(sha);
            repo.GetFileRevision(sha, file, @"c:\tmp\git-command-test");
            var text = repo.ListTree(sha);
            ReadKey();
        }
    }
}
