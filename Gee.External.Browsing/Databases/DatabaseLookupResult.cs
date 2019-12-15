using Gee.Common;
using Gee.Common.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Database Lookup Result.
    /// </summary>
    public sealed class DatabaseLookupResult {
        /// <summary>
        ///     SHA256 Hash.
        /// </summary>
        private readonly string _sha256Hash;

        /// <summary>
        ///     SHA256 Hash Prefix.
        /// </summary>
        private readonly string _sha256HashPrefix;

        /// <summary>
        ///     Threat Lists.
        /// </summary>
        private readonly HashSet<ThreatList> _threatLists;

        /// <summary>
        ///     Determine if <see cref="DatabaseLookupResult" /> Indicates a Database Hit.
        /// </summary>
        /// <remarks>
        ///     Conveniently determines if the <see cref="DatabaseLookupResult" /> indicates a database hit; that is
        ///     <see cref="ResultCode" /> is equal to <see cref="DatabaseLookupResultCode.Hit" />.
        /// </remarks>
        public bool IsDatabaseHit => this.ResultCode == DatabaseLookupResultCode.Hit;

        /// <summary>
        ///     Determine if <see cref="DatabaseLookupResult" /> Indicates a Database Miss.
        /// </summary>
        /// <remarks>
        ///     Conveniently determines if the <see cref="DatabaseLookupResult" /> indicates a database miss; that is
        ///     <see cref="ResultCode" /> is equal to <see cref="DatabaseLookupResultCode.Miss" />.
        /// </remarks>
        public bool IsDatabaseMiss => this.ResultCode == DatabaseLookupResultCode.Miss;

        /// <summary>
        ///     Determine if <see cref="DatabaseLookupResult" /> Indicates a Stale Database.
        /// </summary>
        /// <remarks>
        ///     Conveniently determines if the <see cref="DatabaseLookupResult" /> indicates a stale database; that is
        ///     <see cref="ResultCode" /> is equal to <see cref="DatabaseLookupResultCode.Stale" />.
        /// </remarks>
        public bool IsDatabaseStale => this.ResultCode == DatabaseLookupResultCode.Stale;

        /// <summary>
        ///     Get <see cref="DatabaseLookupResultCode" />.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="DatabaseLookupResultCode" /> indicating the nature of the
        ///     <see cref="DatabaseLookupResult" />.
        /// </remarks>
        public DatabaseLookupResultCode ResultCode { get; }

        /// <summary>
        ///     Get SHA256 Hash.
        /// </summary>
        /// <remarks>
        ///     Represents the full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat
        ///     that was looked up in a local database.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if <see cref="ResultCode" /> is not equal to <see cref="DatabaseLookupResultCode.Hit" />.
        /// </exception>
        public string Sha256Hash {
            get {
                if (!this.IsDatabaseHit) {
                    var detailMessage = $"An operation ({nameof(DatabaseLookupResult.Sha256Hash)}) is invalid.";
                    throw new InvalidOperationException(detailMessage);
                }

                return this._sha256Hash;
            }
        }

        /// <summary>
        ///     Get SHA256 Hash Prefix.
        /// </summary>
        /// <remarks>
        ///     Represents the SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat
        ///     that was looked up in a local database.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if <see cref="ResultCode" /> is not equal to <see cref="DatabaseLookupResultCode.Hit" />.
        /// </exception>
        public string Sha256HashPrefix {
            get {
                if (!this.IsDatabaseHit) {
                    var detailMessage = $"An operation ({nameof(DatabaseLookupResult.Sha256HashPrefix)}) is invalid.";
                    throw new InvalidOperationException(detailMessage);
                }

                return this._sha256HashPrefix;
            }
        }

        /// <summary>
        ///     Get Threat Lists.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of threat lists the threat identified by <see cref="Sha256Hash" /> and
        ///     <see cref="Sha256HashPrefix" /> is associated with if, and only if, <see cref="ResultCode" /> is equal
        ///     to <see cref="DatabaseLookupResultCode.Hit" />.
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown if <see cref="ResultCode" /> is not equal to <see cref="DatabaseLookupResultCode.Hit" />.
        /// </exception>
        public IEnumerable<ThreatList> ThreatLists {
            get {
                if (!this.IsDatabaseHit) {
                    var detailMessage = $"An operation ({nameof(DatabaseLookupResult.ThreatLists)}) is invalid.";
                    throw new InvalidOperationException(detailMessage);
                }

                return this._threatLists;
            }
        }

        /// <summary>
        ///     Create a <see cref="DatabaseLookupResult" /> Indicating a Database Hit.
        /// </summary>
        /// <param name="sha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat that was looked
        ///     up in a local database.
        /// </param>
        /// <param name="sha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat that was
        ///     looked up in a local database.
        /// </param>
        /// <param name="threatLists">
        ///     A collection of threat lists the threat identified by <paramref name="sha256Hash" /> and
        ///     <paramref name="sha256HashPrefix" /> is associated with.
        /// </param>
        /// <returns>
        ///     A <see cref="DatabaseLookupResult" /> indicating a database hit.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DatabaseLookupResult DatabaseHit(string sha256Hash, string sha256HashPrefix, IEnumerable<ThreatList> threatLists) {
            // ...
            //
            // Throws an exception if the operation fails.
            return new DatabaseLookupResult(DatabaseLookupResultCode.Hit, sha256Hash, sha256HashPrefix, threatLists);
        }

        /// <summary>
        ///     Create a <see cref="DatabaseLookupResult" /> Indicating a Database Miss.
        /// </summary>
        /// <returns>
        ///     A <see cref="DatabaseLookupResult" /> indicating a database miss.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DatabaseLookupResult DatabaseMiss() {
            // ...
            //
            // Throws an exception if the operation fails.
            return new DatabaseLookupResult(DatabaseLookupResultCode.Miss, null, null, null);
        }

        /// <summary>
        ///     Create a <see cref="DatabaseLookupResult" /> Indicating a Stale Database.
        /// </summary>
        /// <returns>
        ///     A <see cref="DatabaseLookupResult" /> indicating a stale database.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DatabaseLookupResult DatabaseStale() {
            // ...
            //
            // Throws an exception if the operation fails.
            return new DatabaseLookupResult(DatabaseLookupResultCode.Stale, null, null, null);
        }

        /// <summary>
        ///     Create a Database Lookup Result.
        /// </summary>
        /// <param name="resultCode">
        ///     A <see cref="DatabaseLookupResultCode" /> indicating the nature of the <see cref="DatabaseLookupResult" />.
        /// </param>
        /// <param name="sha256Hash">
        ///     A full SHA256 hash, formatted as a hexadecimal encoded string, identifying the threat that was looked
        ///     up in a local database.
        /// </param>
        /// <param name="sha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying the threat that was
        ///     looked up in a local database.
        /// </param>
        /// <param name="threatLists">
        ///     A collection of threat lists the threat identified by <paramref name="sha256Hash" /> and
        ///     <paramref name="sha256HashPrefix" /> is associated with if, and only if,
        ///     <paramref name="resultCode" /> is equal to <see cref="DatabaseLookupResultCode.Hit" />. A null
        ///     reference indicates <paramref name="resultCode" /> is not equal to
        ///     <see cref="DatabaseLookupResultCode.Hit" />.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="sha256Hash" /> is a null reference, or if
        ///     <paramref name="sha256HashPrefix" /> is a null reference, or if <paramref name="resultCode" /> is
        ///     equal to <see cref="DatabaseLookupResultCode.Hit" /> and <paramref name="threatLists" /> is a null
        ///     reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Thrown if <paramref name="sha256Hash" /> is not formatted as a hexadecimal encoded string, or if
        ///     <paramref name="sha256HashPrefix" /> is not formatted as a hexadecimal encoded string.
        /// </exception>
        private DatabaseLookupResult(DatabaseLookupResultCode resultCode, string sha256Hash, string sha256HashPrefix, IEnumerable<ThreatList> threatLists) {
            if (resultCode == DatabaseLookupResultCode.Hit) {
                // ...
                //
                // Throws an exception if the operation fails.
                var isSha256HashHexadecimalEncoded = sha256Hash.IsHexadecimalEncoded();
                if (!isSha256HashHexadecimalEncoded) {
                    var detailMessage = $"A SHA256 hash ({sha256Hash}) is not formatted as a hexadecimal encoded string.";
                    throw new FormatException(detailMessage);
                }
            }

            if (resultCode == DatabaseLookupResultCode.Hit) {
                // ...
                //
                // Throws an exception if the operation fails.
                var isSha256HashPrefixHexadecimalEncoded = sha256Hash.IsHexadecimalEncoded();
                if (!isSha256HashPrefixHexadecimalEncoded) {
                    var detailMessage = $"A SHA256 hash ({sha256HashPrefix}) is not formatted as a hexadecimal encoded string.";
                    throw new FormatException(detailMessage);
                }
            }

            if (resultCode == DatabaseLookupResultCode.Hit && threatLists == null) {
                var detailMessage = $"A parameter ({nameof(threatLists)}) cannot be a null reference.";
                throw new ArgumentNullException(nameof(threatLists), detailMessage);
            }

            this._sha256Hash = sha256Hash;
            this._sha256HashPrefix = sha256HashPrefix;
            this._threatLists = threatLists.ValueOrEmpty().AsHashSet(true);
            this.ResultCode = resultCode;
        }

        /// <summary>
        ///     Determine if This Object is Equal to Another Object.
        /// </summary>
        /// <param name="object">
        ///     An object to compare to.
        /// </param>
        /// <returns>
        ///     A boolean true if this object is equal to the object. A boolean false otherwise.
        /// </returns>
        public override bool Equals(object @object) {
            var isEqual = @object != null &&
                          @object is DatabaseLookupResult databaseLookupResult &&
                          this.ResultCode == databaseLookupResult.ResultCode &&
                          this.Sha256Hash == databaseLookupResult.Sha256Hash &&
                          this.Sha256HashPrefix == databaseLookupResult.Sha256HashPrefix &&
                          this.ThreatLists.SequentiallyEquals(databaseLookupResult.ThreatLists);

            return isEqual;
        }

        /// <summary>
        ///     Get Object's Hash Code.
        /// </summary>
        /// <returns>
        ///     The object's hash code.
        /// </returns>
        public override int GetHashCode() {
            var hashCode = 13;
            hashCode = hashCode * 7 + this.ResultCode.GetHashCode();
            hashCode = hashCode * 7 + this.Sha256Hash.GetHashCodeOrDefault();
            hashCode = hashCode * 7 + this.Sha256HashPrefix.GetHashCodeOrDefault();
            hashCode = hashCode * 7 + this.ThreatLists.ComputeHashCode();

            return hashCode;
        }

        /// <summary>
        ///     Get Object's String Representation.
        /// </summary>
        /// <returns>
        ///     The object's string representation.
        /// </returns>
        public override string ToString() {
            return this._sha256Hash;
        }
    }
}