using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Result Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListUpdateResultModel {
        /// <summary>
        ///     Get and Set Checksum.
        /// </summary>
        [JsonProperty(PropertyName = "checksum", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ThreatListUpdateChecksumModel Checksum { get; set; }

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
        [JsonProperty(PropertyName = "newClientState", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ThreatListState { get; set; }

        /// <summary>
        ///     Get and Set Threats to Add.
        /// </summary>
        [JsonProperty(PropertyName = "additions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ThreatEntrySetModel> ThreatsToAdd { get; set; }

        /// <summary>
        ///     Get and Set Threats to Remove.
        /// </summary>
        [JsonProperty(PropertyName = "removals", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ThreatEntrySetModel> ThreatsToRemove { get; set; }

        /// <summary>
        ///     Get and Set Threat Type.
        /// </summary>
        [JsonProperty(PropertyName = "threatType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ThreatType { get; set; }

        /// <summary>
        ///     Get and Set Update Type.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UpdateType { get; set; }
    }
}