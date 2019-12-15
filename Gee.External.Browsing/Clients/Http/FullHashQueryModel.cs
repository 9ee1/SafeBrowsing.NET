using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Full Hash Query Model.
    /// </summary>
    [Serializable]
    internal sealed class FullHashQueryModel {
        /// <summary>
        ///     Get and Set Platform Types.
        /// </summary>
        [JsonProperty(PropertyName = "platformTypes", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> PlatformTypes { get; set; }

        /// <summary>
        ///     Get and Set Threat Entries.
        /// </summary>
        [JsonProperty(PropertyName = "threatEntries", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ThreatEntryModel> ThreatEntries { get; set; }

        /// <summary>
        ///     Get and Set Threat Entry Types.
        /// </summary>
        [JsonProperty(PropertyName = "threatEntryTypes", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> ThreatEntryTypes { get; set; }

        /// <summary>
        ///     Get and Set Threat Types.
        /// </summary>
        [JsonProperty(PropertyName = "threatTypes", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> ThreatTypes { get; set; }
    }
}