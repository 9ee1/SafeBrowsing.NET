using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Response Model Extension.
    /// </summary>
    internal static class ThreatListUpdateResponseModelExtension {
        /// <summary>
        ///     Create a Threat List Update Response.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatListUpdateResponseModel" />.
        /// </param>
        /// <param name="request">
        ///     The <see cref="ThreatListUpdateRequest" /> made to the Google Safe Browsing API for which the
        ///     <see cref="ThreatListUpdateResponse" /> has been returned.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResponse" /> if <paramref name="this" /> is not a null reference. A null
        ///     reference otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="request" /> is a null reference.
        /// </exception>
        internal static ThreatListUpdateResponse AsThreatListUpdateResponse(this ThreatListUpdateResponseModel @this, ThreatListUpdateRequest request) {
            ThreatListUpdateResponse threatListUpdateResponse = null;
            if (@this != null) {
                // ...
                //
                // Throws an exception if the operation fails.
                var threatListUpdateResponseBuilder = ThreatListUpdateResponse.Build();
                threatListUpdateResponseBuilder.SetRequest(request);
                
                SetResults(@this, threatListUpdateResponseBuilder, request);
                threatListUpdateResponse = threatListUpdateResponseBuilder.Build();
            }

            return threatListUpdateResponse;

            // <summary>
            //      Create Wait to Date.
            // </summary>
            DateTime? CreateWaitToDate(ThreatListUpdateResponseModel cThis) {
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

            // <summary>
            //      Set Results.
            // </summary>
            void SetResults(ThreatListUpdateResponseModel cThis, ThreatListUpdateResponseBuilder cBuilder, ThreatListUpdateRequest cRequest) {
                if (cThis.Results != null) {
                    var cThreatListRetrieveDate = DateTime.UtcNow;
                    var cThreatListWaitToDate = CreateWaitToDate(cThis);
                    foreach (var cResultModel in cThis.Results) {
                        var cResult = cResultModel.AsThreatListUpdateResult(cRequest, cThreatListRetrieveDate, cThreatListWaitToDate);
                        cBuilder.AddResult(cResult);
                    }
                }
            }
        }
    }
}