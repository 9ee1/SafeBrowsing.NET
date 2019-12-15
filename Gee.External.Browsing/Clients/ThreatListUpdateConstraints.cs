using Gee.Common.Guards;
using Gee.External.Browsing.Databases;
using System.Runtime.CompilerServices;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Threat List Update Constraints.
    /// </summary>
    public sealed class ThreatListUpdateConstraints {
        /// <summary>
        ///     Default Threat List Update Constraints.
        /// </summary>
        public static readonly ThreatListUpdateConstraints Default;

        /// <summary>
        ///     Get Client Location.
        /// </summary>
        /// <remarks>
        ///     Represents the geographic location, formatted as an ISO 31166-1 alpha-2 region code, of a client. An
        ///     invalid geographic location is ignored by the Google Safe Browsing API. A null reference indicates the
        ///     geographic location of the client is unknown.
        /// </remarks>
        public string ClientLocation { get; }

        /// <summary>
        ///     Get Maximum Database Entries.
        /// </summary>
        /// <remarks>
        ///     Represents the maximum number of threats associated with a <see cref="ThreatList" /> a client is
        ///     willing, or is capable, of storing in its local <see cref="IManagedBrowsingDatabase" />. A <c>0</c>
        ///     indicates there is no limit to the number of threats the client is willing to store.
        /// </remarks>
        public int MaximumDatabaseEntries { get; }

        /// <summary>
        ///     Get Maximum Response Entries.
        /// </summary>
        /// <remarks>
        ///     Represents the maximum number of threats associated with a <see cref="ThreatList" /> that will be
        ///     retrieved in a single request. A <c>0</c> indicates there is no limit to the number of threats that
        ///     will be retrieved.
        /// </remarks>
        public int MaximumResponseEntries { get; }

        /// <summary>
        ///     Get Threat List Language.
        /// </summary>
        /// <remarks>
        ///     Represents the language, formatted as an ISO 639 alpha-2 language code, a <see cref="ThreatList" />
        ///     should be retrieved for. An invalid language is ignored by the Google Safe Browsing API. A null
        ///     reference indicates a language should not be considered.
        /// </remarks>
        public string ThreatListLanguage { get; }

        /// <summary>
        ///     Get Threat List Location.
        /// </summary>
        /// <remarks>
        ///     Represents the geographic location, formatted as an ISO 31166-1 alpha-2 region code, a
        ///     <see cref="ThreatList" /> should be retrieved for. An invalid geographic location is ignored by the
        ///     Google Safe Browsing API. A null reference indicates a geographic location should not be considered.
        /// </remarks>
        public string ThreatListLocation { get; }

        /// <summary>
        ///     Create a Threat List Update Constraints.
        /// </summary>
        static ThreatListUpdateConstraints() {
            ThreatListUpdateConstraints.Default = CreateDefault();

            // <summary>
            //      Create Default Threat List Update Constraints.
            // </summary>
            ThreatListUpdateConstraints CreateDefault() {
                var cThreatListUpdateConstraints = ThreatListUpdateConstraints.Build()
                    .SetThreatListLocation(null)
                    .SetClientLocation(null)
                    .SetMaximumDatabaseEntries(0)
                    .SetMaximumResponseEntries(0)
                    .SetThreatListLanguage(null)
                    .Build();

                return cThreatListUpdateConstraints;
            }
        }

        /// <summary>
        ///     Build a Threat List Update Constraints.
        /// </summary>
        /// <returns>
        ///     A <see cref="ThreatListUpdateConstraintsBuilder" /> to build a threat list update constraints with.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ThreatListUpdateConstraintsBuilder Build() {
            return new ThreatListUpdateConstraintsBuilder();
        }

        /// <summary>
        ///     Create a Threat List Update Constraints.
        /// </summary>
        /// <param name="builder">
        ///     A <see cref="ThreatListUpdateConstraintsBuilder" /> to initialize the  threat list update constraints
        ///     with.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="builder" /> is a null reference.
        /// </exception>
        internal ThreatListUpdateConstraints(ThreatListUpdateConstraintsBuilder builder) {
            Guard.ThrowIf(nameof(builder), builder).Null();

            this.ClientLocation = builder.ClientLocation;
            this.MaximumDatabaseEntries = builder.MaximumDatabaseEntries;
            this.MaximumResponseEntries = builder.MaximumResponseEntries;
            this.ThreatListLanguage = builder.ThreatListLanguage;
            this.ThreatListLocation = builder.ThreatListLocation;
        }
    }
}