using Gee.Common;
using Gee.Common.Guards;
using System;

namespace Gee.External.Browsing {
    /// <summary>
    ///     Threat List Builder.
    /// </summary>
    public sealed class ThreatListBuilder {
        /// <summary>
        ///     Get and Set Threat List's Descriptor.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" />.
        /// </remarks>
        internal ThreatListDescriptor Descriptor { get; private set; }

        /// <summary>
        ///     Get and Set Threat List's Retrieve Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), the <see cref="ThreatList" /> was retrieved
        ///     from the Google Safe Browsing API.
        /// </remarks>
        internal DateTime RetrieveDate { get; private set; }

        /// <summary>
        ///     Get and Set Threat List's State.
        /// </summary>
        /// <remarks>
        ///     Represents the state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> when
        ///     it was retrieved from the Google Safe Browsing API.
        /// </remarks>
        internal string State { get; private set; }

        /// <summary>
        ///     Get and Set Threat List's Wait to Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), a client must wait to before retrieving the
        ///     <see cref="ThreatList" /> from the Google Safe Browsing API again. A null reference indicates a client
        ///     does not need to wait before retrieving the threat list from the Google Safe Browsing API again.
        /// </remarks>
        internal DateTime? WaitToDate { get; private set; }

        /// <summary>
        ///     Create a Threat List Builder.
        /// </summary>
        internal ThreatListBuilder() { }

        /// <summary>
        ///     Build a Threat List.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatList" />.
        /// </returns>
        public ThreatList Build() {
            // ...
            //
            // Throws an exception if the operation fails.
            var threatList = new ThreatList(this);

            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            this.Descriptor = null;
            this.RetrieveDate = default;
            this.State = null;
            this.WaitToDate = default;

            return threatList;
        }

        /// <summary>
        ///     Set Threat List's Descriptor.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" />.
        /// </param>
        /// <returns>
        ///     This threat list builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public ThreatListBuilder SetDescriptor(ThreatListDescriptor value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Descriptor = value;
            return this;
        }

        /// <summary>
        ///     Set Threat List's Descriptor.
        /// </summary>
        /// <param name="threatType">
        ///     A <see cref="ThreatType" /> identifying the <see cref="ThreatList" />.
        /// </param>
        /// <param name="platformType">
        ///     A <see cref="PlatformType" /> identifying the <see cref="ThreatList" />.
        /// </param>
        /// <param name="threatEntryType">
        ///     A <see cref="ThreatEntryType" /> identifying the <see cref="ThreatList" />.
        /// </param>
        /// <returns>
        ///     This threat list builder.
        /// </returns>
        public ThreatListBuilder SetDescriptor(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType) {
            this.Descriptor = new ThreatListDescriptor(threatType, platformType, threatEntryType);
            return this;
        }

        /// <summary>
        ///     Set Threat List's Retrieve Date.
        /// </summary>
        /// <param name="value">
        ///     The date, in Coordinated Universal Time (UTC), the <see cref="ThreatList" /> was retrieved from the
        ///     Google Safe Browsing API. If the date is not expressed in UTC, it is converted to it.
        /// </param>
        /// <returns>
        ///     This threat list builder.
        /// </returns>
        public ThreatListBuilder SetRetrieveDate(DateTime value) {
            this.RetrieveDate = value.ToUniversalTime();
            return this;
        }

        /// <summary>
        ///     Set Threat List's State.
        /// </summary>
        /// <param name="value">
        ///     The state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> when it was
        ///     retrieved from the Google Safe Browsing API.
        /// </param>
        /// <returns>
        ///     This threat list builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="value" /> is not hexadecimal encoded.
        /// </exception>
        public ThreatListBuilder SetState(string value) {
            // ...
            //
            // Throws an exception if the operation fails.
            var isValueHexadecimalEncoded = value.IsHexadecimalEncoded();
            if (!isValueHexadecimalEncoded) {
                var detailMessage = $"A value ({value}) is not hexadecimal encoded.";
                throw new FormatException(detailMessage);
            }

            this.State = value;
            return this;
        }

        /// <summary>
        ///     Set Threat List's Wait to Date.
        /// </summary>
        /// <param name="value">
        ///     The date, in Coordinated Universal Time (UTC), a client must wait to before retrieving the
        ///     <see cref="ThreatList" /> from the Google Safe Browsing API again. If the date is not expressed in
        ///     UTC, it is converted to it. A null reference indicates a client does not need to wait before
        ///     retrieving the threat list from the Google Safe Browsing API again.
        /// </param>
        /// <returns>
        ///     This threat list builder.
        /// </returns>
        public ThreatListBuilder SetWaitToDate(DateTime? value) {
            this.WaitToDate = value?.ToUniversalTime();
            return this;
        }
    }
}