using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    [Serializable]
    internal sealed class FullHashResponseModel {
        /// <summary>
        ///     Get and Set Safe Threats Duration.
        /// </summary>
        [JsonProperty(PropertyName = "negativeCacheDuration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SafeThreatsDuration { get; set; }

        /// <summary>
        ///     Get and Set Unsafe Threats.
        /// </summary>
        [JsonProperty(PropertyName = "matches", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<UnsafeThreatModel> UnsafeThreats { get; set; }

        /// <summary>
        ///     Get and Set Wait Duration.
        /// </summary>
        [JsonProperty(PropertyName = "minimumWaitDuration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string WaitDuration { get; set; }
    }
}