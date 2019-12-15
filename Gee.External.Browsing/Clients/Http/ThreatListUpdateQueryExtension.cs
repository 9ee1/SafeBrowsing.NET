using Gee.Common;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Query Extension.
    /// </summary>
    internal static class ThreatListUpdateQueryExtension {
        /// <summary>
        ///     Create a Threat List Update Query Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatListUpdateQuery" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateQueryModel" /> if <paramref name="this" /> is not a null reference. A null
        ///     reference otherwise.
        /// </returns>
        internal static ThreatListUpdateQueryModel AsThreatListUpdateQueryModel(this ThreatListUpdateQuery @this) {
            ThreatListUpdateQueryModel threatListUpdateQueryModel = null;
            if (@this != null) {
                threatListUpdateQueryModel = new ThreatListUpdateQueryModel();
                threatListUpdateQueryModel.Constraints = @this.UpdateConstraints.AsClientConstraintsModel();
                threatListUpdateQueryModel.PlatformType = @this.ThreatListDescriptor.PlatformType.AsPlatformTypeModel();
                threatListUpdateQueryModel.ThreatEntryType = @this.ThreatListDescriptor.ThreatEntryType.AsThreatEntryTypeModel();
                threatListUpdateQueryModel.ThreatListState = @this.ThreatListState?.HexadecimalDecode().Base64Encode();
                threatListUpdateQueryModel.ThreatType = @this.ThreatListDescriptor.ThreatType.AsThreatTypeModel();
            }

            return threatListUpdateQueryModel;
        }
    }
}