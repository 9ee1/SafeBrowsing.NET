using Gee.External.Browsing.Cache;
using Gee.External.Browsing.Clients;
using Gee.External.Browsing.Databases;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Base Service.
    /// </summary>
    public abstract class BaseBrowsingService : IBrowsingService {
        /// <summary>
        ///     Create a Base Service.
        /// </summary>
        private protected BaseBrowsingService() { }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        ///     Lookup a URL.
        /// </summary>
        /// <param name="url">
        ///     A <see cref="Url" /> to lookup.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A <see cref="UrlLookupResult" /> indicating whether <paramref name="url" /> is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs. If you're not interested in handling this exception, catch
        ///     <see cref="BrowsingException" /> instead.
        /// </exception>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs. If you're not interested
        ///     in handling this exception, catch <see cref="BrowsingException" /> instead.
        /// </exception>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs. If you're not interested in handling this exception, catch
        ///     <see cref="BrowsingException" /> instead.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="url" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public abstract Task<UrlLookupResult> LookupAsync(Url url, CancellationToken cancellationToken);

        /// <summary>
        ///     Lookup a URL.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="database">
        ///     An <see cref="IUnmanagedBrowsingDatabase" />.
        /// </param>
        /// <param name="url">
        ///     A <see cref="Url" /> to lookup.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A <see cref="UrlLookupResult" /> indicating whether <paramref name="url" /> is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Cache.BrowsingCacheException">
        ///     Thrown if a caching error occurs. If you're not interested in handling this exception, catch
        ///     <see cref="BrowsingException" /> instead.
        /// </exception>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs. If you're not interested
        ///     in handling this exception, catch <see cref="BrowsingException" /> instead.
        /// </exception>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs. If you're not interested in handling this exception, catch
        ///     <see cref="BrowsingException" /> instead.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="url" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="cache" /> is disposed, or if <paramref name="client" /> is disposed, or if
        ///     <paramref name="database" /> is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        private protected async Task<UrlLookupResult> LookupAsync(IBrowsingCache cache, IBrowsingClient client, IUnmanagedBrowsingDatabase database, Url url, CancellationToken cancellationToken) {
            // ...
            //
            // First, lookup the URL in the local database. Throws an exception if the operation fails.
            var urlLookupDate = DateTime.UtcNow;
            UrlLookupResult urlLookupResult;
            var databaseLookupTask = database.LookupAsync(url);
            var databaseLookResult = await databaseLookupTask.ConfigureAwait(false);
            if (databaseLookResult.IsDatabaseMiss) {
                // ...
                //
                // If the URL does not exist in the local database, indicate the URL is safe.
                urlLookupResult = UrlLookupResult.Safe(url, urlLookupDate);
            }
            else if (databaseLookResult.IsDatabaseStale) {
                // ...
                //
                // If the local database is expired/out-of-date/stale, indicate the nature of the URL cannot
                // be determined as a result.
                urlLookupResult = UrlLookupResult.DatabaseStale(url, urlLookupDate);
            }
            else {
                // ...
                //
                // If the URL exists in the local database, look it up in the cache. Throws an exception if
                // the operation fails.
                var sha256Hash = databaseLookResult.Sha256Hash;
                var sha256HashPrefix = databaseLookResult.Sha256HashPrefix;
                var cacheLookupTask = cache.LookupAsync(sha256Hash, sha256HashPrefix, cancellationToken);
                var cacheLookupResult = await cacheLookupTask.ConfigureAwait(false);
                if (cacheLookupResult.IsCacheSafeHit) {
                    // ...
                    //
                    // If we get a cache safe hit, indicate the URL is safe.
                    urlLookupResult = UrlLookupResult.Safe(url, urlLookupDate);
                }
                else if (cacheLookupResult.IsCacheUnsafeHit) {
                    // ...
                    //
                    // If we get a cache unsafe hit, indicate the URL is unsafe. If we get a cache unsafe hit,
                    // it is guaranteed we find a URL expression for the threat, since it is essentially the
                    // URL expression that is cached.
                    var unsafeThreatListDescriptors = cacheLookupResult.UnsafeThreatListDescriptors;
                    url.TryGetExpressionForSha256Hash(sha256Hash, out var unsafeUrlExpression);
                    urlLookupResult = UrlLookupResult.Unsafe(url, urlLookupDate, unsafeUrlExpression, unsafeThreatListDescriptors);
                }
                else {
                    // ...
                    //
                    // If we get a cache miss, verify the URL with the Google Safe Browsing API. Throws an
                    // exception if the operation fails.
                    var threatLists = databaseLookResult.ThreatLists;
                    var findFullHashesTask = client.FindFullHashesAsync(sha256HashPrefix, threatLists, cancellationToken);
                    var fullHashResponse = await findFullHashesTask.ConfigureAwait(false);

                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var safeCacheEntryExpirationDate = fullHashResponse.SafeThreatsExpirationDate;
                    var putSafeCacheEntryTask = cache.PutSafeCacheEntryAsync(sha256HashPrefix, safeCacheEntryExpirationDate, cancellationToken);
                    await putSafeCacheEntryTask.ConfigureAwait(false);

                    var unsafeThreats = fullHashResponse.UnsafeThreats;
                    if (unsafeThreats.Count != 0) {
                        var unsafeThreatGroups = unsafeThreats.GroupBy(ut => ut.Sha256Hash);
                        foreach (var unsafeThreatGroup in unsafeThreatGroups) {
                            // ...
                            //
                            // Throws an exception if the operation fails.
                            var unsafeThreatSha256Hash = unsafeThreatGroup.Key;
                            var putUnsafeCacheEntryTask = cache.PutUnsafeCacheEntryAsync(unsafeThreatSha256Hash, unsafeThreatGroup, cancellationToken);
                            await putUnsafeCacheEntryTask.ConfigureAwait(false);
                        }
                    }

                    // ...
                    //
                    // Lookup the URL in the cache again. Throws an exception if the operation fails.
                    cacheLookupTask = cache.LookupAsync(sha256Hash, sha256HashPrefix, cancellationToken);
                    cacheLookupResult = await cacheLookupTask.ConfigureAwait(false);
                    if (cacheLookupResult.IsCacheSafeHit) {
                        // ...
                        //
                        // If we get a cache safe hit, indicate the URL is safe.
                        urlLookupResult = UrlLookupResult.Safe(url, urlLookupDate);
                    }
                    else if (cacheLookupResult.IsCacheUnsafeHit) {
                        // ...
                        //
                        // If we get a cache unsafe hit, indicate the URL is unsafe. If we get a cache unsafe hit,
                        // it is guaranteed we find a URL expression for the threat, since it is essentially the
                        // URL expression that is cached.
                        var unsafeThreatListDescriptors = cacheLookupResult.UnsafeThreatListDescriptors;
                        url.TryGetExpressionForSha256Hash(sha256Hash, out var unsafeUrlExpression);
                        urlLookupResult = UrlLookupResult.Unsafe(url, urlLookupDate, unsafeUrlExpression, unsafeThreatListDescriptors);
                    }
                    else {
                        urlLookupResult = UrlLookupResult.DatabaseStale(url, urlLookupDate);
                    }
                }
            }

            return urlLookupResult;
        }
    }
}