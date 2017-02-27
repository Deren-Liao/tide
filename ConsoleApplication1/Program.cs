using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
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
        static void Main1(string[] args)
        {
            Console.WriteLine(GetRelativePath(@"c:\a\bc\de.csproj", @"c:\a\bc\de.cs"));
            Console.WriteLine(GetRelativePath(@"c:\a\bc\de.csproj", @"c:\a\bc\ff\de.cs"));
        }


        static void Main(string[] args)
        {
            int[] a = new int[] { 3 };

            Console.WriteLine($"{a?[2]}");
        }
    }
}
