using Gee.Common.Guards;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Browsing Service Extension.
    /// </summary>
    public static class BrowsingServiceExtension {
        /// <summary>
        ///     Lookup a URL.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingService" />.
        /// </param>
        /// <param name="url">
        ///     A <see cref="Url" /> to lookup.
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
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<UrlLookupResult> LookupAsync(this IBrowsingService @this, string url) => @this.LookupAsync(url, CancellationToken.None);

        /// <summary>
        ///     Lookup a URL.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingService" />.
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
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public static Task<UrlLookupResult> LookupAsync(this IBrowsingService @this, string url, CancellationToken cancellationToken) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            var canonicalizedUrl = new Url(url);
            var lookupTask = @this.LookupAsync(canonicalizedUrl, cancellationToken);
            return lookupTask;
        }

        /// <summary>
        ///     Lookup a URL.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingService" />.
        /// </param>
        /// <param name="url">
        ///     A <see cref="Url" /> to lookup.
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
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<UrlLookupResult> LookupAsync(this IBrowsingService @this, Url url) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            var lookupTask = @this.LookupAsync(url, CancellationToken.None);
            return lookupTask;
        }
    }
}