// <copyright file="ADClientHelper.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AzureAD.Utilities
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.ActiveDirectory.GraphClient;
    using Konsolute.Profiles.AzureAD.Authentication;

    /// <summary>
    /// Azure active directory client helper
    /// </summary>
    internal class ADClientHelper
    {
        /// <summary>
        /// The token
        /// </summary>
        private string token = string.Empty;

        /// <summary>
        ///     Async task to acquire token for Application.
        /// </summary>
        /// <returns>Async Token for application.</returns>
        internal async Task<string> AcquireTokenAsync(AuthenticationHelper authHelper)
        {
            if (authHelper != null && authHelper.AccessToken != null)
            {
                return authHelper.AccessToken.Item1;
            }

            throw new Exception("Request not authenticated");
        }

        /// <summary>
        /// Get Active Directory Client for Application.
        /// </summary>
        /// <param name="authHelper">The authentication helper.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="resourceUrl">The resource URL.</param>
        /// <returns>
        /// ActiveDirectoryClient for Application.
        /// </returns>
        internal ActiveDirectoryClient GetActiveDirectoryClient(AuthenticationHelper authHelper, string tenantId, string resourceUrl)
        {
            Uri baseServiceUri = new Uri(resourceUrl);
            ActiveDirectoryClient activeDirectoryClient =
                new ActiveDirectoryClient(new Uri(baseServiceUri, tenantId),
                    async () => await AcquireTokenAsync(authHelper));
            return activeDirectoryClient;
        }
    }
}
