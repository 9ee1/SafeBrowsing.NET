using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Response Builder.
    /// </summary>
    public sealed class ThreatListUpdateResponseBuilder {
        /// <summary>
        ///     Get and Set Request.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatListUpdateRequest" /> made to the Google Safe Browsing API for which
        ///     the threat list update response has been returned.
        /// </remarks>
        internal ThreatListUpdateRequest Request { get; private set; }

        /// <summary>
        ///     Get and Set Results.
        /// </summary>
        /// <remarks>
        ///     Represents a collection of <see cref="ThreatListUpdateResult" /> indicating the collection of
        ///     retrieved <see cref="ThreatList" /> and the threats associated with them that should be added to and
        ///     removed from the locally stored copies of the threat lists. An empty collection indicates no threat
        ///     lists were retrieved.
        /// </remarks>
        internal List<ThreatListUpdateResult> Results { get; private set; }

        /// <summary>
        ///     Create a Threat List Update Response Builder.
        /// </summary>
        internal ThreatListUpdateResponseBuilder() {
            this.Results = new List<ThreatListUpdateResult>();
        }

        /// <summary>
        ///     Add a Result.
        /// </summary>
        /// <param name="valueAction">
        ///     An action to create a <see cref="ThreatListUpdateResult" /> indicating a retrieved
        ///     <see cref="ThreatList" /> and the threats associated with it that should be added to and removed from
        ///     the locally stored copy of the threat list.
        /// </param>
        /// <returns>
        ///     This threat list update response builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="valueAction" /> is a null reference.
        /// </exception>
        public ThreatListUpdateResponseBuilder AddResult(Func<ThreatListUpdateResultBuilder, ThreatListUpdateResult> valueAction) {
            Guard.ThrowIf(nameof(valueAction), valueAction).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateResultBuilder = ThreatListUpdateResult.Build();
            var threatListUpdateResult = valueAction(threatListUpdateResultBuilder);
            this.AddResult(threatListUpdateResult);

            return this;
        }

        /// <summary>
        ///     Add a Result.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="ThreatListUpdateResult" /> indicating a retrieved <see cref="ThreatList" /> and the
        ///     threats associated with it that should be added to and removed from the locally stored copy of the
        ///     threat list.
        /// </param>
        /// <returns>
        ///     This threat list update response builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        internal ThreatListUpdateResponseBuilder AddResult(ThreatListUpdateResult value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Results.Add(value);
            return this;
        }

        /// <summary>
        ///     Build a Threat List Update Response.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResponse" />.
        /// </returns>
        public ThreatListUpdateResponse Build() {
            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateResponse = new ThreatListUpdateResponse(this);

            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            this.Request = null;
            this.Results = new List<ThreatListUpdateResult>();

            return threatListUpdateResponse;
        }

        /// <summary>
        ///     Set Request.
        /// </summary>
        /// <param name="valueAction">
        ///     An action to create the <see cref="ThreatListUpdateRequest" /> made to the Google Safe Browsing API
        ///     for which the threat list update response has been returned.
        /// </param>
        /// <returns>
        ///     This threat list update response builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="valueAction" /> is a null reference.
        /// </exception>
        public ThreatListUpdateResponseBuilder SetRequest(Func<ThreatListUpdateRequestBuilder, ThreatListUpdateRequest> valueAction) {
            Guard.ThrowIf(nameof(valueAction), valueAction).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateRequestBuilder = ThreatListUpdateRequest.Build();
            var threatListUpdateRequest = valueAction(threatListUpdateRequestBuilder);
            this.SetRequest(threatListUpdateRequest);

            return this;
        }

        /// <summary>
        ///     Set Request.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="ThreatListUpdateRequest" /> made to the Google Safe Browsing API for which the threat
        ///     list update response has been returned.
        /// </param>
        /// <returns>
        ///     This threat list update response builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public ThreatListUpdateResponseBuilder SetRequest(ThreatListUpdateRequest value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Request = value;
            return this;
        }

        /// <summary>
        ///     Set Results.
        /// </summary>
        /// <param name="value">
        ///     A collection of <see cref="ThreatListUpdateResult" /> indicating the collection of retrieved
        ///     <see cref="ThreatList" /> and the threats associated with them that should be added to and removed
        ///     from the locally stored copies of the threat lists. A null reference or an empty collection indicates
        ///     no threat lists were retrieved.
        /// </param>
        /// <returns>
        ///     This threat list update response builder.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ThreatListUpdateResponseBuilder SetResults(IEnumerable<ThreatListUpdateResult> value) {
            this.Results = value != null ? value.ToList() : new List<ThreatListUpdateResult>();
            return this;
        }
    }
}