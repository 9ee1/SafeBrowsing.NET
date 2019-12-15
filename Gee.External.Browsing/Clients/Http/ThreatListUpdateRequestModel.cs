using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Request Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListUpdateRequestModel {
        /// <summary>
        ///     Get and Set Client Metadata.
        /// </summary>
        [JsonProperty(PropertyName = "client", NullValueHandling = NullValueHandling.Ignore)]
        public ClientMetadataModel ClientMetadata { get; set; }

        /// <summary>
        ///     Get and Set Queries.
        /// </summary>
        [JsonProperty(PropertyName = "listUpdateRequests", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ThreatListUpdateQueryModel> Queries { get; set; }
    }
}