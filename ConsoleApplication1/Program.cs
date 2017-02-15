using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using System.IO;
>>>>>>> 85dffc7d4162981e47f3f8215937d566ed130188

namespace ConsoleApplication1
{
    class Program
    {
<<<<<<< HEAD
        static void Main(string[] args)
        {
            Lazy.TestLazyIsValueCreated();
=======
        private static string GetRelativePath(string project, string projectItem)
        {
            int baseIndex = 0;
            for (int i = 0; i < projectItem.Length && i < project.Length && projectItem[i] == project[i]; ++i)
            {
                if (projectItem[i] == Path.DirectorySeparatorChar)
                {
                    baseIndex = i;
                }
            }
            return projectItem.Substring(baseIndex);
        }
        static void Main(string[] args)
        {
            Console.WriteLine(GetRelativePath(@"c:\a\bc\de.csproj", @"c:\a\bc\de.cs"));
            Console.WriteLine(GetRelativePath(@"c:\a\bc\de.csproj", @"c:\a\bc\ff\de.cs"));
>>>>>>> 85dffc7d4162981e47f3f8215937d566ed130188
        }
    }
}
