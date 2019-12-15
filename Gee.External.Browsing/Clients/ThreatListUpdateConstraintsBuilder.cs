using Gee.Common.Guards;
using Gee.External.Browsing.Databases;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Constraints Builder.
    /// </summary>
    public sealed class ThreatListUpdateConstraintsBuilder {
        /// <summary>
        ///     Get and Set Client Location.
        /// </summary>
        /// <remarks>
        ///     Represents the geographic location, formatted as an ISO 31166-1 alpha-2 region code, of a client. An
        ///     invalid geographic location is ignored by the Google Safe Browsing API. A null reference indicates the
        ///     geographic location of the client is unknown.
        /// </remarks>
        internal string ClientLocation { get; private set; }

        /// <summary>
        ///     Get and Set Maximum Database Entries.
        /// </summary>
        /// <remarks>
        ///     Represents the maximum number of threats associated with a <see cref="ThreatList" /> a client is
        ///     willing, or is capable, of storing in its local <see cref="IManagedBrowsingDatabase" />. A <c>0</c>
        ///     indicates there is no limit to the number of threats the client is willing to store.
        /// </remarks>
        internal int MaximumDatabaseEntries { get; private set; }

        /// <summary>
        ///     Get and Set Maximum Response Entries.
        /// </summary>
        /// <remarks>
        ///     Represents the maximum number of threats associated with a <see cref="ThreatList" /> that will be
        ///     retrieved in a single request. A <c>0</c> indicates there is no limit to the number of threats that
        ///     will be retrieved.
        /// </remarks>
        internal int MaximumResponseEntries { get; private set; }

        /// <summary>
        ///     Get and Set Threat List Language.
        /// </summary>
        /// <remarks>
        ///     Represents the language, formatted as an ISO 639 alpha-2 language code, a <see cref="ThreatList" />
        ///     should be retrieved for. An invalid language is ignored by the Google Safe Browsing API. A null
        ///     reference indicates a language should not be considered.
        /// </remarks>
        internal string ThreatListLanguage { get; private set; }

        /// <summary>
        ///     Get and Set Threat List Location.
        /// </summary>
        /// <remarks>
        ///     Represents the geographic location, formatted as an ISO 31166-1 alpha-2 region code, a
        ///     <see cref="ThreatList" /> should be retrieved for. An invalid geographic location is ignored by the
        ///     Google Safe Browsing API. A null reference indicates a geographic location should not be considered.
        /// </remarks>
        internal string ThreatListLocation { get; private set; }

        /// <summary>
        ///     Build a Threat List Update Constraints.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateConstraints" />.
        /// </returns>
        public ThreatListUpdateConstraints Build() {
            var threatListUpdateConstraints = new ThreatListUpdateConstraints(this);

            // ...
            //
            // Reinitialize the builder's state to prevent it from corrupting the immutable built object's state after
            // its built. If the object holds a reference to the builder's state, any mutation to the builder's state
            // will be reflected in the built object's state.
            this.ClientLocation = null;
            this.MaximumDatabaseEntries = default;
            this.MaximumResponseEntries = default;
            this.ThreatListLanguage = null;
            this.ThreatListLocation = null;

            return threatListUpdateConstraints;
        }

        /// <summary>
        ///     Set Client Location.
        /// </summary>
        /// <param name="value">
        ///     The geographic location, formatted as an ISO 31166-1 alpha-2 region code, of a client. An invalid
        ///     geographic location is ignored by the Google Safe Browsing API. A null reference indicates the
        ///     geographic location of the client is unknown.
        /// </param>
        /// <returns>
        ///     This threat list update constraints builder.
        /// </returns>
        public ThreatListUpdateConstraintsBuilder SetClientLocation(string value) {
            this.ClientLocation = value;
            return this;
        }

        /// <summary>
        ///     Set Maximum Database Entries.
        /// </summary>
        /// <param name="value">
        ///     The maximum number of threats associated with a <see cref="ThreatList" /> a client is willing, or is
        ///     capable, of storing in its local <see cref="IManagedBrowsingDatabase" />. A <c>0</c> indicates there is
        ///     no limit to the number of threats the client is willing to store.
        /// </param>
        /// <returns>
        ///     This threat list update constraints builder.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="value" /> is less than <c>0</c>.
        /// </exception>
        public ThreatListUpdateConstraintsBuilder SetMaximumDatabaseEntries(int value) {
            Guard.ThrowIf(nameof(value), value).LessThan(0);

            this.MaximumDatabaseEntries = value;
            return this;
        }

        /// <summary>
        ///     Set Maximum Response Entries.
        /// </summary>
        /// <param name="value">
        ///     The maximum number of threats associated with a <see cref="ThreatList" /> that will be retrieved in a
        ///     single request. A <c>0</c> indicates there is no limit to the number of threats that will be
        ///     retrieved.
        /// </param>
        /// <returns>
        ///     This threat list update constraints builder.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="value" /> is less than <c>0</c>.
        /// </exception>
        public ThreatListUpdateConstraintsBuilder SetMaximumResponseEntries(int value) {
            Guard.ThrowIf(nameof(value), value).LessThan(0);

            this.MaximumResponseEntries = value;
            return this;
        }

        /// <summary>
        ///     Set Threat List Language.
        /// </summary>
        /// <param name="value">
        ///     The language, formatted as an ISO 639 alpha-2 language code, a <see cref="ThreatList" /> should be
        ///     retrieved for. An invalid language is ignored by the Google Safe Browsing API. A null reference
        ///     indicates a language should not be considered.
        /// </param>
        /// <returns>
        ///     This threat list update constraints builder.
        /// </returns>
        public ThreatListUpdateConstraintsBuilder SetThreatListLanguage(string value) {
            this.ThreatListLanguage = value;
            return this;
        }

        /// <summary>
        ///     Set Threat List Location.
        /// </summary>
        /// <param name="value">
        ///     The geographic location, formatted as an ISO 31166-1 alpha-2 region code, a <see cref="ThreatList" />
        ///     should be retrieved for. An invalid geographic location is ignored by the Google Safe Browsing API. A
        ///     null reference indicates a geographic location should not be considered.
        /// </param>
        /// <returns>
        ///     This threat list update constraints builder.
        /// </returns>
        public ThreatListUpdateConstraintsBuilder SetThreatListLocation(string value) {
            this.ThreatListLocation = value;
            return this;
        }
    }
}