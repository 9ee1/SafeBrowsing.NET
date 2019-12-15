using Gee.Common.Guards;
using System;
using System.Reflection;

namespace Gee.External.Browsing.Clients {
    /// <summary>
    ///     Client Metadata.
    /// </summary>
    public sealed class ClientMetadata {
        /// <summary>
        ///     Default Client Metadata.
        /// </summary>
        public static readonly ClientMetadata Default;

        /// <summary>
        ///     Get Client's Unique Identifier.
        /// </summary>
        /// <remarks>
        ///     Represents the unique identifier identifying the client.
        /// </remarks>
        public string Id { get; }

        /// <summary>
        ///     Get Client's Version.
        /// </summary>
        /// <remarks>
        ///     Represents the client's version.
        /// </remarks>
        public Version Version { get; }

        /// <summary>
        ///     Create a Client Metadata.
        /// </summary>
        static ClientMetadata() {
            ClientMetadata.Default = CreateDefault();

            // <summary>
            //      Create Default Client Metadata.
            // </summary>
            ClientMetadata CreateDefault() {
                var thisAssembly = Assembly.GetExecutingAssembly();
                var thisAssemblyName = thisAssembly.GetName();
                var clientMetadata = new ClientMetadata(thisAssemblyName.Name, thisAssemblyName.Version);

                return clientMetadata;
            }
        }

        /// <summary>
        ///     Create a Client Metadata.
        /// </summary>
        /// <param name="id">
        ///     A unique identifier identifying the client.
        /// </param>
        /// <param name="version">
        ///     The client's version.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="id" /> consists exclusively of whitespace characters.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="id" /> is a null reference, or if <paramref name="version" /> is a null
        ///     reference.
        /// </exception>
        public ClientMetadata(string id, Version version) {
            Guard.ThrowIf(nameof(id), id).Null().Whitespace();
            Guard.ThrowIf(nameof(version), version).Null();

            this.Id = id;
            this.Version = version;
        }

        /// <summary>
        ///     Create a Client Metadata.
        /// </summary>
        /// <param name="id">
        ///     A unique identifier identifying the client.
        /// </param>
        /// <param name="majorVersion">
        ///     The client's major version.
        /// </param>
        /// <param name="minorVersion">
        ///     The client's minor version.
        /// </param>
        /// <param name="patchVersion">
        ///     The client's patch version.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="id" /> consists exclusively of whitespace characters.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="id" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="majorVersion" /> is less than <c>0</c>, or if
        ///     <paramref name="minorVersion" /> is less than <c>0</c>, or if <paramref name="patchVersion" /> is less
        ///     than <c>0</c>.
        /// </exception>
        public ClientMetadata(string id, int majorVersion, int minorVersion, int patchVersion) {
            Guard.ThrowIf(nameof(id), id).Null().Whitespace();

            this.Id = id;
            this.Version = new Version(majorVersion, minorVersion, patchVersion);
        }

        /// <summary>
        ///     Determine if This Object is Equal to Another Object.
        /// </summary>
        /// <param name="object">
        ///     An object to compare to.
        /// </param>
        /// <returns>
        ///     A boolean true if this object is equal to <paramref name="object"/>. A boolean false otherwise.
        /// </returns>
        public override bool Equals(object @object) {
            var isEqual = @object != null &&
                          @object is ClientMetadata clientMetadata &&
                          this.Id == clientMetadata.Id &&
                          object.Equals(this.Version, clientMetadata.Version);

            return isEqual;
        }

        /// <summary>
        ///     Get Object's Hash Code.
        /// </summary>
        /// <returns>
        ///     The object's hash code.
        /// </returns>
        public override int GetHashCode() {
            var hashCode = 13;
            hashCode = hashCode * 7 + this.Id.GetHashCode();
            hashCode = hashCode * 7 + this.Version.GetHashCode();

            return hashCode;
        }

        /// <summary>
        ///     Get Object's String Representation.
        /// </summary>
        /// <returns>
        ///     The object's string representation.
        /// </returns>
        public override string ToString() {
            var @string = $"{this.Id} {this.Version}";
            return @string;
        }
    }
}