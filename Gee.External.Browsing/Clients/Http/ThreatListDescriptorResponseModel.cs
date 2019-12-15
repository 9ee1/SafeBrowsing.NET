using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Threat List Descriptor Response Model.
    /// </summary>
    [Serializable]
    internal sealed class ThreatListDescriptorResponseModel {
        /// <summary>
        ///     Get and Set Threat List Descriptors.
        /// </summary>
        [JsonProperty(PropertyName = "threatLists", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ThreatListDescriptorModel> ThreatListDescriptors { get; set; }
    }
}