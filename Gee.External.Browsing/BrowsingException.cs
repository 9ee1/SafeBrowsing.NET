using System;

namespace Gee.External.Browsing {
    /// <summary>
    ///     Browsing Exception.
    /// </summary>
    public abstract class BrowsingException : Exception {
        /// <summary>
        ///     Create a Browsing Exception.
        /// </summary>
        /// <param name="detailMessage">
        ///     A detail message describing the reason the exception is being thrown.
        /// </param>
        private protected BrowsingException(string detailMessage) : base(detailMessage) { }

        /// <summary>
        ///     Create a Browsing Exception.
        /// </summary>
        /// <param name="detailMessage">
        ///     A detail message describing the reason the exception is being thrown.
        /// </param>
        /// <param name="innerException">
        ///     An exception that is the cause of the exception being thrown.
        /// </param>
        private protected BrowsingException(string detailMessage, Exception innerException) : base(detailMessage, innerException) { }
    }
}