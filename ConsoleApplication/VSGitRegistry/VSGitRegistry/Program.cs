using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GoogleCloudExtension.GitUtils.VSGitData;
namespace VSGitRegistry
{
    class Program
    {
        static void Main(string[] args)
        {
            var all = GetLocalRepositories("14.0");
            foreach (var a in all)
            {
                Console.WriteLine(a);
            }
        }
    }
}
