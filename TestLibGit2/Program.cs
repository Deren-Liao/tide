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
        private const string projectRoot = @"c:\git\tide\1-hello-world-exceptional-logging\1-hello-world.csproj";
        private const string build_time_file = @"h:\myanotherroot\testgit\1-hello-world-exceptional-logging\App_Start\WebApiConfig.cs";
        private const string file = @"1-hello-world-exceptional-logging\App_Start\WebApiConfig.cs";
        private const string remote_name = @"https://github.com/Deren-Liao/tide.git";
        private const string sha = "8babece202d55d9fca22a884acbe9c0fcffab765";

        static void Main(string[] args)
        {
            Git();

            string dir = projectRoot;
            RepositoryInformation repo = null;
            do
            {
                dir = Directory.GetParent(dir)?.FullName;
                if (dir == null)
                {
                    break;
                }
                repo = RepositoryInformation.GetRepositoryInformationForPath(dir);
                if (repo != null)
                {
                    var remoteURL = repo.Repo.Config.Get<string>("remote", "origin", "url").Value;
                    Console.WriteLine($"{remoteURL}");
                    if (String.CompareOrdinal(remoteURL, remote_name) == 0)
                    {
                        Console.WriteLine($"remoteURL");
                    }
                    else
                    {
                        repo = null;
                    }
                }
                
            } while (repo == null && dir != null);

            if (repo == null)
            {
                return;
            }

            //  git show 8babece202d55d9fca22a884acbe9c0fcffab765:1-hello-world-exceptional-logging/App_Start/WebApiConfig.cs
        }

        private static void Git()
        {
            using (var repo = new Repository(@"c:\git\tide"))
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
}
