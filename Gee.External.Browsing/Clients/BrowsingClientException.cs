using System;
using System.Net;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Client Exception.
    /// </summary>
    public sealed class BrowsingClientException : BrowsingException {
        /// <summary>
        ///     Get HTTP Status Code.
        /// </summary>
        /// <remarks>
        ///     Represents the HTTP status code indicating the nature of the HTTP response for the failed HTTP request
        ///     the exception was thrown for.
        /// </remarks>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        ///     Create a Client Exception.
        /// </summary>
        /// <param name="detailMessage">
        ///     A detail message describing the reason the exception is being thrown.
        /// </param>
        /// <param name="httpStatusCode">
        ///     An HTTP status code indicating the nature of the HTTP response for the failed HTTP request the
        ///     exception is being thrown for.
        /// </param>
        public BrowsingClientException(string detailMessage, HttpStatusCode httpStatusCode) : this(detailMessage, httpStatusCode, null) { }

        /// <summary>
        ///     Create a Client Exception.
        /// </summary>
        /// <param name="detailMessage">
        ///     A detail message describing the reason the exception is being thrown.
        /// </param>
        /// <param name="httpStatusCode">
        ///     An HTTP status code indicating the nature of the HTTP response for the failed HTTP request the
        ///     exception is being thrown for.
        /// </param>
        /// <param name="innerException">
        ///     An exception that is the cause of the exception being thrown.
        /// </param>
        public BrowsingClientException(string detailMessage, HttpStatusCode httpStatusCode, Exception innerException) : base(detailMessage, innerException) {
            this.HttpStatusCode = httpStatusCode;
        }
    }
}