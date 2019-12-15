using Gee.Common;
using System;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Safe Cache Entry.
    /// </summary>
    public sealed class SafeCacheEntry {
        /// <summary>
        ///     Get Safe Cache Entry's Expiration Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), the safe cache entry expires and the threat
        ///     identified by <see cref="ThreatSha256HashPrefix" /> should be considered safe to.
        /// </remarks>
        public DateTime ExpirationDate { get; }

        /// <summary>
        ///     Determine if Safe Cache Entry Has Expired.
        /// </summary>
        /// <remarks>
        ///     Determines if the safe cache entry has expired and the threat identified by
        ///     <see cref="ThreatSha256HashPrefix" /> should no longer be considered safe.
        /// </remarks>
        public bool Expired => DateTime.UtcNow >= this.ExpirationDate;

        /// <summary>
        ///     Get Threat's SHA256 Hash Prefix.
        /// </summary>
        /// <remarks>
        ///     Represents the SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat
        ///     that was retrieved from a <see cref="IBrowsingCache" />.
        /// </remarks>
        public string ThreatSha256HashPrefix { get; }

        /// <summary>
        ///     Create a Safe Cache Entry.
        /// </summary>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat that was 
        ///     retrieved from a <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="expirationDate">
        ///     The date, in Coordinated Universal Time (UTC), the safe cache entry expires and the threat identified
        ///     by <paramref name="threatSha256HashPrefix" /> should be considered safe to. If the date is not in UTC,
        ///     it is converted to it.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatSha256HashPrefix" /> is not formatted as a hexadecimal encoded string.
        /// </exception>
        public SafeCacheEntry(string threatSha256HashPrefix, DateTime expirationDate) {
            this.ExpirationDate = expirationDate.ToUniversalTime();
            this.ThreatSha256HashPrefix = CreateThreatSha256HashPrefix(threatSha256HashPrefix);

            // <summary>
            //      Create Threat's SHA256 Hash Prefix.
            // </summary>
            string CreateThreatSha256HashPrefix(string cThreatSha256HashPrefix) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cIsThreatSha256HashPrefixHexadecimalEncoded = cThreatSha256HashPrefix.IsHexadecimalEncoded();
                if (!cIsThreatSha256HashPrefixHexadecimalEncoded) {
                    var cDetailMessage = $"A threat's SHA256 hash prefix ({cThreatSha256HashPrefix}) is not hexadecimal encoded.";
                    throw new FormatException(cDetailMessage);
                }

                return cThreatSha256HashPrefix;
            }
        }

        /// <summary>
        ///     Determine if Object is Equal to Another Object.
        /// </summary>
        /// <param name="object">
        ///     An object to compare to.
        /// </param>
        /// <returns>
        ///     A boolean true if the object is equal to <paramref name="object" />. A boolean false otherwise.
        /// </returns>
        public override bool Equals(object @object) {
            var isEqual = @object != null &&
                          @object is SafeCacheEntry safeCacheEntry &&
                          this.ThreatSha256HashPrefix == safeCacheEntry.ThreatSha256HashPrefix;

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
                hashCode = hashCode * 7 + this.ThreatSha256HashPrefix.GetHashCode();
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
            return this.ThreatSha256HashPrefix;
        }
    }
}