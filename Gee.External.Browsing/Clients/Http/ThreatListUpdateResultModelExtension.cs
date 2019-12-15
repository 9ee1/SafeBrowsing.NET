using Gee.Common;
using Gee.Common.Guards;
using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Result Model Extension.
    /// </summary>
    internal static class ThreatListUpdateResultModelExtension {
        /// <summary>
        ///     Create a Threat List Update Result.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ThreatListUpdateResultModel" />.
        /// </param>
        /// <param name="request">
        ///     The <see cref="ThreatListUpdateRequest" /> made to the Google Safe Browsing API for which the
        ///     <see cref="ThreatListUpdateResult" /> has been returned.
        /// </param>
        /// <param name="threatListRetrieveDate">
        ///     The date, in Coordinated Universal Time (UTC), the <see cref="ThreatList" /> was retrieved from the
        ///     Google Safe Browsing API. If the date is not expressed in UTC, it is converted to it.
        /// </param>
        /// <param name="threatListWaitToDate">
        ///     The date, in Coordinated Universal Time (UTC), a client must wait to before retrieving the
        ///     <see cref="ThreatList" /> from the Google Safe Browsing API again. If the date is not expressed in UTC,
        ///     it is converted to it. A null reference indicates a client does not need to wait.
        /// </param>
        /// <returns>
        ///     A <see cref="ThreatListUpdateResult" /> if <paramref name="this" /> is not a null reference. A null
        ///     reference otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="request" /> is a null reference.
        /// </exception>
        internal static ThreatListUpdateResult AsThreatListUpdateResult(this ThreatListUpdateResultModel @this, ThreatListUpdateRequest request, DateTime threatListRetrieveDate, DateTime? threatListWaitToDate) {
            ThreatListUpdateResult threatListUpdateResult = null;
            if (@this != null) {
                Guard.ThrowIf(nameof(request), request).Null();

                var threatListUpdateResultBuilder = ThreatListUpdateResult.Build();
                SetThreatsToAdd(@this, threatListUpdateResultBuilder);
                SetThreatsToRemove(@this, threatListUpdateResultBuilder);
                SetRetrievedThreatList(@this, threatListUpdateResultBuilder, threatListRetrieveDate, threatListWaitToDate);
                SetRetrievedThreatListChecksum(@this, threatListUpdateResultBuilder);
                SetUpdateType(@this, threatListUpdateResultBuilder);

                // ...
                //
                // We need to set the query after the retrieved threat list is set.
                SetQuery(threatListUpdateResultBuilder, request);
                threatListUpdateResult = threatListUpdateResultBuilder.Build();
            }

            return threatListUpdateResult;

            // <summary>
            //      Set Query.
            // </summary>
            void SetQuery(ThreatListUpdateResultBuilder cBuilder, ThreatListUpdateRequest cRequest) {
                foreach (var cQuery in cRequest.Queries) {
                    var cIsMatchedQuery = cQuery.ThreatListDescriptor.Equals(cBuilder.RetrievedThreatList.Descriptor);
                    if (cIsMatchedQuery) {
                        cBuilder.SetQuery(cQuery);
                        break;
                    }
                }
            }

            // <summary>
            //      Set Retrieved Threat List.
            // </summary>
            void SetRetrievedThreatList(ThreatListUpdateResultModel cModel, ThreatListUpdateResultBuilder cBuilder, DateTime cThreatListRetrieveDate, DateTime? cThreatListWaitToDate) {
                var cPlatformType = cModel.PlatformType.AsPlatformType();
                var cState = cModel.ThreatListState.Base64Decode().HexadecimalEncode();
                var cThreatEntryType = cModel.ThreatEntryType.AsThreatEntryType();
                var cThreatType = cModel.ThreatType.AsThreatType();
                cBuilder.SetRetrievedThreatList(b => {
                    b.SetDescriptor(cThreatType, cPlatformType, cThreatEntryType);
                    b.SetRetrieveDate(cThreatListRetrieveDate);
                    b.SetState(cState);
                    b.SetWaitToDate(cThreatListWaitToDate);
                    return b.Build();
                });
            }

            // <summary>
            //      Set Retrieved Threat List's Checksum.
            // </summary>
            void SetRetrievedThreatListChecksum(ThreatListUpdateResultModel cModel, ThreatListUpdateResultBuilder cBuilder) {
                var cRetrievedThreatListChecksum = cModel.Checksum.Sha256Hash.Base64Decode().HexadecimalEncode();
                cBuilder.SetRetrievedThreatListChecksum(cRetrievedThreatListChecksum);
            }

            // <summary>
            //      Set Threats to Add.
            // </summary>
            void SetThreatsToAdd(ThreatListUpdateResultModel cModel, ThreatListUpdateResultBuilder cBuilder) {
                if (cModel.ThreatsToAdd != null) {
                    foreach (var cThreatToAdd in cModel.ThreatsToAdd) {
                        var cCompressionType = cThreatToAdd.CompressionType.AsCompressionType();
                        if (cCompressionType == CompressionType.Uncompressed) {
                            // ...
                            //
                            // It's faster to 1) Base64 decode the SHA256 hashes, 2) hexadecimal encode them all, and
                            // 3) split them into subsets rather then 1) Base64 decode the SHA256 hashes and 2) encode
                            // them individually.
                            var cSha256Hashes = cThreatToAdd.UncompressedSha256HashPrefixes
                                .Sha256HashPrefixes
                                .Base64Decode()
                                .HexadecimalEncode();

                            // ...
                            //
                            // 1 byte is equal to 2 hexadecimal characters.
                            var cSha256HashPrefixSize = cThreatToAdd.UncompressedSha256HashPrefixes.Sha256HashPrefixSize * 2;
                            for (var cI = 0; cI < cSha256Hashes.Length; cI += cSha256HashPrefixSize) {
                                var cSha256Hash = cSha256Hashes.Substring(cI, cSha256HashPrefixSize);
                                cBuilder.AddThreatToAdd(cSha256Hash);
                            }
                        }
                    }
                }
            }

            // <summary>
            //      Set Threats to Remove.
            // </summary>
            void SetThreatsToRemove(ThreatListUpdateResultModel cModel, ThreatListUpdateResultBuilder cBuilder) {
                if (cModel.ThreatsToRemove != null) {
                    foreach (var cThreatToRemove in cModel.ThreatsToRemove) {
                        var cCompressionType = cThreatToRemove.CompressionType.AsCompressionType();
                        if (cCompressionType == CompressionType.Uncompressed) {
                            var cIndices = cThreatToRemove.UncompressedIndices.Indices;
                            foreach (var cIndex in cIndices) {
                                cBuilder.AddThreatToRemove(cIndex);
                            }
                        }
                    }
                }
            }

            // <summary>
            //      Set Update Type.
            // </summary>
            void SetUpdateType(ThreatListUpdateResultModel cModel, ThreatListUpdateResultBuilder cBuilder) {
                var cUpdateType = cModel.UpdateType.AsThreatListUpdateType();
                cBuilder.SetUpdateType(cUpdateType);
            }
        }
    }
}