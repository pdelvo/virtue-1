using NGit;
using NGit.Api;
using NGit.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtue.API.VersionControl;

namespace Virtue.GitPlugin
{
    public class GitProvider : IVersionControlProvider
    {
        public GitProvider()
        {
            DefaultRemote = "origin";
        }

        public GitProvider(IAuthor author) : this()
        {
            Identity = author;
        }

        public GitProvider(IAuthor author, CredentialsProvider credentials) : this(author)
        {
            Credentials = credentials;
        }

        public Git Repository { get; set; }

        public IAuthor Identity { get; private set; }
        public CredentialsProvider Credentials { get; set; }
        public string DefaultRemote { get; set; }

        public void Load(string localPath)
        {
            if (Repository != null)
                throw new InvalidOperationException("This provider is already associated with a repository.");
            Repository = Git.Open(new Sharpen.FilePath(localPath));
        }

        public void Clone(string url, string localPath)
        {
            if (Repository != null)
                throw new InvalidOperationException("This provider is already associated with a repository.");
            var command = Git.CloneRepository();
            command.SetCloneAllBranches(true);
            command.SetCloneSubmodules(true);
            command.SetDirectory(new Sharpen.FilePath(localPath));
            command.SetURI(url);
            command.SetProgressMonitor(new TextProgressMonitor());
            Repository = command.Call();
        }

        public IModifiedFile[] GetPendingChanges()
        {
            var command = Repository.Status();
            var status = command.Call();
            if (status.IsClean()) return new IModifiedFile[0];
            return
                status.GetAdded()    .Select(s => new ModifiedFile(s, FileStatus.Added)).Concat(
                status.GetChanged()  .Select(s => new ModifiedFile(s, FileStatus.Renamed))).Concat(
                status.GetModified() .Select(s => new ModifiedFile(s, FileStatus.Modified))).Concat(
                status.GetRemoved()  .Select(s => new ModifiedFile(s, FileStatus.Removed))).Concat(
                status.GetUntracked().Select(s => new ModifiedFile(s, FileStatus.Untracked)))
                .ToArray();
        }

        public void StageForCommit(IModifiedFile[] files)
        {
            var command = Repository.Add();
            foreach (var file in files)
                command.AddFilepattern(file.Path);
            command.Call();
        }

        public IChangeset Commit(string message)
        {
            var command = Repository.Commit();
            command.SetMessage(message);
            command.SetAuthor(Identity.Name, Identity.Email);
            var result = command.Call();
            return new Changeset(Identity, result.Name);
        }

        public void Push()
        {
            var remoteCommand = Repository.LsRemote();
            var remotes = remoteCommand.Call();
            var remote = remotes.FirstOrDefault(r => r.GetName() == DefaultRemote);
            if (remote == null)
                remote = remotes.FirstOrDefault();
            if (remote == null)
                throw new InvalidOperationException("No remotes to push to.");

            var command = Repository.Push();
            command.SetCredentialsProvider(Credentials);
            command.SetRemote(remote.GetName());
            command.Call();
        }

        public IChangeset[] Pull()
        {
            var command = Repository.Pull();
            if (Credentials != null)
                command.SetCredentialsProvider(Credentials);
            var result = command.Call();
            if (!result.IsSuccessful())
                throw new Exception("Unable to pull changes from remote."); // TODO: GitException?
            var merge = result.GetMergeResult();
            var commits = merge.GetMergedCommits();
            return commits.Select(c => 
                {
                    var authorCommand = Repository.Log();
                    authorCommand.Add(c);
                    authorCommand.SetMaxCount(1);
                    var commit = authorCommand.Call().FirstOrDefault();
                    var author = commit.GetAuthorIdent();
                    return new Changeset(new ChangeAuthor(author.GetName(), author.GetEmailAddress()), c.Name);
                }).ToArray();
        }

        public string[] ListBranches()
        {
            var command = Repository.BranchList();
            return command.Call().Select(b => b.GetName()).ToArray();
        }

        public void SwitchToBranch(string name)
        {
            var command = Repository.Checkout();
            command.SetName(name);
            var result = command.Call();
        }

        public void GetRevision(object identifier)
        {
            var hash = (string)identifier;
            var command = Repository.Checkout();
            command.SetName(hash);
            command.Call();
        }

        public void GetLatestRevision()
        {
            var log = Repository.Log();
            log.SetMaxCount(1);
            var latest = log.Call().FirstOrDefault().Name;
            var command = Repository.Checkout();
            command.SetName(latest);
            command.Call();
        }
    }
}
