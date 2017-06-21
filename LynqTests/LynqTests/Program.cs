using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace LynqTests
{

    public class TestClass
    {
        public string Path { get; set; }
    }

    class Program
    {

        static TestClass LoadEncryptedCredentials(string path)
        {
            return new TestClass { Path = path };
        }

        static void WillItReturnNull()
        {
            var result = Directory.EnumerateFiles(@"c:\tmp")
                .Where(x => Path.GetExtension(x) == ".txt")
                .Select(x => LoadEncryptedCredentials(x));

            Console.WriteLine(result.Count());
        }

        static void Main(string[] args)
        {
            WillItReturnNull();

            while (Console.ReadKey().Key != ConsoleKey.X) { }
        }
    }
}
