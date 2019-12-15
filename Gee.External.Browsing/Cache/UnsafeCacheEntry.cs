using Gee.Common;
using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Unsafe Cache Entry.
    /// </summary>
    public sealed class UnsafeCacheEntry {
        /// <summary>
        ///     Determine if Unsafe Cache Entry Has Expired.
        /// </summary>
        /// <remarks>
        ///     Determines if the unsafe cache entry has expired and the threat identified by
        ///     <see cref="ThreatSha256Hash" /> should no longer be considered unsafe.
        /// </remarks>
        public bool Expired => this.UnsafeThreats.Any(ut => ut.Expired);

        /// <summary>
        ///     Get Threat's SHA256 Hash.
        /// </summary>
        /// <remarks>
        ///     Represents the full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat
        ///     that was retrieved from a <see cref="IBrowsingCache" />.
        /// </remarks>
        public string ThreatSha256Hash { get; }

        /// <summary>
        ///     Get Unsafe Threats.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="UnsafeThreat" /> that was retrieved from a
        ///     <see cref="IBrowsingCache" />.
        /// </remarks>
        public IEnumerable<UnsafeThreat> UnsafeThreats { get; }

        /// <summary>
        ///     Create an Unsafe Cache Entry.
        /// </summary>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat that was retrieved
        ///     from a <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="unsafeThreats">
        ///     A collection of <see cref="UnsafeThreat" /> that was retrieved from a <see cref="IBrowsingCache" />.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is a null reference, or if
        ///     <paramref name="unsafeThreats" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is not hexadecimal encoded.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public UnsafeCacheEntry(string threatSha256Hash, IEnumerable<UnsafeThreat> unsafeThreats) {
            Guard.ThrowIf(nameof(threatSha256Hash), threatSha256Hash).Null();
            Guard.ThrowIf(nameof(unsafeThreats), unsafeThreats).Null();

            this.ThreatSha256Hash = CreateThreatSha256Hash(threatSha256Hash);
            this.UnsafeThreats = new HashSet<UnsafeThreat>(unsafeThreats);

            // <summary>
            //      Create Threat's SHA256 Hash Prefix.
            // </summary>
            string CreateThreatSha256Hash(string cThreatSha256Hash) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cIsThreatSha256HashHexadecimalEncoded = cThreatSha256Hash.IsHexadecimalEncoded();
                if (!cIsThreatSha256HashHexadecimalEncoded) {
                    var cDetailMessage = $"A threat's SHA256 hash ({cThreatSha256Hash}) is not hexadecimal encoded.";
                    throw new FormatException(cDetailMessage);
                }

                return cThreatSha256Hash;
            }
        }

        /// <summary>
        ///     Determine if This Object is Equal to Another Object.
        /// </summary>
        /// <param name="object">
        ///     An object to compare to.
        /// </param>
        /// <returns>
        ///     A boolean true if the object is equal to <paramref name="object" />. A boolean false otherwise.
        /// </returns>
        public override bool Equals(object @object) {
            var isEqual = @object != null &&
                          @object is UnsafeCacheEntry unsafeCacheEntry &&
                          this.ThreatSha256Hash == unsafeCacheEntry.ThreatSha256Hash;

            return isEqual;
        }

        /// <summary>
        ///     Get Object's Hash Code.
        /// </summary>
        /// <returns>
        ///     The object's hash code.
        /// </returns>
        public override int GetHashCode() {
            unchecked {
                var hashCode = 13;
                hashCode = hashCode * 7 + this.ThreatSha256Hash.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Get Object's String Representation.
        /// </summary>
        /// <returns>
        ///     The object's string representation.
        /// </returns>
        public override string ToString() {
            return this.ThreatSha256Hash;
        }
    }
}