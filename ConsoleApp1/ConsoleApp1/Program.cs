using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            GoogleCloudExtension.GitUtils.VsGitData.AddLocalRepositories("14.0", "aws-first-copy-2", @"c:\git\csr\aws-first-copy-2");
        }
    }
}
