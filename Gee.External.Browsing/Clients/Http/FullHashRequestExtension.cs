using Gee.Common;
using System.Collections.Generic;
using System.Linq;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Full Hash Request Extension.
    /// </summary>
    internal static class FullHashRequestExtension {
        /// <summary>
        ///     Create a Full Hash Request Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="FullHashRequest" />.
        /// </param>
        /// <returns>
        ///     A <see cref="FullHashRequestModel" /> if <paramref name="this" /> is not a null reference. A null
        ///     reference otherwise.
        /// </returns>
        internal static FullHashRequestModel AsFullHashRequestModel(this FullHashRequest @this) {
            FullHashRequestModel fullHashRequestModel = null;
            if (@this != null) {
                // ...
                //
                // We use hash sets for platform types, threat entry types, and threat types because we want a unique
                // set of each to prevent duplicate unsafe threats being returned as part of the response. We do not
                // use a hash set for threat list states, but rather a regular list, because different threat lists
                // might have the same state but we want to include them anyway.
                var platformTypeModels = new HashSet<string>();
                var threatEntryTypeModels = new HashSet<string>();
                var threatListStates = new List<string>();
                var threatTypeModels = new HashSet<string>();
                foreach (var query in @this.Queries) {
                    var platformTypeModel = query.ThreatListDescriptor.PlatformType.AsPlatformTypeModel();
                    var threatEntryTypeModel = query.ThreatListDescriptor.ThreatEntryType.AsThreatEntryTypeModel();
                    var threatListState = query.ThreatListState.HexadecimalDecode().Base64Encode();
                    var threatTypeModel = query.ThreatListDescriptor.ThreatType.AsThreatTypeModel();

                    platformTypeModels.Add(platformTypeModel);
                    threatEntryTypeModels.Add(threatEntryTypeModel);
                    threatListStates.Add(threatListState);
                    threatTypeModels.Add(threatTypeModel);
                }

                // ...
                //
                // Using LINQ here is actually better than enumerating the existing collection and projecting it to a
                // new collection manually because it forces the enumeration to occur only once when the collection is
                // being serialized to JSON.
                var threatEntryModels = @this.Sha256HashPrefixes
                    .Select(m => m.HexadecimalDecode())
                    .Select(m => m.Base64Encode())
                    .Select(m => new ThreatEntryModel {Sha256Hash = m});

                var fullHashRequestQueryModel = new FullHashQueryModel();
                fullHashRequestQueryModel.PlatformTypes = platformTypeModels;
                fullHashRequestQueryModel.ThreatEntries = threatEntryModels;
                fullHashRequestQueryModel.ThreatEntryTypes = threatEntryTypeModels;
                fullHashRequestQueryModel.ThreatTypes = threatTypeModels;

                fullHashRequestModel = new FullHashRequestModel();
                fullHashRequestModel.ClientMetadata = @this.ClientMetadata.AsClientMetadataModel();
                fullHashRequestModel.Query = fullHashRequestQueryModel;
                fullHashRequestModel.ThreatListStates = threatListStates;
            }

            return fullHashRequestModel;
        }
    }
}