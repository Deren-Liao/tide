using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using static System.Console;
using System.Reflection;

namespace credential_helper
{
    class Program
    {
        const string Username = "derenl@google.com";
        private static string password => s_lazySecret.Value;
        private static readonly Lazy<string> s_lazySecret = 
            new Lazy<string>(() => OpenFileGetLine(@".\password.txt"));

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private static string OpenFileGetLine(string relativePath)
        {
            string path = Path.Combine(AssemblyDirectory, relativePath);
            using (var sr = new StreamReader(path))
            {
                var sec = sr.ReadLine().Trim();
                if (string.IsNullOrEmpty(sec))
                {
                    throw new Exception("Failed to get secret");
                }

                return sec;
            }
        }


        static void Main(string[] args)
        {
            appendlog a = new appendlog(@"c:\tmp\credential-help.log");
            string text = string.Join("  ", args);
            a.Append(text);

            while (true)
            {
                string s = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(s))
                    break;
                a.Append(s);
            }

            if (args.Length < 1)
            {
                return;
            }
            switch (args[0])
            {
                case "get":
                    WriteLine($"username={Username}");
                    WriteLine($"password={s_lazySecret.Value}");
                    break;
                case "store":
                    a.Append("store command");
                    break;
                case "erase":
                    a.Append("erase command");
                    break;
                default:
                    WriteLine("Wrong input");
                    a.Append("Wrong input");
                    break;
            }
        }
    }
}
