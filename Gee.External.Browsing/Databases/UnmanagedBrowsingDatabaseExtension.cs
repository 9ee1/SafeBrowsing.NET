using Gee.Common;
using Gee.Common.Guards;
using Gee.Common.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Unmanaged Database Extension.
    /// </summary>
    public static class UnmanagedBrowsingDatabaseExtension {
        /// <summary>
        ///     Compute a Threat List's Checksum Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     An <see cref="IUnmanagedBrowsingDatabase" />.
        /// </param>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> whose checksum should
        ///     be computed.
        /// </param>
        /// <returns>
        ///     The checksum, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatListDescriptor" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatListDescriptor" />
        ///     is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<string> ComputeThreatListChecksumAsync(this IUnmanagedBrowsingDatabase @this, ThreatListDescriptor threatListDescriptor) {
            // ...
            //
            // Throws an exception if the operation fails.
            return @this.ComputeThreatListChecksumAsync(threatListDescriptor, CancellationToken.None);
        }

        /// <summary>
        ///     Compute a Threat List's Checksum Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     An <see cref="IUnmanagedBrowsingDatabase" />.
        /// </param>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> whose checksum should
        ///     be computed.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     The checksum, formatted as a hexadecimal encoded string, of the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatListDescriptor" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatListDescriptor" />
        ///     is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        public static async Task<string> ComputeThreatListChecksumAsync(this IUnmanagedBrowsingDatabase @this, ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatsTask = @this.GetThreatsAsync(threatListDescriptor, cancellationToken);
            var threats = await getThreatsTask.ConfigureAwait(false);
            try {
                string threatListChecksum = null;
                if (threats.Count != 0) {
                    // ...
                    //
                    // Throws an exception if the decoding, hashing, or encoding operations fail. They will typically
                    // fail if the database is corrupt in anyway.
                    threatListChecksum = threats.OrderBy(t => t)
                        .Join()
                        .HexadecimalDecode()
                        .Sha256Hash()
                        .HexadecimalEncode();
                }

                return threatListChecksum;
            }
            catch (Exception ex) {
                const string detailMessage = "A threat list's checksum could not be computed.";
                throw new BrowsingDatabaseException(detailMessage, ex);
            }
        }

        /// <summary>
        ///     Find Threat Lists Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     An <see cref="IUnmanagedBrowsingDatabase" />.
        /// </param>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat associated with
        ///     the collection of <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <returns>
        ///     A collection of <see cref="ThreatList" /> the threat identified by
        ///     <paramref name="threatSha256HashPrefix" /> is associated with. An empty collection indicates no threat
        ///     lists were found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<IReadOnlyCollection<ThreatList>> FindThreatListsAsync(this IUnmanagedBrowsingDatabase @this, string threatSha256HashPrefix) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var findThreatListsTask = @this.FindThreatListsAsync(threatSha256HashPrefix, CancellationToken.None);
            return findThreatListsTask;
        }

        /// <summary>
        ///     Get a Threat List Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     An <see cref="IUnmanagedBrowsingDatabase" />.
        /// </param>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <returns>
        ///     The <see cref="ThreatList" /> identified by <paramref name="threatListDescriptor" />. A null reference
        ///     indicates a threat list could not be found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatListDescriptor" />
        ///     is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<ThreatList> GetThreatListAsync(this IUnmanagedBrowsingDatabase @this, ThreatListDescriptor threatListDescriptor) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatListTask = @this.GetThreatListAsync(threatListDescriptor, CancellationToken.None);
            return getThreatListTask;
        }

        /// <summary>
        ///     Get Threat Lists Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     An <see cref="IUnmanagedBrowsingDatabase" />.
        /// </param>
        /// <returns>
        ///     A collection of <see cref="ThreatList" />. An empty collection indicates no threat lists were found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<IReadOnlyCollection<ThreatList>> GetThreatListsAsync(this IUnmanagedBrowsingDatabase @this) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatListsTask = @this.GetThreatListsAsync(CancellationToken.None);
            return getThreatListsTask;
        }

        /// <summary>
        ///     Get Threat Lists Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A browsing database.
        /// </param>
        /// <param name="threatListDescriptors">
        ///     A collection of threat list descriptors identifying the threat lists to retrieve.
        /// </param>
        /// <returns>
        ///     A collection of threat lists identified by the threat list descriptors contained in
        ///     <paramref name="threatListDescriptors" />. The length of the collection is not guaranteed to equal the
        ///     length of <paramref name="threatListDescriptors" />. More specifically, the collection will only
        ///     contain threat lists that can be identified by a threat list descriptor contained in
        ///     <paramref name="threatListDescriptors" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static async Task<IReadOnlyCollection<ThreatList>> GetThreatListsAsync(this IUnmanagedBrowsingDatabase @this, IEnumerable<ThreatListDescriptor> threatListDescriptors) {
            Guard.ThrowIf(nameof(@this), @this).Null();
            Guard.ThrowIf(nameof(threatListDescriptors), threatListDescriptors).Null();

            var threatLists = new List<ThreatList>();
            foreach (var threatListDescriptor in threatListDescriptors) {
                // ...
                //
                // Throws an exception if the operation fails.
                var getThreatListTask = @this.GetThreatListAsync(threatListDescriptor);
                var threatList = await getThreatListTask.ConfigureAwait(false);

                threatList = threatList ?? ThreatList.CreateInvalid(threatListDescriptor);
                threatLists.Add(threatList);
            }

            return threatLists;
        }

        /// <summary>
        ///     Get Threats Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     An <see cref="IUnmanagedBrowsingDatabase" />.
        /// </param>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> the threats that should
        ///     be retrieved are associated with.
        /// </param>
        /// <returns>
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the threats
        ///     that are associated with the <see cref="ThreatList" /> identified by
        ///     <paramref name="threatListDescriptor" />. An empty collection indicates no threats were found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatListDescriptor" />
        ///     is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<IReadOnlyCollection<string>> GetThreatsAsync(this IUnmanagedBrowsingDatabase @this, ThreatListDescriptor threatListDescriptor) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var getThreatsTask = @this.GetThreatsAsync(threatListDescriptor, CancellationToken.None);
            return getThreatsTask;
        }

        /// <summary>
        ///     Lookup a URL Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A database.
        /// </param>
        /// <param name="url">
        ///     A URL to lookup.
        /// </param>
        /// <returns>
        ///     A database lookup result indicating the nature of lookup operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="url" /> is a null
        ///     reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task<DatabaseLookupResult> LookupAsync(this IUnmanagedBrowsingDatabase @this, Url url) {
            Guard.ThrowIf(nameof(@this), @this).Null();
            Guard.ThrowIf(nameof(url), url).Null();

            var lookupTask = LookupAsync(@this, url);
            return lookupTask;

            // <summary>
            //      Lookup a URL Asynchronously.
            // </summary>
            async Task<DatabaseLookupResult> LookupAsync(IUnmanagedBrowsingDatabase cThis, Url cUrl) {
                DatabaseLookupResult cDatabaseLookupResult = null;
                foreach (var cUrlExpression in cUrl.Expressions) {
                    foreach (var cSha256HashPrefix in cUrlExpression.Sha256HashPrefixes) {
                        // ...
                        //
                        // Throws an exception if the operation fails.
                        var cFindThreatListsTask = cThis.FindThreatListsAsync(cSha256HashPrefix);
                        var cThreatLists = await cFindThreatListsTask.ConfigureAwait(false);
                        if (cThreatLists.Count != 0) {
                            var cSha256Hash = cUrlExpression.Sha256Hash;
                            cDatabaseLookupResult = DatabaseLookupResult.DatabaseHit(cSha256Hash, cSha256HashPrefix, cThreatLists);
                            break;
                        }
                    }

                    if (cDatabaseLookupResult != null) {
                        break;
                    }
                }

                if (cDatabaseLookupResult == null) {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    var cGetThreatListsTask = cThis.GetThreatListsAsync();
                    var cThreatLists = await cGetThreatListsTask.ConfigureAwait(false);
                    if (cThreatLists.Count == 0) {
                        cDatabaseLookupResult = DatabaseLookupResult.DatabaseStale();
                    }
                    else {
                        foreach (var cThreatList in cThreatLists) {
                            if (cThreatList.Expired) {
                                cDatabaseLookupResult = DatabaseLookupResult.DatabaseStale();
                                break;
                            }
                        }
                    }
                }

                cDatabaseLookupResult = cDatabaseLookupResult ?? DatabaseLookupResult.DatabaseMiss();
                return cDatabaseLookupResult;
            }
        }
    }
}