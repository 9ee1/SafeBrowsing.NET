using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Resilient Managed Database.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Represents a resilient managed database that automatically retries failed operations performed on a
    ///         <see cref="IManagedBrowsingDatabase" />. The number of retry attempts is either caller or
    ///         implementation specific if you do not specify a value. Between each retry attempt, the resilient
    ///         managed database will pause for an implementation specific interval. If all retry attempts are
    ///         exhausted and the attempted operation never succeeds, the exception the failed operation threw will be
    ///         propagated up to you.
    ///     </para>
    ///     <para>
    ///         If you specify a number of retry attempts when you create a resilient managed database, be practical
    ///         with the value you specify. The resilient unmanaged database will pause for an implementation specific
    ///         interval between each retry attempt, which effectively means if an attempted operation always fails,
    ///         the exception it threw will not be propagated up to you until all the retry attempts are exhausted. If
    ///         you specify a very high number of retry attempts, this could have self inflicted performance
    ///         implications.
    ///     </para>
    ///     <para>
    ///         When you create a resilient managed database, you can specify whether or not it takes ownership of the
    ///         managed database you want to proxy to and dispose it when the resilient managed database itself is
    ///         disposed. The recommended behavior is that you allow the resilient managed database to take ownership
    ///         of the managed database you want to proxy to but take note that if you reference or dispose the managed
    ///         database you want to proxy to after you create the resilient managed database, the behavior of the
    ///         resilient managed database and managed database you want to proxy to is undefined.
    ///     </para>
    ///     <para>
    ///         Since a resilient managed database itself implements <see cref="IManagedBrowsingDatabase" />, it is
    ///         technically possible to create a new resilient managed database for an existing resilient managed
    ///         database, though the reasons for doing so, in most cases, are unjustified. To avoid doing do, consider
    ///         creating a resilient managed database using <see cref="Create(IManagedBrowsingDatabase)" />, or one of
    ///         its overloads, instead of one of the constructor overloads.
    ///         <see cref="Create(IManagedBrowsingDatabase)" /> will conveniently create a resilient managed database
    ///         if, and only if, the managed database you want to proxy to itself is not a resilient managed database.
    ///     </para>
    /// </remarks>
    public sealed class ResilientManagedBrowsingDatabase : BaseResilientBrowsingDatabase, IManagedBrowsingDatabase {
        /// <summary>
        ///     Database.
        /// </summary>
        private readonly IManagedBrowsingDatabase _database;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Own Database Flag.
        /// </summary>
        private readonly bool _ownDatabase;

        /// <summary>
        ///     Create a Resilient Managed Database.
        /// </summary>
        /// <param name="database">
        ///     A <see cref="IManagedBrowsingDatabase" /> to proxy to. The resilient managed database takes ownership
        ///     of <paramref name="database" /> and will dispose it when the resilient managed database itself is
        ///     disposed. If you reference or dispose <paramref name="database" /> after you create the resilient
        ///     managed database, the behavior of the resilient managed database and <paramref name="database" /> is
        ///     undefined.
        /// </param>
        /// <returns>
        ///     A resilient managed database.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        public static ResilientManagedBrowsingDatabase Create(IManagedBrowsingDatabase database) => ResilientManagedBrowsingDatabase.Create(database, 5);

        /// <summary>
        ///     Create a Resilient Managed Database.
        /// </summary>
        /// <param name="database">
        ///     A <see cref="IManagedBrowsingDatabase" /> to proxy to. The resilient managed database takes ownership
        ///     of <paramref name="database" /> and will dispose it when the resilient managed database itself is
        ///     disposed. If you reference or dispose <paramref name="database" /> after you create the resilient
        ///     managed database, the behavior of the resilient managed database and <paramref name="database" /> is
        ///     undefined.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <returns>
        ///     A resilient managed database.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public static ResilientManagedBrowsingDatabase Create(IManagedBrowsingDatabase database, int retryAttempts) => ResilientManagedBrowsingDatabase.Create(database, retryAttempts, true);

        /// <summary>
        ///     Create a Resilient Managed Database.
        /// </summary>
        /// <param name="database">
        ///     A <see cref="IManagedBrowsingDatabase" /> to proxy to.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <param name="ownDatabase">
        ///     A boolean flag indicating whether or not the resilient managed database takes ownership of
        ///     <paramref name="database" /> and disposes it when the resilient managed database itself is disposed. If
        ///     the resilient managed database takes ownership of <paramref name="database" /> and you reference or
        ///     dispose <paramref name="database" /> after you create the resilient managed database, the behavior of
        ///     the resilient managed database and <paramref name="database" /> is undefined.
        /// </param>
        /// <returns>
        ///     A resilient managed database.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public static ResilientManagedBrowsingDatabase Create(IManagedBrowsingDatabase database, int retryAttempts, bool ownDatabase) {
            // ...
            //
            // Throws an exception if the operation fails.
            return database is ResilientManagedBrowsingDatabase resilientDatabase ? resilientDatabase : new ResilientManagedBrowsingDatabase(database, retryAttempts, ownDatabase);
        }

        /// <summary>
        ///     Create a Resilient Managed Database.
        /// </summary>
        /// <param name="database">
        ///     A <see cref="IManagedBrowsingDatabase" /> to proxy to. The resilient managed database takes ownership
        ///     of <paramref name="database" /> and will dispose it when the resilient managed database itself is
        ///     disposed. If you reference or dispose <paramref name="database" /> after you create the resilient
        ///     managed database, the behavior of the resilient managed database and <paramref name="database" /> is
        ///     undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        public ResilientManagedBrowsingDatabase(IManagedBrowsingDatabase database) : this(database, 5) { }

        /// <summary>
        ///     Create a Resilient Managed Database.
        /// </summary>
        /// <param name="database">
        ///     A <see cref="IManagedBrowsingDatabase" /> to proxy to. The resilient managed database takes ownership
        ///     of <paramref name="database" /> and will dispose it when the resilient managed database itself is
        ///     disposed. If you reference or dispose <paramref name="database" /> after you create the resilient
        ///     managed database, the behavior of the resilient managed database and <paramref name="database" /> is
        ///     undefined.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public ResilientManagedBrowsingDatabase(IManagedBrowsingDatabase database, int retryAttempts) : this(database, retryAttempts, true) { }

        /// <summary>
        ///     Create a Resilient Managed Database.
        /// </summary>
        /// <param name="database">
        ///     A <see cref="IManagedBrowsingDatabase" /> to proxy to.
        /// </param>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <param name="ownDatabase">
        ///     A boolean flag indicating whether or not the resilient managed database takes ownership of
        ///     <paramref name="database" /> and disposes it when the resilient managed database itself is disposed. If
        ///     the resilient managed database takes ownership of <paramref name="database" /> and you reference or
        ///     dispose <paramref name="database" /> after you create the resilient managed database, the behavior of
        ///     the resilient managed database and <paramref name="database" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        public ResilientManagedBrowsingDatabase(IManagedBrowsingDatabase database, int retryAttempts, bool ownDatabase) : base(retryAttempts) {
            Guard.ThrowIf(nameof(retryAttempts), retryAttempts).LessThanOrEqualTo(0);

            this._database = ManagedBrowsingDatabaseProxy.Create(database, ownDatabase);
            this._disposed = false;
            this._ownDatabase = ownDatabase;
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        /// <param name="disposing">
        ///     A boolean true if the object is being disposed by a caller. A boolean false if the object is being
        ///     disposed by a finalizer.
        /// </param>
        private protected override void Dispose(bool disposing) {
            if (!this._disposed) {
                this._disposed = true;
                if (disposing && this._ownDatabase) {
                    this._database.Dispose();
                }

                base.Dispose(disposing);
            }
        }

        /// <summary>
        ///     Find Threat Lists Asynchronously.
        /// </summary>
        /// <param name="threatSha256HashPrefix">
        ///     A SHA256 hash prefix, formatted as a hexadecimal encoded string, identifying a threat associated with
        ///     the collection of <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
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
        ///     Thrown if <paramref name="threatSha256HashPrefix" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<IReadOnlyCollection<ThreatList>> FindThreatListsAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<IReadOnlyCollection<ThreatList>>> resiliencyPolicyAction = () => this._database.FindThreatListsAsync(threatSha256HashPrefix, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Get a Threat List Asynchronously.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> to retrieve.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     The <see cref="ThreatList" /> identified by <paramref name="threatListDescriptor" />. A null reference
        ///     indicates a threat list could not be found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<ThreatList> GetThreatListAsync(ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<ThreatList>> resiliencyPolicyAction = () => this._database.GetThreatListAsync(threatListDescriptor, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Get Threat Lists Asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A collection of <see cref="ThreatList" />. An empty collection indicates no threat lists were found.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<IReadOnlyCollection<ThreatList>> GetThreatListsAsync(CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<IReadOnlyCollection<ThreatList>>> resiliencyPolicyAction = () => this._database.GetThreatListsAsync(cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Get Threats Asynchronously.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying the <see cref="ThreatList" /> the threats that should
        ///     be retrieved are associated with.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
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
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task<IReadOnlyCollection<string>> GetThreatsAsync(ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task<IReadOnlyCollection<string>>> resiliencyPolicyAction = () => this._database.GetThreatsAsync(threatListDescriptor, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Modify a Threat List Asynchronously.
        /// </summary>
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
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatList" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefixes" /> is a null reference, or if
        ///     <paramref name="threatIndices" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task ModifyThreatListAsync(ThreatList threatList, IEnumerable<string> threatSha256HashPrefixes, IEnumerable<int> threatIndices, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task> resiliencyPolicyAction = () => this._database.ModifyThreatListAsync(threatList, threatSha256HashPrefixes, threatIndices, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Store a Threat List Asynchronously.
        /// </summary>
        /// <param name="threatList">
        ///     A <see cref="ThreatList" /> to store.
        /// </param>
        /// <param name="threatSha256HashPrefixes">
        ///     A collection of SHA256 hash prefixes, formatted as hexadecimal encoded strings, identifying the threats
        ///     associated with <paramref name="threatList" /> and should be stored.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token to cancel the asynchronous operation with.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if a database error occurs.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatList" /> is a null reference, or if
        ///     <paramref name="threatSha256HashPrefixes" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        ///     Thrown if the asynchronous operation is cancelled.
        /// </exception>
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public Task StoreThreatListAsync(ThreatList threatList, IEnumerable<string> threatSha256HashPrefixes, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();

            Func<Task> resiliencyPolicyAction = () => this._database.StoreThreatListAsync(threatList, threatSha256HashPrefixes, cancellationToken);
            var executeResiliencyPolicyTask = this.ExecuteResiliencyPolicyAsync(resiliencyPolicyAction);
            return executeResiliencyPolicyTask;
        }

        /// <summary>
        ///     Throw an Exception if Object is Disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        private void ThrowIfDisposed() {
            if (this._disposed) {
                var detailMessage = $"An object ({nameof(ResilientManagedBrowsingDatabase)}) is disposed.";
                throw new ObjectDisposedException(nameof(ResilientManagedBrowsingDatabase), detailMessage);
            }
        }
    }
}