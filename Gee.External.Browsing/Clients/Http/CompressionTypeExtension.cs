namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Compression Type Extension.
    /// </summary>
    internal static class CompressionTypeExtension {
        /// <summary>
        ///     Create a Compression Type.
        /// </summary>
        /// <param name="this">
        ///     A string identifying a <see cref="CompressionType" />.
        /// </param>
        /// <returns>
        ///     A <see cref="CompressionType" />.
        /// </returns>
        internal static CompressionType AsCompressionType(this string @this) {
            CompressionType compressionType;
            switch (@this) {
                case "COMPRESSION_TYPE_UNSPECIFIED":
                    compressionType = CompressionType.Unknown;
                    break;
                case "RAW":
                    compressionType = CompressionType.Uncompressed;
                    break;
                case "RICE":
                    compressionType = CompressionType.RiceGolomb;
                    break;
                default:
                    compressionType = CompressionType.Unknown;
                    break;
            }

            return compressionType;
        }

        /// <summary>
        ///     Create a Compression Type Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="CompressionType" />.
        /// </param>
        /// <returns>
        ///     A string identifying a <see cref="CompressionType" />.
        /// </returns>
        internal static string AsCompressionTypeModel(this CompressionType @this) {
            string compressionTypeModel;
            switch (@this) {
                case CompressionType.RiceGolomb:
                    compressionTypeModel = "RICE";
                    break;
                case CompressionType.Uncompressed:
                    compressionTypeModel = "RAW";
                    break;
                case CompressionType.Unknown:
                    compressionTypeModel = "COMPRESSION_TYPE_UNSPECIFIED";
                    break;
                default:
                    compressionTypeModel = "COMPRESSION_TYPE_UNSPECIFIED";
                    break;
            }

            return compressionTypeModel;
        }
    }
}