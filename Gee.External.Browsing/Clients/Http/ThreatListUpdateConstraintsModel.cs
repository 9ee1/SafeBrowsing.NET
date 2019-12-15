using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Constraints Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListUpdateConstraintsModel {
        /// <summary>
        ///     Get and Set Client Location.
        /// </summary>
        [JsonProperty(PropertyName = "deviceLocation", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientLocation { get; set; }

        /// <summary>
        ///     Get and Set Maximum Database Entries.
        /// </summary>
        [JsonProperty(PropertyName = "maxDatabaseEntries", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaximumDatabaseEntries { get; set; }

        /// <summary>
        ///     Get and Set Maximum Response Entries.
        /// </summary>
        [JsonProperty(PropertyName = "maxUpdateEntries", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaximumResponseEntries { get; set; }

        /// <summary>
        ///     Get and Set Supported Compressions.
        /// </summary>
        [JsonProperty(PropertyName = "supportedCompressions", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> SupportedCompressionTypes { get; set; }

        /// <summary>
        ///     Get and Set Threat List Language.
        /// </summary>
        [JsonProperty(PropertyName = "language", NullValueHandling = NullValueHandling.Ignore)]
        public string ThreatListLanguage { get; set; }

        /// <summary>
        ///     Get and Set Threat List Location.
        /// </summary>
        [JsonProperty(PropertyName = "region", NullValueHandling = NullValueHandling.Ignore)]
        public string ThreatListLocation { get; set; }
    }
}