using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Full Hash Request Model.
    /// </summary>
    [Serializable]
    internal sealed class FullHashRequestModel {
        /// <summary>
        ///     Get and Set Client Metadata.
        /// </summary>
        [JsonProperty(PropertyName = "client", NullValueHandling = NullValueHandling.Ignore)]
        public ClientMetadataModel ClientMetadata { get; set; }

        /// <summary>
        ///     Get and Set Query.
        /// </summary>
        [JsonProperty(PropertyName = "threatInfo", NullValueHandling = NullValueHandling.Ignore)]
        public FullHashQueryModel Query { get; set; }

        /// <summary>
        ///     Get and Set Threat List States.
        /// </summary>
        [JsonProperty(PropertyName = "clientStates", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> ThreatListStates { get; set; }
    }
}