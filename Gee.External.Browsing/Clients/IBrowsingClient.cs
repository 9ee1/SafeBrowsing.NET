using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Abstract Client.
    /// </summary>
    public interface IBrowsingClient : IDisposable {
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
        Task<FullHashResponse> FindFullHashesAsync(FullHashRequest request, CancellationToken cancellationToken);

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
        Task<IEnumerable<ThreatListDescriptor>> GetThreatListDescriptorsAsync(CancellationToken cancellationToken);

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
        Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(ThreatListUpdateRequest request, CancellationToken cancellationToken);
    }
}