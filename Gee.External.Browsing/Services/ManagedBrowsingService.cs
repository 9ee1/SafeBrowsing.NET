using Gee.Common.Guards;
using Gee.External.Browsing.Cache;
using Gee.External.Browsing.Clients;
using Gee.External.Browsing.Databases;
using Polly;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Managed Service.
    /// </summary>
    public sealed class ManagedBrowsingService : BaseBrowsingService {
        /// <summary>
        ///     Threat List Synchronization Completed Event.
        /// </summary>
        public event Action<ThreatListSynchronizationCompletedEventArgs> ThreatListSynchronizationCompleted {
            add => this._databaseManager.ThreatListSynchronizationCompleted += value;
            remove => this._databaseManager.ThreatListSynchronizationCompleted -= value;
        }

        /// <summary>
        ///     Threat List Synchronization Failed Event.
        /// </summary>
        public event Action<ThreatListSynchronizationFailedEventArgs> ThreatListSynchronizationFailed {
            add => this._databaseManager.ThreatListSynchronizationFailed += value;
            remove => this._databaseManager.ThreatListSynchronizationFailed -= value;
        }

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
        private readonly ResilientManagedBrowsingDatabase _database;

        /// <summary>
        ///     Database Manager.
        /// </summary>
        private readonly BrowsingDatabaseManager _databaseManager;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Resiliency Policy.
        /// </summary>
        private readonly IAsyncPolicy<UrlLookupResult> _resiliencyPolicy;

        /// <summary>
        ///     Retry Attempts.
        /// </summary>
        private readonly int _retryAttempts;

        /// <summary>
        ///     Build a Managed Service.
        /// </summary>
        /// <returns>
        ///     A <see cref="ManagedBrowsingServiceBuilder" /> to build a managed service with.
        /// </returns>
        public static ManagedBrowsingServiceBuilder Build() => new ManagedBrowsingServiceBuilder();

        /// <summary>
        ///     Create a Managed Service.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="ManagedBrowsingServiceBuilder" /> to initialize the managed service with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal ManagedBrowsingService(ManagedBrowsingServiceBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();

            this._disposed = false;
            this._retryAttempts = 10;
            // ...
            //
            // ...
            this._cache = ResilientBrowsingCache.Create(builder.Cache, this._retryAttempts, builder.OwnCache);
            this._client = ResilientBrowsingClient.Create(builder.Client, this._retryAttempts, builder.OwnClient);
            this._database = ResilientManagedBrowsingDatabase.Create(builder.Database, this._retryAttempts, builder.OwnDatabase);
            // ...
            //
            // ...
            this._databaseManager = CreateDatabaseManager(this, builder);
            this._resiliencyPolicy = CreateResiliencyPolicy(this);

            // <summary>
            //      Create Database Manager.
            // </summary>
            BrowsingDatabaseManager CreateDatabaseManager(ManagedBrowsingService @this, ManagedBrowsingServiceBuilder cBuilder) {
                // ...
                //
                // Throws an exception if the operation fails.
                return BrowsingDatabaseManager.Build()
                    .SetClient(@this._client, true)
                    .SetDatabase(@this._database, true)
                    .SetUpdateConstraints(cBuilder.UpdateConstraints)
                    .Build();
            }

            // <summary>
            //      Create Resiliency Policy.
            // </summary>
            IAsyncPolicy<UrlLookupResult> CreateResiliencyPolicy(ManagedBrowsingService @this) {
                var cPolicyBuilder = Policy<UrlLookupResult>.HandleResult(ulr => ulr.IsDatabaseStale);
                var cPolicy = cPolicyBuilder.WaitAndRetryAsync(@this._retryAttempts, CreateRetryPolicyTimeout);
                return cPolicy;
            }

            // <summary>
            //      Create Retry Policy Timeout.
            // </summary>
            TimeSpan CreateRetryPolicyTimeout(int cRetryAttempt) {
                var cDelaySeconds = Math.Pow(2, cRetryAttempt);
                var cDelay = TimeSpan.FromSeconds(cDelaySeconds);
                return cDelay;
            }
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public override void Dispose() {
            if (!this._disposed) {
                // ...
                //
                // When the resilient cache is disposed, whether or not it disposes the cache it proxies to is
                // dependent on whether or not it took ownership of it when it was created.
                this._disposed = true;
                this._cache.Dispose();

                // ...
                //
                // When the database manager is disposed, it will dispose the resilient client and the resilient
                // database because it took ownership of them when it was created. However, whether or not they dispose
                // the client and the database they proxy to is dependent on whether or not they took ownership of them
                // when the were created.
                this._databaseManager.Dispose();
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
            // We're doing something interesting here. If we get a URL lookup result indicating a stale database, we
            // invoke a retry policy until the database is no longer stale. We can only do this because we are managing
            // the database so we know it will immediately synchronize again once it become stale. We put a limit on
            // the number of retries in case the synchronization fails.
            Func<Task<UrlLookupResult>> resiliencyPolicyAction = () => base.LookupAsync(this._cache, this._client, this._database, url, cancellationToken);
            var executeResiliencyPolicyTask = this._resiliencyPolicy.ExecuteAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Throw an Exception if Object is Disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        private void ThrowIfDisposed() {
            if (this._disposed) {
                var detailMessage = $"An object ({nameof(ManagedBrowsingService)}) is disposed.";
                throw new ObjectDisposedException(nameof(ManagedBrowsingService), detailMessage);
            }
        }
    }
}