using Gee.Common;
using Gee.Common.Guards;
using System;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Query Builder.
    /// </summary>
    public sealed class ThreatListUpdateQueryBuilder {
        /// <summary>
        ///     Get and Set Threat List Descriptor.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="Browsing.ThreatListDescriptor" /> identifying the <see cref="ThreatList" />
        ///     to retrieve.
        /// </remarks>
        internal ThreatListDescriptor ThreatListDescriptor { get; private set; }

        /// <summary>
        ///     Get and Set Threat List State.
        /// </summary>
        /// <remarks>
        ///     Represents the state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" />
        ///     identified by <see cref="ThreatListDescriptor" />. A null reference indicates the state of the threat
        ///     list is unknown and will force the threat list to be retrieved as a
        ///     <see cref="ThreatListUpdateType.Full" /> update.
        /// </remarks>
        internal string ThreatListState { get; private set; }

        /// <summary>
        ///     Get and Set Update Constraints.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListUpdateConstraints" /> to apply when the <see cref="ThreatList" />
        ///     identified by <see cref="ThreatListDescriptor" /> is retrieved. A null reference indicates no update
        ///     constraints should be applied.
        /// </remarks>
        internal ThreatListUpdateConstraints UpdateConstraints { get; private set; }

        /// <summary>
        ///     Build a Threat List Update Query.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateQuery" />.
        /// </returns>
        public ThreatListUpdateQuery Build() {
            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateQuery = new ThreatListUpdateQuery(this);

            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            this.ThreatListDescriptor = null;
            this.ThreatListState = null;
            this.UpdateConstraints = null;

            return threatListUpdateQuery;
        }

        /// <summary>
        ///     Set Threat List Descriptor.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="Browsing.ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <returns>
        ///     This threat list update query builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public ThreatListUpdateQueryBuilder SetThreatListDescriptor(ThreatListDescriptor value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.ThreatListDescriptor = value;
            return this;
        }

        /// <summary>
        ///     Set Threat List Descriptor.
        /// </summary>
        /// <param name="threatType">
        ///     A <see cref="ThreatType" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="platformType">
        ///     A <see cref="PlatformType" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="threatEntryType">
        ///     A <see cref="ThreatEntryType" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <returns>
        ///     This threat list update query builder.
        /// </returns>
        public ThreatListUpdateQueryBuilder SetThreatListDescriptor(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType) {
            this.ThreatListDescriptor = new ThreatListDescriptor(threatType, platformType, threatEntryType);
            return this;
        }

        /// <summary>
        ///     Set Threat List State.
        /// </summary>
        /// <param name="value">
        ///     The state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> to retrieve.
        ///     This should be the value returned by the Google Safe Browsing API when the threat list was most
        ///     recently retrieved. An invalid state will be ignored by the Google Safe Browsing API and will force
        ///     the threat list to be retrieved as a <see cref="ThreatListUpdateType.Full" /> update. A null reference
        ///     indicates the state of the threat list is unknown and will force the threat list to be retrieved as a
        ///     <see cref="ThreatListUpdateType.Full" /> update.
        /// </param>
        /// <returns>
        ///     This threat list update query builder.
        /// </returns>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="value" /> is not a null reference and it is not hexadecimal encoded.
        /// </exception>
        public ThreatListUpdateQueryBuilder SetThreatListState(string value) {
            if (value != null) {
                var isValueHexadecimalEncoded = value.IsHexadecimalEncoded();
                if (!isValueHexadecimalEncoded) {
                    var detailMessage = $"A value ({value}) is not hexadecimal encoded.";
                    throw new FormatException(detailMessage);
                }
            }

            this.ThreatListState = value;
            return this;
        }

        /// <summary>
        ///     Set Update Constraints.
        /// </summary>
        /// <param name="valueAction">
        ///     An action to create the <see cref="ThreatListUpdateConstraints" /> to apply when the
        ///     <see cref="ThreatList" /> is retrieved.
        /// </param>
        /// <returns>
        ///     This threat list update query builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="valueAction" /> is a null reference.
        /// </exception>
        public ThreatListUpdateQueryBuilder SetUpdateConstraints(Func<ThreatListUpdateConstraintsBuilder, ThreatListUpdateConstraints> valueAction) {
            Guard.ThrowIf(nameof(valueAction), valueAction).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateConstraintsBuilder = ThreatListUpdateConstraints.Build();
            var threatListUpdateConstraints = valueAction(threatListUpdateConstraintsBuilder);
            this.SetUpdateConstraints(threatListUpdateConstraints);

            return this;
        }

        /// <summary>
        ///     Set Update Constraints.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="ThreatListUpdateConstraints" /> to apply when the <see cref="ThreatList" /> is
        ///     retrieved. A null reference indicates no update constraints should be applied.
        /// </param>
        /// <returns>
        ///     This threat list update query builder.
        /// </returns>
        public ThreatListUpdateQueryBuilder SetUpdateConstraints(ThreatListUpdateConstraints value) {
            this.UpdateConstraints = value;
            return this;
        }
    }
}