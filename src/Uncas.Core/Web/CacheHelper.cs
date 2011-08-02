namespace Uncas.Core.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Caching;

    /// <summary>
    /// Cache helper.
    /// </summary>
    public class CacheHelper
    {
        private const double CacheDuration = 60.0 * 5.0;

        private string[] _masterCacheKeyArray = { string.Empty };

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheHelper"/> class.
        /// </summary>
        /// <param name="masterCacheKey">The master cache key.</param>
        public CacheHelper(string masterCacheKey)
        {
            _masterCacheKeyArray = new string[1] { masterCacheKey };
        }

        /// <summary>
        /// Gets the cache item.
        /// </summary>
        /// <param name="rawKey">The raw key.</param>
        /// <returns>The cached item.</returns>
        public object GetCacheItem(string rawKey)
        {
            return HttpRuntime.Cache[GetCacheKey(rawKey)];
        }

        /// <summary>
        /// Adds the cache item.
        /// </summary>
        /// <param name="rawKey">The raw key.</param>
        /// <param name="value">The value.</param>
        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "The cache dependency is stored in cache.")]
        public void AddCacheItem(string rawKey, object value)
        {
            if (value == null)
            {
                return;
            }

            Cache dataCache = HttpRuntime.Cache;

            // Make sure MasterCacheKeyArray[0] is in the cache - if not, add it.
            if (dataCache[_masterCacheKeyArray[0]] == null)
            {
                dataCache[_masterCacheKeyArray[0]] = DateTime.UtcNow;
            }

            // TODO: Test what happens if I dispose this cache dependency after inserting in cache.
            // Adding a cache dependency:
            var dependency =
                new CacheDependency(null, _masterCacheKeyArray);
            dataCache.Insert(
                GetCacheKey(rawKey),
                value,
                dependency,
                DateTime.UtcNow.AddSeconds(CacheDuration),
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Invalidates the cache.
        /// </summary>
        public void InvalidateCache()
        {
            // Remove the cache dependency 
            HttpRuntime.Cache.Remove(_masterCacheKeyArray[0]);
        }

        private string GetCacheKey(string rawKey)
        {
            return string.Concat(_masterCacheKeyArray[0], "-", rawKey);
        }
    }
}