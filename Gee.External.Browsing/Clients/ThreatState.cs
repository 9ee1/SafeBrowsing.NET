namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat State.
    /// </summary>
    public enum ThreatState {
        /// <summary>
        ///     Indicates the state of a threat is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Indicates a threat is expired and should be retrieved from the Google Safe Browsing API again.
        /// </summary>
        Expired = 1,

        /// <summary>
        ///     Indicates a threat is safe.
        /// </summary>
        Safe = 2,

        /// <summary>
        ///     Indicates a threat is unsafe.
        /// </summary>
        Unsafe = 3
    }
}