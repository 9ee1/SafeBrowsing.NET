using Flurl.Http;
using Flurl.Http.Configuration;
using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     HTTP Client.
    /// </summary>
    public sealed class HttpBrowsingClient : IBrowsingClient {
        /// <summary>
        ///     API Key Query Parameter Name.
        /// </summary>
        private const string ApiKeyQueryParameterName = "key";

        /// <summary>
        ///     Base URI.
        /// </summary>
        private const string BaseUri = "https://safebrowsing.googleapis.com/v4";

        /// <summary>
        ///     Find Full Hashes URI.
        /// </summary>
        private static readonly string FindFullHashesUri = $"{HttpBrowsingClient.BaseUri}/fullHashes:find";

        /// <summary>
        ///     Get ThreatList Descriptors URI.
        /// </summary>
        private static readonly string GetThreatListDescriptorsUri = $"{HttpBrowsingClient.BaseUri}/threatLists";

        /// <summary>
        ///     Get Threat List Updates URI.
        /// </summary>
        private static readonly string GetThreatListUpdatesUri = $"{HttpBrowsingClient.BaseUri}/threatListUpdates:fetch";

        /// <summary>
        ///     API Key.
        /// </summary>
        [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
        private readonly string _apiKey;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Create a Browsing Client.
        /// </summary>
        /// <param name="apiKey">
        ///     A Google Safe Browsing API key to authenticate to the Google Safe Browsing API with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="apiKey" /> is a null reference.
        /// </exception>
        public HttpBrowsingClient(string apiKey) {
            Guard.ThrowIf(nameof(apiKey), apiKey).Null();

            this._apiKey = apiKey;
            this._disposed = false;
            // ...
            //
            // ...
            FlurlHttp.Configure(s => {
                s.CookiesEnabled = false;
                s.BeforeCall = OnFlurlClientBeforeCall;
                s.HttpClientFactory = new HttpClientFactory();
                s.Timeout = TimeSpan.FromMinutes(1);
            });

            // <summary>
            //      On Flurl Client Before Call.
            // </summary>
            void OnFlurlClientBeforeCall(HttpCall httpCall) {
                // ...
                //
                // Add the Google Safe Browsing API Key to every HTTP request so that we don't have explicitly add it
                // when creating every HTTP request.
                var requestQueryParameters = httpCall.FlurlRequest.Url.QueryParams;
                requestQueryParameters.Add(HttpBrowsingClient.ApiKeyQueryParameterName, this._apiKey, true);
            }
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
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
        public Task<FullHashResponse> FindFullHashesAsync(FullHashRequest request, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            Guard.ThrowIf(nameof(request), request).Null();

            var findFullHashesTask = FindFullHashesAsync(request, cancellationToken);
            return findFullHashesTask;

            // <summary>
            //      Find Full Hashes Asynchronously.
            // </summary>
            async Task<FullHashResponse> FindFullHashesAsync(FullHashRequest cRequest, CancellationToken cCancellationToken) {
                try {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var cRequestModel = cRequest.AsFullHashRequestModel();
                    var cResponseModel = await HttpBrowsingClient.FindFullHashesUri
                                             .PostJsonAsync(cRequestModel, cCancellationToken)
                                             .ReceiveJson<FullHashResponseModel>()
                                             .ConfigureAwait(false);

                    var cResponse = cResponseModel.AsFullHashResponse(cRequest);
                    return cResponse;
                }
                catch (FlurlHttpTimeoutException cEx) {
                    // ...
                    //
                    // An HTTP 504 is not the most appropriate HTTP response code to use if there is a client timeout
                    // but its the closest one that makes sense, so we will go ahead and use it.
                    const string cDetailMessage = "An HTTP request to the Google Safe Browsing API timed out.";
                    throw new TimeoutException(cDetailMessage, cEx);
                }
                catch (FlurlHttpException cEx) {
                    var httpStatusCode = cEx.Call.HttpStatus ?? HttpStatusCode.BadRequest;
                    const string cDetailMessage = "An HTTP request to the Google Safe Browsing API failed.";
                    throw new BrowsingClientException(cDetailMessage, httpStatusCode, cEx);
                }
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
        public Task<IEnumerable<ThreatListDescriptor>> GetThreatListDescriptorsAsync(CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            var getThreatListDescriptorsTask = GetThreatListDescriptorsAsync(cancellationToken);
            return getThreatListDescriptorsTask;

            // <summary>
            //      Get Threat List Descriptors Asynchronously.
            // </summary>
            async Task<IEnumerable<ThreatListDescriptor>> GetThreatListDescriptorsAsync(CancellationToken cCancellationToken) {
                try {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var cResponseModel = await HttpBrowsingClient.GetThreatListDescriptorsUri
                                             .GetJsonAsync<ThreatListDescriptorResponseModel>(cCancellationToken)
                                             .ConfigureAwait(false);

                    var cResponse = cResponseModel.ThreatListDescriptors.Select(m => m.AsThreatListDescriptor());
                    return cResponse;
                }
                catch (FlurlHttpTimeoutException cEx) {
                    const string cDetailMessage = "An HTTP request to the Google Safe Browsing API timed out.";
                    throw new TimeoutException(cDetailMessage, cEx);
                }
                catch (FlurlHttpException ex) {
                    var httpStatusCode = ex.Call.HttpStatus ?? HttpStatusCode.BadRequest;
                    const string cDetailMessage = "An HTTP request to the Google Safe Browsing API failed.";
                    throw new BrowsingClientException(cDetailMessage, httpStatusCode, ex);
                }
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
        public Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(ThreatListUpdateRequest request, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            Guard.ThrowIf(nameof(request), request).Null();

            var getThreatListUpdatesTask = GetThreatListUpdatesAsync(request, cancellationToken);
            return getThreatListUpdatesTask;

            // <summary>
            //      Get Threat List Updates Asynchronously.
            // </summary>
            async Task<ThreatListUpdateResponse> GetThreatListUpdatesAsync(ThreatListUpdateRequest cRequest, CancellationToken cCancellationToken) {
                try {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var cRequestModel = cRequest.AsThreatListUpdateRequestModel();
                    var cResponseModel = await HttpBrowsingClient.GetThreatListUpdatesUri
                                             .PostJsonAsync(cRequestModel, cCancellationToken)
                                             .ReceiveJson<ThreatListUpdateResponseModel>()
                                             .ConfigureAwait(false);

                    var cResponse = cResponseModel.AsThreatListUpdateResponse(cRequest);
                    return cResponse;
                }
                catch (FlurlHttpTimeoutException cEx) {
                    const string cDetailMessage = "An HTTP request to the Google Safe Browsing API timed out.";
                    throw new TimeoutException(cDetailMessage, cEx);
                }
                catch (FlurlHttpException cEx) {
                    var httpStatusCode = cEx.Call.HttpStatus ?? HttpStatusCode.BadRequest;
                    const string cDetailMessage = "An HTTP request to the Google Safe Browsing API failed.";
                    throw new BrowsingClientException(cDetailMessage, httpStatusCode, cEx);
                }
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
                var detailMessage = $"An object ({nameof(HttpBrowsingClient)}) is disposed.";
                throw new ObjectDisposedException(nameof(HttpBrowsingClient), detailMessage);
            }
        }

        /// <summary>
        ///     HTTP Client Factory.
        /// </summary>
        private sealed class HttpClientFactory : DefaultHttpClientFactory {
            /// <summary>
            ///     Create an HTTP Client.
            /// </summary>
            /// <param name="httpMessageHandler">
            ///     An HTTP message handler for the HTTP client to use.
            /// </param>
            /// <returns>
            ///     An HTTP client.
            /// </returns>
            public override HttpClient CreateHttpClient(HttpMessageHandler httpMessageHandler) {
                var httpClient = base.CreateHttpClient(httpMessageHandler);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(1);

                return httpClient;
            }

            /// <summary>
            ///     Create an HTTP Message Handler.
            /// </summary>
            /// <returns>
            ///     An HTTP message handler.
            /// </returns>
            public override HttpMessageHandler CreateMessageHandler() {
                var httpMessageHandler = base.CreateMessageHandler();
                if (httpMessageHandler is HttpClientHandler httpClientHandler) {
                    if (httpClientHandler.SupportsAutomaticDecompression) {
                        // ...
                        //
                        // The Google Safe Browsing API supports compressed HTTP responses if the correct HTTP headers
                        // are set in an HTTP request. This should make the HTTP client automatically add the correct
                        // HTTP headers with every HTTP request.
                        httpClientHandler.AutomaticDecompression = DecompressionMethods.Deflate |
                                                                   DecompressionMethods.GZip;
                    }

                    httpClientHandler.UseCookies = false;
                    httpClientHandler.UseDefaultCredentials = false;
                }

                return httpMessageHandler;
            }
        }
    }
}