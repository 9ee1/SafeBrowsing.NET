using Gee.Common;
using Gee.Common.Security.Cryptography;
using Gee.Common.Text;
using System.Collections.Generic;

namespace Gee.External.Browsing {
    /// <summary>
    ///     Canonicalized Uniform Resource Locator (URL) Expression.
    /// </summary>
    public sealed class UrlExpression {
        /// <summary>
        ///     Get URL Expression's SHA256 Hash.
        /// </summary>
        public string Sha256Hash { get; }

        /// <summary>
        ///     Get URL Expression's SHA256 Hash Prefixes.
        /// </summary>
        public IEnumerable<string> Sha256HashPrefixes { get; }

        /// <summary>
        ///     Get URL Expression's URL.
        /// </summary>
        public Url Url { get; }

        /// <summary>
        ///     Get URL Expression's Value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Create a URL Expression.
        /// </summary>
        /// <param name="value">
        ///     The URL Expression's value.
        /// </param>
        /// <param name="url">
        ///     The URL Expressions' URL.
        /// </param>
        internal UrlExpression(string value, Url url) {
            this.Url = url;
            this.Value = value;
            // ...
            //
            // ...
            this.Sha256Hash = this.Value.AsciiEncode().Sha256Hash().HexadecimalEncode();
            // ...
            //
            // ...
            this.Sha256HashPrefixes = CreateSha256HashPrefixes(this);

            // <summary>
            //      Create URL Expression's SHA256 Hash Prefixes.
            // </summary>
            IEnumerable<string> CreateSha256HashPrefixes(UrlExpression @this) {
                var cSha256HashPrefixes = new List<string>();
                for (var cI = 4; cI <= @this.Sha256Hash.Length; cI++) {
                    var cSha256HashPrefix = @this.Sha256Hash.Substring(0, cI);
                    cSha256HashPrefixes.Add(cSha256HashPrefix);
                }

                return cSha256HashPrefixes;
            }
        }

        /// <summary>
        ///     Get Object's String Representation.
        /// </summary>
        /// <returns>
        ///     The object's string representation.
        /// </returns>
        public override string ToString() {
            return this.Value;
        }
    }
}