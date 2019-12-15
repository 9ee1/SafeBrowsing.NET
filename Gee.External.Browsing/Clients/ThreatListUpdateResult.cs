using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Result.
    /// </summary>
    public sealed class ThreatListUpdateResult {
        /// <summary>
        ///     Threats to Remove.
        /// </summary>
        private readonly IReadOnlyCollection<int> _threatsToRemove;

        /// <summary>
        ///     Determine if Threat List Was Retrieved as a Full Update.
        /// </summary>
        /// <remarks>
        ///     Determines if the <see cref="RetrievedThreatList" /> was retrieved from the Google Safe Browsing API
        ///     as a <see cref="ThreatListUpdateType.Full" /> update.
        /// </remarks>
        public bool IsFullUpdate => this.UpdateType == ThreatListUpdateType.Full;

        /// <summary>
        ///     Determine if Threat List Was Retrieved as a Partial Update.
        /// </summary>
        /// <remarks>
        ///     Determines if the <see cref="RetrievedThreatList" /> was retrieved from the Google Safe Browsing API
        ///     as a <see cref="ThreatListUpdateType.Partial" /> update.
        /// </remarks>
        public bool IsPartialUpdate => this.UpdateType == ThreatListUpdateType.Partial;

        /// <summary>
        ///     Get Query.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListUpdateQuery" /> made to the Google Safe Browsing API for which the
        ///     threat list update result has been returned.
        /// </remarks>
        public ThreatListUpdateQuery Query { get; }

        /// <summary>
        ///     Get Retrieved Threat List.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatList" /> retrieved from the Google Safe Browsing API.
        /// </remarks>
        public ThreatList RetrievedThreatList { get; }

        /// <summary>
        ///     Get Retrieved Threat List's Checksum.
        /// </summary>
        /// <remarks>
        ///     Represents the checksum, formatted as a hexadecimal encoded string, of the lexicographically sorted
        ///     <see cref="RetrievedThreatList" />.
        /// </remarks>
        public string RetrievedThreatListChecksum { get; }

        /// <summary>
        ///     Get Threats to Add.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings,
        ///     identifying the threats associated with the <see cref="RetrievedThreatList" /> and should be added to
        ///     the locally stored copy of the <see cref="ThreatList" />. An empty collection indicates there are no
        ///     threats to add.
        /// </remarks>
        public IReadOnlyCollection<string> ThreatsToAdd { get; }

        /// <summary>
        ///     Get Threats to Remove.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of zero-based indices identifying the threats associated with the
        ///     lexicographically sorted <see cref="RetrievedThreatList" /> and should be removed from the locally
        ///     stored copy of the <see cref="ThreatList" /> if, and only if, the threat list was retrieved as a
        ///     <see cref="ThreatListUpdateType.Partial" /> update. To determine if the threat list was retrieved as a
        ///     partial update, call <see cref="IsPartialUpdate" />. An empty collection indicates there are no
        ///     threats to remove.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if the threat list was not retrieved as a <see cref="ThreatListUpdateType.Partial" /> update.
        /// </exception>
        public IReadOnlyCollection<int> ThreatsToRemove {
            get {
                if (!this.IsPartialUpdate) {
                    var detailMessage = $"An operation ({nameof(ThreatListUpdateResult.ThreatsToRemove)}) is invalid.";
                    throw new InvalidOperationException(detailMessage);
                }

                return this._threatsToRemove;
            }
        }

        /// <summary>
        ///     Get Update Type.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListUpdateType" /> indicating how the
        ///     <see cref="RetrievedThreatList" /> was retrieved from the Google Safe Browsing API. You can also
        ///     conveniently use <see cref="IsFullUpdate" /> and <see cref="IsPartialUpdate" />.
        /// </remarks>
        public ThreatListUpdateType UpdateType { get; }

        /// <summary>
        ///     Build a Threat List Update Result.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResultBuilder" /> to build a threat list update result with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ThreatListUpdateResultBuilder Build() {
            return new ThreatListUpdateResultBuilder();
        }

        /// <summary>
        ///     Create a Threat List Update Result.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="ThreatListUpdateResultBuilder" /> to initialize the threat list update result with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal ThreatListUpdateResult(ThreatListUpdateResultBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();
            Guard.ThrowIf(nameof(builder), builder.RetrievedThreatListChecksum).Null();
            Guard.ThrowIf(nameof(builder), builder.Query).Null();
            Guard.ThrowIf(nameof(builder), builder.RetrievedThreatList).Null();

            this.Query = builder.Query;
            this.RetrievedThreatList = builder.RetrievedThreatList;
            this.RetrievedThreatListChecksum = builder.RetrievedThreatListChecksum;
            this.ThreatsToAdd = builder.ThreatsToAdd;
            this.UpdateType = builder.UpdateType;
            // ...
            //
            // ...
            this._threatsToRemove = CreateThreatsToRemove(this, builder);

            // <summary>
            //      Create Threats to Remove.
            // </summary>
            IReadOnlyCollection<int> CreateThreatsToRemove(ThreatListUpdateResult @this, ThreatListUpdateResultBuilder cBuilder) {
                IReadOnlyCollection<int> cThreatsToRemove = Array.Empty<int>();
                if (@this.IsPartialUpdate) {
                    cThreatsToRemove = cBuilder.ThreatsToRemove;
                }

                return cThreatsToRemove;
            }
        }
    }
}