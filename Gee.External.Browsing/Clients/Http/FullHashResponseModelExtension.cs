using System;
using System.Linq;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Full Hash Response Model Extension.
    /// </summary>
    internal static class FullHashResponseModelExtension {
        /// <summary>
        ///     Create a Full Hash Response.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="FullHashResponseModel" />.
        /// </param>
        /// <param name="request">
        ///     The <see cref="FullHashRequest" /> made to the Google Safe Browsing API for which the
        ///     <see cref="FullHashResponse" /> has been returned.
        /// </param>
        /// <returns>
        ///     A <see cref="FullHashResponse" /> if <paramref name="this" /> is not a null reference. A null reference
        ///     otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="request" /> is a null reference.
        /// </exception>
        internal static FullHashResponse AsFullHashResponse(this FullHashResponseModel @this, FullHashRequest request) {
            FullHashResponse fullHashResponse = null;
            if (@this != null) {
                var safeThreatsExpirationDate = CreateSafeThreatsExpirationDate(@this);
                var unsafeThreats = @this.UnsafeThreats?.Select(utm => utm.AsUnsafeThreat());
                var waitToDate = CreateWaitToDate(@this);

                // ...
                //
                // Throws an exception if the operation fails.
                fullHashResponse = new FullHashResponse(request, safeThreatsExpirationDate, unsafeThreats, waitToDate);
            }

            return fullHashResponse;

            // <summary>
            //      Create Safe Threats Expiration Date.
            // </summary>
            DateTime CreateSafeThreatsExpirationDate(FullHashResponseModel cThis) {
                var cSafeThreatsExpirationDate = DateTime.UtcNow;
                if (cThis.SafeThreatsDuration != null) {
                    var cSafeThreatsDuration = cThis.SafeThreatsDuration.Substring(0, cThis.SafeThreatsDuration.Length - 1);
                    var cIsSafeThreatsDurationParsed = double.TryParse(cSafeThreatsDuration, out var cSafeThreatsDurationDouble);
                    if (cIsSafeThreatsDurationParsed) {
                        cSafeThreatsExpirationDate = cSafeThreatsExpirationDate.AddSeconds(cSafeThreatsDurationDouble);
                    }
                }

                return cSafeThreatsExpirationDate;
            }

            // <summary>
            //      Create Wait to Date.
            // </summary>
            DateTime? CreateWaitToDate(FullHashResponseModel cThis) {
                DateTime? cWaitToDate = null;
                if (cThis.WaitDuration != null) {
                    var cWaitDuration = cThis.WaitDuration.Substring(0, cThis.WaitDuration.Length - 1);
                    var cIsWaitDurationParsed = double.TryParse(cWaitDuration, out var cWaitDurationDouble);
                    if (cIsWaitDurationParsed) {
                        cWaitToDate = DateTime.UtcNow.AddSeconds(cWaitDurationDouble);
                    }
                }

                return cWaitToDate;
            }
        }
    }
}