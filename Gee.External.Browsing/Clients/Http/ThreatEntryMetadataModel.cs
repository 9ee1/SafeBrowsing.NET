using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat Entry Metadata Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatEntryMetadataModel {
        /// <summary>
        ///     Get and Set Threat Entry's Metadata Entries.
        /// </summary>
        [JsonProperty(PropertyName = "entries", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ThreatEntryMetadataEntryModel> Entries { get; set; }
    }
}