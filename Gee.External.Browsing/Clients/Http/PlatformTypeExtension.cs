namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Platform Type Extension.
    /// </summary>
    internal static class PlatformTypeExtension {
        /// <summary>
        ///     Create a Platform Type.
        /// </summary>
        /// <param name="this">
        ///     A string identifying a <see cref="PlatformType" />.
        /// </param>
        /// <returns>
        ///     A <see cref="PlatformType" />.
        /// </returns>
        internal static PlatformType AsPlatformType(this string @this) {
            PlatformType platformType;
            switch (@this) {
                case "ALL_PLATFORMS":
                    platformType = PlatformType.All;
                    break;
                case "ANDROID":
                    platformType = PlatformType.Android;
                    break;
                case "ANY_PLATFORM":
                    platformType = PlatformType.Any;
                    break;
                case "CHROME":
                    platformType = PlatformType.Chrome;
                    break;
                case "IOS":
                    platformType = PlatformType.Ios;
                    break;
                case "LINUX":
                    platformType = PlatformType.Linux;
                    break;
                case "OSX":
                    platformType = PlatformType.MacOs;
                    break;
                case "PLATFORM_TYPE_UNSPECIFIED":
                    platformType = PlatformType.Unknown;
                    break;
                case "WINDOWS":
                    platformType = PlatformType.Windows;
                    break;
                default:
                    platformType = PlatformType.Unknown;
                    break;
            }

            return platformType;
        }

        /// <summary>
        ///     Create a Platform Type Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="PlatformType" />.
        /// </param>
        /// <returns>
        ///     A string identifying a <see cref="PlatformType" />.
        /// </returns>
        internal static string AsPlatformTypeModel(this PlatformType @this) {
            string platformTypeModel;
            switch (@this) {
                case PlatformType.All:
                    platformTypeModel = "ALL_PLATFORMS";
                    break;
                case PlatformType.Android:
                    platformTypeModel = "ANDROID";
                    break;
                case PlatformType.Any:
                    platformTypeModel = "ANY_PLATFORM";
                    break;
                case PlatformType.Chrome:
                    platformTypeModel = "CHROME";
                    break;
                case PlatformType.Ios:
                    platformTypeModel = "IOS";
                    break;
                case PlatformType.Linux:
                    platformTypeModel = "LINUX";
                    break;
                case PlatformType.MacOs:
                    platformTypeModel = "OSX";
                    break;
                case PlatformType.Unknown:
                    platformTypeModel = "PLATFORM_TYPE_UNSPECIFIED";
                    break;
                case PlatformType.Windows:
                    platformTypeModel = "WINDOWS";
                    break;
                default:
                    platformTypeModel = "PLATFORM_TYPE_UNSPECIFIED";
                    break;
            }

            return platformTypeModel;
        }
    }
}