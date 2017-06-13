using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleEcho
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamWriter sw = new StreamWriter(@"c:\tmp\echo.txt", append: false))
            {
                foreach (string x in args)
                { 
                    Console.WriteLine(x);
                    sw.WriteLine(x);
                };

                string s;
                while (!String.IsNullOrWhiteSpace((s = Console.ReadLine())))
                {
                    Console.WriteLine(s);
                    sw.WriteLine(s);
                }
            }
        }
    }
}
