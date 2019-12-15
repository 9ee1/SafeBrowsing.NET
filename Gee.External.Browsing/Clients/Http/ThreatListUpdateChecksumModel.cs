using Newtonsoft.Json;
using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Checksum Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListUpdateChecksumModel {
        /// <summary>
        ///     Get and Set SHA256 Hash.
        /// </summary>
        [JsonProperty(PropertyName = "sha256", NullValueHandling = NullValueHandling.Ignore)]
        public string Sha256Hash { get; set; }
    }
}