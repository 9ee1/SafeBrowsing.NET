using Gee.Common.Guards;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Cache Proxy.
    /// </summary>
    internal sealed class BrowsingCacheProxy : IBrowsingCache {
        /// <summary>
        ///     Cache.
        /// </summary>
        private readonly IBrowsingCache _cache;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Own Cache Flag.
        /// </summary>
        private readonly bool _ownCache;

        /// <summary>
        ///     Create a Cache Proxy.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to. The cache proxy takes ownership of
        ///     <paramref name="cache" /> and will dispose it when the cache proxy itself is disposed. If you reference
        ///     or dispose <paramref name="cache" /> after you create the cache proxy, the behavior of the cache proxy
        ///     and <paramref name="cache" /> is undefined.
        /// </param>
        /// <returns>
        ///     A cache proxy.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        internal static BrowsingCacheProxy Create(IBrowsingCache cache) => BrowsingCacheProxy.Create(cache, true);

        /// <summary>
        ///     Create a Cache Proxy.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to.
        /// </param>
        /// <param name="ownCache">
        ///     A boolean flag indicating whether or not the cache proxy takes ownership of <paramref name="cache" />
        ///     and disposes it when the cache proxy itself is disposed. If the cache proxy takes ownership of
        ///     <paramref name="cache" /> and you reference or dispose <paramref name="cache" /> after you create the
        ///     cache proxy, the behavior of the cache proxy and <paramref name="cache" /> is undefined.
        /// </param>
        /// <returns>
        ///     A cache proxy.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        internal static BrowsingCacheProxy Create(IBrowsingCache cache, bool ownCache) {
            // ...
            //
            // Throws an exception if the operation fails.
            return cache is BrowsingCacheProxy cacheProxy ? cacheProxy : new BrowsingCacheProxy(cache, ownCache);
        }

        /// <summary>
        ///     Create a Cache Proxy.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to. The cache proxy takes ownership of
        ///     <paramref name="cache" /> and will dispose it when the cache proxy itself is disposed. If you reference
        ///     or dispose <paramref name="cache" /> after you create the cache proxy, the behavior of the cache proxy
        ///     and <paramref name="cache" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        [SuppressMessage("ReSharper", "IntroduceOptionalParameters.Local")]
        private BrowsingCacheProxy(IBrowsingCache cache) : this(cache, true) { }

        /// <summary>
        ///     Create a Cache Proxy.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to.
        /// </param>
        /// <param name="ownCache">
        ///     A boolean flag indicating whether or not the cache proxy takes ownership of <paramref name="cache" />
        ///     and disposes it when the cache proxy itself is disposed. If the cache proxy takes ownership of
        ///     <paramref name="cache" /> and you reference or dispose <paramref name="cache" /> after you create the
        ///     cache proxy, the behavior of the cache proxy and <paramref name="cache" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        private BrowsingCacheProxy(IBrowsingCache cache, bool ownCache) {
            Guard.ThrowIf(nameof(cache), cache).Null();

            this._cache = cache;
            this._disposed = false;
            this._ownCache = ownCache;
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
                this._disposed = true;
                if (this._ownCache) {
                    this._cache.Dispose();
                }
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
        public async Task<SafeCacheEntry> GetSafeCacheEntryAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var getSafeCacheEntryTask = this._cache.GetSafeCacheEntryAsync(threatSha256HashPrefix, cancellationToken);
                var safeCacheEntry = await getSafeCacheEntryTask.ConfigureAwait(false);
                return safeCacheEntry;
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingCacheException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "A safe cache entry could not be retrieved.";
                throw new BrowsingCacheException(detailMessage, ex);
            }
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
        public async Task<UnsafeCacheEntry> GetUnsafeCacheEntryAsync(string threatSha256Hash, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var getUnsafeCacheEntryTask = this._cache.GetUnsafeCacheEntryAsync(threatSha256Hash, cancellationToken);
                var unsafeCacheEntry = await getUnsafeCacheEntryTask.ConfigureAwait(false);
                return unsafeCacheEntry;
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingCacheException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "An unsafe cache entry could not be retrieved.";
                throw new BrowsingCacheException(detailMessage, ex);
            }
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
        public async Task PutSafeCacheEntryAsync(SafeCacheEntry safeCacheEntry, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var putSafeCacheEntryTask = this._cache.PutSafeCacheEntryAsync(safeCacheEntry, cancellationToken);
                await putSafeCacheEntryTask.ConfigureAwait(false);
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingCacheException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "A safe cache entry could not be cached.";
                throw new BrowsingCacheException(detailMessage, ex);
            }
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
        public async Task PutUnsafeCacheEntryAsync(UnsafeCacheEntry unsafeCacheEntry, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var putUnsafeCacheEntryTask = this._cache.PutUnsafeCacheEntryAsync(unsafeCacheEntry, cancellationToken);
                await putUnsafeCacheEntryTask.ConfigureAwait(false);
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingCacheException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "An unsafe cache entry could not be cached.";
                throw new BrowsingCacheException(detailMessage, ex);
            }
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
        public async Task RemoveSafeCacheEntryAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var removeSafeCacheEntryTask = this._cache.RemoveSafeCacheEntryAsync(threatSha256HashPrefix, cancellationToken);
                await removeSafeCacheEntryTask.ConfigureAwait(false);
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingCacheException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "A safe cache entry could not be removed.";
                throw new BrowsingCacheException(detailMessage, ex);
            }
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
        public async Task RemoveUnsafeCacheEntryAsync(string threatSha256Hash, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var removeUnsafeCacheEntryTask = this._cache.RemoveUnsafeCacheEntryAsync(threatSha256Hash, cancellationToken);
                await removeUnsafeCacheEntryTask.ConfigureAwait(false);
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingCacheException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "An unsafe cache entry could not be removed.";
                throw new BrowsingCacheException(detailMessage, ex);
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
                var detailMessage = $"An object ({nameof(BrowsingCacheProxy)}) is disposed.";
                throw new ObjectDisposedException(nameof(BrowsingCacheProxy), detailMessage);
            }
        }
    }
}