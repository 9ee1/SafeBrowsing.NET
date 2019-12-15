using Gee.Common.Guards;
using Polly;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Resilient Cache.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Represents a resilient cache that automatically retries failed operations performed on a
    ///         <see cref="IBrowsingCache" />. The number of retry attempts is either caller or implementation specific
    ///         if you do not specify a value. Between each retry attempt, the resilient cache will pause for an
    ///         implementation specific interval. If all retry attempts are exhausted and the attempted operation never
    ///         succeeds, the exception the failed operation threw will be propagated up to you.
    ///     </para>
    ///     <para>
    ///         If you specify a number of retry attempts when you create a resilient cache, be practical with the
    ///         value you specify. The resilient cache will pause for an implementation specific interval between each
    ///         retry attempt, which effectively means if an attempted operation always fails, the exception it threw
    ///         will not be propagated up to you until all the retry attempts are exhausted. If you specify a very high
    ///         number of retry attempts, this could have self inflicted performance implications.
    ///     </para>
    ///     <para>
    ///         When you create a resilient cache, you can specify whether or not it takes ownership of the cache you
    ///         want to proxy to and dispose it when the resilient cache itself is disposed. The recommended behavior
    ///         is that you allow the resilient cache to take ownership of the cache you want to proxy to but take note
    ///         that if you reference or dispose the cache you want to proxy to after you create the resilient cache,
    ///         the behavior of the resilient cache and cache you want to proxy to is undefined.
    ///     </para>
    ///     <para>
    ///         Since a resilient cache itself implements <see cref="IBrowsingCache" />, it is technically possible to
    ///         create a new resilient cache for an existing resilient cache, though the reasons for doing so, in most
    ///         cases, are unjustified. To avoid doing do, consider creating a resilient cache using
    ///         <see cref="Create(IBrowsingCache)" />, or one of its overloads, instead of one of the constructor
    ///         overloads. <see cref="Create(IBrowsingCache)" /> will conveniently create a resilient cache if, and
    ///         only if, the cache you want to proxy to itself is not a resilient cache.
    ///     </para>
    /// </remarks>
    public sealed class ResilientBrowsingCache : IBrowsingCache {
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
        ///     Resiliency Policy.
        /// </summary>
        private readonly IAsyncPolicy _resiliencyPolicy;

        /// <summary>
        ///     Retry Attempts.
        /// </summary>
        private readonly int _retryAttempts;

        /// <summary>
        ///     Create a Resilient Cache.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to. The resilient cache takes ownership of
        ///     <paramref name="cache" /> and will dispose it when the resilient cache itself is disposed. If you
        ///     reference or dispose <paramref name="cache" /> after you create the resilient cache, the behavior of
        ///     the resilient cache and <paramref name="cache" /> is undefined.
        /// </param>
        /// <returns>
        ///     A resilient cache.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        public static ResilientBrowsingCache Create(IBrowsingCache cache) => ResilientBrowsingCache.Create(cache, 5);

        /// <summary>
        ///     Create a Resilient Cache.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to. The resilient cache takes ownership of
        ///     <paramref name="cache" /> and will dispose it when the resilient cache itself is disposed. If you
        ///     reference or dispose <paramref name="cache" /> after you create the resilient cache, the behavior of
        ///     the resilient cache and <paramref name="cache" /> is undefined.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <returns>
        ///     A resilient cache.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public static ResilientBrowsingCache Create(IBrowsingCache cache, int retryAttempts) => ResilientBrowsingCache.Create(cache, retryAttempts, true);

        /// <summary>
        ///     Create a Resilient Cache.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <param name="ownCache">
        ///     A boolean flag indicating whether or not the resilient cache takes ownership of
        ///     <paramref name="cache" /> and disposes it when the resilient cache itself is disposed. If the resilient
        ///     cache takes ownership of <paramref name="cache" /> and you reference or dispose
        ///     <paramref name="cache" /> after you create the resilient cache, the behavior of the resilient cache and
        ///     <paramref name="cache" /> is undefined.
        /// </param>
        /// <returns>
        ///     A resilient cache.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public static ResilientBrowsingCache Create(IBrowsingCache cache, int retryAttempts, bool ownCache) {
            // ...
            //
            // Throws an exception if the operation fails.
            return cache is ResilientBrowsingCache resilientCache ? resilientCache : new ResilientBrowsingCache(cache, retryAttempts, ownCache);
        }

        /// <summary>
        ///     Create a Resilient Cache.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to. The resilient cache takes ownership of
        ///     <paramref name="cache" /> and will dispose it when the resilient cache itself is disposed. If you
        ///     reference or dispose <paramref name="cache" /> after you create the resilient cache, the behavior of
        ///     the resilient cache and <paramref name="cache" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        public ResilientBrowsingCache(IBrowsingCache cache) : this(cache, 5) { }

        /// <summary>
        ///     Create a Resilient Cache.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to. The resilient cache takes ownership of
        ///     <paramref name="cache" /> and will dispose it when the resilient cache itself is disposed. If you
        ///     reference or dispose <paramref name="cache" /> after you create the resilient cache, the behavior of
        ///     the resilient cache and <paramref name="cache" /> is undefined.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public ResilientBrowsingCache(IBrowsingCache cache, int retryAttempts) : this(cache, retryAttempts, true) { }

        /// <summary>
        ///     Create a Resilient Cache.
        /// </summary>
        /// <param name="cache">
        ///     A <see cref="IBrowsingCache" /> to proxy to.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <param name="ownCache">
        ///     A boolean flag indicating whether or not the resilient cache takes ownership of
        ///     <paramref name="cache" /> and disposes it when the resilient cache itself is disposed. If the resilient
        ///     cache takes ownership of <paramref name="cache" /> and you reference or dispose
        ///     <paramref name="cache" /> after you create the resilient cache, the behavior of the resilient cache and
        ///     <paramref name="cache" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="cache" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public ResilientBrowsingCache(IBrowsingCache cache, int retryAttempts, bool ownCache) {
            Guard.ThrowIf(nameof(retryAttempts), retryAttempts).LessThanOrEqualTo(0);

            this._cache = BrowsingCacheProxy.Create(cache, ownCache);
            this._disposed = false;
            this._ownCache = ownCache;
            this._retryAttempts = retryAttempts;
            // ...
            //
            // ...
            this._resiliencyPolicy = CreateResiliencyPolicy(this);

            // <summary>
            //      Create Resiliency Policy.
            // </summary>
            IAsyncPolicy CreateResiliencyPolicy(ResilientBrowsingCache @this) {
                var cPolicyBuilder = Policy.Handle<BrowsingCacheException>();
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
        public void Dispose() {
            if (!this._disposed) {
                this._disposed = true;
                if (this._ownCache) {
                    this._cache.Dispose();
                }
            }
        }

        /// <summary>
        ///     Execute a Resiliency Policy Action.
        /// </summary>
        /// <param name="resiliencyPolicyAction">
        ///     An action for the resilience policy to execute.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        private async Task ExecuteResiliencyPolicyAsync(Func<Task> resiliencyPolicyAction) {
            try {
                var executeTask = this._resiliencyPolicy.ExecuteAsync(resiliencyPolicyAction);
                await executeTask.ConfigureAwait(false);
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Execute a Resiliency Policy Action.
        /// </summary>
        /// <typeparam name="T">
        ///     The return type of <paramref name="resiliencyPolicyAction" />.
        /// </typeparam>
        /// <param name="resiliencyPolicyAction">
        ///     An action for the resilience policy to execute.
        /// </param>
        /// <returns>
        ///     The return value of <paramref name="resiliencyPolicyAction" />.
        /// </returns>
        private async Task<T> ExecuteResiliencyPolicyAsync<T>(Func<Task<T>> resiliencyPolicyAction) {
            try {
                var executeTask = this._resiliencyPolicy.ExecuteAsync(resiliencyPolicyAction);
                var executeResult = await executeTask.ConfigureAwait(false);
                return executeResult;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
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
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<SafeCacheEntry> GetSafeCacheEntryAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<SafeCacheEntry>> resiliencyPolicyAction = () => this._cache.GetSafeCacheEntryAsync(threatSha256HashPrefix, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
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
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<UnsafeCacheEntry> GetUnsafeCacheEntryAsync(string threatSha256Hash, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<UnsafeCacheEntry>> resiliencyPolicyAction = () => this._cache.GetUnsafeCacheEntryAsync(threatSha256Hash, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
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
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task PutSafeCacheEntryAsync(SafeCacheEntry safeCacheEntry, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task> resiliencyPolicyAction = () => this._cache.PutSafeCacheEntryAsync(safeCacheEntry, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
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
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task PutUnsafeCacheEntryAsync(UnsafeCacheEntry unsafeCacheEntry, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task> resiliencyPolicyAction = () => this._cache.PutUnsafeCacheEntryAsync(unsafeCacheEntry, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
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
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task RemoveSafeCacheEntryAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task> resiliencyPolicyAction = () => this._cache.RemoveSafeCacheEntryAsync(threatSha256HashPrefix, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
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
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task RemoveUnsafeCacheEntryAsync(string threatSha256Hash, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task> resiliencyPolicyAction = () => this._cache.RemoveUnsafeCacheEntryAsync(threatSha256Hash, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
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
                var detailMessage = $"An object ({nameof(ResilientBrowsingCache)}) is disposed.";
                throw new ObjectDisposedException(nameof(ResilientBrowsingCache), detailMessage);
            }
        }
    }
}