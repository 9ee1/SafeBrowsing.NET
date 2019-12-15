namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Constraints Extension.
    /// </summary>
    internal static class ThreatListUpdateConstraintsExtension {
        /// <summary>
        ///     Create a Threat List Update Constraints Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatListUpdateConstraints" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateConstraintsModel" /> if <paramref name="this" /> is not a null reference.
        ///     A null reference otherwise.
        /// </returns>
        internal static ThreatListUpdateConstraintsModel AsClientConstraintsModel(this ThreatListUpdateConstraints @this) {
            ThreatListUpdateConstraintsModel threatListUpdateConstraintsModel = null;
            if (@this != null) {
                threatListUpdateConstraintsModel = new ThreatListUpdateConstraintsModel();
                threatListUpdateConstraintsModel.ClientLocation = @this.ClientLocation;
                threatListUpdateConstraintsModel.MaximumDatabaseEntries = @this.MaximumDatabaseEntries;
                threatListUpdateConstraintsModel.MaximumResponseEntries = @this.MaximumResponseEntries;
                threatListUpdateConstraintsModel.SupportedCompressionTypes = CreateSupportedCompressionTypes();
                threatListUpdateConstraintsModel.ThreatListLanguage = @this.ThreatListLanguage;
                threatListUpdateConstraintsModel.ThreatListLocation = @this.ThreatListLocation;
            }

            return threatListUpdateConstraintsModel;

            // <summary>
            //      Create Supported Compression Types.
            // </summary>
            string[] CreateSupportedCompressionTypes() {
                var cSupportedCompressionTypes = new string[1];
                cSupportedCompressionTypes[0] = CompressionType.Uncompressed.AsCompressionTypeModel();
                return cSupportedCompressionTypes;
            }
        }
    }
}