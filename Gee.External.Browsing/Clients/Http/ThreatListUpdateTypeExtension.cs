namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Type Extension.
    /// </summary>
    internal static class ThreatListUpdateTypeExtension {
        /// <summary>
        ///     Create a Threat List Update Type.
        /// </summary>
        /// <param name="this">
        ///     A string identifying a <see cref="ThreatListUpdateType" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateType" />.
        /// </returns>
        internal static ThreatListUpdateType AsThreatListUpdateType(this string @this) {
            ThreatListUpdateType threatListUpdateType;
            switch (@this) {
                case "FULL_UPDATE":
                    threatListUpdateType = ThreatListUpdateType.Full;
                    break;
                case "PARTIAL_UPDATE":
                    threatListUpdateType = ThreatListUpdateType.Partial;
                    break;
                case "RESPONSE_TYPE_UNSPECIFIED":
                    threatListUpdateType = ThreatListUpdateType.Unknown;
                    break;
                default:
                    threatListUpdateType = ThreatListUpdateType.Unknown;
                    break;
            }

            return threatListUpdateType;
        }

        /// <summary>
        ///     Create a Threat List Update Type Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatListUpdateType" />.
        /// </param>
        /// <returns>
        ///     A string identifying a <see cref="ThreatListUpdateType" />.
        /// </returns>
        internal static string AsThreatListUpdateTypeModel(this ThreatListUpdateType @this) {
            string threatListUpdateTypeModel;
            switch (@this) {
                case ThreatListUpdateType.Full:
                    threatListUpdateTypeModel = "FULL_UPDATE";
                    break;
                case ThreatListUpdateType.Partial:
                    threatListUpdateTypeModel = "PARTIAL_UPDATE";
                    break;
                case ThreatListUpdateType.Unknown:
                    threatListUpdateTypeModel = "RESPONSE_TYPE_UNSPECIFIED";
                    break;
                default:
                    threatListUpdateTypeModel = "RESPONSE_TYPE_UNSPECIFIED";
                    break;
            }

            return threatListUpdateTypeModel;
        }
    }
}