using Newtonsoft.Json;
using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat Entry Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatEntryModel {
        /// <summary>
        ///     Get and Set SHA256 Hash.
        /// </summary>
        [JsonProperty(PropertyName = "hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Sha256Hash { get; set; }
    }
}