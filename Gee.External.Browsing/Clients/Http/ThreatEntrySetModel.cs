using Newtonsoft.Json;
using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat Entry Set Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatEntrySetModel {
        /// <summary>
        ///     Get and Set Compression Type.
        /// </summary>
        [JsonProperty(PropertyName = "compressionType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CompressionType { get; set; }

        /// <summary>
        ///     Get and Set Uncompressed Indices.
        /// </summary>
        [JsonProperty(PropertyName = "rawIndices", NullValueHandling = NullValueHandling.Ignore)]
        public ThreatListUpdateIndexModel UncompressedIndices { get; set; }

        /// <summary>
        ///     Get and Set Uncompressed SHA256 Hashes.
        /// </summary>
        [JsonProperty(PropertyName = "rawHashes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ThreatListUpdateHashModel UncompressedSha256HashPrefixes { get; set; }
    }
}