using Gee.Common;
using Gee.Common.Guards;
using System;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Query.
    /// </summary>
    public sealed class ThreatListUpdateQuery {
        /// <summary>
        ///     Get Threat List Descriptor.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="Browsing.ThreatListDescriptor" /> identifying the <see cref="ThreatList" />
        ///     to retrieve.
        /// </remarks>
        public ThreatListDescriptor ThreatListDescriptor { get; }

        /// <summary>
        ///     Get Threat List State.
        /// </summary>
        /// <remarks>
        ///     Represents the state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" />
        ///     identified by <see cref="ThreatListDescriptor" />. A null reference indicates the state of the threat
        ///     list is unknown and will force the threat list to be retrieved as a
        ///     <see cref="ThreatListUpdateType.Full" /> update.
        /// </remarks>
        public string ThreatListState { get; }

        /// <summary>
        ///     Get Update Constraints.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListUpdateConstraints" /> to apply when the <see cref="ThreatList" />
        ///     identified by <see cref="ThreatListDescriptor" /> is retrieved. A null reference indicates no update
        ///     constraints should be applied.
        /// </remarks>
        public ThreatListUpdateConstraints UpdateConstraints { get; }

        /// <summary>
        ///     Build a Threat List Update Query.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateQueryBuilder" /> to build the threat list update query with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ThreatListUpdateQueryBuilder Build() {
            return new ThreatListUpdateQueryBuilder();
        }

        /// <summary>
        ///     Create a Threat List Update Query.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="Browsing.ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        public ThreatListUpdateQuery(ThreatListDescriptor threatListDescriptor) : this(threatListDescriptor, null, null) { }

        /// <summary>
        ///     Create a Threat List Update Query.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="Browsing.ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="threatListState">
        ///     The state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatListDescriptor" />. This should be the value returned by the Google Safe Browsing
        ///     API when the threat list was most recently retrieved. An invalid state will be ignored by the Google
        ///     Safe Browsing API and will force the threat list to be retrieved as a
        ///     <see cref="ThreatListUpdateType.Full" /> update. A null reference indicates the state of the threat
        ///     list is unknown and will force the threat list to be retrieved as a
        ///     <see cref="ThreatListUpdateType.Full" /> update.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatListState" /> is not a null reference and it is not hexadecimal
        ///     encoded.
        /// </exception>
        public ThreatListUpdateQuery(ThreatListDescriptor threatListDescriptor, string threatListState) : this(threatListDescriptor, threatListState, null) { }

        /// <summary>
        ///     Create a Threat List Update Query.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="Browsing.ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="threatListState">
        ///     The state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatListDescriptor" />. This should be the value returned by the Google Safe Browsing
        ///     API when the threat list was most recently retrieved. An invalid state will be ignored by the Google
        ///     Safe Browsing API and will force the threat list to be retrieved as a
        ///     <see cref="ThreatListUpdateType.Full" /> update. A null reference indicates the state of the threat
        ///     list is unknown and will force the threat list to be retrieved as a
        ///     <see cref="ThreatListUpdateType.Full" /> update.
        /// </param>
        /// <param name="updateConstraints">
        ///     The <see cref="ThreatListUpdateConstraints" /> to apply when the <see cref="ThreatList" /> identified
        ///     by <paramref name="threatListDescriptor" /> is retrieved. A null reference indicates no update
        ///     constraints should be applied.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatListState" /> is not a null reference and it is not hexadecimal
        ///     encoded.
        /// </exception>
        public ThreatListUpdateQuery(ThreatListDescriptor threatListDescriptor, string threatListState, ThreatListUpdateConstraints updateConstraints) {
            Guard.ThrowIf(nameof(threatListDescriptor), threatListDescriptor).Null();

            this.ThreatListDescriptor = threatListDescriptor;
            this.ThreatListState = CreateThreatListState(threatListState);
            this.UpdateConstraints = updateConstraints;

            // <summary>
            //      Create Threat List State.
            // </summary>
            string CreateThreatListState(string cThreatListState) {
                if (cThreatListState != null) {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var cIsThreatListStateHexadecimalEncoded = cThreatListState.IsHexadecimalEncoded();
                    if (!cIsThreatListStateHexadecimalEncoded) {
                        var cDetailMessage = $"A threat list state ({cThreatListState}) is not hexadecimal encoded.";
                        throw new FormatException(cDetailMessage);
                    }
                }

                return cThreatListState;
            }
        }

        /// <summary>
        ///     Create a Threat List Update Query.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="ThreatListUpdateQueryBuilder" /> to initialize the threat list update query with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal ThreatListUpdateQuery(ThreatListUpdateQueryBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();
            Guard.ThrowIf(nameof(builder), builder.ThreatListDescriptor).Null();

            this.ThreatListDescriptor = builder.ThreatListDescriptor;
            this.ThreatListState = builder.ThreatListState;
            this.UpdateConstraints = builder.UpdateConstraints;
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
                          @object is ThreatListUpdateQuery threatListUpdateQuery &&
                          object.Equals(this.ThreatListDescriptor, threatListUpdateQuery.ThreatListDescriptor) &&
                          this.ThreatListState == threatListUpdateQuery.ThreatListState &&
                          object.Equals(this.UpdateConstraints, threatListUpdateQuery.UpdateConstraints);

            return isEqual;
        }

        /// <summary>
        ///     Get Object's Hash Code.
        /// </summary>
        /// <returns>
        ///     The object's hash code.
        /// </returns>
        public override int GetHashCode() {
            var hashCode = 13;
            hashCode = hashCode * 7 + this.ThreatListDescriptor.GetHashCode();
            hashCode = hashCode * 7 + this.ThreatListState.GetHashCodeOrDefault();
            hashCode = hashCode * 7 + this.UpdateConstraints.GetHashCodeOrDefault();

            return hashCode;
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