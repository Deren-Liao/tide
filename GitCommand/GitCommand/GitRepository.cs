﻿using System;
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

    private bool _disposed;
    private readonly Process _gitProcess;
}