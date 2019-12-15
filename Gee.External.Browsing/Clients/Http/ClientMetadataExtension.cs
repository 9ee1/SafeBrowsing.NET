namespace Gee.External.Browsing.Clients.Http {
    /// <summary>
    ///     Client Metadata Extension.
    /// </summary>
    internal static class ClientMetadataExtension {
        /// <summary>
        ///     Create a Client Metadata Model.
        /// </summary>
        /// <param name="this">
        ///     A <see cref="ClientMetadata" />.
        /// </param>
        /// <returns>
        ///     A <see cref="ClientMetadataModel" /> if <paramref name="this" /> is not a null reference. A null
        ///     reference otherwise.
        /// </returns>
        internal static ClientMetadataModel AsClientMetadataModel(this ClientMetadata @this) {
            ClientMetadataModel clientMetadataModel = null;
            if (@this != null) {
                clientMetadataModel = new ClientMetadataModel();
                clientMetadataModel.Id = @this.Id;
                clientMetadataModel.Version = @this.Version.ToString(3);
            }

            return clientMetadataModel;
        }
    }
}