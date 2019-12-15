using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Full Hash Response.
    /// </summary>
    public sealed class FullHashResponse {
        /// <summary>
        ///     Get Request.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="FullHashRequest" /> made to the Google Safe Browsing API for which the full
        ///     hash response has been returned.
        /// </remarks>
        public FullHashRequest Request { get; }

        /// <summary>
        ///     Get Safe Threats Expiration Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), safe threats should be considered safe to.
        /// </remarks>
        public DateTime SafeThreatsExpirationDate { get; }

        /// <summary>
        ///     Determine if Safe Threats Have Expired.
        /// </summary>
        /// <remarks>
        ///     Determines if safe threats have expired and should no longer be considered safe.
        /// </remarks>
        public bool SafeThreatsExpired => DateTime.UtcNow >= this.SafeThreatsExpirationDate;

        /// <summary>
        ///     Get Unsafe Threats.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="UnsafeThreat" />. An empty collection indicates no threats were
        ///     determined to be unsafe.
        /// </remarks>
        public IReadOnlyCollection<UnsafeThreat> UnsafeThreats { get; }

        /// <summary>
        ///     Get Wait to Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), a client must wait to before issuing another
        ///     <see cref="FullHashRequest" /> to the Google Safe Browsing API. A null reference indicates a client
        ///     does not have to wait.
        /// </remarks>
        public DateTime? WaitToDate { get; }

        /// <summary>
        ///     Build a Full Hash Response.
        /// </summary>
        /// <returns>
        ///     A <see cref="FullHashResponseBuilder" /> to build a full hash response with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FullHashResponseBuilder Build() {
            return new FullHashResponseBuilder();
        }

        /// <summary>
        ///     Create a Full Hash Response.
        /// </summary>
        /// <param name="request">
        ///     The <see cref="FullHashRequest" /> made to the Google Safe Browsing API for which the full hash
        ///     response has been returned.
        /// </param>
        /// <param name="safeThreatsExpirationDate">
        ///     The date, in Coordinated Universal Time (UTC), safe threats should be considered safe to. If the date
        ///     is not in UTC, it is converted to it.
        /// </param>
        /// <param name="unsafeThreats">
        ///     A collection of <see cref="UnsafeThreat" />. A null reference indicates no threats were determined to
        ///     be unsafe.
        /// </param>
        /// <param name="waitToDate">
        ///     The date, in Coordinated Universal Time (UTC), a client must wait to before issuing another
        ///     <see cref="FullHashRequest" /> to the Google Safe Browsing API. If the date is not in UTC, it is
        ///     converted to it. A null reference indicates a client does not have to wait.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="request"/> is a null reference.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public FullHashResponse(FullHashRequest request, DateTime safeThreatsExpirationDate, IEnumerable<UnsafeThreat> unsafeThreats, DateTime? waitToDate) {
            Guard.ThrowIf(nameof(request), request).Null();

            this.Request = request;
            this.SafeThreatsExpirationDate = safeThreatsExpirationDate.ToUniversalTime();
            this.UnsafeThreats = CreateUnsafeThreats(unsafeThreats);
            this.WaitToDate = waitToDate?.ToUniversalTime();

            // <summary>
            //      Create Unsafe Threats.
            // </summary>
            IReadOnlyCollection<UnsafeThreat> CreateUnsafeThreats(IEnumerable<UnsafeThreat> cUnsafeThreats) {
                IReadOnlyCollection<UnsafeThreat> cNewUnsafeThreats = Array.Empty<UnsafeThreat>();
                if (cUnsafeThreats != null) {
                    cNewUnsafeThreats = new List<UnsafeThreat>(cUnsafeThreats);
                }

                return cNewUnsafeThreats;
            }
        }

        /// <summary>
        ///     Create a Full Hash Response.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="FullHashResponseBuilder" /> to initialize the full hash response with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal FullHashResponse(FullHashResponseBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();
            Guard.ThrowIf(nameof(builder), builder.Request).Null();

            this.Request = builder.Request;
            this.SafeThreatsExpirationDate = builder.SafeThreatsExpirationDate;
            this.UnsafeThreats = builder.UnsafeThreats;
            this.WaitToDate = builder.WaitToDate;
        }

        /// <summary>
        ///     Get a Threat's State.
        /// </summary>
        /// <param name="sha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying a threat.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatState" /> indicating the state of the threat identified by
        ///     <paramref name="sha256Hash" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="sha256Hash" /> is a null reference.
        /// </exception>
        public ThreatState GetThreatState(string sha256Hash) {
            Guard.ThrowIf(nameof(sha256Hash), sha256Hash).Null();

            ThreatState threatState;
            var unsafeThreats = this.UnsafeThreats.Where(ut => ut.Sha256Hash == sha256Hash).ToList();
            if (unsafeThreats.Count == 0) {
                // ...
                //
                // If the SHA256 hash identifies a safe threat, determine its state based on whether or not safe
                // threats have expired.
                threatState = this.SafeThreatsExpired ? ThreatState.Expired : ThreatState.Safe;
            }
            else {
                // ...
                //
                // If the SHA256 hash identifies an unsafe threat, determine its state based on whether or not any
                // of the unsafe threats have expired.
                threatState = unsafeThreats.Any(ut => ut.Expired) ? ThreatState.Expired : ThreatState.Unsafe;
            }

            return threatState;
        }
    }
}