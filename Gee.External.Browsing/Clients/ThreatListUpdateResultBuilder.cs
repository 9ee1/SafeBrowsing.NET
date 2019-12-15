using Gee.Common;
using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Result Builder.
    /// </summary>
    public sealed class ThreatListUpdateResultBuilder {
        /// <summary>
        ///     Get and Set Query.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListUpdateQuery" /> made to the Google Safe Browsing API for which the
        ///     threat list update result has been returned.
        /// </remarks>
        internal ThreatListUpdateQuery Query { get; private set; }

        /// <summary>
        ///     Get and Set Retrieved Threat List.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatList" /> retrieved from the Google Safe Browsing API.
        /// </remarks>
        internal ThreatList RetrievedThreatList { get; private set; }

        /// <summary>
        ///     Get and Set Retrieved Threat List's Checksum.
        /// </summary>
        /// <remarks>
        ///     Represents the checksum, formatted as a hexadecimal encoded string, of the lexicographically sorted
        ///     <see cref="RetrievedThreatList" />.
        /// </remarks>
        internal string RetrievedThreatListChecksum { get; private set; }

        /// <summary>
        ///     Get and Set Threats to Add.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings,
        ///     identifying the threats associated with the <see cref="RetrievedThreatList" /> and should be added to
        ///     the locally stored copy of the <see cref="ThreatList" />. An empty collection indicates there are no
        ///     threats to add.
        /// </remarks>
        internal List<string> ThreatsToAdd { get; private set; }

        /// <summary>
        ///     Get and Set Threats to Remove.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of zero-based indices identifying the threats associated with the
        ///     lexicographically sorted <see cref="RetrievedThreatList" /> and should be removed from the locally
        ///     stored copy of the <see cref="ThreatList" />. An empty collection indicates there are no threats to
        ///     remove.
        /// </remarks>
        internal List<int> ThreatsToRemove { get; private set; }

        /// <summary>
        ///     Get and Set Update Type.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListUpdateType" /> indicating how the
        ///     <see cref="RetrievedThreatList" /> was retrieved.
        /// </remarks>
        internal ThreatListUpdateType UpdateType { get; private set; }

        /// <summary>
        ///     Create a Threat List Update Result Builder.
        /// </summary>
        internal ThreatListUpdateResultBuilder() {
            this.ThreatsToAdd = new List<string>();
            this.ThreatsToRemove = new List<int>();
        }

        /// <summary>
        ///     Add a Threat to Add.
        /// </summary>
        /// <param name="value">
        ///     A SHA256 hash prefix, formatted as hexadecimal encoded string, identifying a threat associated with
        ///     the retrieved <see cref="ThreatList" /> and should be added to the locally stored copy of the threat
        ///     list.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="value" /> is not hexadecimal encoded.
        /// </exception>
        public ThreatListUpdateResultBuilder AddThreatToAdd(string value) {
            // ...
            //
            // Throws an exception if the operation fails.
            var isValueHexadecimalEncoded = value.IsHexadecimalEncoded();
            if (!isValueHexadecimalEncoded) {
                var detailMessage = $"A value ({value}) is not hexadecimal encoded.";
                throw new FormatException(detailMessage);
            }

            this.ThreatsToAdd.Add(value);
            return this;
        }

        /// <summary>
        ///     Add a Threat to Remove.
        /// </summary>
        /// <param name="value">
        ///     A zero-based index identifying a threat associated with the lexicographically sorted retrieved
        ///     <see cref="ThreatList" /> and should be removed from the locally stored copy of the threat list.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="value" /> is less than <c>0</c>.
        /// </exception>
        public ThreatListUpdateResultBuilder AddThreatToRemove(int value) {
            Guard.ThrowIf(nameof(value), value).LessThan(0);

            this.ThreatsToRemove.Add(value);
            return this;
        }

        /// <summary>
        ///     Build a Threat List Update Result.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResult" />.
        /// </returns>
        public ThreatListUpdateResult Build() {
            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateResult = new ThreatListUpdateResult(this);

            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            this.Query = null;
            this.RetrievedThreatList = null;
            this.RetrievedThreatListChecksum = null;
            this.ThreatsToAdd = new List<string>();
            this.ThreatsToRemove = new List<int>();
            this.UpdateType = default;

            return threatListUpdateResult;
        }

        /// <summary>
        ///     Set Query.
        /// </summary>
        /// <param name="valueAction">
        ///     An action to create the <see cref="ThreatListUpdateQuery" /> made to the Google Safe Browsing API for
        ///     which the threat list update result has been returned.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="valueAction" /> is a null reference.
        /// </exception>
        public ThreatListUpdateResultBuilder SetQuery(Func<ThreatListUpdateQueryBuilder, ThreatListUpdateQuery> valueAction) {
            Guard.ThrowIf(nameof(valueAction), valueAction).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateQueryBuilder = ThreatListUpdateQuery.Build();
            var threatListUpdateQuery = valueAction(threatListUpdateQueryBuilder);
            this.SetQuery(threatListUpdateQuery);

            return this;
        }

        /// <summary>
        ///     Set Query.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="ThreatListUpdateQuery" /> made to the Google Safe Browsing API for which the threat
        ///     list update result has been returned.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public ThreatListUpdateResultBuilder SetQuery(ThreatListUpdateQuery value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Query = value;
            return this;
        }

        /// <summary>
        ///     Set Retrieved Threat List.
        /// </summary>
        /// <param name="valueAction">
        ///     An action to create the <see cref="ThreatList" /> retrieved from the Google Safe Browsing API.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="valueAction" /> is a null reference.
        /// </exception>
        public ThreatListUpdateResultBuilder SetRetrievedThreatList(Func<ThreatListBuilder, ThreatList> valueAction) {
            Guard.ThrowIf(nameof(valueAction), valueAction).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListBuilder = ThreatList.Build();
            var threatList = valueAction(threatListBuilder);
            this.SetRetrievedThreatList(threatList);

            return this;
        }

        /// <summary>
        ///     Set Retrieved Threat List.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="ThreatList" /> retrieved from the Google Safe Browsing API.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public ThreatListUpdateResultBuilder SetRetrievedThreatList(ThreatList value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.RetrievedThreatList = value;
            return this;
        }

        /// <summary>
        ///     Set Retrieved Threat List's Checksum.
        /// </summary>
        /// <param name="value">
        ///     The checksum, formatted as a hexadecimal encoded string, of the lexicographically sorted
        ///     retrieved <see cref="ThreatList" />.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="value" /> is not hexadecimal encoded.
        /// </exception>
        public ThreatListUpdateResultBuilder SetRetrievedThreatListChecksum(string value) {
            // ...
            //
            // Throws an exception if the operation fails.
            var isValueHexadecimalEncoded = value.IsHexadecimalEncoded();
            if (!isValueHexadecimalEncoded) {
                var detailMessage = $"A value ({value}) is not hexadecimal encoded.";
                throw new FormatException(detailMessage);
            }

            this.RetrievedThreatListChecksum = value;
            return this;
        }

        /// <summary>
        ///     Set Threats to Add.
        /// </summary>
        /// <param name="value">
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the
        ///     threats associated with the retrieved <see cref="ThreatList" /> and should be added to the locally
        ///     stored copy of the threat list. An empty collection indicates there are no threats to add.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="value" /> contains a value that is not hexadecimal encoded.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ThreatListUpdateResultBuilder SetThreatsToAdd(IEnumerable<string> value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.ThreatsToAdd = new List<string>();
            foreach (var element in value) {
                // ...
                //
                // Throws an exception if the operation fails.
                this.AddThreatToAdd(element);
            }

            return this;
        }

        /// <summary>
        ///     Set Threats to Remove.
        /// </summary>
        /// <param name="value">
        ///     A collection of zero-based indices identifying the threats associated with the lexicographically
        ///     sorted retrieved <see cref="ThreatList" /> and should be removed from the locally stored copy of the
        ///     threat list. An empty collection indicates there are no threats to remove. 
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="value" /> contains a value that is less than <c>0</c>.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ThreatListUpdateResultBuilder SetThreatsToRemove(IEnumerable<int> value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.ThreatsToRemove = new List<int>();
            foreach (var element in value) {
                // ...
                //
                // Throws an exception if the operation fails.
                this.AddThreatToRemove(element);
            }

            return this;
        }

        /// <summary>
        ///     Set Update Type.
        /// </summary>
        /// <param name="value">
        ///     An <see cref="ThreatListUpdateType" /> indicating how the retrieved <see cref="ThreatList" /> was
        ///     retrieved.
        /// </param>
        /// <returns>
        ///     This threat list update result builder.
        /// </returns>
        public ThreatListUpdateResultBuilder SetUpdateType(ThreatListUpdateType value) {
            this.UpdateType = value;
            return this;
        }
    }
}