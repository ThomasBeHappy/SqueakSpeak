using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using SqueakIDE.Controls;

namespace SqueakIDE.Git
{
    public class GitManager
    {
        private readonly Repository _repository;
        private readonly string _repoPath;
        private Signature _signature;
        private string _password;

        public GitManager(string projectPath)
        {
            _repoPath = Repository.Discover(projectPath);
            if (_repoPath != null)
            {
                _repository = new Repository(_repoPath);
            }
        }

        public bool IsGitRepository => _repository != null;

        public Dictionary<string, ProjectTreeItem.FileStatus> GetFileStatuses()
        {
            var statuses = new Dictionary<string, ProjectTreeItem.FileStatus>();
            if (!IsGitRepository) return statuses;

            foreach (var entry in _repository.RetrieveStatus(new StatusOptions()))
            {
                var status = entry.State switch
                {
                    LibGit2Sharp.FileStatus.ModifiedInWorkdir => ProjectTreeItem.FileStatus.Modified,
                    LibGit2Sharp.FileStatus.NewInWorkdir => ProjectTreeItem.FileStatus.New,
                    LibGit2Sharp.FileStatus.DeletedFromWorkdir => ProjectTreeItem.FileStatus.Deleted,
                    _ => ProjectTreeItem.FileStatus.Normal
                };
                statuses[entry.FilePath] = status;
            }

            return statuses;
        }

        public string GetCurrentBranch()
        {
            return IsGitRepository ? _repository.Head.FriendlyName : string.Empty;
        }

        public void SetCredentials(string username, string email, string password)
        {
            _signature = new Signature(username, email, DateTimeOffset.Now);
            _password = password;
        }

        public void Commit(string message, IEnumerable<string> files)
        {
            if (!IsGitRepository || _signature == null) return;

            // Stage selected files
            foreach (var file in files)
            {
                Commands.Stage(_repository, file);
            }

            // Create the commit
            _repository.Commit(message, _signature, _signature);
        }

        public void Pull()
        {
            if (!IsGitRepository || _signature == null) return;
            
            var options = new PullOptions
            {
                FetchOptions = new FetchOptions
                {
                    CredentialsProvider = (_url, _user, _cred) => 
                        new UsernamePasswordCredentials
                        {
                            Username = _signature.Name,
                            Password = _password
                        }
                }
            };

            Commands.Pull(_repository, _signature, options);
        }

        public void Push()
        {
            if (!IsGitRepository || _signature == null) return;

            var remote = _repository.Network.Remotes["origin"];
            var options = new PushOptions
            {
                CredentialsProvider = (_url, _user, _cred) => 
                    new UsernamePasswordCredentials
                    {
                        Username = _signature.Name,
                        Password = _password
                    }
            };
            
            _repository.Network.Push(remote, @"refs/heads/" + _repository.Head.FriendlyName, options);
        }

        public IEnumerable<string> GetBranches()
        {
            if (!IsGitRepository) return Enumerable.Empty<string>();
            
            return _repository.Branches.Select(b => b.FriendlyName);
        }

        public void SwitchBranch(string branchName)
        {
            if (!IsGitRepository) return;

            var branch = _repository.Branches[branchName];
            if (branch != null)
            {
                Commands.Checkout(_repository, branch);
            }
        }

        public static GitManager InitializeRepository(string path, string username, string email)
        {
            Repository.Init(path);
            var manager = new GitManager(path);
            manager.SetCredentials(username, email, null);
            return manager;
        }

        public static GitManager CloneRepository(string url, string path, string username, string email, string password)
        {
            var options = new CloneOptions
            {
                CredentialsProvider = (_url, _user, _cred) =>
                    new UsernamePasswordCredentials
                    {
                        Username = username,
                        Password = password
                    }
            };

            Repository.Clone(url, path, options);
            var manager = new GitManager(path);
            manager.SetCredentials(username, email, password);
            return manager;
        }

        public bool HasRemote => _repository?.Network.Remotes.Any() ?? false;
    }
} 