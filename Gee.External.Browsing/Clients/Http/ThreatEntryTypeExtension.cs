namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat Entry Type Extension.
    /// </summary>
    internal static class ThreatEntryTypeExtension {
        /// <summary>
        ///     Create a Threat Entry Type.
        /// </summary>
        /// <param name="this">
        ///     A string identifying a <see cref="ThreatEntryType" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatEntryType" />.
        /// </returns>
        internal static ThreatEntryType AsThreatEntryType(this string @this) {
            ThreatEntryType threatEntryType;
            switch (@this) {
                case "EXECUTABLE":
                    threatEntryType = ThreatEntryType.Executable;
                    break;
                case "IP_RANGE":
                    threatEntryType = ThreatEntryType.IpAddressRange;
                    break;
                case "THREAT_ENTRY_TYPE_UNSPECIFIED":
                    threatEntryType = ThreatEntryType.Unknown;
                    break;
                case "URL":
                    threatEntryType = ThreatEntryType.Url;
                    break;
                default:
                    threatEntryType = ThreatEntryType.Unknown;
                    break;
            }

            return threatEntryType;
        }

        /// <summary>
        ///     Create a Threat Entry Type Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatEntryType" />.
        /// </param>
        /// <returns>
        ///     A string identifying a <see cref="ThreatEntryType" />.
        /// </returns>
        internal static string AsThreatEntryTypeModel(this ThreatEntryType @this) {
            string threatEntryTypeModel;
            switch (@this) {
                case ThreatEntryType.Executable:
                    threatEntryTypeModel = "EXECUTABLE";
                    break;
                case ThreatEntryType.IpAddressRange:
                    threatEntryTypeModel = "IP_RANGE";
                    break;
                case ThreatEntryType.Unknown:
                    threatEntryTypeModel = "THREAT_ENTRY_TYPE_UNSPECIFIED";
                    break;
                case ThreatEntryType.Url:
                    threatEntryTypeModel = "URL";
                    break;
                default:
                    threatEntryTypeModel = "THREAT_ENTRY_TYPE_UNSPECIFIED";
                    break;
            }

            return threatEntryTypeModel;
        }
    }
}