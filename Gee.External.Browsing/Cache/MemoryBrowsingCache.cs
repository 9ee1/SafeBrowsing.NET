using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Memory Cache.
    /// </summary>
    public sealed class MemoryBrowsingCache : IBrowsingCache {
        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Safe Cache.
        /// </summary>
        private readonly ConcurrentDictionary<string, SafeCacheEntry> _safeCache;

        /// <summary>
        ///     Unsafe Cache.
        /// </summary>
        private readonly ConcurrentDictionary<string, UnsafeCacheEntry> _unsafeCache;

        /// <summary>
        ///     Create a Memory Cache.
        /// </summary>
        public MemoryBrowsingCache() {
            this._disposed = false;
            this._safeCache = new ConcurrentDictionary<string, SafeCacheEntry>();
            this._unsafeCache = new ConcurrentDictionary<string, UnsafeCacheEntry>();
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
        ///     Get a Safe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to retrieve from
        ///     the cache.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     The <see cref="SafeCacheEntry" /> cached for the threat identified by
        ///     <paramref name="threatSha256HashPrefix" />. A null reference indicates a safe cache entry is not
        ///     cached.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
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
        public Task<SafeCacheEntry> GetSafeCacheEntryAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            this._safeCache.TryGetValue(threatSha256HashPrefix, out var safeCacheEntry);
            var safeCacheEntryTask = Task.FromResult(safeCacheEntry);
            return safeCacheEntryTask;
        }

        /// <summary>
        ///     Get an Unsafe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat to retrieve from
        ///     the cache.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     The <see cref="UnsafeCacheEntry" /> cached for the threat identified by
        ///     <paramref name="threatSha256Hash" />. A null reference indicates an unsafe cache entry is not cached.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public Task<UnsafeCacheEntry> GetUnsafeCacheEntryAsync(string threatSha256Hash, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            this._unsafeCache.TryGetValue(threatSha256Hash, out var unsafeCacheEntry);
            var unsafeCacheEntryTask = Task.FromResult(unsafeCacheEntry);
            return unsafeCacheEntryTask;
        }

        /// <summary>
        ///     Put a Safe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="safeCacheEntry">
        ///     A <see cref="SafeCacheEntry" /> to cache.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="safeCacheEntry" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Task PutSafeCacheEntryAsync(SafeCacheEntry safeCacheEntry, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatSha256HashPrefix = safeCacheEntry?.ThreatSha256HashPrefix;
            this._safeCache.AddOrUpdate(threatSha256HashPrefix, safeCacheEntry, (k, v) => safeCacheEntry);
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Put an Unsafe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="unsafeCacheEntry">
        ///     An <see cref="UnsafeCacheEntry" /> to cache.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="unsafeCacheEntry" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Task PutUnsafeCacheEntryAsync(UnsafeCacheEntry unsafeCacheEntry, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatSha256Hash = unsafeCacheEntry?.ThreatSha256Hash;
            this._unsafeCache.AddOrUpdate(threatSha256Hash, unsafeCacheEntry, (k, v) => unsafeCacheEntry);
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Remove a Safe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to remove from
        ///     the cache.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
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
        public Task RemoveSafeCacheEntryAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            this._safeCache.TryRemove(threatSha256HashPrefix, out _);
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Remove an Unsafe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat to remove from the
        ///     cache.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public Task RemoveUnsafeCacheEntryAsync(string threatSha256Hash, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            this._unsafeCache.TryRemove(threatSha256Hash, out _);
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Throw an Exception if Object is Disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        private void ThrowIfDisposed() {
            if (this._disposed) {
                var detailMessage = $"An object ({nameof(MemoryBrowsingCache)}) is disposed.";
                throw new ObjectDisposedException(nameof(MemoryBrowsingCache), detailMessage);
            }
        }
    }
}