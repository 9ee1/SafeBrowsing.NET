using Gee.Common.Guards;
using Gee.External.Browsing.Cache;
using Gee.External.Browsing.Clients;
using Gee.External.Browsing.Clients.Http;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Base Service Builder.
    /// </summary>
    /// <typeparam name="T">
    ///     The service builder's type.
    /// </typeparam>
    public abstract class BaseBrowsingServiceBuilder<T> where T : BaseBrowsingServiceBuilder<T> {
        /// <summary>
        ///     Get and Set Cache.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="IBrowsingCache" /> to cache a looked up <see cref="Url" /> in.
        /// </remarks>
        internal IBrowsingCache Cache { get; private protected set; }

        /// <summary>
        ///     Get and Set Client.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="IBrowsingClient" /> to retrieve the collection of <see cref="ThreatList" />
        ///     to store locally with.
        /// </remarks>
        internal IBrowsingClient Client { get; private protected set; }

        /// <summary>
        ///     Get and Set Own Cache Flag.
        /// </summary>
        /// <remarks>
        ///     Represents a boolean flag indicating whether or not the <see cref="BaseBrowsingService" /> takes
        ///     ownership of the <see cref="Cache" /> and disposes it when the service itself is disposed. If the
        ///     service takes ownership of the cache and you reference or dispose the cache after you create the
        ///     service, the behavior of the service and the cache is undefined.
        /// </remarks>
        internal bool OwnCache { get; private protected set; }

        /// <summary>
        ///     Get and Set Own Client Flag.
        /// </summary>
        /// <remarks>
        ///     Represents a boolean flag indicating whether or not the <see cref="BaseBrowsingService" /> takes
        ///     ownership of the <see cref="Client" /> and disposes it when the service itself is disposed. If the
        ///     service takes ownership of the client and you reference or dispose the client after you create the
        ///     service, the behavior of the client and the client is undefined.
        /// </remarks>
        internal bool OwnClient { get; private protected set; }

        /// <summary>
        ///     Create a Base Service Builder.
        /// </summary>
        private protected BaseBrowsingServiceBuilder() { }

        /// <summary>
        ///     Set Cache.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="IBrowsingCache" />.
        /// </param>
        /// <param name="ownCache">
        ///     A boolean flag indicating whether or not the <see cref="BaseBrowsingService" /> takes ownership of the
        ///     cache and disposes it when the service itself is disposed. If the service takes ownership of the cache
        ///     and you reference or dispose the cache after you create the service, the behavior of the service and
        ///     the cache is undefined.
        /// </param>
        /// <returns>
        ///     This service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public T SetCache(IBrowsingCache value, bool ownCache) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Cache = value;
            this.OwnCache = ownCache;
            return (T) this;
        }

        /// <summary>
        ///     Set Client.
        /// </summary>
        /// <param name="value">
        ///     A <see cref="IBrowsingClient" />.
        /// </param>
        /// <param name="ownClient">
        ///     A boolean flag indicating whether or not the <see cref="BaseBrowsingService" /> takes ownership of the
        ///     client and disposes it when the service itself is disposed. If the service takes ownership of the
        ///     client and you reference or dispose the client after you create the service, the behavior of the
        ///     service and the client is undefined.
        /// </param>
        /// <returns>
        ///     This service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="value" /> is a null reference.
        /// </exception>
        public T SetClient(IBrowsingClient value, bool ownClient) {
            Guard.ThrowIf(nameof(value), value).Null();

            this.Client = value;
            this.OwnClient = ownClient;
            return (T) this;
        }

        /// <summary>
        ///     Use a HTTP Client.
        /// </summary>
        /// <param name="apiKey">
        ///     A Google Safe Browsing API key to authenticate to the Google Safe Browsing API with.
        /// </param>
        /// <remarks>
        ///     Use an <see cref="HttpBrowsingClient" /> to communicate with the Google Safe Browsing API. The
        ///     <see cref="BaseBrowsingService" /> takes ownership of the HTTP client and will dispose it when the
        ///     service itself is disposed.
        /// </remarks>
        /// <returns>
        ///     This service builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="apiKey" /> is a null reference.
        /// </exception>
        public T UseHttpClient(string apiKey) {
            // ...
            //
            // Throws an exception if the operation fails.
            var httpClient = new HttpBrowsingClient(apiKey);
            this.SetClient(httpClient, true);
            return (T) this;
        }

        /// <summary>
        ///     Use a Memory Cache.
        /// </summary>
        /// <remarks>
        ///     Use a <see cref="MemoryBrowsingCache" /> to cache a threat that is looked up on the Google Safe
        ///     Browsing API. The <see cref="BaseBrowsingService" /> takes ownership off the memory cache and will
        ///     dispose it when the service itself is disposed.
        /// </remarks>
        /// <returns>
        ///     This service builder.
        /// </returns>
        public T UseMemoryCache() {
            var memoryCache = new MemoryBrowsingCache();
            this.SetCache(memoryCache, true);
            return (T) this;
        }
    }
}