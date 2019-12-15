using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Response.
    /// </summary>
    public sealed class ThreatListUpdateResponse {
        /// <summary>
        ///     Get Request.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListUpdateRequest" /> made to the Google Safe Browsing API for which
        ///     the threat list update response has been returned.
        /// </remarks>
        public ThreatListUpdateRequest Request { get; }

        /// <summary>
        ///     Get Results.
        /// </summary>
        /// <remarks>
        ///     Represents a collection of <see cref="ThreatListUpdateResult" /> indicating the collection of
        ///     retrieved <see cref="ThreatList" /> and the threats associated with them that should be added to and
        ///     removed from the locally stored copies of the threat lists. An empty collection indicates no threat
        ///     lists were retrieved.
        /// </remarks>
        public IReadOnlyCollection<ThreatListUpdateResult> Results { get; }

        /// <summary>
        ///     Build a Threat List Update Response.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResponseBuilder" /> to build a threat list update response with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ThreatListUpdateResponseBuilder Build() {
            return new ThreatListUpdateResponseBuilder();
        }

        /// <summary>
        ///     Create a Threat List Update Response.
        /// </summary>
        /// <param name="request">
        ///     The <see cref="ThreatListUpdateRequest" /> made to the Google Safe Browsing API for which the threat
        ///     list update response has been returned.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="request" /> is a null reference.
        /// </exception>
        public ThreatListUpdateResponse(ThreatListUpdateRequest request) : this(request, null) { }

        /// <summary>
        ///     Create a Threat List Update Response.
        /// </summary>
        /// <param name="request">
        ///     The <see cref="ThreatListUpdateRequest" /> made to the Google Safe Browsing API for which the threat
        ///     list update response has been returned.
        /// </param>
        /// <param name="results">
        ///     A collection of <see cref="ThreatListUpdateResult" /> indicating the collection of retrieved
        ///     <see cref="ThreatList" /> and the threats associated with them that should be added to and removed
        ///     from the locally stored copies of the threat lists. A null reference or an empty collection indicates
        ///     no threat lists were retrieved.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="request" /> is a null reference.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ThreatListUpdateResponse(ThreatListUpdateRequest request, IEnumerable<ThreatListUpdateResult> results) {
            Guard.ThrowIf(nameof(request), request).Null();

            this.Request = request;
            this.Results = results != null ? results.ToArray() : Array.Empty<ThreatListUpdateResult>();
        }

        /// <summary>
        ///     Create a Threat List Update Response.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="ThreatListUpdateResponseBuilder" /> to initialize the threat list update response with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal ThreatListUpdateResponse(ThreatListUpdateResponseBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();
            Guard.ThrowIf(nameof(builder), builder.Request).Null();

            this.Request = builder.Request;
            this.Results = builder.Results;
        }
    }
}