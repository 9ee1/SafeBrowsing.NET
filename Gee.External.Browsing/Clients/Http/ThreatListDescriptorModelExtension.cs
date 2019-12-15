namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Descriptor Model Extension.
    /// </summary>
    internal static class ThreatListDescriptorModelExtension {
        /// <summary>
        ///     Create a Threat List Descriptor.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatListDescriptorModel" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListDescriptor" /> if <paramref name="this" /> is not a null reference. A null
        ///     reference otherwise.
        /// </returns>
        internal static ThreatListDescriptor AsThreatListDescriptor(this ThreatListDescriptorModel @this) {
            ThreatListDescriptor threatListDescriptor = null;
            if (@this != null) {
                var platformType = @this.PlatformType.AsPlatformType();
                var threatEntryType = @this.ThreatEntryType.AsThreatEntryType();
                var threatType = @this.ThreatType.AsThreatType();
                threatListDescriptor = new ThreatListDescriptor(threatType, platformType, threatEntryType);
            }

            return threatListDescriptor;
        }
    }
}