using Gee.Common;
using Gee.Common.Guards;
using System;
using System.Collections.Concurrent;

namespace Gee.External.Browsing {
    /// <summary>
    ///     Threat List.
    /// </summary>
    /// <remarks>
    ///     Represents a threat list retrieved from the Google Safe Browsing API. A threat list is identified using a
    ///     <see cref="ThreatType" />, which indicates the nature of its associated threats, a
    ///     <see cref="PlatformType" />, which indicates the platform its associated threats target, and a
    ///     <see cref="ThreatEntryType" />, which indicates how its associated threats is posed.
    /// </remarks>
    public sealed class ThreatList {
        /// <summary>
        ///     Get Threat List's Descriptor.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListDescriptor" /> identifying the threat list.
        /// </remarks>
        public ThreatListDescriptor Descriptor { get; }

        /// <summary>
        ///     Determine if Threat List Has Expired.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list has expired and should be retrieved from the Google Safe Browsing API
        ///     again.
        /// </remarks>
        public bool Expired => this.WaitToDate == null || DateTime.UtcNow >= this.WaitToDate.Value;

        /// <summary>
        ///     Determine if Threat List is a Malware List.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list is a <see cref="ThreatType.Malware" /> list.
        /// </remarks>
        public bool IsMalwareList => this.Descriptor.IsMalwareList;

        /// <summary>
        ///     Determine if Threat List is a Potentially Harmful Application List.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list is a <see cref="ThreatType.PotentiallyHarmfulApplication" /> (PHA) list.
        /// </remarks>
        public bool IsPotentiallyHarmfulApplicationList => this.Descriptor.IsPotentiallyHarmfulApplicationList;

        /// <summary>
        ///     Determine if Threat List is a Social Engineering List.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list is a <see cref="ThreatType.SocialEngineering" /> list.
        /// </remarks>
        public bool IsSocialEngineeringList => this.Descriptor.IsSocialEngineeringList;

        /// <summary>
        ///     Determine if Threat List is an Unwanted Software List.
        /// </summary>
        /// <remarks>
        ///     Determines if the threat list is an <see cref="ThreatType.UnwantedSoftware" /> list.
        /// </remarks>
        public bool IsUnwantedSoftwareList => this.Descriptor.IsUnwantedSoftwareList;

        /// <summary>
        ///     Get Threat List's Retrieve Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), the threat list was retrieved from the
        ///     Google Safe Browsing API.
        /// </remarks>
        public DateTime RetrieveDate { get; }

        /// <summary>
        ///     Get Threat List's State.
        /// </summary>
        /// <remarks>
        ///     Represents the state, formatted as a hexadecimal encoded string, of the threat list when it was
        ///     retrieved from the Google Safe Browsing API.
        /// </remarks>
        public string State { get; }

        /// <summary>
        ///     Get Threat List's Wait to Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), a client must wait to before retrieving the
        ///     threat list from the Google Safe Browsing API again. A null reference indicates a client does not need
        ///     to wait before retrieving the threat list from the Google Safe Browsing API again.
        /// </remarks>
        public DateTime? WaitToDate { get; }

        /// <summary>
        ///     Build a Threat List.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListBuilder" /> to build a threat list with.
        /// </returns>
        public static ThreatListBuilder Build() {
            return new ThreatListBuilder();
        }

        /// <summary>
        ///     Create an Expired Threat List.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the threat list.
        /// </param>
        /// <param name="threatListState">
        ///     The state, formatted as a hexadecimal encoded string, of the threat list when it was retrieved from the
        ///     Google Safe Browsing API.
        /// </param>
        /// <returns>
        ///     An expired threat list.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference, or if
        ///     <paramref name="threatListState" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatListState" /> is not hexadecimal encoded.
        /// </exception>
        public static ThreatList CreateExpired(ThreatListDescriptor threatListDescriptor, string threatListState) {
            // ...
            //
            // Throws an exception if the operation fails.
            return new ThreatList(threatListDescriptor, threatListState, DateTime.UtcNow);
        }

        /// <summary>
        ///     Create an Invalid Threat List.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the threat list.
        /// </param>
        /// <returns>
        ///     An invalid threat list.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        public static ThreatList CreateInvalid(ThreatListDescriptor threatListDescriptor) {
            // ...
            //
            // Throws an exception if the operation fails.
            return InvalidThreatListCache.CreateInvalidThreatList(threatListDescriptor);
        }

        /// <summary>
        ///     Create a Threat List.
        /// </summary>
        /// <param name="descriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the threat list.
        /// </param>
        /// <param name="state">
        ///     The state, formatted as a hexadecimal encoded string, of the threat list when it was retrieved from the
        ///     Google Safe Browsing API.
        /// </param>
        /// <param name="retrieveDate">
        ///     The date, in Coordinated Universal Time (UTC), the threat list was retrieved from the Google Safe
        ///     Browsing API. If the date is not expressed in UTC, it is converted to it.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="descriptor" /> is a null reference, or if <paramref name="state" /> is a
        ///     null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="state" /> is not hexadecimal encoded.
        /// </exception>
        public ThreatList(ThreatListDescriptor descriptor, string state, DateTime retrieveDate) : this(descriptor, state, retrieveDate, null) { }

        /// <summary>
        ///     Create a Threat List.
        /// </summary>
        /// <param name="descriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the threat list.
        /// </param>
        /// <param name="state">
        ///     The state, formatted as a hexadecimal encoded string, of the threat list when it was retrieved from the
        ///     Google Safe Browsing API.
        /// </param>
        /// <param name="retrieveDate">
        ///     The date, in Coordinated Universal Time (UTC), the threat list was retrieved from the Google Safe
        ///     Browsing API. If the date is not expressed in UTC, it is converted to it.
        /// </param>
        /// <param name="waitToDate">
        ///     The date, in Coordinated Universal Time (UTC), a client must wait to before retrieving the threat list
        ///     from the Google Safe Browsing API again. If the date is not expressed in UTC, it is converted to it. A
        ///     null reference indicates a client does not need to wait before retrieving the threat list from the
        ///     Google Safe Browsing API again.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="descriptor" /> is a null reference, or if <paramref name="state" /> is a
        ///     null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="state" /> is not hexadecimal encoded.
        /// </exception>
        public ThreatList(ThreatListDescriptor descriptor, string state, DateTime retrieveDate, DateTime? waitToDate) {
            Guard.ThrowIf(nameof(descriptor), descriptor).Null();

            this.Descriptor = descriptor;
            this.RetrieveDate = retrieveDate.ToUniversalTime();
            this.State = CreateState(state);
            this.WaitToDate = waitToDate?.ToUniversalTime();

            // <summary>
            //      Create State.
            // </summary>
            string CreateState(string cState) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cIsStateHexadecimalEncoded = cState.IsHexadecimalEncoded();
                if (!cIsStateHexadecimalEncoded) {
                    var cDetailMessage = $"A state ({cState}) is not hexadecimal encoded.";
                    throw new FormatException(cDetailMessage);
                }

                return cState;
            }
        }

        /// <summary>
        ///     Create a Threat List.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="ThreatListBuilder" /> to initialize the threat list with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal ThreatList(ThreatListBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();
            Guard.ThrowIf(nameof(builder), builder.Descriptor).Null();
            Guard.ThrowIf(nameof(builder), builder.State).Null();

            this.Descriptor = builder.Descriptor;
            this.RetrieveDate = builder.RetrieveDate;
            this.State = builder.State;
            this.WaitToDate = builder.WaitToDate;
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
                          @object is ThreatList threatList &&
                          object.Equals(this.Descriptor, threatList.Descriptor) &&
                          this.RetrieveDate == threatList.RetrieveDate &&
                          this.State == threatList.State &&
                          this.WaitToDate == threatList.WaitToDate;

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
                hashCode = hashCode * 7 + this.Descriptor.GetHashCode();
                hashCode = hashCode * 7 + this.RetrieveDate.GetHashCode();
                hashCode = hashCode * 7 + this.State.GetHashCode();
                hashCode = hashCode * 7 + this.WaitToDate.GetHashCodeOrDefault();

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
            var @string = this.Descriptor.ToString();
            return @string;
        }

        /// <summary>
        ///     Invalid Threat List Cache.
        /// </summary>
        private static class InvalidThreatListCache {
            /// <summary>
            ///     Cache.
            /// </summary>
            private static readonly ConcurrentDictionary<ThreatListDescriptor, ThreatList> Cache;

            /// <summary>
            ///     Invalid Threat List State.
            /// </summary>
            private static readonly string InvalidThreatListState;

            /// <summary>
            ///     Create an Invalid Threat List Cache.
            /// </summary>
            static InvalidThreatListCache() {
                InvalidThreatListCache.Cache = new ConcurrentDictionary<ThreatListDescriptor, ThreatList>();
                InvalidThreatListCache.InvalidThreatListState = "AA";
            }

            /// <summary>
            ///     Create an Invalid Threat List.
            /// </summary>
            /// <param name="threatListDescriptor">
            ///     A <see cref="ThreatListDescriptor" /> identifying the threat list.
            /// </param>
            /// <returns>
            ///     An invalid threat list.
            /// </returns>
            /// <exception cref="System.ArgumentNullException">
            ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
            /// </exception>
            internal static ThreatList CreateInvalidThreatList(ThreatListDescriptor threatListDescriptor) {
                Guard.ThrowIf(nameof(threatListDescriptor), threatListDescriptor).Null();

                var threatList = InvalidThreatListCache.Cache.GetOrAdd(threatListDescriptor, tld => {
                    var cThreatList = new ThreatList(tld, InvalidThreatListCache.InvalidThreatListState, DateTime.UtcNow);
                    return cThreatList;
                });

                return threatList;
            }
        }
    }
}