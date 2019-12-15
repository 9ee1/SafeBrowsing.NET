using Newtonsoft.Json;
using System;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Client Metadata Model.
    /// </summary>
    [Serializable]
    internal sealed class ClientMetadataModel {
        /// <summary>
        ///     Get and Set Client's Unique Identifier.
        /// </summary>
        [JsonProperty(PropertyName = "clientId", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        ///     Get and Set Client's Version.
        /// </summary>
        [JsonProperty(PropertyName = "clientVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }
    }
}