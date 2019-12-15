using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Cache Extension.
    /// </summary>
    public static class BrowsingCacheExtension {
        /// <summary>
        ///     Get a Safe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to retrieve from
        ///     the cache.
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
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<SafeCacheEntry> GetSafeCacheEntryAsync(this IBrowsingCache @this, string threatSha256HashPrefix) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var getSafeCacheEntryTask = @this.GetSafeCacheEntryAsync(threatSha256HashPrefix, CancellationToken.None);
            return getSafeCacheEntryTask;
        }

        /// <summary>
        ///     Get an Unsafe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat to retrieve from
        ///     the cache.
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
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<UnsafeCacheEntry> GetUnsafeCacheEntryAsync(this IBrowsingCache @this, string threatSha256Hash) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var getUnsafeCacheEntryTask = @this.GetUnsafeCacheEntryAsync(threatSha256Hash, CancellationToken.None);
            return getUnsafeCacheEntryTask;
        }

        /// <summary>
        ///     Lookup a Threat Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat to lookup in the
        ///     cache.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to lookup in
        ///     the cache.
        /// </param>
        /// <returns>
        ///     A <see cref="CacheLookupResult" /> indicating the nature of the operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatSha256Hash" /> is
        ///     a null reference, or if <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<CacheLookupResult> LookupAsync(this IBrowsingCache @this, string threatSha256Hash, string threatSha256HashPrefix) => @this.LookupAsync(threatSha256Hash, threatSha256HashPrefix, CancellationToken.None);

        /// <summary>
        ///     Lookup a Threat Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat to lookup in the
        ///     cache.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to lookup in
        ///     the cache.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A <see cref="CacheLookupResult" /> indicating the nature of the operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatSha256Hash" /> is
        ///     a null reference, or if <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public static async Task<CacheLookupResult> LookupAsync(this IBrowsingCache @this, string threatSha256Hash, string threatSha256HashPrefix, CancellationToken cancellationToken) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // First, check if the full SHA256 hash identifies a cached unsafe threat. Throws an exception if the
            // operation fails.
            CacheLookupResult cacheLookupResult;
            var getUnsafeCacheEntryTask = @this.GetUnsafeCacheEntryAsync(threatSha256Hash, cancellationToken);
            var unsafeCacheEntry = await getUnsafeCacheEntryTask.ConfigureAwait(false);
            if (unsafeCacheEntry != null) {
                if (!unsafeCacheEntry.Expired) {
                    // ..
                    //
                    // If the full SHA256 hash identifies an unexpired cached unsafe threat, indicate a cache unsafe
                    // hit.
                    var unsafeThreats = unsafeCacheEntry.UnsafeThreats;
                    cacheLookupResult = CacheLookupResult.CacheUnsafeHit(threatSha256Hash, threatSha256HashPrefix, unsafeThreats);
                }
                else {
                    // ...
                    //
                    // If the full SHA256 hash identifies an expired cached unsafe threat, indicate a cache miss.
                    // Throws an exception if the operation fails.
                    var removeUnsafeCacheEntryTask = @this.RemoveUnsafeCacheEntryAsync(threatSha256Hash, cancellationToken);
                    await removeUnsafeCacheEntryTask.ConfigureAwait(false);
                    cacheLookupResult = CacheLookupResult.CacheMiss(threatSha256Hash, threatSha256HashPrefix);
                }
            }
            else {
                // ...
                //
                // If the full SHA256 hash does not identify a cached unsafe threat, check if the SHA256 hash prefix
                // identifies a cached safe threat.
                //
                // Throws an exception if the operation fails.
                var getSafeCacheEntryTask = @this.GetSafeCacheEntryAsync(threatSha256HashPrefix, cancellationToken);
                var safeCacheEntry = await getSafeCacheEntryTask.ConfigureAwait(false);
                if (safeCacheEntry != null && !safeCacheEntry.Expired) {
                    // ...
                    //
                    // If the SHA256 hash prefix identifies an unexpired cache unsafe hit, indicate a cache safe hit.
                    cacheLookupResult = CacheLookupResult.CacheSafeHit(threatSha256Hash, threatSha256HashPrefix);
                }
                else {
                    // ...
                    //
                    // If the SHA256 hash prefix does not identify a cached safe threat, indicate a cache miss. Throws
                    // an exception if the operation fails.
                    var removeSafeCacheEntryTask = @this.RemoveSafeCacheEntryAsync(threatSha256HashPrefix, cancellationToken);
                    await removeSafeCacheEntryTask.ConfigureAwait(false);
                    cacheLookupResult = CacheLookupResult.CacheMiss(threatSha256Hash, threatSha256HashPrefix);
                }
            }

            return cacheLookupResult;
        }

        /// <summary>
        ///     Put a Safe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="safeCacheEntry">
        ///     A <see cref="SafeCacheEntry" /> to cache.
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
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task PutSafeCacheEntryAsync(this IBrowsingCache @this, SafeCacheEntry safeCacheEntry) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var putSafeCacheEntryTask = @this.PutSafeCacheEntryAsync(safeCacheEntry, CancellationToken.None);
            return putSafeCacheEntryTask;
        }

        /// <summary>
        ///     Put a Safe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to cache.
        /// </param>
        /// <param name="expirationDate">
        ///     The date, in Coordinated Universal Time (UTC), the safe cache entry expires and the threat identified
        ///     by <paramref name="threatSha256HashPrefix" /> should be considered safe to. If the date is not in UTC,
        ///     it is converted to it.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this"/> is disposed.
        /// </exception>
        public static Task PutSafeCacheEntryAsync(this IBrowsingCache @this, string threatSha256HashPrefix, DateTime expirationDate) => @this.PutSafeCacheEntryAsync(threatSha256HashPrefix, expirationDate, CancellationToken.None);

        /// <summary>
        ///     Put a Safe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to cache.
        /// </param>
        /// <param name="expirationDate">
        ///     The date, in Coordinated Universal Time (UTC), the safe cache entry expires and the threat identified
        ///     by <paramref name="threatSha256HashPrefix" /> should be considered safe to. If the date is not in UTC,
        ///     it is converted to it.
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
        ///     Thrown if <paramref name="this" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public static Task PutSafeCacheEntryAsync(this IBrowsingCache @this, string threatSha256HashPrefix, DateTime expirationDate, CancellationToken cancellationToken) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var safeCacheEntry = new SafeCacheEntry(threatSha256HashPrefix, expirationDate);
            var putSafeCacheEntryTask = @this.PutSafeCacheEntryAsync(safeCacheEntry, cancellationToken);
            return putSafeCacheEntryTask;
        }

        /// <summary>
        ///     Put an Unsafe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="unsafeCacheEntry">
        ///     An <see cref="UnsafeCacheEntry" /> to cache.
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
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task PutUnsafeCacheEntryAsync(this IBrowsingCache @this, UnsafeCacheEntry unsafeCacheEntry) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            var putUnsafeCacheEntryTask = @this.PutUnsafeCacheEntryAsync(unsafeCacheEntry, CancellationToken.None);
            return putUnsafeCacheEntryTask;
        }

        /// <summary>
        ///     Put an Unsafe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat to cache.
        /// </param>
        /// <param name="unsafeThreats">
        ///     A collection of <see cref="UnsafeThreat" /> to cache.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatSha256Hash" /> is a
        ///     null reference, or if <paramref name="unsafeThreats" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this"/> is disposed.
        /// </exception>
        public static Task PutUnsafeCacheEntryAsync(this IBrowsingCache @this, string threatSha256Hash, IEnumerable<UnsafeThreat> unsafeThreats) => @this.PutUnsafeCacheEntryAsync(threatSha256Hash, unsafeThreats, CancellationToken.None);

        /// <summary>
        ///     Put an Unsafe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat to cache.
        /// </param>
        /// <param name="unsafeThreats">
        ///     A collection of <see cref="UnsafeThreat" /> to cache.
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
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatSha256Hash" /> is a
        ///     null reference, or if <paramref name="unsafeThreats" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this"/> is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public static Task PutUnsafeCacheEntryAsync(this IBrowsingCache @this, string threatSha256Hash, IEnumerable<UnsafeThreat> unsafeThreats, CancellationToken cancellationToken) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var unsafeCacheEntry = new UnsafeCacheEntry(threatSha256Hash, unsafeThreats);
            var putUnsafeCacheEntryTask = @this.PutUnsafeCacheEntryAsync(unsafeCacheEntry, cancellationToken);
            return putUnsafeCacheEntryTask;
        }

        /// <summary>
        ///     Remove a Safe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to remove from
        ///     the cache.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task RemoveSafeCacheEntryAsync(this IBrowsingCache @this, string threatSha256HashPrefix) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var removeSafeCacheEntryTask = @this.RemoveSafeCacheEntryAsync(threatSha256HashPrefix, CancellationToken.None);
            return removeSafeCacheEntryTask;
        }

        /// <summary>
        ///     Remove an Unsafe Cache Entry Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat to remove from the
        ///     cache.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task RemoveUnsafeCacheEntryAsync(this IBrowsingCache @this, string threatSha256Hash) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var removeUnsafeCacheEntryTask = @this.RemoveUnsafeCacheEntryAsync(threatSha256Hash, CancellationToken.None);
            return removeUnsafeCacheEntryTask;
        }
    }
}