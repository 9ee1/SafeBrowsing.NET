using Gee.Common.Guards;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     URL Lookup Result Builder.
    /// </summary>
    internal sealed class UrlLookupResultBuilder {
        /// <summary>
        ///     Get and Set Lookup Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), <see cref="Url" /> was looked up to determine
        ///     whether it is <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </remarks>
        internal DateTime LookupDate { get; private set; }

        /// <summary>
        ///     Get and Set <see cref="UrlLookupResultCode" />.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="UrlLookupResultCode" /> indicating the nature of the URL lookup result.
        /// </remarks>
        internal UrlLookupResultCode ResultCode { get; private set; }

        /// <summary>
        ///     Get and Set Unsafe Threat List Descriptors.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of unsafe <see cref="ThreatListDescriptor" /> identifying the collection of
        ///     <see cref="ThreatList" /> <see cref="Url" /> is associated with if, and only if, the URL is
        ///     <see cref="UrlLookupResultCode.Unsafe" />.
        /// </remarks>
        internal List<ThreatListDescriptor> UnsafeThreatListDescriptors { get; private set; }

        /// <summary>
        ///     Get and Set Unsafe URL Expression.
        /// </summary>
        /// <remarks>
        ///     Represents the computed unsafe <see cref="UrlExpression" /> for <see cref="Url" /> if, and only if, the
        ///     URL is <see cref="UrlLookupResultCode.Unsafe" />.
        /// </remarks>
        internal UrlExpression UnsafeUrlExpression { get; private set; }

        /// <summary>
        ///     Get and Set URL.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="Browsing.Url" /> that was looked up to determine whether it is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </remarks>
        internal Url Url { get; private set; }

        /// <summary>
        ///     Build a URL Lookup Result.
        /// </summary>
        /// <returns>
        ///     A <see cref="UrlLookupResult" />.
        /// </returns>
        internal UrlLookupResult Build() {
            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            var urlLookupResult = new UrlLookupResult(this);
            this.LookupDate = default;
            this.ResultCode = default;
            this.UnsafeThreatListDescriptors = new List<ThreatListDescriptor>();
            this.UnsafeUrlExpression = null;
            this.Url = null;

            return urlLookupResult;
        }

        /// <summary>
        ///     Set Lookup Date.
        /// </summary>
        /// <param name="value">
        ///     The date, in Coordinated Universal Time (UTC), the URL was looked up to determine whether it is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </param>
        /// <returns>
        ///     This URL lookup result builder.
        /// </returns>
        internal UrlLookupResultBuilder SetLookupDate(DateTime value) {
            this.LookupDate = value.ToUniversalTime();
            return this;
        }

        /// <summary>
        ///     Set Result Code.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="UrlLookupResultCode" />  indicating the nature of the URL lookup result.
        /// </param>
        /// <returns>
        ///     This URL lookup result builder.
        /// </returns>
        internal UrlLookupResultBuilder SetResultCode(UrlLookupResultCode value) {
            this.ResultCode = value;
            return this;
        }

        /// <summary>
        ///     Set Unsafe Threat List Descriptors.
        /// </summary>
        /// <param name="value">
        ///     A collection of <see cref="ThreatListDescriptor" /> identifying the collection of
        ///     <see cref="ThreatList" /> the URL is associated with. An empty collection or a null reference indicates
        ///     the URL is <see cref="UrlLookupResultCode.Safe" />.
        /// </param>
        /// <returns>
        ///     This URL lookup result builder.
        /// </returns>
        internal UrlLookupResultBuilder SetUnsafeThreatListDescriptors(IEnumerable<ThreatListDescriptor> value) {
            this.UnsafeThreatListDescriptors = value != null ? new List<ThreatListDescriptor>(value) : new List<ThreatListDescriptor>();
            return this;
        }

        /// <summary>
        ///     Set Unsafe URL Expression.
        /// </summary>
        /// <param name="value">
        ///     The computed unsafe <see cref="UrlExpression" /> for the URL. A null reference indicates the URL is
        ///     <see cref="UrlLookupResultCode.Safe" />.
        /// </param>
        /// <returns>
        ///     This URL lookup result builder.
        /// </returns>
        internal UrlLookupResultBuilder SetUnsafeUrlExpression(UrlExpression value) {
            this.UnsafeUrlExpression = value;
            return this;
        }

        /// <summary>
        ///     Set URL.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="Browsing.Url" /> that was looked up to determine whether it is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </param>
        /// <returns>
        ///     This URL lookup result builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        internal UrlLookupResultBuilder SetUrl(Url value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Url = value;
            return this;
        }
    }
}