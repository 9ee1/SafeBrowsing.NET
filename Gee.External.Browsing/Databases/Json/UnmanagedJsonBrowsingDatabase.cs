using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases.Json {
    /// <summary>
    ///     Unmanaged JSON Database.
    /// </summary>
    public sealed class UnmanagedJsonBrowsingDatabase : BaseJsonBrowsingDatabase, IUnmanagedBrowsingDatabase {
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
        ///     Create an Unmanaged JSON Database.
        /// </summary>
        static UnmanagedJsonBrowsingDatabase() {
            UnmanagedJsonBrowsingDatabase.SyncInterval = TimeSpan.FromMinutes(1);
        }

        /// <summary>
        ///     Create an Unmanaged JSON Database.
        /// </summary>
        /// <param name="databaseFilePath">
        ///     An absolute file path to the database file. If the file does not exist, it will be created.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="databaseFilePath" /> is a null reference.
        /// </exception>
        public UnmanagedJsonBrowsingDatabase(string databaseFilePath) : base(databaseFilePath) {
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
                    var fileModel = this.DatabaseFileManager.Read();
                    if (fileModel.ThreatLists != null) {
                        foreach (var threatListModel in fileModel.ThreatLists) {
                            var platformType = threatListModel.PlatformType;
                            var threatEntryType = threatListModel.ThreatEntryType;
                            var threatType = threatListModel.ThreatType;
                            var threatList = ThreatList.Build()
                                .SetDescriptor(threatType, platformType, threatEntryType)
                                .SetRetrieveDate(threatListModel.RetrieveDate)
                                .SetState(threatListModel.State)
                                .SetWaitToDate(threatListModel.WaitToDate)
                                .Build();

                            // ...
                            //
                            // Throws an exception if the operation fails.
                            var threatSha256HashPrefixes = threatListModel.ThreatSha256HashPrefixes;
                            var storeThreatListTask = this.StoreThreatListAsync(threatList, threatSha256HashPrefixes, cancellationToken);
                            await storeThreatListTask.ConfigureAwait(false);
                        }
                    }
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
            async Task DelayAsync(UnmanagedJsonBrowsingDatabase @this) {
                try {
                    var cCancellationToken = @this._syncTaskCancellationTokenSource.Token;
                    var cDelayTask = Task.Delay(UnmanagedJsonBrowsingDatabase.SyncInterval, cCancellationToken);
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