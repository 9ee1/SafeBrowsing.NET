using System.Linq;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Request Extension.
    /// </summary>
    internal static class ThreatListUpdateRequestExtension {
        /// <summary>
        ///     Create a Threat List Update Request Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatListUpdateRequest" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateRequestModel" /> if <paramref name="this" /> is not a null reference. A
        ///     null reference otherwise.
        /// </returns>
        internal static ThreatListUpdateRequestModel AsThreatListUpdateRequestModel(this ThreatListUpdateRequest @this) {
            ThreatListUpdateRequestModel threatListUpdateRequestModel = null;
            if (@this != null) {
                threatListUpdateRequestModel = new ThreatListUpdateRequestModel();
                threatListUpdateRequestModel.ClientMetadata = @this.ClientMetadata.AsClientMetadataModel();
                threatListUpdateRequestModel.Queries = @this.Queries.Select(m => m.AsThreatListUpdateQueryModel());
            }

            return threatListUpdateRequestModel;
        }
    }
}