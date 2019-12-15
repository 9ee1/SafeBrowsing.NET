namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Database Lookup Result Code.
    /// </summary>
    public enum DatabaseLookupResultCode {
        /// <summary>
        ///     Indicates an error has occurred looking up a threat in a local database.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Indicates a threat has an entry in a local database and should be considered tentatively unsafe until
        ///     it is verified by the Google Safe Browsing API. 
        /// </summary>
        Hit = 1,

        /// <summary>
        ///     Indicates a threat does not have an entry in a local database and should be considered safe.
        /// </summary>
        Miss = 2,

        /// <summary>
        ///     Indicates a determination as to whether a threat has an entry in a local database could not be made
        ///     because the local database is expired/out-of-date/stale and needs to be retrieved from the Google Safe
        ///     Browsing API again.
        /// </summary>
        Stale = 3
    }
}