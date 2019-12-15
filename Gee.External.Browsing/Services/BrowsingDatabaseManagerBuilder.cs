using Gee.Common.Guards;
using Gee.External.Browsing.Clients;
using Gee.External.Browsing.Clients.Http;
using Gee.External.Browsing.Databases;
using Gee.External.Browsing.Databases.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Database Manager Builder.
    /// </summary>
    public sealed class BrowsingDatabaseManagerBuilder {
        /// <summary>
        ///     Get and Set Client.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="IBrowsingClient" /> to retrieve the collection of <see cref="ThreatList" />
        ///     to store locally with.
        /// </remarks>
        internal IBrowsingClient Client { get; private set; }

        /// <summary>
        ///     Get and Set Database.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="IManagedBrowsingDatabase" /> to store the collection of retrieved
        ///     <see cref="ThreatList" /> in.
        /// </remarks>
        internal IManagedBrowsingDatabase Database { get; private set; }

        /// <summary>
        ///     Get and Set Own Client Flag.
        /// </summary>
        /// <remarks>
        ///     Represents a boolean flag indicating whether or not the <see cref="BrowsingDatabaseManager" /> takes
        ///     ownership of the <see cref="Client" /> and disposes it when the database manager itself is disposed. If
        ///     the database manager takes ownership of the client and you reference or dispose the client after you
        ///     create the database manager, the behavior of the database manager and the client is undefined.
        /// </remarks>
        internal bool OwnClient { get; private set; }

        /// <summary>
        ///     Get and Set Own Database Flag.
        /// </summary>
        /// <remarks>
        ///     Represents a boolean flag indicating whether or not the <see cref="BrowsingDatabaseManager" /> takes
        ///     ownership of the <see cref="Database" /> and disposes it when the database manager itself is disposed.
        ///     If the database manager takes ownership of the database and you reference or dispose the database after
        ///     you create the database manager, the behavior of the database manager and the database is undefined.
        /// </remarks>
        internal bool OwnDatabase { get; private set; }

        /// <summary>
        ///     Get and Set Update Constraints.
        /// </summary>
        /// <remarks>
        ///     Represents the collection of <see cref="ThreatList" /> to retrieve and optionally the
        ///     <see cref="ThreatListUpdateConstraints" /> to apply when retrieving them. An empty collection indicates
        ///     all threat lists available by the Google Safe Browsing API should be retrieved.
        /// </remarks>
        internal Dictionary<ThreatListDescriptor, ThreatListUpdateConstraints> UpdateConstraints { get; private set; }

        /// <summary>
        ///     Create a Database Manager Builder.
        /// </summary>
        internal BrowsingDatabaseManagerBuilder() {
            this.UpdateConstraints = new Dictionary<ThreatListDescriptor, ThreatListUpdateConstraints>();
        }

        /// <summary>
        ///     Build a Database Manager.
        /// </summary>
        /// <returns>
        ///     A <see cref="BrowsingDatabaseManager" />.
        /// </returns>
        public BrowsingDatabaseManager Build() {
            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            //
            // Throws an exception if the operation fails.
            var databaseManager = new BrowsingDatabaseManager(this);
            this.Client = null;
            this.Database = null;
            this.OwnClient = false;
            this.OwnDatabase = false;
            this.UpdateConstraints = new Dictionary<ThreatListDescriptor, ThreatListUpdateConstraints>();

            return databaseManager;
        }

        /// <summary>
        ///     Restrict Updates.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <returns>
        ///     This database synchronizer builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder RestrictUpdatesTo(ThreatListDescriptor threatListDescriptor) {
            // ...
            //
            // Throws an exception if the operation fails.
            return this.RestrictUpdatesTo(threatListDescriptor, ThreatListUpdateConstraints.Default);
        }

        /// <summary>
        ///     Restrict Updates.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="updateConstraintsAction">
        ///     An action to create the <see cref="ThreatListUpdateConstraints" /> to apply when the
        ///     <see cref="ThreatList" /> identified by <paramref name="threatListDescriptor" /> is retrieved.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference, or if
        ///     <paramref name="updateConstraintsAction" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder RestrictUpdatesTo(ThreatListDescriptor threatListDescriptor, Func<ThreatListUpdateConstraintsBuilder, ThreatListUpdateConstraints> updateConstraintsAction) {
            Guard.ThrowIf(nameof(updateConstraintsAction), updateConstraintsAction).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateConstraintsBuilder = ThreatListUpdateConstraints.Build();
            var threatListUpdateConstraints = updateConstraintsAction(threatListUpdateConstraintsBuilder);
            this.RestrictUpdatesTo(threatListDescriptor, threatListUpdateConstraints);

            return this;
        }

        /// <summary>
        ///     Restrict Updates.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="updateConstraints">
        ///     The <see cref="ThreatListUpdateConstraints" /> to apply when the <see cref="ThreatList" /> identified
        ///     by <paramref name="threatListDescriptor" /> is retrieved.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference, or if
        ///     <paramref name="updateConstraints" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder RestrictUpdatesTo(ThreatListDescriptor threatListDescriptor, ThreatListUpdateConstraints updateConstraints) {
            Guard.ThrowIf(nameof(threatListDescriptor), threatListDescriptor).Null();
            Guard.ThrowIf(nameof(updateConstraints), updateConstraints).Null();

            this.UpdateConstraints[threatListDescriptor] = updateConstraints;
            return this;
        }

        /// <summary>
        ///     Restrict Updates.
        /// </summary>
        /// <param name="threatType">
        ///     A <see cref="ThreatType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="platformType">
        ///     A <see cref="PlatformType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="threatEntryType">
        ///     A <see cref="ThreatEntryType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        public BrowsingDatabaseManagerBuilder RestrictUpdatesTo(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType) {
            return this.RestrictUpdatesTo(threatType, platformType, threatEntryType, ThreatListUpdateConstraints.Default);
        }

        /// <summary>
        ///     Restrict Updates.
        /// </summary>
        /// <param name="threatType">
        ///     A <see cref="ThreatType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="platformType">
        ///     A <see cref="PlatformType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="threatEntryType">
        ///     A <see cref="ThreatEntryType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="updateConstraintsAction">
        ///     An action to create the <see cref="ThreatListUpdateConstraints" /> to apply when the
        ///     <see cref="ThreatList" /> identified by <paramref name="threatType" />,
        ///     <paramref name="platformType" />, and <paramref name="threatEntryType" /> is retrieved.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="updateConstraintsAction" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder RestrictUpdatesTo(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType, Func<ThreatListUpdateConstraintsBuilder, ThreatListUpdateConstraints> updateConstraintsAction) {
            Guard.ThrowIf(nameof(updateConstraintsAction), updateConstraintsAction).Null();

            // ...
            //
            // Throws an exception if the operation fails.
            var threatListUpdateConstraintsBuilder = ThreatListUpdateConstraints.Build();
            var threatListUpdateConstraints = updateConstraintsAction(threatListUpdateConstraintsBuilder);
            this.RestrictUpdatesTo(threatType, platformType, threatEntryType, threatListUpdateConstraints);

            return this;
        }

        /// <summary>
        ///     Restrict Updates.
        /// </summary>
        /// <param name="threatType">
        ///     A <see cref="ThreatType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="platformType">
        ///     A <see cref="PlatformType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="threatEntryType">
        ///     A <see cref="ThreatEntryType" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <param name="updateConstraints">
        ///     The <see cref="ThreatListUpdateConstraints" /> to apply when the <see cref="ThreatList" /> identified
        ///     by <paramref name="threatType" />, <paramref name="platformType" />, and
        ///     <paramref name="threatEntryType" /> is retrieved.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="updateConstraints" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder RestrictUpdatesTo(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType, ThreatListUpdateConstraints updateConstraints) {
            Guard.ThrowIf(nameof(updateConstraints), updateConstraints).Null();

            var threatListDescriptor = new ThreatListDescriptor(threatType, platformType, threatEntryType);
            this.UpdateConstraints[threatListDescriptor] = updateConstraints;
            return this;
        }

        /// <summary>
        ///     Set Client.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="ownClient">
        ///     A boolean flag indicating whether or not the <see cref="BrowsingDatabaseManager" /> takes ownership of
        ///     the client and disposes it when the database manager itself is disposed. If the database manager takes
        ///     ownership of the client and you reference or dispose the client after you create the database manager,
        ///     the behavior of the database manager and the client is undefined.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder SetClient(IBrowsingClient value, bool ownClient) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Client = value;
            this.OwnClient = ownClient;
            return this;
        }

        /// <summary>
        ///     Set Database.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="IManagedBrowsingDatabase" />.
        /// </param>
        /// <param name="ownDatabase">
        ///     A boolean flag indicating whether or not the <see cref="BrowsingDatabaseManager" /> takes ownership of
        ///     the database and disposes it when the database manager itself is disposed. If the database manager
        ///     takes ownership of the database and you reference or dispose the database after you create the database
        ///     manager, the behavior of the database manager and the database is undefined.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder SetDatabase(IManagedBrowsingDatabase value, bool ownDatabase) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Database = value;
            this.OwnDatabase = ownDatabase;
            return this;
        }

        /// <summary>
        ///     Set Update Constraints.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder SetUpdateConstraints(IDictionary<ThreatListDescriptor, ThreatListUpdateConstraints> value) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.UpdateConstraints = new Dictionary<ThreatListDescriptor, ThreatListUpdateConstraints>(value);
            return this;
        }

        /// <summary>
        ///     Use an HTTP Client.
        /// </summary>
        /// <remarks>
        ///     Use an <see cref="HttpBrowsingClient" /> to communicate with the Google Safe Browsing API. The
        ///     <see cref="BrowsingDatabaseManager" /> takes ownership of the HTTP client and will dispose it when the
        ///     database manager itself is disposed.
        /// </remarks>
        /// <param name="apiKey">
        ///     A Google Safe Browsing API key to authenticate to the Google Safe Browsing API with.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="apiKey" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder UseHttpClient(string apiKey) {
            // ...
            //
            // Throws an exception if the operation fails.
            var httpClient = new HttpBrowsingClient(apiKey);
            this.SetClient(httpClient, true);
            return this;
        }

        /// <summary>
        ///     Use a JSON Database.
        /// </summary>
        /// <remarks>
        ///     Use a <see cref="ManagedJsonBrowsingDatabase"/> to store threat list updates retrieved from the Google
        ///     Safe Browsing API. The <see cref="BrowsingDatabaseManager" /> takes ownership of the JSON database and
        ///     will dispose it when the database manager itself is disposed.
        /// </remarks>
        /// <param name="databaseFilePath">
        ///     An absolute file path to the database file. If the file does not exist, it will be created.
        /// </param>
        /// <returns>
        ///     This database manager builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="databaseFilePath" /> is a null reference.
        /// </exception>
        public BrowsingDatabaseManagerBuilder UseJsonDatabase(string databaseFilePath) {
            // ...
            //
            // Throws an exception if the operation fails.
            var jsonDatabase = new ManagedJsonBrowsingDatabase(databaseFilePath);
            this.SetDatabase(jsonDatabase, true);
            return this;
        }
    }
}