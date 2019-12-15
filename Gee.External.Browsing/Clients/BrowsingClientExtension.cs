using Gee.Common.Guards;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Client Extension.
    /// </summary>
    public static class BrowsingClientExtension {
        /// <summary>
        ///     Find Full Hashes Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="request">
        ///     A <see cref="FullHashRequest" />.
        /// </param>
        /// <returns>
        ///     A <see cref="FullHashResponse" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="request" /> is a null
        ///     reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        public static Task<FullHashResponse> FindFullHashesAsync(this IBrowsingClient @this, FullHashRequest request) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var findFullHashesTask = @this.FindFullHashesAsync(request, CancellationToken.None);
            return findFullHashesTask;
        }

        /// <summary>
        ///     Find Full Hashes Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="sha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to query.
        /// </param>
        /// <param name="threatLists">
        ///     A collection of <see cref="ThreatList" /> the threat identified by <paramref name="sha256HashPrefix" />
        ///     is associated with.
        /// </param>
        /// <returns>
        ///     A <see cref="FullHashResponse" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="sha256HashPrefix" /> is a
        ///     null reference, or if <paramref name="threatLists" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        public static Task<FullHashResponse> FindFullHashesAsync(this IBrowsingClient @this, string sha256HashPrefix, IEnumerable<ThreatList> threatLists) => @this.FindFullHashesAsync(sha256HashPrefix, threatLists, CancellationToken.None);

        /// <summary>
        ///     Find Full Hashes Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="sha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat to query.
        /// </param>
        /// <param name="threatLists">
        ///     A collection of <see cref="ThreatList" /> the threat identified by <paramref name="sha256HashPrefix" />
        ///     is associated with.
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
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="sha256HashPrefix" /> is a
        ///     null reference, or if <paramref name="threatLists" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static Task<FullHashResponse> FindFullHashesAsync(this IBrowsingClient @this, string sha256HashPrefix, IEnumerable<ThreatList> threatLists, CancellationToken cancellationToken) {
            Guard.ThrowIf(nameof(@this), @this).Null();
            Guard.ThrowIf(nameof(threatLists), threatLists).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var fullHashRequestBuilder = FullHashRequest.Build();
            fullHashRequestBuilder.AddSha256HashPrefix(sha256HashPrefix);
            foreach (var threatList in threatLists) {
                fullHashRequestBuilder.AddQuery(threatList.Descriptor, threatList.State);
            }

            // ...
            //
            // Throws an exception if the operation fails.
            var fullHashRequest = fullHashRequestBuilder.Build();
            var findFullHashesTask = @this.FindFullHashesAsync(fullHashRequest, cancellationToken);
            return findFullHashesTask;
        }

        /// <summary>
        ///     Get Threat List Descriptors Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <returns>
        ///     A collection of <see cref="ThreatListDescriptor" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        public static Task<IEnumerable<ThreatListDescriptor>> GetThreatListDescriptorsAsync(this IBrowsingClient @this) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatListDescriptorsTask = @this.GetThreatListDescriptorsAsync(CancellationToken.None);
            return getThreatListDescriptorsTask;
        }

        /// <summary>
        ///     Get Threat List Updates Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="request">
        ///     A <see cref="ThreatListUpdateRequest" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResponse" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="request" /> is a null
        ///     reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        public static Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(this IBrowsingClient @this, ThreatListUpdateRequest request) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatListUpdatesTask = @this.GetThreatListUpdatesAsync(request, CancellationToken.None);
            return getThreatListUpdatesTask;
        }

        /// <summary>
        ///     Get Threat List Updates Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="threatLists">
        ///     A collection of <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResponse" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatLists" /> is a
        ///     null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        public static Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(this IBrowsingClient @this, IEnumerable<ThreatList> threatLists) => @this.GetThreatListUpdatesAsync(threatLists, CancellationToken.None);

        /// <summary>
        ///     Get Threat List Updates Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="threatLists">
        ///     A collection of <see cref="ThreatList" /> to retrieve.
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
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatLists" /> is a
        ///     null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(this IBrowsingClient @this, IEnumerable<ThreatList> threatLists, CancellationToken cancellationToken) {
            Guard.ThrowIf(nameof(@this), @this).Null();
            Guard.ThrowIf(nameof(threatLists), threatLists).Null();

            var threatListUpdateRequestBuilder = ThreatListUpdateRequest.Build();
            foreach (var threatList in threatLists) {
                threatListUpdateRequestBuilder.AddQuery(b => {
                    b.SetThreatListDescriptor(threatList.Descriptor);
                    b.SetThreatListState(threatList.State);
                    return b.Build();
                });
            }

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateRequest = threatListUpdateRequestBuilder.Build();
            var getThreatListUpdatesTask = @this.GetThreatListUpdatesAsync(threatListUpdateRequest, cancellationToken);
            return getThreatListUpdatesTask;
        }

        /// <summary>
        ///     Get Threat List Updates Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="threatList">
        ///     A <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="updateConstraints">
        ///     The <see cref="ThreatListUpdateConstraints" /> to apply when <paramref name="threatList" /> is
        ///     retrieved. A null reference indicates no <see cref="ThreatListUpdateConstraints" /> should be applied.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResponse" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Clients.BrowsingClientException">
        ///     Thrown if an error communicating with the Google Safe Browsing API occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatList" /> is a
        ///     null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        public static Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(this IBrowsingClient @this, ThreatList threatList, ThreatListUpdateConstraints updateConstraints) => @this.GetThreatListUpdatesAsync(threatList, updateConstraints, CancellationToken.None);

        /// <summary>
        ///     Get Threat List Updates Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="threatList">
        ///     A <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="updateConstraints">
        ///     The <see cref="ThreatListUpdateConstraints" /> to apply when <paramref name="threatList" /> is
        ///     retrieved. A null reference indicates no <see cref="ThreatListUpdateConstraints" /> should be applied.
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
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatList" /> is a
        ///     null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        /// <exception cref="System.TimeoutException">
        ///     Thrown if communication with the Google Safe Browsing API times out.
        /// </exception>
        public static Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(this IBrowsingClient @this, ThreatList threatList, ThreatListUpdateConstraints updateConstraints, CancellationToken cancellationToken) {
            Guard.ThrowIf(nameof(@this), @this).Null();
            Guard.ThrowIf(nameof(threatList), threatList).Null();

            var threatListUpdateRequestBuilder = ThreatListUpdateRequest.Build();
            threatListUpdateRequestBuilder.AddQuery(b => {
                b.SetThreatListDescriptor(threatList.Descriptor);
                b.SetThreatListState(threatList.State);
                b.SetUpdateConstraints(updateConstraints);
                return b.Build();
            });

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateRequest = threatListUpdateRequestBuilder.Build();
            var getThreatListUpdatesTask = @this.GetThreatListUpdatesAsync(threatListUpdateRequest, cancellationToken);
            return getThreatListUpdatesTask;
        }
    }
}