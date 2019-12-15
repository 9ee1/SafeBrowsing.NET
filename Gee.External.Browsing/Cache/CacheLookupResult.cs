using Gee.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Cache Lookup Result.
    /// </summary>
    public sealed class CacheLookupResult {
        /// <summary>
        ///     Unsafe Threat List Descriptors.
        /// </summary>
        private readonly Lazy<IEnumerable<ThreatListDescriptor>> _unsafeThreatListDescriptors;

        /// <summary>
        ///     Unsafe Threats.
        /// </summary>
        private readonly IEnumerable<UnsafeThreat> _unsafeThreats;

        /// <summary>
        ///     Determine if Cache Lookup Result Indicates a Cache Miss.
        /// </summary>
        /// <remarks>
        ///     Determines if the cache lookup result indicates a <see cref="CacheLookupResultCode.Miss" />.
        /// </remarks>
        public bool IsCacheMiss => this.ResultCode == CacheLookupResultCode.Miss;

        /// <summary>
        ///     Determine if Cache Lookup Result Indicates a Cache Safe Hit.
        /// </summary>
        /// <remarks>
        ///     Determines if the cache lookup result indicates a <see cref="CacheLookupResultCode.SafeHit" />.
        /// </remarks>
        public bool IsCacheSafeHit => this.ResultCode == CacheLookupResultCode.SafeHit;

        /// <summary>
        ///     Determine if Cache Lookup Result Indicates a Cache Unsafe Hit.
        /// </summary>
        /// <remarks>
        ///     Determines if the cache lookup result indicates an <see cref="CacheLookupResultCode.UnsafeHit" />.
        /// </remarks>
        public bool IsCacheUnsafeHit => this.ResultCode == CacheLookupResultCode.UnsafeHit;

        /// <summary>
        ///     Get Result Code.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="CacheLookupResultCode" /> indicating the nature of the cache lookup result.
        /// </remarks>
        public CacheLookupResultCode ResultCode { get; }

        /// <summary>
        ///     Get Threat's SHA256 Hash.
        /// </summary>
        /// <remarks>
        ///     Represents the full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat
        ///     that was looked up in a <see cref="IBrowsingCache" />.
        /// </remarks>
        public string ThreatSha256Hash { get; }

        /// <summary>
        ///     Get Threat's SHA256 Hash Prefix.
        /// </summary>
        /// <remarks>
        ///     Represents the SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat
        ///     that was looked up in a <see cref="IBrowsingCache" />.
        /// </remarks>
        public string ThreatSha256HashPrefix { get; }

        /// <summary>
        ///     Get Unsafe Threat List Descriptors.
        /// </summary>
        /// <remarks>
        ///     Represents the unique collection of <see cref="ThreatListDescriptor" /> identifying the unique
        ///     collection of <see cref="ThreatList" /> the collection of <see cref="UnsafeThreats" /> is associated
        ///     with if, and only if, the cache lookup result indicates an
        ///     <see cref="CacheLookupResultCode.UnsafeHit" />. To determine if the cache lookup result indicates a
        ///     cache unsafe hit, call <see cref="IsCacheUnsafeHit" />.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if the cache lookup result does not indicate a <see cref="CacheLookupResultCode.UnsafeHit" />.
        /// </exception>
        public IEnumerable<ThreatListDescriptor> UnsafeThreatListDescriptors => this._unsafeThreatListDescriptors.Value;

        /// <summary>
        ///     Get Unsafe Threats
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="UnsafeThreat" /> that was retrieved from a
        ///     <see cref="IBrowsingCache" /> if, and only if, the cache lookup result indicates an
        ///     <see cref="CacheLookupResultCode.UnsafeHit" />. To determine if the cache lookup result indicates a
        ///     cache unsafe hit, call <see cref="IsCacheUnsafeHit" />.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if the cache lookup result does not indicate a <see cref="CacheLookupResultCode.UnsafeHit" />.
        /// </exception>
        public IEnumerable<UnsafeThreat> UnsafeThreats {
            get {
                if (!this.IsCacheUnsafeHit) {
                    var detailMessage = $"An operation ({nameof(CacheLookupResult.UnsafeThreats)}) is invalid.";
                    throw new InvalidOperationException(detailMessage);
                }

                return this._unsafeThreats;
            }
        }

        /// <summary>
        ///     Create a Cache Result Indicating a Cache Miss.
        /// </summary>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat that was looked
        ///     up in a <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat that was
        ///     looked up in a <see cref="IBrowsingCache" />.
        /// </param>
        /// <returns>
        ///     A cache lookup result indicating a <see cref="CacheLookupResultCode.Miss" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is not formatted as a hexadecimal encoded string, or if
        ///     <paramref name="threatSha256HashPrefix" /> is not formatted as a hexadecimal encoded string.
        /// </exception>
        public static CacheLookupResult CacheMiss(string threatSha256Hash, string threatSha256HashPrefix) => new CacheLookupResult(CacheLookupResultCode.Miss, threatSha256Hash, threatSha256HashPrefix, null);

        /// <summary>
        ///     Create a Cache Result Indicating a Cache Safe Hit.
        /// </summary>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat that was looked
        ///     up in a <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat that was
        ///     looked up in a <see cref="IBrowsingCache" />.
        /// </param>
        /// <returns>
        ///     A cache lookup result indicating a <see cref="CacheLookupResultCode.SafeHit" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is not formatted as a hexadecimal encoded string, or if
        ///     <paramref name="threatSha256HashPrefix" /> is not formatted as a hexadecimal encoded string.
        /// </exception>
        public static CacheLookupResult CacheSafeHit(string threatSha256Hash, string threatSha256HashPrefix) => new CacheLookupResult(CacheLookupResultCode.SafeHit, threatSha256Hash, threatSha256HashPrefix, null);

        /// <summary>
        ///     Create a Cache Result Indicating a Cache Unsafe Hit.
        /// </summary>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat that was looked
        ///     up in a <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat that was
        ///     looked up in a <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="unsafeThreats">
        ///     A collection of <see cref="UnsafeThreat" /> that was retrieved from a <see cref="IBrowsingCache" />.
        /// </param>
        /// <returns>
        ///     A cache lookup result indicating a <see cref="CacheLookupResultCode.UnsafeHit" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefix" /> is a null reference, or if
        ///     <paramref name="unsafeThreats" /> is a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is not formatted as a hexadecimal encoded string, or if
        ///     <paramref name="threatSha256HashPrefix" /> is not formatted as a hexadecimal encoded string.
        /// </exception>
        public static CacheLookupResult CacheUnsafeHit(string threatSha256Hash, string threatSha256HashPrefix, IEnumerable<UnsafeThreat> unsafeThreats) => new CacheLookupResult(CacheLookupResultCode.UnsafeHit, threatSha256Hash, threatSha256HashPrefix, unsafeThreats);

        /// <summary>
        ///     Create a Cache Lookup Result.
        /// </summary>
        /// <param name="resultCode">
        ///     A <see cref="CacheLookupResultCode" /> indicating the nature of the cache lookup result.
        /// </param>
        /// <param name="threatSha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat that was looked
        ///     up in a <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat that was
        ///     looked up in a <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="unsafeThreats">
        ///     A collection of <see cref="UnsafeThreat" /> that was retrieved from a <see cref="IBrowsingCache" />
        ///     if, and only if, <paramref name="resultCode" /> is equal to
        ///     <see cref="CacheLookupResultCode.UnsafeHit" />. A null reference otherwise.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefix" /> is a null reference, or if <paramref name="resultCode" /> is
        ///     equal to <see cref="CacheLookupResultCode.UnsafeHit" /> and <paramref name="unsafeThreats" /> is a
        ///     null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="threatSha256Hash" /> is not formatted as a hexadecimal encoded string, or if
        ///     <paramref name="threatSha256HashPrefix" /> is not formatted as a hexadecimal encoded string.
        /// </exception>
        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private CacheLookupResult(CacheLookupResultCode resultCode, string threatSha256Hash, string threatSha256HashPrefix, IEnumerable<UnsafeThreat> unsafeThreats) {
            this._unsafeThreatListDescriptors = new Lazy<IEnumerable<ThreatListDescriptor>>(this.OnUnsafeThreatListDescriptorsInitialization);
            this.ResultCode = resultCode;
            this.ThreatSha256Hash = CreateThreatSha256Hash(threatSha256Hash);
            this.ThreatSha256HashPrefix = CreateThreatSha256HashPrefix(threatSha256HashPrefix);
            // ...
            //
            // ...
            this._unsafeThreats = CreateUnsafeThreats(this, unsafeThreats);

            // <summary>
            //      Create Threat's SHA256 Hash.
            // </summary>
            string CreateThreatSha256Hash(string cThreatSha256Hash) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cIsThreatSha256HashHexadecimalEncoded = cThreatSha256Hash.IsHexadecimalEncoded();
                if (!cIsThreatSha256HashHexadecimalEncoded) {
                    var cDetailMessage = $"A threat's SHA256 hash ({cThreatSha256Hash}) is not hexadecimal encoded.";
                    throw new FormatException(cDetailMessage);
                }

                return cThreatSha256Hash;
            }

            // <summary>
            //      Create Threat's SHA256 Hash Prefix.
            // </summary>
            string CreateThreatSha256HashPrefix(string cThreatSha256HashPrefix) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cIsThreatSha256HashPrefixHexadecimalEncoded = cThreatSha256HashPrefix.IsHexadecimalEncoded();
                if (!cIsThreatSha256HashPrefixHexadecimalEncoded) {
                    var cDetailMessage = $"A threat's SHA256 hash prefix ({cThreatSha256HashPrefix}) is not hexadecimal encoded.";
                    throw new FormatException(cDetailMessage);
                }

                return cThreatSha256HashPrefix;
            }

            // <summary>
            //      Create Unsafe Threats.
            // </summary>
            IEnumerable<UnsafeThreat> CreateUnsafeThreats(CacheLookupResult @this, IEnumerable<UnsafeThreat> cUnsafeThreats) {
                if (@this.IsCacheUnsafeHit && cUnsafeThreats == null) {
                    var cDetailMessage = $"A parameter ({nameof(cUnsafeThreats)}) cannot be a null reference.";
                    throw new ArgumentNullException(nameof(cUnsafeThreats), cDetailMessage);
                }

                cUnsafeThreats = cUnsafeThreats != null ? cUnsafeThreats.Distinct() : Array.Empty<UnsafeThreat>();
                return cUnsafeThreats;
            }
        }

        /// <summary>
        ///     On Unsafe Threat List Descriptors Initialization.
        /// </summary>
        /// <returns>
        ///     A unique collection of <see cref="ThreatListDescriptor" /> identifying the unique collection of
        ///     <see cref="ThreatList" /> the collection of <see cref="UnsafeThreats" /> is associated with.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if the cache lookup result does not indicate a <see cref="CacheLookupResultCode.UnsafeHit" />.
        /// </exception>
        private IEnumerable<ThreatListDescriptor> OnUnsafeThreatListDescriptorsInitialization() {
            if (!this.IsCacheUnsafeHit) {
                const string operationName = nameof(CacheLookupResult.OnUnsafeThreatListDescriptorsInitialization);
                var detailMessage = $"An operation ({operationName}) is invalid.";
                throw new InvalidOperationException(detailMessage);
            }

            // ...
            //
            // We're going to cache this collection so we force evaluate the query so the result of the query is
            // cached instead of the deferred execution commands.
            var unsafeThreatListDescriptors = this._unsafeThreats.Select(ut => ut.AssociatedThreatListDescriptor)
                .Distinct()
                .ToList();

            return unsafeThreatListDescriptors;
        }

        /// <summary>
        ///     Get Object's String Representation.
        /// </summary>
        /// <returns>
        ///     The object's string representation.
        /// </returns>
        public override string ToString() {
            return this.ThreatSha256Hash;
        }
    }
}