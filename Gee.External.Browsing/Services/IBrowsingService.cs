using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Abstract Service.
    /// </summary>
    public interface IBrowsingService : IDisposable {
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
        Task<UrlLookupResult> LookupAsync(Url url, CancellationToken cancellationToken);
    }
}