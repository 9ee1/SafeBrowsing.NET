using Newtonsoft.Json;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat Entry Metadata Entry Model.
    /// </summary>
    internal sealed class ThreatEntryMetadataEntryModel {
        /// <summary>
        ///     Get and Set Threat Entry's Metadata Key.
        /// </summary>
        [JsonProperty(PropertyName = "key", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Key { get; set; }

        /// <summary>
        ///     Get and Set Threat Entry's Metadata Value.
        /// </summary>
        [JsonProperty(PropertyName = "value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Value { get; set; }
    }
}