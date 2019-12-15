using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Abstract Cache.
    /// </summary>
    public interface IBrowsingCache : IDisposable {
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
        Task<SafeCacheEntry> GetSafeCacheEntryAsync(string threatSha256HashPrefix, CancellationToken cancellationToken);

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
        Task<UnsafeCacheEntry> GetUnsafeCacheEntryAsync(string threatSha256Hash, CancellationToken cancellationToken);

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
        Task PutSafeCacheEntryAsync(SafeCacheEntry safeCacheEntry, CancellationToken cancellationToken);

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
        Task PutUnsafeCacheEntryAsync(UnsafeCacheEntry unsafeCacheEntry, CancellationToken cancellationToken);

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
        Task RemoveSafeCacheEntryAsync(string threatSha256HashPrefix, CancellationToken cancellationToken);

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
        Task RemoveUnsafeCacheEntryAsync(string threatSha256Hash, CancellationToken cancellationToken);
    }
}