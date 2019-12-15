using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Client Proxy.
    /// </summary>
    internal sealed class BrowsingClientProxy : IBrowsingClient {
        /// <summary>
        ///     Client.
        /// </summary>
        private readonly IBrowsingClient _client;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Create a Client Proxy.
        /// </summary>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" /> to proxy to. The client proxy takes ownership of
        ///     <paramref name="client" /> and will dispose it when the client proxy itself is disposed. If you
        ///     reference or dispose <paramref name="client" /> after you create the client proxy, the behavior of the
        ///     client proxy and <paramref name="client" /> is undefined.
        /// </param>
        /// <returns>
        ///     A client proxy.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="client" /> is a null reference.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BrowsingClientProxy Create(IBrowsingClient client) {
            // ...
            //
            // Throws an exception if the operation fails.
            return client is BrowsingClientProxy clientProxy ? clientProxy : new BrowsingClientProxy(client);
        }

        /// <summary>
        ///     Create a Client Proxy.
        /// </summary>
        /// <param name="client">
        ///     A <see cref="IBrowsingClient" /> to proxy to. The client proxy takes ownership of
        ///     <paramref name="client" /> and will dispose it when the client proxy itself is disposed. If you
        ///     reference or dispose <paramref name="client" /> after you create the client proxy, the behavior of the
        ///     client proxy and <paramref name="client" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="client" /> is a null reference.
        /// </exception>
        private BrowsingClientProxy(IBrowsingClient client) {
            Guard.ThrowIf(nameof(client), client).Null();

            this._client = client;
            this._disposed = false;
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
                this._client.Dispose();
                this._disposed = true;
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
        public async Task<FullHashResponse> FindFullHashesAsync(FullHashRequest request, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var findFullHashesTask = this._client.FindFullHashesAsync(request, cancellationToken);
                var response = await findFullHashesTask.ConfigureAwait(false);
                return response;
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingClientException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (TimeoutException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "An HTTP request to the Google Safe Browsing API failed.";
                throw new BrowsingClientException(detailMessage, HttpStatusCode.BadRequest, ex);
            }
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
        public async Task<IEnumerable<ThreatListDescriptor>> GetThreatListDescriptorsAsync(CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var getThreatListDescriptorsTask = this._client.GetThreatListDescriptorsAsync(cancellationToken);
                var threatListDescriptors = await getThreatListDescriptorsTask.ConfigureAwait(false);
                return threatListDescriptors;
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingClientException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (TimeoutException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "An HTTP request to the Google Safe Browsing API failed.";
                throw new BrowsingClientException(detailMessage, HttpStatusCode.BadRequest, ex);
            }
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
        public async Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(ThreatListUpdateRequest request, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var getThreatListUpdatesTask = this._client.GetThreatListUpdatesAsync(request, cancellationToken);
                var response = await getThreatListUpdatesTask.ConfigureAwait(false);
                return response;
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingClientException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (TimeoutException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "An HTTP request to the Google Safe Browsing API failed.";
                throw new BrowsingClientException(detailMessage, HttpStatusCode.BadRequest, ex);
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
                var detailMessage = $"An object ({nameof(BrowsingClientProxy)}) is disposed.";
                throw new ObjectDisposedException(nameof(BrowsingClientProxy), detailMessage);
            }
        }
    }
}