using Gee.Common.Guards;
using Gee.External.Browsing.Clients;
using Gee.External.Browsing.Databases;
using Gee.External.Browsing.Databases.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Managed Service Builder.
    /// </summary>
    public sealed class ManagedBrowsingServiceBuilder : BaseBrowsingServiceBuilder<ManagedBrowsingServiceBuilder> {
        /// <summary>
        ///     Get and Set Database.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="IManagedBrowsingDatabase" /> to store the collection of retrieved
        ///     <see cref="ThreatList" /> in.
        /// </remarks>
        internal IManagedBrowsingDatabase Database { get; private set; }

        /// <summary>
        ///     Get and Set Own Database Flag.
        /// </summary>
        /// <remarks>
        ///     Represents a boolean flag indicating whether or not the <see cref="ManagedBrowsingService" /> takes
        ///     ownership of the <see cref="Database" /> and disposes it when the managed service itself is disposed.
        ///     If the managed service takes ownership of the database and you reference or dispose the database after
        ///     you create the managed service, the behavior of the managed service and the database is undefined.
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
        ///     Create a Managed Service Builder.
        /// </summary>
        internal ManagedBrowsingServiceBuilder() {
            this.UpdateConstraints = new Dictionary<ThreatListDescriptor, ThreatListUpdateConstraints>();
        }

        /// <summary>
        ///     Build a Managed Service.
        /// </summary>
        /// <returns>
        ///     A <see cref="ManagedBrowsingService" />.
        /// </returns>
        public ManagedBrowsingService Build() {
            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            //
            // Throws an exception if the operation fails.
            var managedService = new ManagedBrowsingService(this);
            this.Cache = null;
            this.Client = null;
            this.Database = null;
            this.OwnDatabase = false;
            this.UpdateConstraints = new Dictionary<ThreatListDescriptor, ThreatListUpdateConstraints>();

            return managedService;
        }

        /// <summary>
        ///     Restrict Updates.
        /// </summary>
        /// <param name="threatListDescriptor">
        ///     A <see cref="ThreatListDescriptor" /> identifying a <see cref="ThreatList" /> to restrict updates to.
        /// </param>
        /// <returns>
        ///     This managed service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference.
        /// </exception>
        public ManagedBrowsingServiceBuilder RestrictUpdatesTo(ThreatListDescriptor threatListDescriptor) {
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
        ///     This managed service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference, or if
        ///     <paramref name="updateConstraintsAction" /> is a null reference.
        /// </exception>
        public ManagedBrowsingServiceBuilder RestrictUpdatesTo(ThreatListDescriptor threatListDescriptor, Func<ThreatListUpdateConstraintsBuilder, ThreatListUpdateConstraints> updateConstraintsAction) {
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
        ///     This managed service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="threatListDescriptor" /> is a null reference, or if
        ///     <paramref name="updateConstraints" /> is a null reference.
        /// </exception>
        public ManagedBrowsingServiceBuilder RestrictUpdatesTo(ThreatListDescriptor threatListDescriptor, ThreatListUpdateConstraints updateConstraints) {
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
        ///     This managed service builder.
        /// </returns>
        public ManagedBrowsingServiceBuilder RestrictUpdatesTo(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType) {
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
        ///     This managed service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="updateConstraintsAction" /> is a null reference.
        /// </exception>
        public ManagedBrowsingServiceBuilder RestrictUpdatesTo(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType, Func<ThreatListUpdateConstraintsBuilder, ThreatListUpdateConstraints> updateConstraintsAction) {
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
        ///     This managed service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="updateConstraints" /> is a null reference.
        /// </exception>
        public ManagedBrowsingServiceBuilder RestrictUpdatesTo(ThreatType threatType, PlatformType platformType, ThreatEntryType threatEntryType, ThreatListUpdateConstraints updateConstraints) {
            Guard.ThrowIf(nameof(updateConstraints), updateConstraints).Null();

            var threatListDescriptor = new ThreatListDescriptor(threatType, platformType, threatEntryType);
            this.UpdateConstraints[threatListDescriptor] = updateConstraints;
            return this;
        }

        /// <summary>
        ///     Set Database.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="IManagedBrowsingDatabase" /> to store the collection of retrieved
        ///     <see cref="ThreatList" /> in.
        /// </param>
        /// <param name="ownDatabase">
        ///     A boolean flag indicating whether or not the <see cref="ManagedBrowsingService" /> takes ownership of
        ///     the database and disposes it when the managed service itself is disposed. If the managed service takes
        ///     ownership of the database and you reference or dispose the database after you create the managed
        ///     service, the behavior of the managed service and the database is undefined.
        /// </param>
        /// <returns>
        ///     This managed service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public ManagedBrowsingServiceBuilder SetDatabase(IManagedBrowsingDatabase value, bool ownDatabase) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Database = value;
            this.OwnDatabase = ownDatabase;
            return this;
        }

        /// <summary>
        ///     Use a JSON Database.
        /// </summary>
        /// <remarks>
        ///     Use a <see cref="ManagedJsonBrowsingDatabase"/> to store threat list updates retrieved from the Google
        ///     Safe Browsing API. The <see cref="ManagedBrowsingService" /> takes ownership of the JSON database and
        ///     will dispose it when the managed service itself is disposed.
        /// </remarks>
        /// <param name="databaseFilePath">
        ///     An absolute file path to the database file. If the file does not exist, it will be created.
        /// </param>
        /// <returns>
        ///     This managed service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="databaseFilePath" /> is a null reference.
        /// </exception>
        public ManagedBrowsingServiceBuilder UseJsonDatabase(string databaseFilePath) {
            // ...
            //
            // Throws an exception if the operation fails.
            var jsonDatabase = new ManagedJsonBrowsingDatabase(databaseFilePath);
            this.SetDatabase(jsonDatabase, true);
            return this;
        }

        /// <summary>
        ///     Use a Memory Database.
        /// </summary>
        /// <remarks>
        ///     Use a <see cref="MemoryBrowsingDatabase"/> to store threat list updates retrieved from the Google Safe
        ///     Browsing API. The <see cref="ManagedBrowsingService" /> takes ownership of the memory database and will
        ///     dispose it when the managed service itself is disposed.
        /// </remarks>
        /// <returns>
        ///     This managed service builder.
        /// </returns>
        public ManagedBrowsingServiceBuilder UseMemoryDatabase() {
            var memoryDatabase = new MemoryBrowsingDatabase();
            this.SetDatabase(memoryDatabase, true);
            return this;
        }
    }
}