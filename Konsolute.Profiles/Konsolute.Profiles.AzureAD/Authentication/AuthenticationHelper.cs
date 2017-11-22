// <copyright file="AuthenticationHelper.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AzureAD.Authentication
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Konsolute.Profiles.Core;

    /// <summary>
    /// Azure active directory helper
    /// </summary>
    internal class AuthenticationHelper
    {
        /// <summary>
        /// The authority
        /// </summary>
        private readonly string authority;

        /// <summary>
        /// The application redirect URL
        /// </summary>
        private readonly string appRedirectUrl;

        /// <summary>
        /// The resource URL
        /// </summary>
        private readonly string resourceUrl;

        /// <summary>
        /// The client identifier
        /// </summary>
        private readonly string clientId;

        /// <summary>
        /// The client credential
        /// </summary>
        private readonly ClientCredential clientCredential;

        /// <summary>
        /// The authentication context
        /// </summary>
        private readonly AuthenticationContext authenticationContext;

        /// <summary>
        /// The configuration provider
        /// </summary>
        private IConfigurationProvider configurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationHelper"/> class.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        public AuthenticationHelper(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
            this.authority = configurationProvider.GetConfigurationValue("ida:Tenant");
            this.appRedirectUrl = configurationProvider.GetConfigurationValue("ida:PostLogoutRedirectUri");
            this.resourceUrl = configurationProvider.GetConfigurationValue("ida:GraphUrl");
            this.clientId = configurationProvider.GetConfigurationValue("ida:ClientId");
            var appKey = configurationProvider.GetConfigurationValue("ida:AppKey");
            this.clientCredential = new ClientCredential(this.clientId, appKey);
            this.authenticationContext = new AuthenticationContext("https://login.windows.net/common/" +
                                              this.authority);
        }

        /// <summary>
        /// Gets the authorize URL.
        /// </summary>
        /// <value>
        /// The authorize URL.
        /// </value>
        private string AuthorizeUrl
        {
            get
            {
                return string.Format("https://login.windows.net/{0}/oauth2/authorize?response_type=code&redirect_uri={1}&client_id={2}&state={3}",
                    authority,
                    appRedirectUrl,
                    clientId,
                    Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public Tuple<string, DateTimeOffset> AccessToken
        {
            get
            {
                return HttpRuntime.Cache["AccessTokenWithExpireTime-" + resourceUrl]
                    as Tuple<string, DateTimeOffset>;
            }
            set
            {
                HttpRuntime.Cache["AccessTokenWithExpireTime-" + resourceUrl] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is access token valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is access token valid; otherwise, <c>false</c>.
        /// </value>
        private bool IsAccessTokenValid
        {
            get
            {
                if (AccessToken != null)
                {
                    return (!string.IsNullOrEmpty(AccessToken.Item1) &&
                        AccessToken.Item2 > DateTimeOffset.UtcNow);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Determines whether [is authorization code not null] [the specified authentication code].
        /// </summary>
        /// <param name="authCode">The authentication code.</param>
        /// <returns>
        ///   <c>true</c> if [is authorization code not null] [the specified authentication code]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAuthorizationCodeNotNull(string authCode)
        {
            return !string.IsNullOrEmpty(authCode);
        }

        /// <summary>
        /// Acquires the tokens using authentication code.
        /// </summary>
        /// <param name="authCode">The authentication code.</param>
        /// <returns>the authentication code information</returns>
        private async Task<Tuple<Tuple<string, DateTimeOffset>, string>> AcquireTokensUsingAuthCode(string authCode)
        {
            var authResult = await authenticationContext.AcquireTokenByAuthorizationCodeAsync(
                        authCode,
                        new Uri(appRedirectUrl),
                        clientCredential,
                        resourceUrl);

            return new Tuple<Tuple<string, DateTimeOffset>, string>(
                        new Tuple<string, DateTimeOffset>(authResult.AccessToken, authResult.ExpiresOn),
                        string.Empty);
        }

        /// <summary>
        /// Ensures the valid access token.
        /// </summary>
        /// <returns>the awaitable task</returns>
        internal async Task EnsureValidAccessToken()
        {
            if (IsAccessTokenValid)
            {
                return;
            }
            else if (IsAuthorizationCodeNotNull(HttpContext.Current.Request.QueryString["code"]))
            {
                Tuple<Tuple<string, DateTimeOffset>, string> tokens = null;

                try
                {
                    tokens = await AcquireTokensUsingAuthCode(HttpContext.Current.Request.QueryString["code"]);
                }
                catch
                {
                    HttpContext.Current.Response.Redirect(AuthorizeUrl);
                }

                AccessToken = tokens.Item1;
                return;
            }
            else
            {
                HttpContext.Current.Response.Redirect(AuthorizeUrl);
            }
        }
    }
}
