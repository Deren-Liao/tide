using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibGit2Sharp;


class RepositoryInformation : IDisposable
{
    private bool _disposed;
    public readonly Repository Repo;

    public static RepositoryInformation GetRepositoryInformationForPath(string path)
    {
        if (Repository.IsValid(path))
        {
            return new RepositoryInformation(path);
        }
        return null;
    }

    public string CommitHash
    {
        get
        {
            return Repo.Head.Tip.Sha;
        }
    }

    public string BranchName
    {
        get
        {
            return Repo.Head.CanonicalName;
        }
    }

    public string TrackedBranchName
    {
        get
        {
            return Repo.Head.IsTracking ? Repo.Head.TrackedBranch.CanonicalName : String.Empty;
        }
    }

    public bool HasUnpushedCommits
    {
        get
        {
            return Repo.Head.TrackingDetails.AheadBy > 0;
        }
    }

    public bool HasUncommittedChanges
    {
        get
        {
            return Repo.RetrieveStatus().Any(s => s.State != FileStatus.Ignored);
        }
    }

    public IEnumerable<Commit> Log
    {
        get
        {
            return Repo.Head.Commits;
        }
    }

    private RepositoryInformation(string path)
    {
        Repo = new Repository(path);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            Repo.Dispose();
        }
    }


}
