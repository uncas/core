﻿using System;
using System.Web;
using System.Web.Caching;

namespace Uncas.Core.Web
{
    /// <summary>
    /// Cache helper.
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheHelper"/> class.
        /// </summary>
        /// <param name="masterCacheKey">The master cache key.</param>
        public CacheHelper(string masterCacheKey)
        {
            MasterCacheKeyArray = new string[1] { masterCacheKey };
        }

        private const double CacheDuration = 60.0 * 5.0;

        /// <summary>
        /// Gets the cache item.
        /// </summary>
        /// <param name="rawKey">The raw key.</param>
        /// <returns></returns>
        public object GetCacheItem(string rawKey)
        {
            return HttpRuntime.Cache[GetCacheKey(rawKey)];
        }

        private string[] MasterCacheKeyArray = { "" };
        private string GetCacheKey(string rawKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", rawKey);
        }

        /// <summary>
        /// Adds the cache item.
        /// </summary>
        /// <param name="rawKey">The raw key.</param>
        /// <param name="value">The value.</param>
        public void AddCacheItem(string rawKey, object value)
        {
            if (value == null)
                return;
            Cache dataCache = HttpRuntime.Cache;
            // Make sure MasterCacheKeyArray[0] is in the cache - if not, add it 
            if (dataCache[MasterCacheKeyArray[0]] == null)
                dataCache[MasterCacheKeyArray[0]] = DateTime.UtcNow;
            // Add a CacheDependency 
            CacheDependency dependency =
                new CacheDependency(null, MasterCacheKeyArray);
            dataCache.Insert(GetCacheKey(rawKey), value, dependency,
                DateTime.UtcNow.AddSeconds(CacheDuration),
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Invalidates the cache.
        /// </summary>
        public void InvalidateCache()
        {
            // Remove the cache dependency 
            HttpRuntime.Cache.Remove(MasterCacheKeyArray[0]);
        }
    }
}