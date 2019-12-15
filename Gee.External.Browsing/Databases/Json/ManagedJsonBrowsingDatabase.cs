using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases.Json {
    /// <summary>
    ///     Managed JSON Browsing Database.
    /// </summary>
    public sealed class ManagedJsonBrowsingDatabase : BaseJsonBrowsingDatabase, IManagedBrowsingDatabase {
        /// <summary>
        ///     Synchronization Interval.
        /// </summary>
        private static readonly TimeSpan SyncInterval;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Synchronization Task.
        /// </summary>
        private readonly Task _syncTask;

        /// <summary>
        ///     Synchronization Task Cancellation Token Source.
        /// </summary>
        private readonly CancellationTokenSource _syncTaskCancellationTokenSource;

        /// <summary>
        ///     Create a Managed JSON Database.
        /// </summary>
        static ManagedJsonBrowsingDatabase() {
            ManagedJsonBrowsingDatabase.SyncInterval = TimeSpan.FromMinutes(1);
        }

        /// <summary>
        ///     Create a Managed JSON Database.
        /// </summary>
        /// <param name="databaseFilePath">
        ///     An absolute file path to the database file. If the file does not exist, it will be created.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="databaseFilePath" /> is a null reference.
        /// </exception>
        public ManagedJsonBrowsingDatabase(string databaseFilePath) : base(databaseFilePath) {
            this._disposed = false;
            this._syncTaskCancellationTokenSource = new CancellationTokenSource();
            // ...
            //
            // ...
            this._syncTask = Task.Run(this.SyncDatabaseFileAsync);
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        /// <param name="disposing">
        ///     A boolean true if the object is being disposed by a caller. A boolean false if the object is being
        ///     disposed by a finalizer.
        /// </param>
        private protected override void Dispose(bool disposing) {
            if (!this._disposed) {
                if (disposing) {
                    // ...
                    //
                    // Cancel the synchronization task.
                    this._syncTaskCancellationTokenSource.Cancel();
                    this._syncTask.Wait();
                    this._syncTask.Dispose();
                    this._syncTaskCancellationTokenSource.Dispose();
                }

                base.Dispose(disposing);
                this._disposed = true;
            }
        }

        /// <summary>
        ///     Synchronize Database File Asynchronously.
        /// </summary>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        private async Task SyncDatabaseFileAsync() {
            var cancellationToken = this._syncTaskCancellationTokenSource.Token;
            while (!cancellationToken.IsCancellationRequested) {
                try {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var getThreatListsTask = this.GetThreatListsAsync();
                    var threatLists = await getThreatListsTask.ConfigureAwait(false);
                    List<ThreatListModel> threatListModels = null;
                    foreach (var threatList in threatLists) {
                        // ...
                        //
                        // Throws an exception if the operation fails.
                        var getThreatsTask = this.GetThreatsAsync(threatList.Descriptor);
                        var threats = await getThreatsTask.ConfigureAwait(false);

                        threatListModels = threatListModels ?? new List<ThreatListModel>(threatLists.Count);
                        threatListModels.Add(new ThreatListModel {
                            PlatformType = threatList.Descriptor.PlatformType,
                            RetrieveDate = threatList.RetrieveDate,
                            State = threatList.State,
                            ThreatEntryType = threatList.Descriptor.ThreatEntryType,
                            ThreatSha256HashPrefixes = threats,
                            ThreatType = threatList.Descriptor.ThreatType,
                            WaitToDate = threatList.WaitToDate
                        });
                    }

                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var fileModel = new FileModel {ThreatLists = threatListModels};
                    this.DatabaseFileManager.Write(fileModel);
                }
                catch {
                    // ...
                    //
                    // We don't care if we failed to synchronize the database file. We'll try again on the next
                    // iteration.
                }
                finally {
                    var delayTask = DelayAsync(this);
                    await delayTask.ConfigureAwait(false);
                }
            }

            // <summary>
            //      Delay Asynchronously.
            // </summary>
            async Task DelayAsync(ManagedJsonBrowsingDatabase @this) {
                try {
                    var cCancellationToken = @this._syncTaskCancellationTokenSource.Token;
                    var cDelayTask = Task.Delay(ManagedJsonBrowsingDatabase.SyncInterval, cCancellationToken);
                    await cDelayTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException) {
                    // ...
                    //
                    // We don't care if the cancellation token is cancelled.
                }
            }
        }
    }
}