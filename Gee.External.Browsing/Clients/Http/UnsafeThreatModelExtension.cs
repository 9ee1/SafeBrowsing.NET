using Gee.Common;
using Gee.Common.Text;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Unsafe Threat Model Extension.
    /// </summary>
    internal static class UnsafeThreatModelExtension {
        /// <summary>
        ///     Create an Unsafe Threat.
        /// </summary>
        /// <param name="this">
        ///     An <see cref="UnsafeThreatModel" />.
        /// </param>
        /// <returns>
        ///     An <see cref="UnsafeThreat" /> if <paramref name="this" /> is not a null reference. A null reference
        ///     otherwise.
        /// </returns>
        internal static UnsafeThreat AsUnsafeThreat(this UnsafeThreatModel @this) {
            UnsafeThreat unsafeThreat = null;
            if (@this != null) {
                var associatedThreatListDescriptor = CreateAssociatedThreatListDescriptor(@this);
                var expirationDate = CreateExpirationDate(@this);
                var sha256Hash = @this.Threat.Sha256Hash.Base64Decode().HexadecimalEncode();
                unsafeThreat = new UnsafeThreat(sha256Hash, associatedThreatListDescriptor, expirationDate);
                unsafeThreat.Metadata = CreateMetadata(@this);
            }

            return unsafeThreat;

            // <summary>
            //      Create Associated Threat List Descriptor.
            // </summary>
            ThreatListDescriptor CreateAssociatedThreatListDescriptor(UnsafeThreatModel cThis) {
                var cPlatformType = cThis.PlatformType.AsPlatformType();
                var cThreatEntryType = cThis.ThreatEntryType.AsThreatEntryType();
                var cThreatType = cThis.ThreatType.AsThreatType();
                var cAssociatedThreatListDescriptor = new ThreatListDescriptor(cThreatType, cPlatformType, cThreatEntryType);

                return cAssociatedThreatListDescriptor;
            }

            // <summary>
            //      Create Expiration Date.
            // </summary>
            DateTime CreateExpirationDate(UnsafeThreatModel cThis) {
                var cExpirationDate = DateTime.UtcNow;
                if (cThis.CacheDuration != null) {
                    var cCacheDuration = cThis.CacheDuration.Substring(0, cThis.CacheDuration.Length - 1);
                    var cIsCacheDurationParsed = double.TryParse(cCacheDuration, out var cCacheDurationDouble);
                    if (cIsCacheDurationParsed) {
                        cExpirationDate = DateTime.UtcNow.AddSeconds(cCacheDurationDouble);
                    }
                }

                return cExpirationDate;
            }

            Dictionary<string, string> CreateMetadata(UnsafeThreatModel cThis) {
                var cMetadata = new Dictionary<string, string>();
                if (cThis.Metadata?.Entries != null) {
                    foreach (var cMetadataEntry in cThis.Metadata.Entries) {
                        var cKey = cMetadataEntry.Key.Base64Decode().AsciiDecode();
                        var cValue = cMetadataEntry.Value.Base64Decode().AsciiDecode();
                        cMetadata[cKey] = cValue;
                    }
                }

                return cMetadata;
            }
        }
    }
}