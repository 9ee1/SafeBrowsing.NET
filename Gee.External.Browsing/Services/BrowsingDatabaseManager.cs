using Gee.Common.Guards;
using Gee.Common.Threading.Tasks;
using Gee.External.Browsing.Clients;
using Gee.External.Browsing.Databases;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Database Manager.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Represents a database manager that setups a database and periodically synchronizes it with threat lists
    ///         updates retrieved from the Google Safe Browsing API. Once a database manager is created, it will
    ///         periodically run a synchronization operation on a background thread to retrieve threat list updates
    ///         from the Google Safe Browsing API, remove stale threat from, and add new threat to a database until it
    ///         is disposed. Between every synchronization operation, the database manager will pause for a period
    ///         indicated by the Google Safe Browsing API. If the Google Safe Browsing API does not indicate a period
    ///         to pause for, an implementation specific period is used. To indicate when a synchronization operation
    ///         has started and either completed successfully or failed, the database manager raises relevant events
    ///         you can register event handlers for.
    ///     </para>
    ///     <para>
    ///         Unless you restrict it to do so otherwise, by default a database manager will retrieve threat list
    ///         updates for all threat lists made available by the Google Safe Browsing API. When a synchronization
    ///         operation runs, it will not only synchronize existing threat lists in the database but it will also
    ///         synchronize new threat lists made available, if any, since the last synchronization operation. While
    ///         this may have the desired effect of ensuring a database is synchronized with all available threat
    ///         lists, it will have a significant impact on the amount of time needed to do so as well as on bandwidth,
    ///         memory, and disk utilization. As a rule of thumb, the more threat lists that need to be synchronized,
    ///         the longer it will take for a synchronization operation to complete and the more bandwidth, memory, and
    ///         disk utilization that will be required. If you are not interested in retrieving threat list updates for
    ///         all available threat lists, you are encouraged to restrict a database manager to the specific threat
    ///         lists you are interested in to minimize resource utilization.
    ///     </para>
    /// </remarks>
    public sealed class BrowsingDatabaseManager : IDisposable {
        /// <summary>
        ///     Threat List Synchronization Completed Event.
        /// </summary>
        public event Action<ThreatListSynchronizationCompletedEventArgs> ThreatListSynchronizationCompleted;

        /// <summary>
        ///     Threat List Synchronization Failed Event.
        /// </summary>
        public event Action<ThreatListSynchronizationFailedEventArgs> ThreatListSynchronizationFailed;

        /// <summary>
        ///     Client.
        /// </summary>
        private readonly ResilientBrowsingClient _client;

        /// <summary>
        ///     Database.
        /// </summary>
        private readonly ResilientManagedBrowsingDatabase _database;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Synchronization Task.
        /// </summary>
        private readonly Task _synchronizationTask;

        /// <summary>
        ///     Synchronization Task Cancellation Token Source.
        /// </summary>
        private readonly CancellationTokenSource _synchronizationTaskCancellationTokenSource;

        /// <summary>
        ///     Update Constraints.
        /// </summary>
        private readonly Dictionary<ThreatListDescriptor, ThreatListUpdateConstraints> _updateConstraints;

        /// <summary>
        ///     Build a Database Synchronizer.
        /// </summary>
        /// <returns>
        ///     A <see cref="BrowsingDatabaseManagerBuilder" /> to build a database synchronizer with.
        /// </returns>
        public static BrowsingDatabaseManagerBuilder Build() {
            return new BrowsingDatabaseManagerBuilder();
        }

        /// <summary>
        ///     Create a Database Manager.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="BrowsingDatabaseManagerBuilder" /> to initialize the database manager with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal BrowsingDatabaseManager(BrowsingDatabaseManagerBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();

            this._client = ResilientBrowsingClient.Create(builder.Client, 5, builder.OwnClient);
            this._database = ResilientManagedBrowsingDatabase.Create(builder.Database, 5, builder.OwnDatabase);
            this._disposed = false;
            this._synchronizationTaskCancellationTokenSource = new CancellationTokenSource();
            this._updateConstraints = builder.UpdateConstraints;
            // ...
            //
            // ...
            this._synchronizationTask = Task.Run(this.SynchronizeDatabaseAsync);
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
                // ...
                //
                // Cancel the database synchronization task and wait for it to complete.
                this._disposed = true;
                this._synchronizationTaskCancellationTokenSource.Cancel();
                this._synchronizationTask.Wait();

                // ...
                //
                // Dispose the database synchronization task.
                this._synchronizationTask.Dispose();
                this._synchronizationTaskCancellationTokenSource.Dispose();

                // ...
                //
                // Dispose the client and the database.
                this._client.Dispose();
                this._database.Dispose();
            }
        }

        /// <summary>
        ///     Invoke Threat List Synchronization Completed Event.
        /// </summary>
        /// <param name="eventArgs">
        ///     A threat list synchronization completed event arguments indicating the nature of the event.
        /// </param>
        private void OnThreatListSynchronizationCompleted(ThreatListSynchronizationCompletedEventArgs eventArgs) {
            try {
                var @event = this.ThreatListSynchronizationCompleted;
                @event?.Invoke(eventArgs);
            }
            catch {
                // ...
                //
                // Suppress any exceptions thrown by registered event handlers.
            }
        }

        /// <summary>
        ///     Invoke Threat List Synchronization Failed Event.
        /// </summary>
        /// <param name="eventArgs">
        ///     A threat list synchronization failed event arguments indicating the nature of the event.
        /// </param>
        private void OnThreatListSynchronizationFailed(ThreatListSynchronizationFailedEventArgs eventArgs) {
            try {
                var @event = this.ThreatListSynchronizationFailed;
                @event?.Invoke(eventArgs);
            }
            catch {
                // ...
                //
                // Suppress any exceptions thrown by registered event handlers.
            }
        }

        /// <summary>
        ///     Synchronize Database Asynchronously.
        /// </summary>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        private async Task SynchronizeDatabaseAsync() {
            var cancellationToken = this._synchronizationTaskCancellationTokenSource.Token;
            while (!cancellationToken.IsCancellationRequested) {
                var getThreatListUpdatesTask = GetThreatListUpdatesAsync(this);
                var (threatListUpdateResponse, delayToDate) = await getThreatListUpdatesTask.ConfigureAwait(false);
                if (threatListUpdateResponse != null) {
                    foreach (var threatListUpdateResult in threatListUpdateResponse.Results) {
                        var threatListWaitToDate = threatListUpdateResult.RetrievedThreatList.WaitToDate;
                        if (threatListWaitToDate != null && threatListWaitToDate < delayToDate) {
                            delayToDate = threatListWaitToDate.Value;
                        }

                        var synchronizeThreatListTask = SynchronizeThreatListAsync(this, threatListUpdateResult);
                        await synchronizeThreatListTask.ConfigureAwait(false);
                    }
                }

                var delayToTask = DelayToAsync(this, delayToDate);
                await delayToTask.ConfigureAwait(false);
            }

            // <summary>
            //      Delay to a Date Asynchronously.
            // </summary>
            async Task DelayToAsync(BrowsingDatabaseManager @this, DateTime cDate) {
                try {
                    var cCancellationToken = @this._synchronizationTaskCancellationTokenSource.Token;
                    var cDelayToTask = TaskExtension.DelayTo(cDate, cCancellationToken);
                    await cDelayToTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException) {
                    // ...
                    //
                    // We don't care if the cancellation token is cancelled.
                }
            }

            // <summary>
            //      Get Threat List Updates Asynchronously.
            // </summary>
            async Task<(ThreatListUpdateResponse, DateTime)> GetThreatListUpdatesAsync(BrowsingDatabaseManager @this) {
                var cDelayToDate = DateTime.UtcNow.AddMinutes(30);
                try {
                    IEnumerable<ThreatListDescriptor> cThreatListDescriptors = @this._updateConstraints.Keys;
                    if (@this._updateConstraints.Count == 0) {
                        // ...
                        //
                        // If the database synchronizer is not restricted to specific threat lists, we retrieve all
                        // available threat lists from the Google Safe Browsing API. We don't cache the result in case
                        // new threat lists are made available between synchronization iterations.
                        //
                        // Throws an exception if the operation fails.
                        var cGetThreatListDescriptorsTask = @this._client.GetThreatListDescriptorsAsync();
                        cThreatListDescriptors = await cGetThreatListDescriptorsTask.ConfigureAwait(false);
                    }

                    // ...
                    //
                    // Retrieve the threat lists from the database. Throws an exception if the operation fails.
                    var cGetThreatListsTask = @this._database.GetThreatListsAsync(cThreatListDescriptors);
                    var cThreatLists = await cGetThreatListsTask.ConfigureAwait(false);

                    ThreatListUpdateRequestBuilder cThreatListUpdateRequestBuilder = null;
                    foreach (var cThreatList in cThreatLists) {
                        if (cThreatList.Expired) {
                            cThreatListUpdateRequestBuilder = cThreatListUpdateRequestBuilder ?? ThreatListUpdateRequest.Build();
                            var cThreatListDescriptor = cThreatList.Descriptor;
                            var cThreatListState = cThreatList.State;
                            @this._updateConstraints.TryGetValue(cThreatListDescriptor, out var cThreatListUpdateConstraints);
                            cThreatListUpdateRequestBuilder.AddQuery(b => {
                                b.SetThreatListDescriptor(cThreatListDescriptor);
                                b.SetThreatListState(cThreatListState);
                                b.SetUpdateConstraints(cThreatListUpdateConstraints);
                                return b.Build();
                            });
                        }
                        else {
                            if (cThreatList.WaitToDate != null && cThreatList.WaitToDate < cDelayToDate) {
                                // ...
                                //
                                // If the current threat list's wait to date is earlier than the previous threat
                                // list's wait to date, take the current the threat list's wait to date instead.
                                cDelayToDate = cThreatList.WaitToDate.Value;
                            }
                        }
                    }

                    ThreatListUpdateResponse cThreatListUpdateResponse = null;
                    if (cThreatListUpdateRequestBuilder != null) {
                        // ...
                        //
                        // Throws an exception if the operation fails.
                        var cThreatListUpdateRequest = cThreatListUpdateRequestBuilder.Build();
                        var cGetThreatListUpdatesTask = @this._client.GetThreatListUpdatesAsync(cThreatListUpdateRequest);
                        cThreatListUpdateResponse = await cGetThreatListUpdatesTask.ConfigureAwait(false);
                    }

                    return (cThreatListUpdateResponse, cDelayToDate);
                }
                catch {
                    return (null, cDelayToDate);
                }
            }

            // <summary>
            //      Synchronize Threat List Asynchronously.
            // </summary>
            async Task SynchronizeThreatListAsync(BrowsingDatabaseManager @this, ThreatListUpdateResult cThreatListUpdateResult) {
                var cSynchronizationStartDate = DateTime.UtcNow;
                var cThreatList = cThreatListUpdateResult.RetrievedThreatList;
                try {
                    var cSha256HashPrefixes = cThreatListUpdateResult.ThreatsToAdd;
                    if (cThreatListUpdateResult.IsFullUpdate) {
                        // ...
                        //
                        // Throws an exception if the operation fails.
                        var cModifyThreatListTask = @this._database.StoreThreatListAsync(cThreatList, cSha256HashPrefixes);
                        await cModifyThreatListTask.ConfigureAwait(false);
                    }
                    else if (cThreatListUpdateResult.IsPartialUpdate) {
                        // ...
                        //
                        // Throws an exception if the operation fails.
                        var cIndices = cThreatListUpdateResult.ThreatsToRemove;
                        var cModifyThreatListTask = @this._database.ModifyThreatListAsync(cThreatList, cSha256HashPrefixes, cIndices);
                        await cModifyThreatListTask.ConfigureAwait(false);
                    }

                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var cComputeThreatListChecksumTask = @this._database.ComputeThreatListChecksumAsync(cThreatList.Descriptor);
                    var cThreatListChecksum = await cComputeThreatListChecksumTask.ConfigureAwait(false);
                    if (cThreatListChecksum != cThreatListUpdateResult.RetrievedThreatListChecksum) {
                        // ...
                        //
                        // Throws an exception if the operation fails.
                        cThreatList = ThreatList.CreateInvalid(cThreatList.Descriptor);
                        var cUpdateConstraints = cThreatListUpdateResult.Query.UpdateConstraints;
                        var cGetThreatListUpdatesTask = @this._client.GetThreatListUpdatesAsync(cThreatList, cUpdateConstraints);
                        var cThreatListUpdateRequest = await cGetThreatListUpdatesTask.ConfigureAwait(false);

                        cThreatListUpdateResult = cThreatListUpdateRequest.Results.First();
                        cSha256HashPrefixes = cThreatListUpdateResult.ThreatsToAdd;
                        if (cThreatListUpdateResult.IsFullUpdate) {
                            // ...
                            //
                            // Throws an exception if the operation fails.
                            cThreatList = cThreatListUpdateResult.RetrievedThreatList;
                            var cModifyThreatListTask = @this._database.StoreThreatListAsync(cThreatList, cSha256HashPrefixes);
                            await cModifyThreatListTask.ConfigureAwait(false);
                        }
                    }

                    // ...
                    //
                    // Invoke threat list synchronization completed event.
                    var cSynchronizationCompletionDate = DateTime.UtcNow;
                    @this.OnThreatListSynchronizationCompleted(new ThreatListSynchronizationCompletedEventArgs(
                        cThreatList,
                        cSynchronizationStartDate,
                        cSynchronizationCompletionDate
                    ));
                }
                catch (Exception cEx) {
                    // ...
                    //
                    // Invoke threat list synchronization failed event.
                    var cSynchronizationFailureDate = DateTime.UtcNow;
                    @this.OnThreatListSynchronizationFailed(new ThreatListSynchronizationFailedEventArgs(
                        cThreatList,
                        cSynchronizationStartDate,
                        cSynchronizationFailureDate,
                        cEx
                    ));
                }
            }
        }
    }
}