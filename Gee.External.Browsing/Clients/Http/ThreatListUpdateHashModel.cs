using Newtonsoft.Json;
using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Hash Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListUpdateHashModel {
        /// <summary>
        ///     Get and Set SHA256 Hash Prefixes.
        /// </summary>
        [JsonProperty(PropertyName = "rawHashes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Sha256HashPrefixes { get; set; }

        /// <summary>
        ///     Get and Set SHA256 Hash Prefix Size.
        /// </summary>
        [JsonProperty(PropertyName = "prefixSize", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Sha256HashPrefixSize { get; set; }
    }
}