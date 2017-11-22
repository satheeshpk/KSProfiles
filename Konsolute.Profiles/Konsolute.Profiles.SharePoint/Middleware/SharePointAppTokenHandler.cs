// <copyright file="SharePointAppTokenHandler.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.SharePoint
{   
    using System;
    using System.IdentityModel.Tokens;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Owin;

    /// <summary>
    /// SharePoint app token handler
    /// </summary>
    public class SharePointAppTokenHandler : OwinMiddleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointAppTokenHandler"/> class.
        /// </summary>
        /// <param name="next">the next middleware</param>
        public SharePointAppTokenHandler(OwinMiddleware next) 
            : base(next)
        {
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task Invoke(IOwinContext context)
        {   
            this.EnsureSharePointContext();
            await this.Next.Invoke(context);
        }

        /// <summary>
        /// Validates the and cache share point context.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>true if the sharepoint context is valid;otherwise false</returns>
        private void EnsureSharePointContext()
        {
            var paramCacheID = KnownConstants.SP_CONTEXT_PARAM_CACHE_PREFIX + HttpContext.Current.Session.SessionID;
            var tokenCacheID = KnownConstants.SP_CONTEXT_TOKEN_CACHE_PREFIX + HttpContext.Current.Session.SessionID;
            var contextWrapper = new HttpContextWrapper(HttpContext.Current);
            var httpRequest = contextWrapper.Request;
            string contextTokenString = SharePointContextHelper.GetSharePointContextTokenFromCache();
            var contextParameters = SharePointContextHelper.GetSharePointContextParametersFromCache();
            
            if (HttpRuntime.Cache[paramCacheID] == null || HttpRuntime.Cache[tokenCacheID] == null)
            {    
                contextParameters = SharePointContextHelper.ValidateAndGetSharePointParamters(httpRequest);
                HttpRuntime.Cache[paramCacheID] = contextParameters;
                contextTokenString = TokenHelper.GetContextTokenFromRequest(httpRequest);
                if (string.IsNullOrEmpty(contextTokenString))
                {
                    throw new Exception("contextToken");
                }

                HttpRuntime.Cache[tokenCacheID] = contextTokenString;
            }
            else
            {
                contextTokenString = HttpRuntime.Cache[tokenCacheID].ToString();
            }

            SharePointContextToken contextToken = null;
            try
            {
                contextToken = TokenHelper.ReadAndValidateContextToken(contextTokenString, httpRequest.Url.Authority);
            }
            catch (WebException)
            {
                throw new ArgumentNullException("contextToken");
            }
            catch (AudienceUriValidationFailedException)
            {
                throw new ArgumentNullException("contextToken");
            }
        }
    }
}
