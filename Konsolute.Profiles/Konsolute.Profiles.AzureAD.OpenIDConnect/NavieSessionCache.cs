// <copyright file="NaiveSessionCache.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AzureAD.Authentication.Web
{
    using System.Web;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Naive Session Cache
    /// </summary>
    /// <seealso cref="Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache" />
    public class NaiveSessionCache : TokenCache
    {
        /// <summary>
        /// The file lock
        /// </summary>
        private static readonly object FileLock = new object();

        /// <summary>
        /// The cache identifier
        /// </summary>
        private readonly string CacheId = string.Empty;

        /// <summary>
        /// The user object identifier
        /// </summary>
        private string UserObjectId = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaiveSessionCache"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public NaiveSessionCache(string userId)
        {
            UserObjectId = userId;
            CacheId = UserObjectId + "_TokenCache";

            AfterAccess = AfterAccessNotification;
            BeforeAccess = BeforeAccessNotification;
            Load();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            lock (FileLock)
            {
                if (HttpContext.Current != null)
                {
                    Deserialize((byte[])HttpContext.Current.Session[CacheId]);
                }
            }
        }

        /// <summary>
        /// Persists this instance.
        /// </summary>
        public void Persist()
        {
            lock (FileLock)
            {
                if (HttpContext.Current != null)
                {
                    // reflect changes in the persistent store
                    HttpContext.Current.Session[CacheId] = Serialize();
                    // once the write operation took place, restore the HasStateChanged bit to false
                    HasStateChanged = false;
                }
            }
        }

        /// <summary>
        /// Clears the cache by deleting all the items. Note that if the cache is the default shared cache, clearing it would
        /// impact all the instances of <see cref="T:Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext" /> which share that cache.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Remove(CacheId);
            }
        }

        /// <summary>
        /// Deletes an item from the cache.
        /// </summary>
        /// <param name="item">The item to delete from the cache</param>
        public override void DeleteItem(TokenCacheItem item)
        {
            base.DeleteItem(item);
            Persist();
        }

        // Triggered right before ADAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        private void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            Load();
        }

        // Triggered right after ADAL accessed the cache.
        private void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (HasStateChanged)
            {
                Persist();
            }
        }
    }
}
