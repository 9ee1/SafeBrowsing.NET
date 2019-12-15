using Newtonsoft.Json;
using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Query Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListUpdateQueryModel {
        /// <summary>
        ///     Get and Set Constraints.
        /// </summary>
        [JsonProperty(PropertyName = "constraints", NullValueHandling = NullValueHandling.Ignore)]
        public ThreatListUpdateConstraintsModel Constraints { get; set; }

        /// <summary>
        ///     Get and Set Platform Type.
        /// </summary>
        [JsonProperty(PropertyName = "platformType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PlatformType { get; set; }

        /// <summary>
        ///     Get and Set Threat Entry Type.
        /// </summary>
        [JsonProperty(PropertyName = "threatEntryType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ThreatEntryType { get; set; }

        /// <summary>
        ///     Get and Set Threat List State.
        /// </summary>
        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string ThreatListState { get; set; }

        /// <summary>
        ///     Get and Set Threat Type.
        /// </summary>
        [JsonProperty(PropertyName = "threatType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ThreatType { get; set; }
    }
}