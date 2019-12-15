using Gee.Common.Guards;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Managed Database Extension.
    /// </summary>
    public static class ManagedBrowsingDatabaseExtension {
        /// <summary>
        ///     Modify a Threat List Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IManagedBrowsingDatabase" />.
        /// </param>
        /// <param name="threatList">
        ///     A <see cref="ThreatList" /> to modify.
        /// </param>
        /// <param name="threatSha256HashPrefixes">
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the threats
        ///     associated with <paramref name="threatList" /> and should be stored.
        /// </param>
        /// <param name="threatIndices">
        ///     A collection of zero-based indices identifying the threats associated with the lexicographically sorted
        ///     <paramref name="threatList" /> and should be removed.
        /// </param>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatList" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefixes" /> is a null reference, or if
        ///     <paramref name="threatIndices" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task ModifyThreatListAsync(this IManagedBrowsingDatabase @this, ThreatList threatList, IEnumerable<string> threatSha256HashPrefixes, IEnumerable<int> threatIndices) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var modifyThreatListTask = @this.ModifyThreatListAsync(threatList, threatSha256HashPrefixes, threatIndices, CancellationToken.None);
            return modifyThreatListTask;
        }

        /// <summary>
        ///     Store a Threat List Asynchronously.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="IManagedBrowsingDatabase" />.
        /// </param>
        /// <param name="threatList">
        ///     A <see cref="ThreatList" /> to store.
        /// </param>
        /// <param name="threatSha256HashPrefixes">
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the threats
        ///     associated with <paramref name="threatList" /> and should be stored.
        /// </param>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="this" /> is a null reference, or if <paramref name="threatList" /> is a null
        ///     reference, or if <paramref name="threatSha256HashPrefixes" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if <paramref name="this" /> is disposed.
        /// </exception>
        public static Task StoreThreatListAsync(this IManagedBrowsingDatabase @this, ThreatList threatList, IEnumerable<string> threatSha256HashPrefixes) {
            Guard.ThrowIf(nameof(@this), @this).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var storeThreatListTask = @this.StoreThreatListAsync(threatList, threatSha256HashPrefixes, CancellationToken.None);
            return storeThreatListTask;
        }
    }
}