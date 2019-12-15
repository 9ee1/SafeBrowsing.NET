using Gee.Common.Guards;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Request.
    /// </summary>
    public sealed class ThreatListUpdateRequest {
        /// <summary>
        ///     Get Client Metadata.
        /// </summary>
        /// <remarks>
        ///     Represents the metadata of the client making the threat list update request.
        /// </remarks>
        public ClientMetadata ClientMetadata { get; }

        /// <summary>
        ///     Get Queries.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="ThreatListUpdateQuery" /> indicating the collection of
        ///     <see cref="ThreatList" /> to retrieve.
        /// </remarks>
        public IEnumerable<ThreatListUpdateQuery> Queries { get; }

        /// <summary>
        ///     Build a Threat List Update Request.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateRequestBuilder" /> to build a threat list update request with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ThreatListUpdateRequestBuilder Build() {
            return new ThreatListUpdateRequestBuilder();
        }

        /// <summary>
        ///     Create a Threat List Update Request.
        /// </summary>
        /// <param name="queries">
        ///     A collection of <see cref="ThreatListUpdateQuery" /> indicating the collection of
        ///     <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="queries" /> is a null reference.
        /// </exception>
        public ThreatListUpdateRequest(IEnumerable<ThreatListUpdateQuery> queries) : this(queries, null) { }

        /// <summary>
        ///     Create a Threat List Update Request.
        /// </summary>
        /// <param name="queries">
        ///     A collection of <see cref="ThreatListUpdateQuery" /> indicating the collection of
        ///     <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="clientMetadata">
        ///     The metadata of the client making the threat list update request. A null reference indicates the
        ///     client's metadata is unknown.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="queries" /> is a null reference.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ThreatListUpdateRequest(IEnumerable<ThreatListUpdateQuery> queries, ClientMetadata clientMetadata) {
            Guard.ThrowIf(nameof(queries), queries).Null();

            this.ClientMetadata = clientMetadata ?? ClientMetadata.Default;
            this.Queries = new HashSet<ThreatListUpdateQuery>(queries);
        }

        /// <summary>
        ///     Create a Threat List Update Request.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="ThreatListUpdateRequestBuilder" /> to initialize the threat list update request with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal ThreatListUpdateRequest(ThreatListUpdateRequestBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();

            this.ClientMetadata = builder.ClientMetadata;
            this.Queries = builder.Queries;
        }
    }
}