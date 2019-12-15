using Gee.Common;
using Gee.Common.Guards;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing {
    /// <summary>
    ///     Unsafe Threat.
    /// </summary>
    public sealed class UnsafeThreat {
        /// <summary>
        ///     Get Threat's Associated Threat List Descriptor.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> the
        ///     threat is associated with.
        /// </remarks>
        public ThreatListDescriptor AssociatedThreatListDescriptor { get; }

        /// <summary>
        ///     Get Threat's Expiration Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), the threat should be considered unsafe to.
        /// </remarks>
        public DateTime ExpirationDate { get; }

        /// <summary>
        ///     Determine if Threat Has Expired.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat has expired and should no longer be considered unsafe. The threat is expired
        ///     if <see cref="ExpirationDate" /> has elapsed.
        /// </remarks>
        public bool Expired => DateTime.UtcNow >= this.ExpirationDate;

        /// <summary>
        ///     Determine if Threat is Malware.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat is malware. The threat is malware if
        ///     <see cref="AssociatedThreatListDescriptor" /> is identified by <see cref="ThreatType.Malware" />.
        /// </remarks>
        public bool IsMalware => this.AssociatedThreatListDescriptor.IsMalwareList;

        /// <summary>
        ///     Determine if Threat is a Potentially Harmful Application.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat is a potentially harmful application (PHA). The threat is a PHA if
        ///     <see cref="AssociatedThreatListDescriptor" /> is identified by
        ///     <see cref="ThreatType.PotentiallyHarmfulApplication" />.
        /// </remarks>
        public bool IsPotentiallyHarmfulApplication => this.AssociatedThreatListDescriptor.IsPotentiallyHarmfulApplicationList;

        /// <summary>
        ///     Determine if Threat is Social Engineering.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat is social engineering. The threat is social engineering if
        ///     <see cref="AssociatedThreatListDescriptor" /> is identified by
        ///     <see cref="ThreatType.SocialEngineering" />.
        /// </remarks>
        public bool IsSocialEngineering => this.AssociatedThreatListDescriptor.IsSocialEngineeringList;

        /// <summary>
        ///     Determine if Threat is Unwanted Software.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat is unwanted software. The threat is unwanted software if
        ///     <see cref="AssociatedThreatListDescriptor" /> is identified by
        ///     <see cref="ThreatType.UnwantedSoftware" />.
        /// </remarks>
        public bool IsUnwantedSoftware => this.AssociatedThreatListDescriptor.IsUnwantedSoftwareList;

        /// <summary>
        ///     Get Threat's Metadata.
        /// </summary>
        /// <remarks>
        ///     Represents the threat's metadata. 
        /// </remarks>
        public IReadOnlyDictionary<string, string> Metadata { get; set; }

        /// <summary>
        ///     Get Threat's SHA256 Hash.
        /// </summary>
        /// <remarks>
        ///     Represents the full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat.
        /// </remarks>
        public string Sha256Hash { get; }

        /// <summary>
        ///     Create an Unsafe Threat.
        /// </summary>
        /// <param name="sha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat.
        /// </param>
        /// <param name="associatedThreatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> the threat is
        ///     associated with.
        /// </param>
        /// <param name="expirationDate">
        ///     The date, in Coordinated Universal Time (UTC), the threat should be considered unsafe to. If the date
        ///     is not in UTC, it is converted to it.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="sha256Hash" /> is a null reference, or if
        ///     <paramref name="associatedThreatListDescriptor" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="sha256Hash" /> is not hexadecimal encoded.
        /// </exception>
        public UnsafeThreat(string sha256Hash, ThreatListDescriptor associatedThreatListDescriptor, DateTime expirationDate) {
            Guard.ThrowIf(nameof(associatedThreatListDescriptor), associatedThreatListDescriptor).Null();

            this.AssociatedThreatListDescriptor = associatedThreatListDescriptor;
            this.ExpirationDate = expirationDate.ToUniversalTime();
            this.Sha256Hash = CreateSha256Hash(sha256Hash);

            // <summary>
            //      Create SHA256 Hash.
            // </summary>
            string CreateSha256Hash(string cSha256Hash) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cIsSha256HashHexadecimalEncoded = cSha256Hash.IsHexadecimalEncoded();
                if (!cIsSha256HashHexadecimalEncoded) {
                    var cDetailMessage = $"A SHA256 hash ({cSha256Hash}) is not hexadecimal encoded.";
                    throw new FormatException(cDetailMessage);
                }

                return cSha256Hash;
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
                          @object is UnsafeThreat unsafeThreat &&
                          object.Equals(this.AssociatedThreatListDescriptor, unsafeThreat.AssociatedThreatListDescriptor) &&
                          this.Sha256Hash == unsafeThreat.Sha256Hash;

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
                hashCode = hashCode * 7 + this.AssociatedThreatListDescriptor.GetHashCode();
                hashCode = hashCode * 7 + this.Sha256Hash.GetHashCode();

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
            return this.Sha256Hash;
        }
    }
}