using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Update Response Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListUpdateResponseModel {
        /// <summary>
        ///     Get and Set Results.
        /// </summary>
        [JsonProperty(PropertyName = "listUpdateResponses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ThreatListUpdateResultModel> Results { get; set; }

        /// <summary>
        ///     Get and Set Wait Duration.
        /// </summary>
        [JsonProperty(PropertyName = "minimumWaitDuration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string WaitDuration { get; set; }
    }
}