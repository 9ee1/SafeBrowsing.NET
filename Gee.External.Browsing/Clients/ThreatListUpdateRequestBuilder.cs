using Gee.Common.Guards;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Request Builder.
    /// </summary>
    public sealed class ThreatListUpdateRequestBuilder {
        /// <summary>
        ///     Get and Set Client Metadata.
        /// </summary>
        /// <remarks>
        ///     Represents the metadata of the client making the threat list update request.
        /// </remarks>
        internal ClientMetadata ClientMetadata { get; private set; }

        /// <summary>
        ///     Get and Set Queries.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="ThreatListUpdateQuery" /> indicating the collection of
        ///     <see cref="ThreatList" /> to retrieve.
        /// </remarks>
        internal HashSet<ThreatListUpdateQuery> Queries { get; private set; }

        /// <summary>
        ///     Create a Threat List Update Request Builder.
        /// </summary>
        internal ThreatListUpdateRequestBuilder() {
            this.Queries = new HashSet<ThreatListUpdateQuery>();
        }

        /// <summary>
        ///     Add a Query.
        /// </summary>
        /// <param name="valueAction">
        ///     An action to create a <see cref="ThreatListUpdateQuery" /> indicating a <see cref="ThreatList" /> to
        ///     retrieve.
        /// </param>
        /// <returns>
        ///     This threat list update request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="valueAction" /> is a null reference.
        /// </exception>
        public ThreatListUpdateRequestBuilder AddQuery(Func<ThreatListUpdateQueryBuilder, ThreatListUpdateQuery> valueAction) {
            Guard.ThrowIf(nameof(valueAction), valueAction).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateQueryBuilder = ThreatListUpdateQuery.Build();
            var threatListUpdateQuery = valueAction(threatListUpdateQueryBuilder);
            this.AddQuery(threatListUpdateQuery);

            return this;
        }

        /// <summary>
        ///     Add a Query.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="ThreatListUpdateQuery" /> indicating a <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <returns>
        ///     This threat list update request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public ThreatListUpdateRequestBuilder AddQuery(ThreatListUpdateQuery value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Queries.Add(value);
            return this;
        }

        /// <summary>
        ///     Build a Threat List Update Request.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateRequest" />.
        /// </returns>
        public ThreatListUpdateRequest Build() {
            var threatListUpdateRequest = new ThreatListUpdateRequest(this);

            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            this.ClientMetadata = ClientMetadata.Default;
            this.Queries = new HashSet<ThreatListUpdateQuery>();

            return threatListUpdateRequest;
        }

        /// <summary>
        ///     Set Client Metadata.
        /// </summary>
        /// <param name="value">
        ///     The metadata of the client making the threat list update request. A null reference indicates the
        ///     client's metadata is unknown.
        /// </param>
        /// <returns>
        ///     This threat list update request builder.
        /// </returns>
        public ThreatListUpdateRequestBuilder SetClientMetadata(ClientMetadata value) {
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
        ///     This threat list update request builder.
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
        public ThreatListUpdateRequestBuilder SetClientMetadata(string id, int majorVersion, int minorVersion, int patchVersion) {
            // ...
            //
            // Throws an exception if the operation fails.
            this.ClientMetadata = new ClientMetadata(id, majorVersion, minorVersion, patchVersion);
            return this;
        }
    }
}