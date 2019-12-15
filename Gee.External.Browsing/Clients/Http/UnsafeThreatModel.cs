using Newtonsoft.Json;
using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Unsafe Threat Model.
    /// </summary>
    [Serializable]
    internal sealed class UnsafeThreatModel {
        /// <summary>
        ///     Get and Set Cache Duration.
        /// </summary>
        [JsonProperty(PropertyName = "cacheDuration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CacheDuration { get; set; }

        /// <summary>
        ///     Get and Set Metadata.
        /// </summary>
        [JsonProperty(PropertyName = "threatEntryMetadata", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ThreatEntryMetadataModel Metadata { get; set; }

        /// <summary>
        ///     Get and Set Platform Type.
        /// </summary>
        [JsonProperty(PropertyName = "platformType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PlatformType { get; set; }

        /// <summary>
        ///     Get and Set Threat.
        /// </summary>
        [JsonProperty(PropertyName = "threat", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ThreatEntryModel Threat { get; set; }

        /// <summary>
        ///     Get and Set Threat Entry Type.
        /// </summary>
        [JsonProperty(PropertyName = "threatEntryType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ThreatEntryType { get; set; }

        /// <summary>
        ///     Get and Set Threat Type.
        /// </summary>
        [JsonProperty(PropertyName = "threatType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ThreatType { get; set; }
    }
}