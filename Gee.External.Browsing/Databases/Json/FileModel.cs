using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gee.External.Browsing.Databases.Json {
    /// <summary>
    ///     File Model.
    /// </summary>
    [Serializable]
    internal sealed class FileModel {
        /// <summary>
        ///     Get and Set Threat Lists.
        /// </summary>
        [JsonProperty(PropertyName = "ThreatLists")]
        public IEnumerable<ThreatListModel> ThreatLists { get; set; }
    }
}