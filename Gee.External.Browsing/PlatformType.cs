namespace Gee.External.Browsing {
    /// <summary>
    ///     Platform Type.
    /// </summary>
    /// <remarks>
    ///     Indicates the platform a threat targets. A <see cref="PlatformType" /> is also used as one of the 
    ///     identifiers identifying a <see cref="ThreatList" /> together with <see cref="ThreatEntryType" /> and
    ///     <see cref="ThreatType" />.
    /// </remarks>
    public enum PlatformType {
        /// <summary>
        ///     Indicates a threat targets an unknown platform.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Indicates a threat targets all platforms without explicitly indicating which one.
        /// </summary>
        All = 1,

        /// <summary>
        ///     Indicates a threat targets the Android platform.
        /// </summary>
        Android = 2,

        /// <summary>
        ///     Indicates a threat targets at least one platform without explicitly indicating which one.
        /// </summary>
        Any = 3,

        /// <summary>
        ///     Indicates a threat targets the Chrome platform.
        /// </summary>
        Chrome = 4,

        /// <summary>
        ///     Indicates a threat targets the iOS platform.
        /// </summary>
        Ios = 5,

        /// <summary>
        ///     Indicates a threat targets the Linux platform.
        /// </summary>
        Linux = 6,

        /// <summary>
        ///     Indicates a threat targets the MacOS platform.
        /// </summary>
        MacOs = 7,

        /// <summary>
        ///     Indicates a threat targets the Windows platform.
        /// </summary>
        Windows = 8
    }
}