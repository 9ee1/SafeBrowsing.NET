namespace Gee.External.Browsing {
    /// <summary>
    ///     Threat List Descriptor.
    /// </summary>
    /// <remarks>
    ///     Identifies a threat list. A threat list is identified using a <see cref="Browsing.ThreatType" />, which
    ///     indicates the nature of its associated threats, a <see cref="Browsing.PlatformType" />, which indicates
    ///     the platform its associated threats target, and a <see cref="Browsing.ThreatEntryType" />, which indicates
    ///     how its associated threats is posed.
    /// </remarks>
    public sealed class ThreatListDescriptor {
        /// <summary>
        ///     Determine if Threat List is a Malware List.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list is a <see cref="Browsing.ThreatType.Malware" /> list.
        /// </remarks>
        public bool IsMalwareList => this.ThreatType == ThreatType.Malware;

        /// <summary>
        ///     Determine if Threat List is a Potentially Harmful Application List.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list is a <see cref="Browsing.ThreatType.PotentiallyHarmfulApplication" />
        ///     (PHA) list.
        /// </remarks>
        public bool IsPotentiallyHarmfulApplicationList => this.ThreatType == ThreatType.PotentiallyHarmfulApplication;

        /// <summary>
        ///     Determine if Threat List is a Social Engineering List.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list is a <see cref="Browsing.ThreatType.SocialEngineering" /> list.
        /// </remarks>
        public bool IsSocialEngineeringList => this.ThreatType == ThreatType.SocialEngineering;

        /// <summary>
        ///     Determine if Threat List is an Unwanted Software List.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list is an <see cref="Browsing.ThreatType.UnwantedSoftware" /> list.
        /// </remarks>
        public bool IsUnwantedSoftwareList => this.ThreatType == ThreatType.UnwantedSoftware;

        /// <summary>
        ///     Get Threat List's Platform Type.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="PlatformType" /> identifying the threat list.
        /// </remarks>
        public PlatformType PlatformType { get; }

        /// <summary>
        ///     Get Threat List's Threat Entry Type.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatEntryType" /> identifying the threat list.
        /// </remarks>
        public ThreatEntryType ThreatEntryType { get; }

        /// <summary>
        ///     Get Threat List's Threat Type.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatType" /> identifying the threat list.
        /// </remarks>
        public ThreatType ThreatType { get; }

        /// <summary>
        ///     Create a Threat List Descriptor.
        /// </summary>
        /// <param name="threatType">
        ///     A <see cref="ThreatType" /> identifying the threat list.
        /// </param>
        /// <param name="platformType">
        ///     A <see cref="PlatformType" /> identifying the threat list.
        /// </param>
        /// <param name="threatEntryType">
        ///     A <see cref="ThreatEntryType" /> identifying the threat list.
        /// </param>
        public ThreatListDescriptor(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType) {
            this.PlatformType = platformType;
            this.ThreatEntryType = threatEntryType;
            this.ThreatType = threatType;
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
                          @object is ThreatListDescriptor threatListDescriptor &&
                          this.PlatformType == threatListDescriptor.PlatformType &&
                          this.ThreatEntryType == threatListDescriptor.ThreatEntryType &&
                          this.ThreatType == threatListDescriptor.ThreatType;

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
                hashCode = hashCode * 7 + this.PlatformType.GetHashCode();
                hashCode = hashCode * 7 + this.ThreatEntryType.GetHashCode();
                hashCode = hashCode * 7 + this.ThreatType.GetHashCode();

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
            var @string = $"{this.ThreatType}/{this.PlatformType}/{this.ThreatEntryType}";
            return @string;
        }
    }
}