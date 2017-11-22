// <copyright file="AzureOpenIdConnectAppExtension.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AzureAD.OpenIDConnect
{
    using System;
    using System.Web;
    using Konsolute.Profiles.AzureAD.Authentication.Web;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OpenIdConnect;
    using Owin;
    using System.Threading.Tasks;

    /// <summary>
    /// Azure open id connect app extensions
    /// </summary>
    public static class AzureOpenIdConnectAppExtension
    {
        /// <summary>
        /// Uses the azure open identifier connect.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="appKey">The application key.</param>
        /// <param name="authority">The authority.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="resourceId">The resource identifier.</param>
        public static void UseAzureOpenIDConnect(this IAppBuilder app, string clientId, string appKey, string authority, string redirectUri, string resourceId)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = redirectUri,
                    ProtocolValidator = new Microsoft.IdentityModel.Protocols.OpenIdConnectProtocolValidator()
                    {
                        RequireNonce = true,
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    { 
                        AuthorizationCodeReceived = async (context) =>
                        {
                            var code = context.Code;
                            var credential = new ClientCredential(clientId, appKey);
                            var userObjectID = context.AuthenticationTicket.Identity.FindFirst(
                                "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                            AuthenticationContext authContext = new AuthenticationContext(authority, new NaiveSessionCache(userObjectID));
                            AuthenticationResult result = await authContext.AcquireTokenByAuthorizationCodeAsync(
                                code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, resourceId);

                            HttpContext.Current.Session["OID_TOKEN"] = result.AccessToken;
                        },
                        AuthenticationFailed = (context) =>
                        {
                            //context.HandleResponse();
                            //context.Response.Redirect("/Home/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }
                    },
                });

            
        }
    }
}
