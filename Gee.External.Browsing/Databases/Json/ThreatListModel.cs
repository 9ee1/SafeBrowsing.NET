using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Databases.Json {
    /// <summary>
    ///     Threat List Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListModel {
        /// <summary>
        ///     Get and Set Threat List's Platform Type.
        /// </summary>
        [JsonProperty(PropertyName = "PlatformType")]
        public PlatformType PlatformType { get; set; }

        /// <summary>
        ///     Get and Set Threat List's Retrieve Date.
        /// </summary>
        [JsonProperty(PropertyName = "RetrieveDate")]
        public DateTime RetrieveDate { get; set; }

        /// <summary>
        ///     Get and Set Threat List's State.
        /// </summary>
        [JsonProperty(PropertyName = "State")]
        public string State { get; set; }

        /// <summary>
        ///     Get and Set Threat List's Threat Entry Type.
        /// </summary>
        [JsonProperty(PropertyName = "ThreatEntryType")]
        public ThreatEntryType ThreatEntryType { get; set; }

        /// <summary>
        ///     Get and Set Threat SHA256 Hash Prefixes.
        /// </summary>
        [JsonProperty(PropertyName = "Sha256HashPrefixes")]
        public IEnumerable<string> ThreatSha256HashPrefixes { get; set; }

        /// <summary>
        ///     Get and Set Threat List's Threat Type.
        /// </summary>
        [JsonProperty(PropertyName = "ThreatType")]
        public ThreatType ThreatType { get; set; }

        /// <summary>
        ///     Get and Set Threat List's Wait to Date.
        /// </summary>
        [JsonProperty(PropertyName = "WaitToDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? WaitToDate { get; set; }
    }
}