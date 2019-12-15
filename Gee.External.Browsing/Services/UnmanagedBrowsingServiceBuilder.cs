using Gee.Common.Guards;
using Gee.External.Browsing.Databases;
using Gee.External.Browsing.Databases.Json;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Unmanaged Service Builder.
    /// </summary>
    public sealed class UnmanagedBrowsingServiceBuilder : BaseBrowsingServiceBuilder<UnmanagedBrowsingServiceBuilder> {
        /// <summary>
        ///     Get and Set Database.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="IUnmanagedBrowsingDatabase" /> to retrieve the collection of stored
        ///     <see cref="ThreatList" /> from.
        /// </remarks>
        internal IUnmanagedBrowsingDatabase Database { get; private set; }

        /// <summary>
        ///     Get and Set Own Database Flag.
        /// </summary>
        /// <remarks>
        ///     Represents a boolean flag indicating whether or not the <see cref="UnmanagedBrowsingService" /> takes
        ///     ownership of the <see cref="Database" /> and disposes it when the unmanaged service itself is disposed.
        ///     If the unmanaged service takes ownership of the database and you reference or dispose the database
        ///     after you create the unmanaged service, the behavior of the unmanaged service and the database is
        ///     undefined.
        /// </remarks>
        internal bool OwnDatabase { get; private set; }

        /// <summary>
        ///     Build an Unmanaged Service.
        /// </summary>
        /// <returns>
        ///     An <see cref="UnmanagedBrowsingService" />.
        /// </returns>
        public UnmanagedBrowsingService Build() {
            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            //
            // Throws an exception if the operation fails.
            var unmanagedService = new UnmanagedBrowsingService(this);
            this.Cache = null;
            this.Client = null;
            this.Database = null;
            this.OwnDatabase = false;

            return unmanagedService;
        }

        /// <summary>
        ///     Set Database.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="IManagedBrowsingDatabase" /> to store the collection of retrieved
        ///     <see cref="ThreatList" /> in.
        /// </param>
        /// <param name="ownDatabase">
        ///     A boolean flag indicating whether or not the <see cref="UnmanagedBrowsingService" /> takes ownership of
        ///     the database and disposes it when the unmanaged service itself is disposed. If the unmanaged service
        ///     takes ownership of the database and you reference or dispose the database after you create the
        ///     unmanaged service, the behavior of the unmanaged service and the database is undefined.
        /// </param>
        /// <returns>
        ///     This unmanaged service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public UnmanagedBrowsingServiceBuilder SetDatabase(IUnmanagedBrowsingDatabase value, bool ownDatabase) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Database = value;
            this.OwnDatabase = ownDatabase;
            return this;
        }

        /// <summary>
        ///     Use a JSON Database.
        /// </summary>
        /// <param name="databaseFilePath">
        ///     An absolute file path to the database file. If the file does not exist, it will be created.
        /// </param>
        /// <remarks>
        ///     Use a <see cref="UnmanagedJsonBrowsingDatabase"/> to store threat list updates retrieved from the
        ///     Google Safe Browsing API. The <see cref="UnmanagedBrowsingService" /> takes ownership of the JSON
        ///     database and will dispose it when the unmanaged service itself is disposed.
        /// </remarks>
        /// <returns>
        ///     This unmanaged service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="databaseFilePath" /> is a null reference.
        /// </exception>
        public UnmanagedBrowsingServiceBuilder UseJsonDatabase(string databaseFilePath) {
            // ...
            //
            // Throws an exception if the operation fails.
            var jsonDatabase = new UnmanagedJsonBrowsingDatabase(databaseFilePath);
            this.SetDatabase(jsonDatabase, true);
            return this;
        }
    }
}