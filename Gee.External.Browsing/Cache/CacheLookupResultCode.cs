namespace Gee.External.Browsing.Cache {
    /// <summary>
    ///     Cache Lookup Result Code.
    /// </summary>
    public enum CacheLookupResultCode {
        /// <summary>
        ///     Indicates an error.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Indicates a cache miss if a <see cref="SafeCacheEntry" /> and an <see cref="UnsafeCacheEntry" /> do
        ///     not exist in a <see cref="IBrowsingCache"/>, or if a safe cache entry exists but has expired and an
        ///     unsafe cache entry does not exist, or if a safe cache entry and an unsafe cache entry exist but have
        ///     expired.
        /// </summary>
        Miss = 1,

        /// <summary>
        ///     Indicates a cache safe hit if a <see cref="SafeCacheEntry" /> exists in a
        ///     <see cref="IBrowsingCache" /> and has not expired.
        /// </summary>
        SafeHit = 2,

        /// <summary>
        ///     Indicates a cache unsafe hit if an <see cref="UnsafeCacheEntry" /> exists in a
        ///     <see cref="IBrowsingCache" /> and has not expired.
        /// </summary>
        UnsafeHit = 3
    }
}