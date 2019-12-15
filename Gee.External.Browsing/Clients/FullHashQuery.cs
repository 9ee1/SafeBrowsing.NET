using Gee.Common;
using Gee.Common.Guards;
using System;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Full Hash Query.
    /// </summary>
    public sealed class FullHashQuery {
        /// <summary>
        ///     Get Threat List Descriptor.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="Browsing.ThreatListDescriptor" /> identifying the <see cref="ThreatList" />
        ///     to query.
        /// </remarks>
        public ThreatListDescriptor ThreatListDescriptor { get; }

        /// <summary>
        ///     Get and Set Threat List State.
        /// </summary>
        /// <remarks>
        ///     Represents the state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" />
        ///     identified by <see cref="ThreatListDescriptor" />.
        /// </remarks>
        public string ThreatListState { get; }

        /// <summary>
        ///     Create a Full Hash Query.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="Browsing.ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to query.
        /// </param>
        /// <param name="threatListState">
        ///     The state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatListDescriptor" />.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference, or if
        ///     <paramref name="threatListState" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatListState" /> is not hexadecimal encoded.
        /// </exception>
        public FullHashQuery(ThreatListDescriptor threatListDescriptor, string threatListState) {
            Guard.ThrowIf(nameof(threatListDescriptor), threatListDescriptor).Null();

            this.ThreatListDescriptor = threatListDescriptor;
            this.ThreatListState = CreateThreatListState(threatListState);

            // <summary>
            //      Create Threat List State.
            // </summary>
            string CreateThreatListState(string cThreatListState) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cIsThreatListStateHexadecimalEncoded = cThreatListState.IsHexadecimalEncoded();
                if (!cIsThreatListStateHexadecimalEncoded) {
                    var cDetailMessage = $"A threat list state ({cThreatListState}) is not hexadecimal encoded.";
                    throw new FormatException(cDetailMessage);
                }

                return cThreatListState;
            }
        }

        /// <summary>
        ///     Create a Full Hash Query.
        /// </summary>
        /// <param name="threatType">
        ///     A <see cref="ThreatType" /> identifying the <see cref="ThreatList" /> to query.
        /// </param>
        /// <param name="platformType">
        ///     A <see cref="PlatformType" /> identifying the <see cref="ThreatList" /> to query.
        /// </param>
        /// <param name="threatEntryType">
        ///     A <see cref="ThreatEntryType" /> identifying the <see cref="ThreatList" /> to query.
        /// </param>
        /// <param name="threatListState">
        ///     The state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatType" />, <paramref name="platformType" />, and
        ///     <paramref name="threatEntryType" />.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListState" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatListState" /> is not hexadecimal encoded.
        /// </exception>
        public FullHashQuery(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType, string threatListState) {
            this.ThreatListDescriptor = new ThreatListDescriptor(threatType, platformType, threatEntryType);
            this.ThreatListState = CreateThreatListState(threatListState);

            // <summary>
            //      Create Threat List State.
            // </summary>
            string CreateThreatListState(string cThreatListState) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cIsThreatListStateHexadecimalEncoded = cThreatListState.IsHexadecimalEncoded();
                if (!cIsThreatListStateHexadecimalEncoded) {
                    var cDetailMessage = $"A threat list state ({cThreatListState}) is not hexadecimal encoded.";
                    throw new FormatException(cDetailMessage);
                }

                return cThreatListState;
            }
        }

        /// <summary>
        ///     Determine if This Object is Equal to Another Object.
        /// </summary>
        /// <param name="object">
        ///     An object to compare to.
        /// </param>
        /// <returns>
        ///     A boolean true if this object is equal to <paramref name="object" />. A boolean false otherwise.
        /// </returns>
        public override bool Equals(object @object) {
            var isEqual = @object != null &&
                          @object is FullHashQuery fullHashQuery &&
                          object.Equals(this.ThreatListDescriptor, fullHashQuery.ThreatListDescriptor) &&
                          this.ThreatListState == fullHashQuery.ThreatListState;

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
                hashCode = hashCode * 7 + this.ThreatListDescriptor.GetHashCode();
                hashCode = hashCode * 7 + this.ThreatListState.GetHashCode();

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
            var @string = this.ThreatListDescriptor.ToString();
            return @string;
        }
    }
}