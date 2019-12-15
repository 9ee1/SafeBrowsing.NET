using Gee.Common.Guards;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Full Hash Response Builder.
    /// </summary>
    public sealed class FullHashResponseBuilder {
        /// <summary>
        ///     Get and Set Request.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="FullHashRequest" /> made to the Google Safe Browsing API for which the full
        ///     hash response has been returned.
        /// </remarks>
        internal FullHashRequest Request { get; private set; }

        /// <summary>
        ///     Get and Set Safe Threats Expiration Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), safe threats should be considered safe to.
        /// </remarks>
        internal DateTime SafeThreatsExpirationDate { get; private set; }

        /// <summary>
        ///     Get and Set Unsafe Threats.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="UnsafeThreat" />. An empty collection indicates no threats were
        ///     determined to be unsafe.
        /// </remarks>
        internal HashSet<UnsafeThreat> UnsafeThreats { get; private set; }

        /// <summary>
        ///     Get and Set Wait to Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), a client must wait to before issuing another
        ///     <see cref="FullHashRequest" /> to the Google Safe Browsing API. A null reference indicates a client
        ///     does not have to wait.
        /// </remarks>
        internal DateTime? WaitToDate { get; private set; }

        /// <summary>
        ///     Create a Full Hash Response Builder.
        /// </summary>
        internal FullHashResponseBuilder() {
            this.UnsafeThreats = new HashSet<UnsafeThreat>();
        }

        /// <summary>
        ///     Add an Unsafe Threat.
        /// </summary>
        /// <param name="value">
        ///     An <see cref="UnsafeThreat" />.
        /// </param>
        /// <returns>
        ///     This full hash response builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public FullHashResponseBuilder AddUnsafeThreat(UnsafeThreat value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.UnsafeThreats.Add(value);
            return this;
        }

        /// <summary>
        ///     Add an Unsafe Threat.
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
        /// <returns>
        ///     This full hash response builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="sha256Hash" /> is a null reference, or if
        ///     <paramref name="associatedThreatListDescriptor" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="sha256Hash" /> is not hexadecimal encoded.
        /// </exception>
        public FullHashResponseBuilder AddUnsafeThreat(string sha256Hash, ThreatListDescriptor associatedThreatListDescriptor, DateTime expirationDate) {
            // ...
            //
            // Throws an exception if the operation fails.
            var unsafeThreat = new UnsafeThreat(sha256Hash, associatedThreatListDescriptor, expirationDate);
            this.UnsafeThreats.Add(unsafeThreat);

            return this;
        }

        /// <summary>
        ///     Build a Full Hash Response.
        /// </summary>
        /// <returns>
        ///     A <see cref="FullHashResponse" />.
        /// </returns>
        public FullHashResponse Build() {
            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            //
            // Throws an exception if the operation fails.
            var fullHashResponse = new FullHashResponse(this);
            this.Request = null;
            this.SafeThreatsExpirationDate = default;
            this.UnsafeThreats = new HashSet<UnsafeThreat>();
            this.WaitToDate = null;

            return fullHashResponse;
        }

        /// <summary>
        ///     Set Request.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="FullHashRequest" /> made to the Google Safe Browsing API for which the full hash
        ///     response has been returned.
        /// </param>
        /// <returns>
        ///     This full hash response builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public FullHashResponseBuilder SetRequest(FullHashRequest value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Request = value;
            return this;
        }

        /// <summary>
        ///     Set Safe Threats Expiration Date.
        /// </summary>
        /// <param name="value">
        ///     The date, in Coordinated Universal Time (UTC), safe threats should be considered safe to. If the date
        ///     is not in UTC, it is converted to it.
        /// </param>
        /// <returns>
        ///     This full hash response builder.
        /// </returns>
        public FullHashResponseBuilder SetSafeThreatsExpirationDate(DateTime value) {
            this.SafeThreatsExpirationDate = value.ToUniversalTime();
            return this;
        }

        /// <summary>
        ///     Set Wait to Date.
        /// </summary>
        /// <param name="value">
        ///     The date, in Coordinated Universal Time (UTC), a client must wait to before issuing another
        ///     <see cref="FullHashRequest" /> to the Google Safe Browsing API. If the date is not in UTC, it is
        ///     converted to it. A null reference indicates a client does not have to wait.
        /// </param>
        /// <returns>
        ///     This full hash response builder.
        /// </returns>
        public FullHashResponseBuilder SetWaitToDate(DateTime? value) {
            this.WaitToDate = value?.ToUniversalTime();
            return this;
        }
    }
}