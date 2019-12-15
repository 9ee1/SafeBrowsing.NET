using Gee.Common;
using Gee.Common.Guards;
using Gee.Common.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gee.External.Browsing {
    /// <summary>
    ///     Canonicalized Uniform Resource Locator (URL).
    /// </summary>
    public sealed class Url {
        /// <summary>
        ///     Consecutive Dot (".") Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to find consecutive dot (".") characters. The regular
        ///     expression pattern is used on a URI host component.
        /// </remarks>
        private static readonly Regex ConsecutiveDotPattern = new Regex(@"\.{2,}", RegexOptions.Compiled);

        /// <summary>
        ///     Consecutive Slash ("/") Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to find consecutive slash ("/") characters. The regular
        ///     expression pattern is used on a URI path component.
        /// </remarks>
        private static readonly Regex ConsecutiveSlashPattern = new Regex("/{2,}", RegexOptions.Compiled);

        /// <summary>
        ///     Control Characters Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to find CR, LF, and TAB ASCII control characters. The regular
        ///     expression pattern is used on every URI component.
        /// </remarks>
        private static readonly Regex ControlCharactersPattern = new Regex("[\x09\x0D\x0A]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///     Current Directory Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to find a current directory path segment ("/./"). The regular
        ///     expression pattern is used on a URI path component.
        /// </remarks>
        private static readonly Regex CurrentDirectoryPattern = new Regex(@"/\./", RegexOptions.Compiled);

        /// <summary>
        ///     Encoded Character Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to find encoded characters. The regular expression pattern
        ///     supports finding 1 byte, 2 byte, 3 byte, or 4 byte encoded characters. The regular expression pattern
        ///     is used on every URI component.
        /// </remarks>
        private static readonly Regex EncodedCharacterPattern = new Regex("(?:%[0-9a-fA-F]{2}%[0-9a-fA-F]{2}%[0-9a-fA-F]{2}%[0-9a-fA-F]{2})|(?:%[0-9a-fA-F]{2}%[0-9a-fA-F]{2}%[0-9a-fA-F]{2})|(?:%[0-9a-fA-F]{2}%[0-9a-fA-F]{2})|(?:%[0-9a-fA-F]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///     Leading Dot (".") Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to find leading dot (".") characters. The regular expression
        ///     pattern is used on a URI host component.
        /// </remarks>
        private static readonly Regex LeadingDotPattern = new Regex(@"^\.{1,}", RegexOptions.Compiled);

        /// <summary>
        ///     Parent Directory Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to find a parent directory path segment ("/../"). The regular
        ///     expression pattern is used on URI path component.
        /// </remarks>
        private static readonly Regex ParentDirectoryPattern = new Regex(@"/\w+/\.\./?", RegexOptions.Compiled);

        /// <summary>
        ///     Trailing Dot (".") Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to find trailing dot (".") characters. The regular expression
        ///     pattern is used on a URI host component.
        /// </remarks>
        private static readonly Regex TrailingDotPattern = new Regex(@"\.{1,}$", RegexOptions.Compiled);

        /// <summary>
        ///     URI Pattern.
        /// </summary>
        /// <remarks>
        ///     Represents a regular expression pattern to match a URI formatted in accordance with RFC 3986.
        /// </remarks>
        private static readonly Regex UriPattern = new Regex(@"^(?:(?<scheme>[a-zA-Z][a-zA-Z\d+-.]*):)?(?:\/\/(?:(?<user>[a-zA-Z\d\-._~\!$&'()*+,;=%]*)(?::(?<pass>[a-zA-Z\d\-._~\!$&'()*+,;=:%]*))?@)?(?<host>(?:(?:\s+)?[a-zA-Z\d-.%]+)|(?:(?:\s+)?\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})|(?:(?:\s+)?\[(?:[a-fA-F\d.:]+)\]))?(?::(?<port>\d*))?(?<path>(?:\/[a-zA-Z\d\-._~\!$&'()*+,;=:@%]*)*)|(?<user>)(?<pass>)(?<host>)(?<port>)(?<path>\/[a-zA-Z\d\-._~\!$&'()*+,;=:@%]+(?:\/[a-zA-Z\d\-._~\!$&'()*+,;=:@%]*)*)?|(?<user>)(?<pass>)(?<host>)(?<port>)(?<path>[a-zA-Z\d\-._~\!$&'()*+,;=:@%]+(?:\/[a-zA-Z\d\-._~\!$&'()*+,;=:@%]*)*))?(?:\?(?<query>[a-zA-Z\d\-._~\!$&'()*+,;=:@%\/?]*))?(?:\#(?<fragment>[a-zA-Z\d\-._~\!$&'()*+,;=:@%\/?]*))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///     Canonicalized URL.
        /// </summary>
        private readonly (string Value, string Scheme, string Host, string Path, string Query) _canonicalizedUrl;

        /// <summary>
        ///     Get URL Expressions.
        /// </summary>
        public IEnumerable<UrlExpression> Expressions { get; }

        /// <summary>
        ///     Get URL Host Component.
        /// </summary>
        public string Host => this._canonicalizedUrl.Host;

        /// <summary>
        ///     Get URL Path Component.
        /// </summary>
        public string Path => this._canonicalizedUrl.Path;

        /// <summary>
        ///     Get URL Query Component.
        /// </summary>
        public string Query => this._canonicalizedUrl.Query;

        /// <summary>
        ///     Get URL Scheme Component.
        /// </summary>
        public string Scheme => this._canonicalizedUrl.Scheme;

        /// <summary>
        ///     Get URL Value.
        /// </summary>
        public string Value => this._canonicalizedUrl.Value;

        internal Url(string url) {
            this._canonicalizedUrl = CanonicalizeUri(url);
            // ...
            //
            // ...
            this.Expressions = CreateExpressions(this);

            // <summary>
            //      Canonicalize a URI Host Component.
            // </summary>
            string CanonicalizeHost(string cHost) {
                // ...
                //
                // In accordance with the Google Safe Browsing Specification, we first decode the URI host component.
                cHost = Decode(cHost);

                // ...
                //
                // Next, we remove leading dot (".") characters and trailing dot (".") characters, and replace
                // consecutive dot (".") characters with a single dot (".") character.
                cHost = Url.LeadingDotPattern.Replace(cHost, string.Empty);
                cHost = Url.TrailingDotPattern.Replace(cHost, string.Empty);
                cHost = Url.ConsecutiveDotPattern.Replace(cHost, ".");

                // ...
                //
                // Next, if the URI host component is expressed as an IP address, we express it in dotted-decimal
                // notation.
                cHost.TryAsIpAddress(out var cHostIpAddress);
                if (cHostIpAddress != null) {
                    cHost = cHostIpAddress.ToString();
                }

                // ...
                //
                // Next, we convert the URI host component to lowercase.
                cHost = cHost.ToLowerInvariant();

                // ...
                //
                // Last, we encode the URI host component.
                cHost = Encode(cHost);
                return cHost;
            }

            // <summary>
            //      Canonicalize a URI Path Component.
            // </summary>
            string CanonicalizePath(string cPath) {
                // ...
                //
                // In accordance with the Google Safe Browsing Specification, we first decode the URI path component.
                cPath = Decode(cPath);

                // ...
                //
                // Next, we replace current directory path segments ("/./"), parent directory path segments ("/../"),
                // and consecutive slash ("/") characters with a single slash ("/") character.
                cPath = Url.CurrentDirectoryPattern.Replace(cPath, "/");
                cPath = Url.ParentDirectoryPattern.Replace(cPath, "/");
                cPath = Url.ConsecutiveSlashPattern.Replace(cPath, "/");

                // ...
                //
                // Last, we encode the URI path component.
                cPath = Encode(cPath);
                return cPath;
            }

            // <summary>
            //      Canonicalize a URI.
            // </summary>
            (string Value, string Scheme, string Host, string Path, string Query) CanonicalizeUri(string cUri) {
                // ...
                //
                // Throws an exception if the operation fails.
                var cUriMatch = Url.UriPattern.Match(cUri);
                if (!cUriMatch.Success) {
                    var cDetailMessage = $"A string ({cUri}) does not express a URI.";
                    throw new UriFormatException(cDetailMessage);
                }

                var cScheme = "http";
                var cSchemeMatch = cUriMatch.Groups["scheme"];
                if (cSchemeMatch.Success && cSchemeMatch.Value != string.Empty) {
                    // ...
                    //
                    // In accordance with the Google Safe Browsing Specification, we first decode the URI scheme
                    // component and then encode it.
                    cScheme = Decode(cSchemeMatch.Value);
                    cScheme = Encode(cScheme);
                }

                var cHostMatch = cUriMatch.Groups["host"];
                if (!cHostMatch.Success || cHostMatch.Value == string.Empty) {
                    // ...
                    //
                    // We won't find a host component if there is no scheme component. If there is no scheme component,
                    // we add a default one in accordance with the Google Safe Browsing Specification.
                    cUri = $"{cScheme}://{cUri}";
                    cUriMatch = Url.UriPattern.Match(cUri);
                    if (!cUriMatch.Success) {
                        var cDetailMessage = $"A string ({cUri}) does not express a URI.";
                        throw new UriFormatException(cDetailMessage);
                    }

                    cHostMatch = cUriMatch.Groups["host"];
                }

                var cHost = CanonicalizeHost(cHostMatch.Value);
                var cPath = "/";
                var cPathMatch = cUriMatch.Groups["path"];
                if (cPathMatch.Success && cPathMatch.Value != string.Empty) {
                    cPath = CanonicalizePath(cPathMatch.Value);
                }

                string cQuery = null;
                string cValueQuery = null;
                var cQueryMatch = cUriMatch.Groups["query"];
                if (cQueryMatch.Success && cQueryMatch.Value != string.Empty) {
                    // ...
                    //
                    // In accordance with the Google Safe Browsing Specification, we first decode the URI query
                    // component and then encode it.
                    cQuery = Decode(cQueryMatch.Value);
                    cQuery = Encode(cQuery);

                    cValueQuery = $"?{cQuery}";
                }

                var cValue = $"{cScheme}://{cHost}{cPath}{cValueQuery}";
                return (cValue, cScheme, cHost, cPath, cQuery);
            }

            // <summary>
            //      Create URL Expressions.
            // </summary>
            IEnumerable<UrlExpression> CreateExpressions(Url @this) {
                // ...
                //
                // In accordance with the Google Safe Browsing Specification, we first extract the URI host and path
                // expressions.
                var cHostExpressions = ComputeHostExpressions(@this.Host);
                var cPathExpressions = ComputePathExpressions(@this.Path, @this.Query);

                // ...
                //
                // Next, we compute every combination of the URI extracted host and path expressions.
                var cExpressions = new List<UrlExpression>();
                foreach (var cHostExpression in cHostExpressions) {
                    foreach (var cPathExpression in cPathExpressions) {
                        var cExpressionValue = $"{cHostExpression}{cPathExpression}";
                        var cExpression = new UrlExpression(cExpressionValue, @this);
                        cExpressions.Add(cExpression);
                    }
                }

                return cExpressions;
            }

            // <summary>
            //      Compute a URI Host Component's Expressions.
            // </summary>
            IEnumerable<string> ComputeHostExpressions(string cHost) {
                // ...
                //
                // In accordance with the Google Safe Browsing Specification, we first add the URI host component as
                // an expression.
                var cHostExpressions = new HashSet<string>();
                cHostExpressions.Add(cHost);

                // ...
                //
                // Next, if the URI host component is not an IP address, we will extract from it up to 4 domain
                // names, starting with the last 5.
                cHost.TryAsIpAddress(out var cHostIpAddress);
                if (cHostIpAddress == null) {
                    var cHostDomainNames = cHost.Split('.');
                    var cStartIndex = cHostDomainNames.Length > 5 ? cHostDomainNames.Length - 5 : 0;
                    var cEndIndex = cHostDomainNames.Length - 1;
                    for (var cI = cStartIndex; cI < cEndIndex; cI++) {
                        var cNewHostDomainNames = cHostDomainNames.Skip(cI);
                        var cHostExpression = string.Join(".", cNewHostDomainNames);
                        cHostExpressions.Add(cHostExpression);
                    }
                }

                return cHostExpressions;
            }

            // <summary>
            //      Compute a URI Path Component's Expressions.
            // </summary>
            IEnumerable<string> ComputePathExpressions(string cPath, string cQuery) {
                // ...
                //
                // In accordance with the Google Safe Browsing Specification, we first add the URI path component as
                // an expression.
                var cPathExpressions = new HashSet<string>();
                cPathExpressions.Add(cPath);
                if (cQuery != null) {
                    // ...
                    //
                    // Next, we add the URI path component, with the URI query component, as an expression.
                    cPathExpressions.Add($"{cPath}?{cQuery}");
                }

                // ...
                //
                // Next, we will extract up to 4 path segments from the URI path component.
                var cPathSegments = cPath.Split('/');
                var cEndIndex = Math.Min(cPathSegments.Length, 4);
                for (var cI = 0; cI < cEndIndex; cI++) {
                    var cNewPathSegments = cPathSegments.Take(cI + 1);
                    var cPathExpression = string.Join("/", cNewPathSegments);
                    if (cI != cEndIndex - 1) {
                        cPathExpression = $"{cPathExpression}/";
                    }

                    cPathExpressions.Add(cPathExpression);
                }

                return cPathExpressions;
            }

            // <summary>
            //      Decode a String.
            // </summary>
            string Decode(string cString) {
                // ...
                //
                // In accordance with the Google Safe Browsing Specification, we first remove CR, LF, and TAB ASCII
                // control characters from a string before we decode it.
                cString = Url.ControlCharactersPattern.Replace(cString, string.Empty);
                while (true) {
                    // ...
                    //
                    // In accordance with the Google Safe Browsing Specification, we will repeatedly decode a string
                    // until it is no longer encoded.
                    var cEncodedMatches = Url.EncodedCharacterPattern.Matches(cString);
                    if (cEncodedMatches.Count == 0) {
                        break;
                    }

                    // ...
                    //
                    // Each encoded match will specify the index of an encoded substring that needs to be replaced
                    // with its decoded equivalent. The encoded matches are ordered from left to right in the order in
                    // which they are found in the string. If we do the replacement operation from left to right,
                    // the indices of successive encoded matches will no longer be valid. The solution is to reverse
                    // the encoded matches and do the replacement operation from right to left.
                    var cReversedEncodedMatches = cEncodedMatches.OfType<Match>().Reverse();
                    foreach (var cEncodedMatch in cReversedEncodedMatches) {
                        var cDecodedString = Uri.UnescapeDataString(cEncodedMatch.Value);
                        cString = cString.SubstringReplace(cDecodedString, cEncodedMatch.Index, cEncodedMatch.Length);
                    }
                }

                return cString;
            }

            // <summary>
            //      Encode a String.
            // </summary>
            string Encode(string cString) {
                var cEncodedStringBuilder = new StringBuilder();
                foreach (var cChar in cString) {
                    // ...
                    //
                    // We only encode the characters defined by the Google Safe Browsing Specification.
                    if (cChar <= 32 || cChar >= 127 || cChar == '#' || cChar == '%') {
                        var cEncodedString = new string(new[] {cChar});
                        cEncodedString = Uri.EscapeDataString(cEncodedString);
                        cEncodedStringBuilder.Append(cEncodedString);
                    }
                    else {
                        cEncodedStringBuilder.Append(cChar);
                    }
                }

                cString = cEncodedStringBuilder.ToString();
                return cString;
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

        public bool TryGetExpressionForSha256Hash(string sha256Hash, out UrlExpression urlExpression) {
            Guard.ThrowIf(nameof(sha256Hash), sha256Hash).Null();

            urlExpression = null;
            foreach (var currentUrlExpression in this.Expressions) {
                if (currentUrlExpression.Sha256Hash == sha256Hash) {
                    urlExpression = currentUrlExpression;
                    break;
                }
            }

            return urlExpression != null;
        }
    }
}