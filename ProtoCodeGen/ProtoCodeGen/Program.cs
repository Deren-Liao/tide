using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;
using Bird;

using static System.Console;
using System.IO;

using Google.Devtools.Source.V1;
using Google.Cloud.Logging.V2;

namespace ProtoCodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Feed f = new Feed
            {
                 FoodName = "noodle",
                  Seq = 1
            };

            using (TextWriter writer = File.CreateText("C:\\tmp\\noodle.json"))
            {
                JsonFormatter.Default.WriteValue(writer, f);
            }

            GitSourceContext g = new GitSourceContext
            {
                Url = "http://ea",
                RevisionId = "ididid"
            };

            SourceContext sc = new SourceContext
            {
                 Git = g
            };

            var path = "ea.json";
            using (TextWriter writer = File.CreateText(path))
            {
                JsonFormatter.Default.WriteValue(writer, sc);
            }

            SourceContextFile file = new SourceContextFile(path);
            WriteLine($"{file.GitRepoUrl}");
            ReadKey();
        }
    }
}
