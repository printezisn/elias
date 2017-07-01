using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Elias.Shared.Helpers
{
    /// <summary>
    /// Helper class for caching operations
    /// </summary>
    public static class SCICacheHelper
    {
        /// <summary>
        /// Adds an item in the cache
        /// </summary>
        /// <typeparam name="T">The type of item to add in the cache</typeparam>
        /// <param name="key">The key to identify the item</param>
        /// <param name="value">The item to add</param>
        /// <param name="absoluteExpiration">Absolute expiration for the item's existence in the cache</param>
        /// <param name="slidingExpiration">Sliding expiration for the item's existence in the cache</param>
        /// <param name="priority">The priority of the item</param>
        public static void Add<T>(string key, T value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority)
        {
            HttpRuntime.Cache.Add(key, value, null, absoluteExpiration, slidingExpiration, priority, null);
        }

        /// <summary>
        /// Inserts an item in the cache
        /// </summary>
        /// <typeparam name="T">The type of item to insert in the cache</typeparam>
        /// <param name="key">The key to identify the item</param>
        /// <param name="value">The item to insert</param>
        /// <param name="absoluteExpiration">Absolute expiration for the item's existence in the cache</param>
        /// <param name="slidingExpiration">Sliding expiration for the item's existence in the cache</param>
        /// <param name="priority">The priority of the item</param>
        public static void Insert<T>(string key, T value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority)
        {
            HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, slidingExpiration, priority, null);
        }

        /// <summary>
        /// Inserts an item in the cache
        /// </summary>
        /// <typeparam name="T">The type of item to insert in the cache</typeparam>
        /// <param name="key">The key to identify the item</param>
        /// <param name="value">The item to insert</param>
        public static void Insert<T>(string key, T value)
        {
            HttpRuntime.Cache[key] = value;
        }

        /// <summary>
        /// Returns an item from in the cache
        /// </summary>
        /// <typeparam name="T">The type of item to return</typeparam>
        /// <param name="key">The key to identify the item</param>
        /// <returns>The item</returns>
        public static T Get<T>(string key)
        {
            if (!Exists(key))
            {
                return default(T);
            }

            return (T)HttpRuntime.Cache[key];
        }

        /// <summary>
        /// Checks if an item is in the cache
        /// </summary>
        /// <param name="key">The type of item to return</param>
        /// <returns>True if the item is in the cache, otherwise false</returns>
        public static bool Exists(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">The type of item to return</param>
        public static void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public static void Clear()
        {
            List<string> cacheKeys = new List<string>();

            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                cacheKeys.Add(enumerator.Key.ToString());
            }

            foreach (string key in cacheKeys)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }
    }
}
