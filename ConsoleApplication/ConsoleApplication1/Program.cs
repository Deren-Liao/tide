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

        static void QuestionIndex()
        {
            int[] a = new int[] { 3 };

            Console.WriteLine($"{a?[2]}");
        }

        static void JaonsTest()
        {
            string jsonText = @"{ ""data"": ""200"" }";
            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText);
            Console.WriteLine($"{json.data}");
            int a;
            Int32.TryParse(json.data.Value, out a);
        }

        static void Main(string[] args)
        {
            JaonsTest();
            return;

            Uri uri = new Uri("https://a.b.c/hffsadf/asf/ewf?how");
            Console.WriteLine(uri.LocalPath);

            List<int> lint = null;
            Console.WriteLine($"lint?.count is {lint == null || lint.Count == 0}");
            Console.WriteLine($"{DateTime.UtcNow:O}");

            AsyncTask.runTaskTest();

            string s;
            for (int i = 0; i < 3; ++i)
            {
                s = AutoPropertyInitializer.PropObj.text;
            }

            Console.ReadKey();
        }

        static void Main3(string[] args)
        {
            Console.WriteLine($"null to null compare, {string.Compare(null, null)}");
            Console.WriteLine($"null to empty compare, {string.Compare(null, "")}");
            Console.WriteLine($"empty to empty compare, {string.Compare("", "")}");

            DateTimeFormat.ToLocalCultureLocalTime();

            throw new Exception("exception context test");
        }
    }
}
