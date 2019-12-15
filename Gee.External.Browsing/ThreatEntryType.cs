namespace Gee.External.Browsing {
    /// <summary>
    ///     Threat Entry Type.
    /// </summary>
    /// <remarks>
    ///     Indicates how a threat is posed. A <see cref="ThreatEntryType" /> is also used as one of the 
    ///     identifiers identifying a <see cref="ThreatList" /> together with <see cref="PlatformType" /> and
    ///     <see cref="ThreatType" />.
    /// </remarks>
    public enum ThreatEntryType {
        /// <summary>
        ///     Indicates a threat is posed through an unknown type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Indicates a threat is posed through an executable.
        /// </summary>
        Executable = 1,

        /// <summary>
        ///     Indicates a threat is posed through an IP address range.
        /// </summary>
        IpAddressRange = 2,

        /// <summary>
        ///     Indicates a threat is posed through a Uniform Resource Locator (URL).
        /// </summary>
        Url = 3
    }
}