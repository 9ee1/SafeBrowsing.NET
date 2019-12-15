using Gee.Common.Guards;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Resilient Client.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Represents a resilient client that automatically retries failed operations performed on a
    ///         <see cref="IBrowsingClient" />. The number of retry attempts is either caller or
    ///         implementation specific if you do not specify a value. Between each retry attempt, the resilient client
    ///         will pause for an implementation specific interval. If all retry attempts are exhausted and the
    ///         attempted operation never succeeds, the exception the failed operation threw will be propagated up to
    ///         you.
    ///     </para>
    ///     <para>
    ///         The Google Safe Browsing API requires clients to implement a "back-off" mode when an attempted
    ///         operation fails. A failed operation, in this context, means an unsuccessful HTTP response for an HTTP
    ///         request. A resilient client does not implement this "back-off" mode because, quite frankly, it is quite
    ///         unrealistic to implement it in an API. The minimum and maximum intervals between retry attempts
    ///         required by the "back-off" mode are 15 minutes and 24 hours, respectively. It is not realistic to
    ///         declare a method in an API that, at best, returns after 15 minutes or, at worst, returns after 24 hours
    ///         if it retries a failed operation. As such, a resilient client only considers network time-outs as
    ///         failed operations and will only retry those. A network time-out means the Google Safe Browsing API is
    ///         taking too long to reply to an HTTP request and there isn't an HTTP response to fulfill it in a
    ///         predetermined period of time. As such, it should be exempt from the required "back-off" mode
    ///         implementation.
    ///     </para>
    ///     <para>
    ///         If you specify a number of retry attempts when you create a resilient client, be practical with the
    ///         value you specify. The resilient client will pause for an implementation specific interval between each
    ///         retry attempt, which effectively means if an attempted operation always fails, the exception it threw
    ///         will not be propagated up to you until all the retry attempts are exhausted. If you specify a very high
    ///         number of retry attempts, this could have self inflicted performance implications.
    ///     </para>
    ///     <para>
    ///         When you create a resilient client, you can specify whether or not it takes ownership of the client you
    ///         want to proxy to and dispose it when the resilient client itself is disposed. The recommended behavior
    ///         is that you allow the resilient client to take ownership of the client you want to proxy to but take
    ///         note that if you reference or dispose the client you want to proxy to after you create the resilient
    ///         client, the behavior of the resilient client and client you want to proxy to is undefined.
    ///     </para>
    ///     <para>
    ///         Since a resilient client itself implements <see cref="IBrowsingClient" />, it is technically possible
    ///         to create a new resilient client for an existing resilient client, though the reasons for doing so, in
    ///         most cases, are unjustified. To avoid doing do, consider creating a resilient client using
    ///         <see cref="Create(IBrowsingClient)" />, or one of its overloads, instead of one of the constructor
    ///         overloads. <see cref="Create(IBrowsingClient)" /> will conveniently create a resilient client if, and
    ///         only if, the client you want to proxy to itself is not a resilient client.
    ///     </para>
    /// </remarks>
    public sealed class ResilientBrowsingClient : IBrowsingClient {
        /// <summary>
        ///     Client.
        /// </summary>
        private readonly IBrowsingClient _client;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Own Client Flag.
        /// </summary>
        private readonly bool _ownClient;

        /// <summary>
        ///     Resiliency Policy.
        /// </summary>
        private readonly IAsyncPolicy _resiliencyPolicy;

        /// <summary>
        ///     Retry Attempts.
        /// </summary>
        private readonly int _retryAttempts;

        /// <summary>
        ///     Create a Resilient Client.
        /// </summary>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" /> to proxy to. The resilient client takes ownership of
        ///     <paramref name="client" /> and will dispose it when the resilient cache itself is disposed. If you
        ///     reference or dispose <paramref name="client" /> after you create the resilient cache, the behavior of
        ///     the resilient cache and <paramref name="client" /> is undefined.
        /// </param>
        /// <returns>
        ///     A resilient client.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="client" /> is a null reference.
        /// </exception>
        public static ResilientBrowsingClient Create(IBrowsingClient client) => ResilientBrowsingClient.Create(client, 5);

        /// <summary>
        ///     Create a Resilient Client.
        /// </summary>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" /> to proxy to. The resilient client takes ownership of
        ///     <paramref name="client" /> and will dispose it when the resilient cache itself is disposed. If you
        ///     reference or dispose <paramref name="client" /> after you create the resilient cache, the behavior of
        ///     the resilient cache and <paramref name="client" /> is undefined.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="client" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public static ResilientBrowsingClient Create(IBrowsingClient client, int retryAttempts) => ResilientBrowsingClient.Create(client, retryAttempts, true);

        /// <summary>
        ///     Create a Resilient Client.
        /// </summary>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" /> to proxy to.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <param name="ownClient">
        ///     A boolean flag indicating whether or not the resilient client takes ownership of
        ///     <paramref name="client" /> and disposes it when the resilient client itself is disposed. If the
        ///     resilient client takes ownership of <paramref name="client" /> and you reference or dispose
        ///     <paramref name="client" /> after you create the resilient client, the behavior of the resilient client
        ///     and <paramref name="client" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="client" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public static ResilientBrowsingClient Create(IBrowsingClient client, int retryAttempts, bool ownClient) {
            // ...
            //
            // Throws an exception if the operation fails.
            return client is ResilientBrowsingClient resilientClient ? resilientClient : new ResilientBrowsingClient(client, retryAttempts, ownClient);
        }

        /// <summary>
        ///     Create a Resilient Client.
        /// </summary>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" /> to proxy to. The resilient client takes ownership of
        ///     <paramref name="client" /> and will dispose it when the resilient cache itself is disposed. If you
        ///     reference or dispose <paramref name="client" /> after you create the resilient cache, the behavior of
        ///     the resilient cache and <paramref name="client" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="client" /> is a null reference.
        /// </exception>
        public ResilientBrowsingClient(IBrowsingClient client) : this(client, 5) { }

        /// <summary>
        ///     Create a Resilient Client.
        /// </summary>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" /> to proxy to. The resilient client takes ownership of
        ///     <paramref name="client" /> and will dispose it when the resilient cache itself is disposed. If you
        ///     reference or dispose <paramref name="client" /> after you create the resilient cache, the behavior of
        ///     the resilient cache and <paramref name="client" /> is undefined.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="client" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public ResilientBrowsingClient(IBrowsingClient client, int retryAttempts) : this(client, retryAttempts, true) { }

        /// <summary>
        ///     Create a Resilient Client.
        /// </summary>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" /> to proxy to.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <param name="ownClient">
        ///     A boolean flag indicating whether or not the resilient client takes ownership of
        ///     <paramref name="client" /> and disposes it when the resilient client itself is disposed. If the
        ///     resilient client takes ownership of <paramref name="client" /> and you reference or dispose
        ///     <paramref name="client" /> after you create the resilient client, the behavior of the resilient client
        ///     and <paramref name="client" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="client" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public ResilientBrowsingClient(IBrowsingClient client, int retryAttempts, bool ownClient) {
            Guard.ThrowIf(nameof(retryAttempts), retryAttempts).LessThanOrEqualTo(0);

            this._client = BrowsingClientProxy.Create(client);
            this._disposed = false;
            this._ownClient = ownClient;
            this._retryAttempts = retryAttempts;
            // ...
            //
            // ...
            this._resiliencyPolicy = CreateResiliencyPolicy(this);

            // <summary>
            //      Create Resiliency Policy.
            // </summary>
            IAsyncPolicy CreateResiliencyPolicy(ResilientBrowsingClient @this) {
                var cPolicyBuilder = Policy.Handle<TimeoutException>();
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
                if (this._ownClient) {
                    this._client.Dispose();
                }
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
        ///     Find Full Hashes Asynchronously.
        /// </summary>
        /// <param name="request">
        ///     A <see cref="FullHashRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A <see cref="FullHashResponse" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="request" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<FullHashResponse> FindFullHashesAsync(FullHashRequest request, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<FullHashResponse>> resiliencyPolicyAction = () => this._client.FindFullHashesAsync(request, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Get Threat List Descriptors Asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A collection of <see cref="ThreatListDescriptor" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<IEnumerable<ThreatListDescriptor>> GetThreatListDescriptorsAsync(CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<IEnumerable<ThreatListDescriptor>>> resiliencyPolicyAction = () => this._client.GetThreatListDescriptorsAsync(cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Get Threat List Updates Asynchronously.
        /// </summary>
        /// <param name="request">
        ///     A <see cref="ThreatListUpdateRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResponse" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="request" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(ThreatListUpdateRequest request, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<ThreatListUpdateResponse>> resiliencyPolicyAction = () => this._client.GetThreatListUpdatesAsync(request, cancellationToken);
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
                var detailMessage = $"An object ({nameof(ResilientBrowsingClient)}) is disposed.";
                throw new ObjectDisposedException(nameof(ResilientBrowsingClient), detailMessage);
            }
        }
    }
}