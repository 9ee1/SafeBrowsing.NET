using System;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Cache Exception.
    /// </summary>
    public sealed class BrowsingCacheException : BrowsingException {
        /// <summary>
        ///     Create a Cache Exception.
        /// </summary>
        /// <param name="detailMessage">
        ///     A detail message describing the reason the exception is being thrown.
        /// </param>
        public BrowsingCacheException(string detailMessage) : base(detailMessage) { }

        /// <summary>
        ///     Create a Cache Exception.
        /// </summary>
        /// <param name="detailMessage">
        ///     A detail message describing the reason the exception is being thrown.
        /// </param>
        /// <param name="innerException">
        ///     An exception that is the cause of the exception being thrown.
        /// </param>
        public BrowsingCacheException(string detailMessage, Exception innerException) : base(detailMessage, innerException) { }
    }
}