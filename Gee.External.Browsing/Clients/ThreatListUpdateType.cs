namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Type.
    /// </summary>
    public enum ThreatListUpdateType {
        /// <summary>
        ///     Indicates the update type of a <see cref="ThreatList" /> retrieved from the Google Safe Browsing API
        ///     is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Indicates a <see cref="ThreatList" /> was retrieved from the Google Safe Browsing API as a full
        ///     update. The locally stored copy of the threat list should be disregarded in its entirety and replaced
        ///     with the retrieved threat list.
        /// </summary>
        Full = 1,

        /// <summary>
        ///     Indicates a <see cref="ThreatList" /> was retrieved from the Google Safe Browsing API as a partial
        ///     update. The retrieved threat list will contain threats that should be removed from and threats to add
        ///     to the locally stored copy of the threat list.
        /// </summary>
        Partial = 2
    }
}