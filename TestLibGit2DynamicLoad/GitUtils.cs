using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using LibGit2Sharp;

namespace TestLibGit2
{
    public static class GitUtils
    {
        public static Commit FindCommit(string projectRoot, string sha)
        {
            string dir = projectRoot;
            do
            {
                dir = Directory.GetParent(dir)?.FullName;
                if (dir == null)
                {
                    break;
                }
                if (!Repository.IsValid(dir))
                {
                    continue;
                }
                var repo = new Repository(dir);
                if (repo != null)
                {
                    Commit commit = repo.Lookup<Commit>(sha);
                    if (commit != null)
                    {
                        var remoteURL = repo.Config.Get<string>("remote", "origin", "url").Value;
                        Console.WriteLine($"{remoteURL}");
                        return commit;
                    }
                }

            } while (dir != null);

            return null;
        }

        //public static Commit FindCommit(string projectRoot, string sha)
        //{
        //    Repository repo;
        //    try
        //    {
        //        repo = new Repository(projectRoot);
        //    }
        //    catch (RepositoryNotFoundException)
        //    {
        //        return null;
        //    }
        //    Commit commit = repo.Lookup<Commit>(sha);
        //    if (commit != null)
        //    {
        //        var remoteURL = repo.Config.Get<string>("remote", "origin", "url").Value;
        //        Console.WriteLine($"{remoteURL}");
        //        return commit;
        //    }
        //    return commit;
        //}

        public static string OpenFile(Commit commit, string filePath)
        {
            return FindMatchingEntry(commit, filePath).FirstOrDefault()?.GetFileContent();
        }

        public static string GetFileContent(this TreeEntry treeEntry)
        {
            var blob = (Blob)treeEntry.Target;
            var contentStream = blob.GetContentStream();
            Debug.Assert(blob.Size == contentStream.Length);

            using (var tr = new StreamReader(contentStream, Encoding.UTF8))
            {
                return tr.ReadToEnd();
            }
        }

        private static IEnumerable<TreeEntry> FindMatchingEntry(Commit commit, string filePath)
        {
            foreach (string subPath in SubPaths(filePath))
            {
                var treeEntry = commit[subPath];
                if (treeEntry != null)
                {
                    yield return treeEntry;
                }
            }
        }

        private static IEnumerable<string> SubPaths(string filePath)
        {
            if (filePath == null)
            {
                yield break;
            }

            int index = filePath.IndexOf(Path.DirectorySeparatorChar);
            for (; index >= 0; index = filePath.IndexOf(Path.DirectorySeparatorChar, index+1))
            {
                yield return filePath.Substring(index + 1);
            }
        }
    }
}
