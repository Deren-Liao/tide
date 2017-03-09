using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibGit2Sharp;
using System.IO;
using System.Diagnostics;
using static System.Diagnostics.Debug;

namespace TestLibGit2
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
            var commit = GitUtils.FindCommit(projectRoot, sha);
            if (commit == null)
            {
                Console.WriteLine("commit is null");
                return;
            }

            var content = GitUtils.OpenFile(commit, build_time_file);
            if (content != null)
            {
                Console.WriteLine(content.Substring(0, 100));
            }
        }

        private static void Git(Repository repo)
        {
            int i = 0;
            foreach (var mit in repo.Commits)
            {
                if (++i < 5 || mit.Sha == "8babece202d55d9fca22a884acbe9c0fcffab765")
                {
                    Console.WriteLine("Author: {0}", mit.Author.Name);
                    Console.WriteLine("Message: {0}", mit.MessageShort);
                    Console.WriteLine($"SHA: {mit.Sha}");
                }
            }
            Commit commit = repo.Lookup<Commit>("8babece202d55d9fca22a884acbe9c0fcffab765");
            Console.WriteLine("Author: {0}", commit.Author.Name);
            Console.WriteLine("Message: {0}", commit.MessageShort);

            foreach (var t in commit.Tree)
            {
                Console.WriteLine($"Name {t.Name}, Mode {t.Mode}, Path {t.Path}");
            }

            var treeEntry = commit[file];
            var blob = (Blob)treeEntry.Target;
            var contentStream = blob.GetContentStream();
            Debug.Assert(blob.Size == contentStream.Length);

            using (var tr = new StreamReader(contentStream, Encoding.UTF8))
            {
                string content = tr.ReadToEnd();
            }
        }
    }
}
