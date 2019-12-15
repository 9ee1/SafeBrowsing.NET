using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     URL Lookup Result.
    /// </summary>
    public sealed class UrlLookupResult {
        /// <summary>
        ///     Unsafe Threat List Descriptors.
        /// </summary>
        private readonly IEnumerable<ThreatListDescriptor> _unsafeThreatListDescriptors;

        /// <summary>
        ///     Unsafe URL Expression.
        /// </summary>
        private readonly UrlExpression _unsafeUrlExpression;

        /// <summary>
        ///     Determine if URL Lookup Result Indicates a Stale Database.
        /// </summary>
        /// <remarks>
        ///     Determines if the URL lookup result indicates a <see cref="UrlLookupResultCode.DatabaseStale" />.
        /// </remarks>
        public bool IsDatabaseStale => this.ResultCode == UrlLookupResultCode.DatabaseStale;

        /// <summary>
        ///     Determine if URL is Malware.
        /// </summary>
        /// <remarks>
        ///     Determines if <see cref="Url" /> is <see cref="ThreatType.Malware" /> if, and only if, it is
        ///     <see cref="UrlLookupResultCode.Unsafe" />. To determine if the the URL is unsafe, call
        ///     <see cref="IsUnsafe" />.
        /// </remarks>
        public bool IsMalware => this.IsUnsafe && this.UnsafeThreatListDescriptors.Any(tld => tld.IsMalwareList);

        /// <summary>
        ///     Determine if URL is a Potentially Harmful Application (PHA).
        /// </summary>
        /// <remarks>
        ///     Determines if <see cref="Url" /> is a <see cref="ThreatType.PotentiallyHarmfulApplication" /> if, and
        ///     only if, it is <see cref="UrlLookupResultCode.Unsafe" />. To determine if the the URL is unsafe, call
        ///     <see cref="IsUnsafe" />.
        /// </remarks>
        public bool IsPotentiallyHarmfulApplication => this.IsUnsafe && this.UnsafeThreatListDescriptors.Any(tld => tld.IsPotentiallyHarmfulApplicationList);

        /// <summary>
        ///     Determine if URL is Safe.
        /// </summary>
        /// <remarks>
        ///     Determines if <see cref="Url" /> is <see cref="UrlLookupResultCode.Safe" />.
        /// </remarks>
        public bool IsSafe => this.ResultCode == UrlLookupResultCode.Safe;

        /// <summary>
        ///     Determine if URL is Social Engineering.
        /// </summary>
        /// <remarks>
        ///     Determines if <see cref="Url" /> is <see cref="ThreatType.SocialEngineering" /> if, and only if, it is
        ///     <see cref="UrlLookupResultCode.Unsafe" />. To determine if the the URL is unsafe, call
        ///     <see cref="IsUnsafe" />.
        /// </remarks>
        public bool IsSocialEngineering => this.IsUnsafe && this.UnsafeThreatListDescriptors.Any(tld => tld.IsSocialEngineeringList);

        /// <summary>
        ///     Determine if URL is Unsafe.
        /// </summary>
        /// <remarks>
        ///     Determines if <see cref="Url" /> is <see cref="UrlLookupResultCode.Unsafe" />.
        /// </remarks>
        public bool IsUnsafe => this.ResultCode == UrlLookupResultCode.Unsafe;

        /// <summary>
        ///     Determine if URL is Unwanted Software.
        /// </summary>
        /// <remarks>
        ///     Determines if <see cref="Url" /> is <see cref="ThreatType.UnwantedSoftware" /> if, and only if, it is
        ///     <see cref="UrlLookupResultCode.Unsafe" />. To determine if the the URL is unsafe, call
        ///     <see cref="IsUnsafe" />.
        /// </remarks>
        public bool IsUnwantedSoftware => this.IsUnsafe && this.UnsafeThreatListDescriptors.Any(tld => tld.IsUnwantedSoftwareList);

        /// <summary>
        ///     Get Lookup Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), <see cref="Url" /> was looked up to determine
        ///     whether it is <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </remarks>
        public DateTime LookupDate { get; }

        /// <summary>
        ///     Get Result Code.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="UrlLookupResultCode" /> indicating the nature of the URL lookup result.
        /// </remarks>
        public UrlLookupResultCode ResultCode { get; }

        /// <summary>
        ///     Get Target Platforms.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if <see cref="Url" /> is not <see cref="UrlLookupResultCode.Unsafe" />. To determine if the URL
        ///     is unsafe, call <see cref="IsUnsafe" />.
        /// </exception>
        public IEnumerable<PlatformType> TargetPlatforms => this.UnsafeThreatListDescriptors.Select(tld => tld.PlatformType).Distinct();

        /// <summary>
        ///     Get Threat Types.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if <see cref="Url" /> is not <see cref="UrlLookupResultCode.Unsafe" />. To determine if the URL
        ///     is unsafe, call <see cref="IsUnsafe" />.
        /// </exception>
        public IEnumerable<ThreatType> ThreatTypes => this.UnsafeThreatListDescriptors.Select(tld => tld.ThreatType).Distinct();

        /// <summary>
        ///     Get Unsafe Threat List Descriptors.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of unsafe <see cref="ThreatListDescriptor" /> identifying the collection of
        ///     <see cref="ThreatList" /> <see cref="Url" /> is associated with if, and only if, the URL is
        ///     <see cref="UrlLookupResultCode.Unsafe" />. To determine if the URL is unsafe, call
        ///     <see cref="IsUnsafe" />.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if <see cref="Url" /> is not <see cref="UrlLookupResultCode.Unsafe" />. To determine if the URL
        ///     is unsafe, call <see cref="IsUnsafe" />.
        /// </exception>
        public IEnumerable<ThreatListDescriptor> UnsafeThreatListDescriptors {
            get {
                if (!this.IsUnsafe) {
                    const string operationName = nameof(UrlLookupResult.UnsafeThreatListDescriptors);
                    var detailMessage = $"An operation ({operationName}) is invalid.";
                    throw new InvalidOperationException(detailMessage);
                }

                return this._unsafeThreatListDescriptors;
            }
        }

        /// <summary>
        ///     Get Unsafe URL Expression.
        /// </summary>
        /// <remarks>
        ///     Represents the computed unsafe <see cref="UrlExpression" /> for <see cref="Url" /> if, and only if, the
        ///     URL is <see cref="UrlLookupResultCode.Unsafe" />. To determine if the URL is unsafe, call
        ///     <see cref="IsUnsafe" />.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if <see cref="Url" /> is not <see cref="UrlLookupResultCode.Unsafe" />. To determine if the URL
        ///     is unsafe, call <see cref="IsUnsafe" />.
        /// </exception>
        public UrlExpression UnsafeUrlExpression {
            get {
                if (!this.IsUnsafe) {
                    const string operationName = nameof(UrlLookupResult.UnsafeUrlExpression);
                    var detailMessage = $"An operation ({operationName}) is invalid.";
                    throw new InvalidOperationException(detailMessage);
                }

                return this._unsafeUrlExpression;
            }
        }

        /// <summary>
        ///     Get URL.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="Browsing.Url" /> that was looked up to determine whether it is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </remarks>
        public Url Url { get; }

        /// <summary>
        ///     Build a <see cref="UrlLookupResult" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="UrlLookupResultBuilder" /> to build a <see cref="UrlLookupResult" /> with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UrlLookupResultBuilder Build() => new UrlLookupResultBuilder();

        /// <summary>
        ///     Create a URL Lookup Result Indicating a Stale Database.
        /// </summary>
        /// <param name="url">
        ///     A <see cref="Browsing.Url" /> that was looked up to determine whether it is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </param>
        /// <param name="lookupDate">
        ///     The date, in Coordinated Universal Time (UTC), <paramref name="url" /> was looked up to determine
        ///     whether it is <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />. If
        ///     the date is not in UTC, it is converted to it.
        /// </param>
        /// <returns>
        ///     A URL lookup result indicating a <see cref="UrlLookupResultCode.DatabaseStale" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UrlLookupResult DatabaseStale(Url url, DateTime lookupDate) {
            // ...
            //
            // Throws an exception if the operation fails.
            return UrlLookupResult.Build()
                .SetLookupDate(lookupDate)
                .SetResultCode(UrlLookupResultCode.DatabaseStale)
                .SetUrl(url)
                .Build();
        }

        /// <summary>
        ///     Create a URL Lookup Result Indicating a Safe URL.
        /// </summary>
        /// <param name="url">
        ///     A <see cref="Browsing.Url" /> that was looked up to determine whether it is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </param>
        /// <param name="lookupDate">
        ///     The date, in Coordinated Universal Time (UTC), <paramref name="url" /> was looked up to determine
        ///     whether it is <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />. If
        ///     the date is not in UTC, it is converted to it.
        /// </param>
        /// <returns>
        ///     A URL lookup result indicating a <see cref="UrlLookupResultCode.Safe" /> <see cref="Url" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UrlLookupResult Safe(Url url, DateTime lookupDate) {
            // ...
            //
            // Throws an exception if the operation fails.
            return UrlLookupResult.Build()
                .SetLookupDate(lookupDate)
                .SetResultCode(UrlLookupResultCode.Safe)
                .SetUrl(url)
                .Build();
        }

        /// <summary>
        ///     Create a URL Lookup Result Indicating an Unsafe URL.
        /// </summary>
        /// <param name="url">
        ///     A <see cref="Browsing.Url" /> that was looked up to determine whether it is
        ///     <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />.
        /// </param>
        /// <param name="lookupDate">
        ///     The date, in Coordinated Universal Time (UTC), <paramref name="url" /> was looked up to determine
        ///     whether it is <see cref="UrlLookupResultCode.Safe" /> or <see cref="UrlLookupResultCode.Unsafe" />. If
        ///     the date is not in UTC, it is converted to it.
        /// </param>
        /// <param name="unsafeUrlExpression">
        ///     The computed unsafe <see cref="UrlExpression" /> for <paramref name="url" />.
        /// </param>
        /// <param name="unsafeThreatListDescriptors">
        ///     A collection of unsafe <see cref="ThreatListDescriptor" /> identifying the collection of
        ///     <see cref="ThreatList" /> <paramref name="url" /> is associated with.
        /// </param>
        /// <returns>
        ///     A URL lookup result indicating an <see cref="UrlLookupResultCode.Unsafe" /> <see cref="Url" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UrlLookupResult Unsafe(Url url, DateTime lookupDate, UrlExpression unsafeUrlExpression, IEnumerable<ThreatListDescriptor> unsafeThreatListDescriptors) {
            // ...
            //
            // Throws an exception if the operation fails.
            return UrlLookupResult.Build()
                .SetLookupDate(lookupDate)
                .SetResultCode(UrlLookupResultCode.Unsafe)
                .SetUnsafeThreatListDescriptors(unsafeThreatListDescriptors)
                .SetUnsafeUrlExpression(unsafeUrlExpression)
                .SetUrl(url)
                .Build();
        }

        /// <summary>
        ///     Create a URL Lookup Result.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="UrlLookupResultBuilder" /> to initialize the URL lookup result with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal UrlLookupResult(UrlLookupResultBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();
            Guard.ThrowIf(nameof(builder), builder.Url).Null();

            this.LookupDate = builder.LookupDate;
            this.ResultCode = builder.ResultCode;
            this.Url = builder.Url;
            // ...
            //
            // ...
            this._unsafeThreatListDescriptors = CreateUnsafeThreatListDescriptors(this, builder);
            this._unsafeUrlExpression = CreateUnsafeUrlExpression(this, builder);

            // <summary>
            //      Create Unsafe Threat List Descriptors.
            // </summary>
            IEnumerable<ThreatListDescriptor> CreateUnsafeThreatListDescriptors(UrlLookupResult @this, UrlLookupResultBuilder cBuilder) {
                IEnumerable<ThreatListDescriptor> cUnsafeThreatListDescriptors = Array.Empty<ThreatListDescriptor>();
                if (@this.IsUnsafe) {
                    Guard.ThrowIf(nameof(cBuilder.UnsafeThreatListDescriptors), cBuilder.UnsafeThreatListDescriptors);
                    cUnsafeThreatListDescriptors = cBuilder.UnsafeThreatListDescriptors;
                }

                return cUnsafeThreatListDescriptors;
            }

            // <summary>
            //      Create Unsafe URL Expression.
            // </summary>
            UrlExpression CreateUnsafeUrlExpression(UrlLookupResult @this, UrlLookupResultBuilder cBuilder) {
                UrlExpression cUnsafeUrlExpression = null;
                if (@this.IsUnsafe) {
                    Guard.ThrowIf(nameof(cBuilder.UnsafeUrlExpression), cBuilder.UnsafeUrlExpression);
                    cUnsafeUrlExpression = cBuilder.UnsafeUrlExpression;
                }

                return cUnsafeUrlExpression;
            }
        }

        /// <summary>
        ///     Get Object's String Representation.
        /// </summary>
        /// <returns>
        ///     The object's string representation.
        /// </returns>
        public override string ToString() {
            var @string = this.Url.ToString();
            return @string;
        }
    }
}