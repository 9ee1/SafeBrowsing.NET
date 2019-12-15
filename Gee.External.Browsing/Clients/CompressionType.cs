using Gee.External.Browsing.Clients.Http;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Compression Type.
    /// </summary>
    internal enum CompressionType {
        /// <summary>
        ///     Indicates the compression type of a <see cref="ThreatEntrySetModel" /> is unknown. A
        ///     <see cref="ThreatEntrySetModel" /> with an unknown compression type should be considered erroneous and
        ///     be disregarded.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Indicates a <see cref="ThreatEntrySetModel" /> is compressed using the Rice-Golomb algorithm. A
        ///     compressed <see cref="ThreatEntrySetModel" /> must be uncompressed before it can be processed.
        /// </summary>
        RiceGolomb = 1,

        /// <summary>
        ///     Indicates a <see cref="ThreatEntrySetModel" /> is uncompressed. An uncompressed
        ///     <see cref="ThreatEntrySetModel" /> can be processed immediately.
        /// </summary>
        Uncompressed = 2
    }
}