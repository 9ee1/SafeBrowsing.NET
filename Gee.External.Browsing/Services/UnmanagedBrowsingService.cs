using Gee.External.Browsing.Cache;
using Gee.External.Browsing.Clients;
using Gee.External.Browsing.Databases;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Unmanaged Service.
    /// </summary>
    public sealed class UnmanagedBrowsingService : BaseBrowsingService {
        /// <summary>
        ///     Cache.
        /// </summary>
        private readonly ResilientBrowsingCache _cache;

        /// <summary>
        ///     Client.
        /// </summary>
        private readonly ResilientBrowsingClient _client;

        /// <summary>
        ///     Database.
        /// </summary>
        private readonly ResilientUnmanagedBrowsingDatabase _database;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Build an Unmanaged Service.
        /// </summary>
        /// <returns>
        ///     An <see cref="UnmanagedBrowsingServiceBuilder" /> to build an unmanaged service with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnmanagedBrowsingServiceBuilder Build() => new UnmanagedBrowsingServiceBuilder();

        /// <summary>
        ///     Create an Unmanaged Service.
        /// </summary>
        /// <param name="builder">
        ///     An <see cref="UnmanagedBrowsingServiceBuilder" /> to initialize the unmanaged service with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal UnmanagedBrowsingService(UnmanagedBrowsingServiceBuilder builder) {
            this._cache = ResilientBrowsingCache.Create(builder.Cache, 5, builder.OwnCache);
            this._client = ResilientBrowsingClient.Create(builder.Client, 5, builder.OwnClient);
            this._database = ResilientUnmanagedBrowsingDatabase.Create(builder.Database, 5, builder.OwnDatabase);
            this._disposed = false;
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public override void Dispose() {
            if (!this._disposed) {
                // ...
                //
                // When the resilient cache, the resilient client, and the resilient database are disposed, whether or
                // not they dispose the cache, the client, and the database they proxy to is dependent on whether or
                // not they took ownership of them when the were created.
                this._disposed = true;
                this._cache.Dispose();
                this._client.Dispose();
                this._database.Dispose();
            }
        }

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
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public override Task<UrlLookupResult> LookupAsync(Url url, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            // ...
            //
            // Throws an exception if the operation fails.
            var lookupTask = base.LookupAsync(this._cache, this._client, this._database, url, cancellationToken);
            return lookupTask;
        }

        /// <summary>
        ///     Throw an Exception if Object is Disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        private void ThrowIfDisposed() {
            if (this._disposed) {
                var detailMessage = $"An object ({nameof(UnmanagedBrowsingService)}) is disposed.";
                throw new ObjectDisposedException(nameof(UnmanagedBrowsingService), detailMessage);
            }
        }
    }
}