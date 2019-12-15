namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     URL Lookup Result Code.
    /// </summary>
    public enum UrlLookupResultCode {
        /// <summary>
        ///     Indicates an error.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Indicates a determination as to whether a URL is safe or unsafe could not be made because a database
        ///     is expired/out-of-date/stale and needs to be retrieved from the Google Safe Browsing API again.
        /// </summary>
        DatabaseStale = 1,

        /// <summary>
        ///     Indicates a URL is safe.
        /// </summary>
        Safe = 2,

        /// <summary>
        ///     Indicates a URL is unsafe.
        /// </summary>
        Unsafe = 3
    }
}