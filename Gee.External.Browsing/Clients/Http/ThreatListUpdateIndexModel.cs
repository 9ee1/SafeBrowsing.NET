using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Index Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListUpdateIndexModel {
        /// <summary>
        ///     Get and Set Indices.
        /// </summary>
        [JsonProperty(PropertyName = "indices", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<int> Indices { get; set; }
    }
}