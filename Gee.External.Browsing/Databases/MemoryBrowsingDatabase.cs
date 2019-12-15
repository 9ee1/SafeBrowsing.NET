using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Memory Database.
    /// </summary>
    public sealed class MemoryBrowsingDatabase : IManagedBrowsingDatabase {
        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Lock.
        /// </summary>
        private readonly object _lock;

        /// <summary>
        ///     Threat Lists.
        /// </summary>
        private readonly Dictionary<ThreatListDescriptor, ThreatList> _threatLists;

        /// <summary>
        ///     Threat Entries.
        /// </summary>
        private readonly Dictionary<ThreatListDescriptor, List<string>> _threats;

        /// <summary>
        ///     Create a Memory Database.
        /// </summary>
        public MemoryBrowsingDatabase() {
            this._disposed = false;
            this._lock = new object();
            this._threatLists = new Dictionary<ThreatListDescriptor, ThreatList>();
            this._threats = new Dictionary<ThreatListDescriptor, List<string>>();
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
                this._disposed = true;
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
        public Task<IReadOnlyCollection<ThreatList>> FindThreatListsAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            Guard.ThrowIf(nameof(threatSha256HashPrefix), threatSha256HashPrefix).Null();

            lock (this._lock) {
                List<ThreatList> threatLists = null;
                foreach (var threatDictionaryEntry in this._threats) {
                    // ...
                    //
                    // Searching the threat list requires it to be presorted.
                    var threatSha256HashPrefixIndex = threatDictionaryEntry.Value.BinarySearch(threatSha256HashPrefix);
                    if (threatSha256HashPrefixIndex >= 0) {
                        // ...
                        //
                        // Allocate memory if at least one threat list is found.
                        var threatList = this._threatLists[threatDictionaryEntry.Key];
                        threatLists = threatLists ?? new List<ThreatList>();
                        threatLists.Add(threatList);
                    }
                }

                var newThreatLists = threatLists ?? (IReadOnlyCollection<ThreatList>) Array.Empty<ThreatList>();
                var findThreatListsTask = Task.FromResult(newThreatLists);
                return findThreatListsTask;
            }
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
        public Task<ThreatList> GetThreatListAsync(ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            lock (this._lock) {
                // ...
                //
                // Throws an exception if the operation fails.
                this._threatLists.TryGetValue(threatListDescriptor, out var threatList);

                var getThreatListTask = Task.FromResult(threatList);
                return getThreatListTask;
            }
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
        public Task<IReadOnlyCollection<ThreatList>> GetThreatListsAsync(CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            lock (this._lock) {
                IReadOnlyCollection<ThreatList> threatLists = this._threatLists.Values;
                var getThreatListsTask = Task.FromResult(threatLists);
                return getThreatListsTask;
            }
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
        public Task<IReadOnlyCollection<string>> GetThreatsAsync(ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            lock (this._lock) {
                // ...
                //
                // Throws an exception if the operation fails.
                this._threats.TryGetValue(threatListDescriptor, out var threats);

                var newThreats = threats ?? (IReadOnlyCollection<string>) Array.Empty<string>();
                var getThreatsTask = Task.FromResult(newThreats);
                return getThreatsTask;
            }
        }

        /// <summary>
        ///     Initialize Database Asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
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
        public Task InitializeAsync(CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            return Task.CompletedTask;
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
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public Task ModifyThreatListAsync(ThreatList threatList, IEnumerable<string> threatSha256HashPrefixes, IEnumerable<int> threatIndices, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            Guard.ThrowIf(nameof(threatSha256HashPrefixes), threatSha256HashPrefixes).Null();
            Guard.ThrowIf(nameof(threatIndices), threatIndices).Null();

            lock (this._lock) {
                var newSha256HashPrefixes = new List<string>();
                this._threats.TryGetValue(threatList.Descriptor, out var threats);
                if (threats != null) {
                    // ...
                    //
                    // Filtering the threat threat requires it to be presorted.
                    var newIndices = Enumerable.Range(0, threats.Count).Except(threatIndices);
                    foreach (var newIndex in newIndices) {
                        var threat = threats[newIndex];
                        newSha256HashPrefixes.Add(threat);
                    }
                }

                // ...
                //
                // We need to sort the threat list so that it can later be searched.
                newSha256HashPrefixes.AddRange(threatSha256HashPrefixes);
                newSha256HashPrefixes.Sort();
                this._threatLists[threatList.Descriptor] = threatList;
                this._threats[threatList.Descriptor] = newSha256HashPrefixes;

                return Task.CompletedTask;
            }
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
        public Task StoreThreatListAsync(ThreatList threatList, IEnumerable<string> threatSha256HashPrefixes, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            Guard.ThrowIf(nameof(threatList), threatList).Null();

            lock (this._lock) {
                // ...
                //
                // We need to sort the threat list so that it can later be searched. Throws an exception if the
                // operation fails.
                var newSha256HashPrefixes = new List<string>(threatSha256HashPrefixes);
                newSha256HashPrefixes.Sort();

                this._threatLists[threatList.Descriptor] = threatList;
                this._threats[threatList.Descriptor] = newSha256HashPrefixes;
                return Task.CompletedTask;
            }
        }

        /// <summary>
        ///     Throw an Exception if Object is Disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        private void ThrowIfDisposed() {
            if (this._disposed) {
                var detailMessage = $"An object ({nameof(MemoryBrowsingDatabase)}) is disposed.";
                throw new ObjectDisposedException(nameof(MemoryBrowsingDatabase), detailMessage);
            }
        }
    }
}