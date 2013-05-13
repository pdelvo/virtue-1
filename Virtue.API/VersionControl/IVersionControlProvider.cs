namespace Virtue.API.VersionControl
{
    /// <summary>
    /// Represents a version control provider, such as git or subversion.
    /// Depending on the version control system in use, some methods may throw NotSupportedException for
    /// certain operations.
    /// </summary>
    public interface IVersionControlProvider
    {
        /// <summary>
        /// The identity this provider uses to make commits.
        /// </summary>
        IAuthor Identity { get; }

        /// <summary>
        /// Loads the specified repository into this instance of the VCS provider.
        /// </summary>
        void Load(string localPath);

        /// <summary>
        /// Creates a local copy of the specified remote repository by its URL.
        /// </summary>
        void Clone(string url, string localPath);

        /// <summary>
        /// Gets a list of outstanding changes in the repository.
        /// </summary>
        IModifiedFile[] GetPendingChanges();

        /// <summary>
        /// Prepares the specified files to be committed.
        /// </summary>
        void StageForCommit(IModifiedFile[] files);

        /// <summary>
        /// Commits a change to the repository.
        /// </summary>
        IChangeset Commit(string message);

        /// <summary>
        /// Pushes changes from the local to the remote repository.
        /// </summary>
        void Push();

        /// <summary>
        /// Updates the local repository with changes from the remote.
        /// </summary>
        IChangeset[] Pull();

        /// <summary>
        /// Retrieves a list of branches (by name) in the repository.
        /// </summary>
        string[] ListBranches();

        /// <summary>
        /// Switches the repository to work from the specified branch.
        /// </summary>
        void SwitchToBranch(string name);

        /// <summary>
        /// Reverts the repository to the specified revision.
        /// </summary>
        void GetRevision(object identifier);

        /// <summary>
        /// Changes the repository to the latest revision (effectively undos GetRevision)
        /// </summary>
        void GetLatestRevision();
    }
}
