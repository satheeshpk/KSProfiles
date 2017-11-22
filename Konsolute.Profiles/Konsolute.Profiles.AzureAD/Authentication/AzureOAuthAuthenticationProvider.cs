// <copyright file="AzureOAuthAuthenticationProvider.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AzureAD.Authentication
{
    using System.Threading.Tasks;
    using Konsolute.Profiles.Core;
    using Konsolute.Profiles.AzureAD.Utilities;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Azure OAuth authentication provider
    /// </summary>
    public class AzureOAuthAuthenticationProvider : IOAuthAuthenticationProvider
    {
        /// <summary>
        /// The configuration provider
        /// </summary>
        private IConfigurationProvider configurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureOAuthAuthenticationProvider"/> class.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        public AzureOAuthAuthenticationProvider(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        /// <summary>
        /// Authenticates the and get access token.
        /// </summary>
        /// <returns>
        /// the access token
        /// </returns>
        public async Task<string> AuthenticateAndGetAccessToken()
        {
            var authenticationContext = new AuthenticationContext(
                    this.configurationProvider.GetConfigurationValue(KnownConstants.ClientId),
                    false);
            var clientCred = new ClientCredential(
                       this.configurationProvider.GetConfigurationValue(KnownConstants.ClientId),
                       this.configurationProvider.GetConfigurationValue(KnownConstants.AppKey));
            var authenticationResult =
                await authenticationContext.AcquireTokenAsync(this.configurationProvider.GetConfigurationValue(KnownConstants.ResourceUrl), clientCred);
            return authenticationResult.AccessToken;
        }
    }
}
