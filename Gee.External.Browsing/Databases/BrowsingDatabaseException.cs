using System;

namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Browsing Database Exception.
    /// </summary>
    public sealed class BrowsingDatabaseException : BrowsingException {
        /// <summary>
        ///     Create a Browsing Database Exception.
        /// </summary>
        /// <param name="detailMessage">
        ///     A detail message describing the reason the exception is being thrown.
        /// </param>
        public BrowsingDatabaseException(string detailMessage) : base(detailMessage) { }

        /// <summary>
        ///     Create a Browsing Database Exception.
        /// </summary>
        /// <param name="detailMessage">
        ///     A detail message describing the reason the exception is being thrown.
        /// </param>
        /// <param name="innerException">
        ///     An exception that is the cause of the exception being thrown.
        /// </param>
        public BrowsingDatabaseException(string detailMessage, Exception innerException) : base(detailMessage, innerException) { }
    }
}