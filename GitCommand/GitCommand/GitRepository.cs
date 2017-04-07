using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

class RepositoryInformation : IDisposable
{
    public static RepositoryInformation GetRepositoryInformationForPath(string path, string gitPath = null)
    {
        var repositoryInformation = new RepositoryInformation(path, gitPath);
        if (repositoryInformation.IsGitRepository)
        {
            return repositoryInformation;
        }
        return null;
    }

    public string CommitHash
    {
        get
        {
            return RunCommand("rev-parse HEAD");
        }
    }

    public string BranchName
    {
        get
        {
            return RunCommand("rev-parse --abbrev-ref HEAD");
        }
    }

    public string TrackedBranchName
    {
        get
        {
            return RunCommand("rev-parse --abbrev-ref --symbolic-full-name @{u}");
        }
    }

    public bool HasUnpushedCommits
    {
        get
        {
            return !String.IsNullOrWhiteSpace(RunCommand("log @{u}..HEAD"));
        }
    }

    public bool HasUncommittedChanges
    {
        get
        {
            return !String.IsNullOrWhiteSpace(RunCommand("status --porcelain"));
        }
    }

    public bool DoesCommitExists(string sha)
    {
        string text = RunCommand($"cat-file -t {sha}");
        return text == "commint";
    }

    public IEnumerable<string> Log
    {
        get
        {
            int skip = 0;
            while (true)
            {
                string entry = RunCommand(String.Format("log --skip={0} -n1", skip++));
                if (String.IsNullOrWhiteSpace(entry))
                {
                    yield break;
                }

                yield return entry;
            }
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _gitProcess.Dispose();
        }
    }

    private RepositoryInformation(string path, string gitPath)
    {
        var processInfo = new ProcessStartInfo
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = Directory.Exists(gitPath) ? gitPath : "git.exe",
            CreateNoWindow = true,
            WorkingDirectory = (path != null && Directory.Exists(path)) ? path : Environment.CurrentDirectory
        };

        _gitProcess = new Process();
        _gitProcess.StartInfo = processInfo;
    }
    
    public void GetFileRevision(string gitsha, string relativePath, string targetFile)
    {
        string text = RunCommand($"show {gitsha}:{relativePath.Replace('\\', '/')}");
        Console.WriteLine(text.Substring(0, 100));
    }

    public List<string> ListTree(string gitSha)
    {
        string output = RunCommand($"ls-tree -r {gitSha} --name-only");
        if (output == null)
        {
            return null;
        }
        return new List<string>(output.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries));
    }

    private bool IsGitRepository
    {
        get
        {
            return !String.IsNullOrWhiteSpace(RunCommand("log -1"));
        }
    }

    private string RunCommand(string args)
    {
        _gitProcess.StartInfo.Arguments = args;
        _gitProcess.Start();
        string output = _gitProcess.StandardOutput.ReadToEnd().Trim();
        _gitProcess.WaitForExit();
        return output;
    }

    /// <summary>
    /// This method compare two files line by line.
    /// </summary>
    /// <param name="revisionFileContent">The file content of the revision file.</param>
    /// <param name="currentFilePath">The checked out file path.</param>
    /// <returns>
    /// True: the two files content are same.
    /// False: thw two files differs.
    /// </returns>
    private bool FileCompare(List<string> revisionFileContent, string currentFilePath)
    {
        using (var sr = new StreamReader(currentFilePath))
        {
            foreach (var line in revisionFileContent)
            {
                if (sr.EndOfStream || line != sr.ReadLine())
                {
                    return false;
                }
            }
            return sr.EndOfStream;
        }
    }

    private bool _disposed;
    private readonly Process _gitProcess;
}