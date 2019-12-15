namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat Type Extension.
    /// </summary>
    internal static class ThreatTypeExtension {
        /// <summary>
        ///     Create a Threat Type.
        /// </summary>
        /// <param name="this">
        ///     A string identifying a <see cref="ThreatType" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatType" />.
        /// </returns>
        internal static ThreatType AsThreatType(this string @this) {
            ThreatType threatType;
            switch (@this) {
                case "MALWARE":
                    threatType = ThreatType.Malware;
                    break;
                case "POTENTIALLY_HARMFUL_APPLICATION":
                    threatType = ThreatType.PotentiallyHarmfulApplication;
                    break;
                case "SOCIAL_ENGINEERING":
                    threatType = ThreatType.SocialEngineering;
                    break;
                case "THREAT_TYPE_UNSPECIFIED":
                    threatType = ThreatType.Unknown;
                    break;
                case "UNWANTED_SOFTWARE":
                    threatType = ThreatType.UnwantedSoftware;
                    break;
                default:
                    threatType = ThreatType.Unknown;
                    break;
            }

            return threatType;
        }

        /// <summary>
        ///     Create a Threat Type Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatType" />.
        /// </param>
        /// <returns>
        ///     A string identifying a <see cref="ThreatType" />.
        /// </returns>
        internal static string AsThreatTypeModel(this ThreatType @this) {
            string threatTypeModel;
            switch (@this) {
                case ThreatType.Malware:
                    threatTypeModel = "MALWARE";
                    break;
                case ThreatType.PotentiallyHarmfulApplication:
                    threatTypeModel = "POTENTIALLY_HARMFUL_APPLICATION";
                    break;
                case ThreatType.SocialEngineering:
                    threatTypeModel = "SOCIAL_ENGINEERING";
                    break;
                case ThreatType.Unknown:
                    threatTypeModel = "THREAT_TYPE_UNSPECIFIED";
                    break;
                case ThreatType.UnwantedSoftware:
                    threatTypeModel = "UNWANTED_SOFTWARE";
                    break;
                default:
                    threatTypeModel = "THREAT_TYPE_UNSPECIFIED";
                    break;
            }

            return threatTypeModel;
        }
    }
}