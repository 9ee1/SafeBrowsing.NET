using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases.Json {
    /// <summary>
    ///     Base JSON Database.
    /// </summary>
    public abstract class BaseJsonBrowsingDatabase {
        /// <summary>
        ///     Database.
        /// </summary>
        private readonly MemoryBrowsingDatabase _database;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Get Database File Manager.
        /// </summary>
        private protected JsonFileManager DatabaseFileManager { get; }

        /// <summary>
        ///     Get Database File Path.
        /// </summary>
        public string DatabaseFilePath { get; }

        /// <summary>
        ///     Create a Base JSON Database.
        /// </summary>
        /// <param name="databaseFilePath">
        ///     An absolute file path to a database file. If the file does not exist, it will be created.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="databaseFilePath" /> is a null reference.
        /// </exception>
        private protected BaseJsonBrowsingDatabase(string databaseFilePath) {
            this._database = new MemoryBrowsingDatabase();
            this._disposed = false;
            this.DatabaseFileManager = new JsonFileManager(databaseFilePath);
            this.DatabaseFilePath = databaseFilePath;
            // ...
            //
            // ...
            LoadDatabase(this);

            // <summary>
            //      Load Database.
            // </summary>
            void LoadDatabase(BaseJsonBrowsingDatabase @this) {
                try {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var cFileModel = @this.DatabaseFileManager.Read();
                    if (cFileModel.ThreatLists != null) {
                        foreach (var cThreatListModel in cFileModel.ThreatLists) {
                            var cPlatformType = cThreatListModel.PlatformType;
                            var cThreatEntryType = cThreatListModel.ThreatEntryType;
                            var cThreatType = cThreatListModel.ThreatType;
                            var cThreatList = ThreatList.Build()
                                .SetDescriptor(cThreatType, cPlatformType, cThreatEntryType)
                                .SetRetrieveDate(cThreatListModel.RetrieveDate)
                                .SetState(cThreatListModel.State)
                                .SetWaitToDate(cThreatListModel.WaitToDate)
                                .Build();

                            // ...
                            //
                            // Throws an exception if the operation fails.
                            var cThreatSha256HashPrefixes = cThreatListModel.ThreatSha256HashPrefixes;
                            var cStoreThreatListTask = @this._database.StoreThreatListAsync(cThreatList, cThreatSha256HashPrefixes);
                            cStoreThreatListTask.Wait();
                        }
                    }
                }
                catch {
                    // ...
                    //
                    // We don't care if we are unable to load the database.
                }
            }
        }

        /// <summary>
        ///     Destroy a Base JSON Database.
        /// </summary>
        ~BaseJsonBrowsingDatabase() => this.Dispose(false);

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
                this.Dispose(true);
                GC.SuppressFinalize(this);

                this._disposed = true;
            }
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        /// <param name="disposing">
        ///     A boolean true if the object is being disposed by a caller. A boolean false if the object is being
        ///     disposed by a finalizer.
        /// </param>
        private protected virtual void Dispose(bool disposing) {
            if (!this._disposed) {
                this._disposed = true;
                if (disposing) {
                    this._database.Dispose();
                    this.DatabaseFileManager.Dispose();
                }
            }
        }

        /// <summary>
        ///     Find Threat Lists Asynchronously.
        /// </summary>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat associated with
        ///     the collection of <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A collection of <see cref="ThreatList" /> the threat identified by
        ///     <paramref name="threatSha256HashPrefix" /> is associated with. An empty collection indicates no threat
        ///     lists were found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public virtual Task<IReadOnlyCollection<ThreatList>> FindThreatListsAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var findThreatListsTask = this._database.FindThreatListsAsync(threatSha256HashPrefix, cancellationToken);
            return findThreatListsTask;
        }

        /// <summary>
        ///     Get a Threat List Asynchronously.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     The <see cref="ThreatList" /> identified by <paramref name="threatListDescriptor" />. A null reference
        ///     indicates a threat list could not be found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public virtual Task<ThreatList> GetThreatListAsync(ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatListTask = this._database.GetThreatListAsync(threatListDescriptor, cancellationToken);
            return getThreatListTask;
        }

        /// <summary>
        ///     Get Threat Lists Asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A collection of <see cref="ThreatList" />. An empty collection indicates no threat lists were found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public virtual Task<IReadOnlyCollection<ThreatList>> GetThreatListsAsync(CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatListsTask = this._database.GetThreatListsAsync(cancellationToken);
            return getThreatListsTask;
        }

        /// <summary>
        ///     Get Threats Asynchronously.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> the threats that should
        ///     be retrieved are associated with.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the threats
        ///     that are associated with the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatListDescriptor" />. An empty collection indicates no threats were found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public virtual Task<IReadOnlyCollection<string>> GetThreatsAsync(ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatsTask = this._database.GetThreatsAsync(threatListDescriptor, cancellationToken);
            return getThreatsTask;
        }

        /// <summary>
        ///     Modify a Threat List Asynchronously.
        /// </summary>
        /// <param name="threatList">
        ///     A <see cref="ThreatList" /> to modify.
        /// </param>
        /// <param name="threatSha256HashPrefixes">
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the threats
        ///     associated with <paramref name="threatList" /> and should be stored.
        /// </param>
        /// <param name="threatIndices">
        ///     A collection of zero-based indices identifying the threats associated with the lexicographically sorted
        ///     <paramref name="threatList" /> and should be removed.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatList" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefixes" /> is a null reference, or if
        ///     <paramref name="threatIndices" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public virtual Task ModifyThreatListAsync(ThreatList threatList, IEnumerable<string> threatSha256HashPrefixes, IEnumerable<int> threatIndices, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var modifyThreatListTask = this._database.ModifyThreatListAsync(threatList, threatSha256HashPrefixes, threatIndices, cancellationToken);
            return modifyThreatListTask;
        }

        /// <summary>
        ///     Store a Threat List Asynchronously.
        /// </summary>
        /// <param name="threatList">
        ///     A <see cref="ThreatList" /> to store.
        /// </param>
        /// <param name="threatSha256HashPrefixes">
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the threats
        ///     associated with <paramref name="threatList" /> and should be stored.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatList" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefixes" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public virtual Task StoreThreatListAsync(ThreatList threatList, IEnumerable<string> threatSha256HashPrefixes, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var storeThreatListTask = this._database.StoreThreatListAsync(threatList, threatSha256HashPrefixes, cancellationToken);
            return storeThreatListTask;
        }

        /// <summary>
        ///     Throw an Exception if Object is Disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        private void ThrowIfDisposed() {
            if (this._disposed) {
                var detailMessage = $"An object ({nameof(BaseJsonBrowsingDatabase)}) is disposed.";
                throw new ObjectDisposedException(nameof(BaseJsonBrowsingDatabase), detailMessage);
            }
        }
    }
}