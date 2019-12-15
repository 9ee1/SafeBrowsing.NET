using Gee.Common;
using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Full Hash Request.
    /// </summary>
    public sealed class FullHashRequest {
        /// <summary>
        ///     Get Client Metadata.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="Clients.ClientMetadata" /> of the client making the full hash request.
        /// </remarks>
        public ClientMetadata ClientMetadata { get; }

        /// <summary>
        ///     Get Queries.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="FullHashQuery" /> indicating the collection of
        ///     <see cref="ThreatList" /> the threats identified by <see cref="Sha256HashPrefixes" /> are associated
        ///     with.
        /// </remarks>
        public IEnumerable<FullHashQuery> Queries { get; }

        /// <summary>
        ///     Get and Set SHA256 Hash Prefixes.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings,
        ///     identifying the threats to query.
        /// </remarks>
        public IEnumerable<string> Sha256HashPrefixes { get; }

        /// <summary>
        ///     Build a Full Hash Request.
        /// </summary>
        /// <returns>
        ///     A <see cref="FullHashRequestBuilder" /> to build a full hash request with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FullHashRequestBuilder Build() {
            return new FullHashRequestBuilder();
        }

        /// <summary>
        ///     Create a Full Hash Request.
        /// </summary>
        /// <param name="sha256HashPrefixes">
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the
        ///     threats to query.
        /// </param>
        /// <param name="queries">
        ///     A collection of <see cref="FullHashQuery" /> indicating the collection of <see cref="ThreatList" />
        ///     the threats identified by <paramref name="sha256HashPrefixes" /> are associated with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="queries" /> is a null reference, or if <paramref name="sha256HashPrefixes" />
        ///     is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="sha256HashPrefixes" /> contains a SHA256 hash prefix that is not hexadecimal
        ///     encoded.
        /// </exception>
        public FullHashRequest(IEnumerable<string> sha256HashPrefixes, IEnumerable<FullHashQuery> queries) : this(sha256HashPrefixes, queries, null) { }

        /// <summary>
        ///     Create a Full Hash Request.
        /// </summary>
        /// <param name="sha256HashPrefixes">
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the
        ///     threats to query.
        /// </param>
        /// <param name="queries">
        ///     A collection of <see cref="FullHashQuery" /> indicating the collection of <see cref="ThreatList" />
        ///     the threats identified by <paramref name="sha256HashPrefixes" /> are associated with.
        /// </param>
        /// <param name="clientMetadata">
        ///     The <see cref="Clients.ClientMetadata" /> of the client making the full hash request. A null reference
        ///     indicates the metadata of the client is unknown.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="queries" /> is a null reference, or if <paramref name="sha256HashPrefixes" />
        ///     is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="sha256HashPrefixes" /> contains a SHA256 hash prefix that is not hexadecimal
        ///     encoded.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public FullHashRequest(IEnumerable<string> sha256HashPrefixes, IEnumerable<FullHashQuery> queries, ClientMetadata clientMetadata) {
            Guard.ThrowIf(nameof(sha256HashPrefixes), sha256HashPrefixes).Null();

            this.ClientMetadata = clientMetadata ?? ClientMetadata.Default;
            this.Queries = new HashSet<FullHashQuery>(queries);
            this.Sha256HashPrefixes = CreateSha256HashPrefixes(sha256HashPrefixes);

            // <summary>
            //      Create SHA256 Hash Prefixes.
            // </summary>
            IEnumerable<string> CreateSha256HashPrefixes(IEnumerable<string> cSha256HashPrefixes) {
                var cNewSha256HashPrefixes = new HashSet<string>();
                foreach (var cSha256HashPrefix in cSha256HashPrefixes) {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var cIsSha256HashPrefixHexadecimalEncoded = cSha256HashPrefix.IsHexadecimalEncoded();
                    if (!cIsSha256HashPrefixHexadecimalEncoded) {
                        var cDetailMessage = $"A SHA256 hash prefix ({cSha256HashPrefix}) is not hexadecimal encoded.";
                        throw new FormatException(cDetailMessage);
                    }

                    cNewSha256HashPrefixes.Add(cSha256HashPrefix);
                }

                return cNewSha256HashPrefixes;
            }
        }

        /// <summary>
        ///     Create a Full Hash Request.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="FullHashRequestBuilder" /> to initialize the full hash request with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal FullHashRequest(FullHashRequestBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();

            this.ClientMetadata = builder.ClientMetadata;
            this.Queries = builder.Queries;
            this.Sha256HashPrefixes = builder.Sha256HashPrefixes;
        }
    }
}