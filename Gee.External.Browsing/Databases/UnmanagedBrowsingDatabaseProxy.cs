using Gee.Common.Guards;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Unmanaged Database Proxy.
    /// </summary>
    internal sealed class UnmanagedBrowsingDatabaseProxy : IUnmanagedBrowsingDatabase {
        /// <summary>
        ///     Database.
        /// </summary>
        private readonly IUnmanagedBrowsingDatabase _database;

        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Own Database Flag.
        /// </summary>
        private readonly bool _ownDatabase;

        /// <summary>
        ///     Create an Unmanaged Database Proxy.
        /// </summary>
        /// <param name="database">
        ///     An <see cref="IUnmanagedBrowsingDatabase" /> to proxy to. The unmanaged database proxy takes ownership
        ///     of <paramref name="database" /> and will dispose it when the unmanaged database proxy itself is
        ///     disposed. If you reference or dispose <paramref name="database" /> after you create the unmanaged
        ///     database proxy, the behavior of the unmanaged database proxy and <paramref name="database" /> is
        ///     undefined.
        /// </param>
        /// <returns>
        ///     An unmanaged database proxy.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        internal static UnmanagedBrowsingDatabaseProxy Create(IUnmanagedBrowsingDatabase database) => UnmanagedBrowsingDatabaseProxy.Create(database, true);

        /// <summary>
        ///     Create an Unmanaged Database Proxy.
        /// </summary>
        /// <param name="database">
        ///     An <see cref="IUnmanagedBrowsingDatabase" /> to proxy to.
        /// </param>
        /// <param name="ownDatabase">
        ///     A boolean flag indicating whether or not the unmanaged database proxy takes ownership of
        ///     <paramref name="database" /> and disposes it when the unmanaged database proxy itself is disposed. If
        ///     the unmanaged database proxy takes ownership of <paramref name="database" /> and you reference or
        ///     dispose <paramref name="database" /> after you create the unmanaged database proxy, the behavior of the
        ///     unmanaged database proxy and <paramref name="database" /> is undefined.
        /// </param>
        /// <returns>
        ///     An unmanaged database proxy.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        internal static UnmanagedBrowsingDatabaseProxy Create(IUnmanagedBrowsingDatabase database, bool ownDatabase) {
            // ...
            //
            // Throws an exception if the operation fails.
            return database is UnmanagedBrowsingDatabaseProxy databaseProxy ? databaseProxy : new UnmanagedBrowsingDatabaseProxy(database, ownDatabase);
        }

        /// <summary>
        ///     Create an Unmanaged Database Proxy.
        /// </summary>
        /// <param name="database">
        ///     An <see cref="IUnmanagedBrowsingDatabase" /> to proxy to. The unmanaged database proxy takes ownership
        ///     of <paramref name="database" /> and will dispose it when the unmanaged database proxy itself is
        ///     disposed. If you reference or dispose <paramref name="database" /> after you create the unmanaged
        ///     database proxy, the behavior of the unmanaged database proxy and <paramref name="database" /> is
        ///     undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        [SuppressMessage("ReSharper", "IntroduceOptionalParameters.Local")]
        private UnmanagedBrowsingDatabaseProxy(IUnmanagedBrowsingDatabase database) : this(database, true) { }

        /// <summary>
        ///     Create an Unmanaged Database Proxy.
        /// </summary>
        /// <param name="database">
        ///     An <see cref="IUnmanagedBrowsingDatabase" /> to proxy to.
        /// </param>
        /// <param name="ownDatabase">
        ///     A boolean flag indicating whether or not the unmanaged database proxy takes ownership of
        ///     <paramref name="database" /> and disposes it when the unmanaged database proxy itself is disposed. If
        ///     the unmanaged database proxy takes ownership of <paramref name="database" /> and you reference or
        ///     dispose <paramref name="database" /> after you create the unmanaged database proxy, the behavior of the
        ///     unmanaged database proxy and <paramref name="database" /> is undefined.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="database" /> is a null reference.
        /// </exception>
        private UnmanagedBrowsingDatabaseProxy(IUnmanagedBrowsingDatabase database, bool ownDatabase) {
            Guard.ThrowIf(nameof(database), database).Null();

            this._database = database;
            this._disposed = false;
            this._ownDatabase = ownDatabase;
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
                this._disposed = true;
                if (this._ownDatabase) {
                    this._database.Dispose();
                }
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
        public async Task<IReadOnlyCollection<ThreatList>> FindThreatListsAsync(string threatSha256HashPrefix, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var findThreatListsTask = this._database.FindThreatListsAsync(threatSha256HashPrefix, cancellationToken);
                var threatLists = await findThreatListsTask.ConfigureAwait(false);
                return threatLists;
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingDatabaseException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "A threat's associated threat lists could not be retrieved.";
                throw new BrowsingDatabaseException(detailMessage, ex);
            }
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
        public async Task<ThreatList> GetThreatListAsync(ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var getThreatListTask = this._database.GetThreatListAsync(threatListDescriptor, cancellationToken);
                var threatList = await getThreatListTask.ConfigureAwait(false);
                return threatList;
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingDatabaseException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "A threat list could not be retrieved.";
                throw new BrowsingDatabaseException(detailMessage, ex);
            }
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
        public async Task<IReadOnlyCollection<ThreatList>> GetThreatListsAsync(CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var getThreatListsTask = this._database.GetThreatListsAsync(cancellationToken);
                var threatLists = await getThreatListsTask.ConfigureAwait(false);
                return threatLists;
            }
            catch (BrowsingDatabaseException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "A collection of threat lists could not be retrieved.";
                throw new BrowsingDatabaseException(detailMessage, ex);
            }
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
        public async Task<IReadOnlyCollection<string>> GetThreatsAsync(ThreatListDescriptor threatListDescriptor, CancellationToken cancellationToken) {
            this.ThrowIfDisposed();
            try {
                var getThreatsTask = this._database.GetThreatsAsync(threatListDescriptor, cancellationToken);
                var threats = await getThreatsTask.ConfigureAwait(false);
                return threats;
            }
            catch (ArgumentNullException) {
                throw;
            }
            catch (BrowsingDatabaseException) {
                throw;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                const string detailMessage = "A threat list's associated threats could not be retrieved.";
                throw new BrowsingDatabaseException(detailMessage, ex);
            }
        }

        /// <summary>
        ///     Throw an Exception if Object is Disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        private void ThrowIfDisposed() {
            if (this._disposed) {
                var detailMessage = $"An object ({nameof(UnmanagedBrowsingDatabaseProxy)}) is disposed.";
                throw new ObjectDisposedException(nameof(UnmanagedBrowsingDatabaseProxy), detailMessage);
            }
        }
    }
}