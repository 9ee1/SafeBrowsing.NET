using Gee.Common;
using Gee.Common.Guards;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Full Hash Request Builder.
    /// </summary>
    public sealed class FullHashRequestBuilder {
        /// <summary>
        ///     Get and Set Client Metadata.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="Clients.ClientMetadata" /> of the client making the full hash request.
        /// </remarks>
        internal ClientMetadata ClientMetadata { get; private set; }

        /// <summary>
        ///     Get and Set Queries.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="FullHashQuery" /> indicating the collection of
        ///     <see cref="ThreatList" /> the threats identified by <see cref="Sha256HashPrefixes" /> are associated
        ///     with.
        /// </remarks>
        internal HashSet<FullHashQuery> Queries { get; private set; }

        /// <summary>
        ///     Get and Set SHA256 Hash Prefixes.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings,
        ///     identifying the threats to query.
        /// </remarks>
        internal HashSet<string> Sha256HashPrefixes { get; private set; }

        /// <summary>
        ///     Create a Full Hash Request Builder.
        /// </summary>
        internal FullHashRequestBuilder() {
            this.ClientMetadata = ClientMetadata.Default;
            this.Queries = new HashSet<FullHashQuery>();
            this.Sha256HashPrefixes = new HashSet<string>();
        }

        /// <summary>
        ///     Add a Query.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="FullHashQuery" /> indicating a <see cref="ThreatList" /> to query.
        /// </param>
        /// <returns>
        ///     This full hash request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public FullHashRequestBuilder AddQuery(FullHashQuery value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Queries.Add(value);
            return this;
        }

        /// <summary>
        ///     Add a Query.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to query.
        /// </param>
        /// <param name="threatListState">
        ///     The state, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatListDescriptor" />.
        /// </param>
        /// <returns>
        ///     This full hash request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference, or if
        ///     <paramref name="threatListState" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatListState" /> is not hexadecimal encoded.
        /// </exception>
        public FullHashRequestBuilder AddQuery(ThreatListDescriptor threatListDescriptor, string threatListState) {
            // ...
            //
            // Throws an exception if the operation fails.
            var query = new FullHashQuery(threatListDescriptor, threatListState);

            this.Queries.Add(query);
            return this;
        }

        /// <summary>
        ///     Add Query.
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
        /// <returns>
        ///     This full hash request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListState" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatListState" /> is not hexadecimal encoded.
        /// </exception>
        public FullHashRequestBuilder AddQuery(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType, string threatListState) {
            // ...
            //
            // Throws an exception if the operation fails.
            var query = new FullHashQuery(threatType, platformType, threatEntryType, threatListState);

            this.Queries.Add(query);
            return this;
        }

        /// <summary>
        ///     Add a SHA256 Hash Prefix.
        /// </summary>
        /// <param name="value">
        ///     A SHA256 hash prefix, formatted as hexadecimal encoded string, identifying a threat to query.
        /// </param>
        /// <returns>
        ///     This full hash request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="value" /> is not hexadecimal encoded.
        /// </exception>
        public FullHashRequestBuilder AddSha256HashPrefix(string value) {
            // ...
            //
            // Throws an exception if the operation fails.
            var isValueHexadecimalEncoded = value.IsHexadecimalEncoded();
            if (!isValueHexadecimalEncoded) {
                var detailMessage = $"A value ({value}) is not hexadecimal encoded.";
                throw new FormatException(detailMessage);
            }

            this.Sha256HashPrefixes.Add(value);
            return this;
        }

        /// <summary>
        ///     Build a Full Hash Request.
        /// </summary>
        /// <returns>
        ///     A <see cref="FullHashRequest" />.
        /// </returns>
        public FullHashRequest Build() {
            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            var fullHashRequest = new FullHashRequest(this);
            this.ClientMetadata = ClientMetadata.Default;
            this.Queries = new HashSet<FullHashQuery>();
            this.Sha256HashPrefixes = new HashSet<string>();

            return fullHashRequest;
        }

        /// <summary>
        ///     Set Client Metadata.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="Clients.ClientMetadata" /> of the client making the full hash request. A null reference
        ///     indicates the metadata of the client is unknown.
        /// </param>
        /// <returns>
        ///     This full hash request builder.
        /// </returns>
        public FullHashRequestBuilder SetClientMetadata(ClientMetadata value) {
            this.ClientMetadata = value ?? ClientMetadata.Default;
            return this;
        }

        /// <summary>
        ///     Set Client Metadata.
        /// </summary>
        /// <param name="id">
        ///     A unique identifier identifying the client.
        /// </param>
        /// <param name="majorVersion">
        ///     The client's major version.
        /// </param>
        /// <param name="minorVersion">
        ///     The client's minor version.
        /// </param>
        /// <param name="patchVersion">
        ///     The client's patch version.
        /// </param>
        /// <returns>
        ///     This full hash request builder.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="id" /> consists exclusively of whitespace characters.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="id" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="majorVersion" /> is less than <c>0</c>, or if
        ///     <paramref name="minorVersion" /> is less than <c>0</c>, or if <paramref name="patchVersion" /> is less
        ///     than <c>0</c>.
        /// </exception>
        public FullHashRequestBuilder SetClientMetadata(string id, int majorVersion, int minorVersion, int patchVersion) {
            // ...
            //
            // Throws an exception if the operation fails.
            this.ClientMetadata = new ClientMetadata(id, majorVersion, minorVersion, patchVersion);
            return this;
        }
    }
}