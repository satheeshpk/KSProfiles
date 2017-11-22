// <copyright file="SharePointContextHelper.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

using System;
using System.Web;

namespace Konsolute.Profiles.SharePoint
{
    /// <summary>
    /// Sharepoint context helper
    /// </summary>
    public static class SharePointContextHelper
    {
        /// <summary>
        /// Validates the and get share point paramters.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">httpRequest</exception>
        public static SharePointContextParameters ValidateAndGetSharePointParamters(HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException("httpRequest");
            }

            // SPHostUrl
            Uri spHostUrl = SharePointContext.GetSPHostUrl(httpRequest);
            if (spHostUrl == null)
            {
                //return null;
            }

            // SPAppWebUrl
            string spAppWebUrlString = TokenHelper.EnsureTrailingSlash(httpRequest.QueryString[SharePointContext.SPAppWebUrlKey]);
            Uri spAppWebUrl;
            if (!Uri.TryCreate(spAppWebUrlString, UriKind.Absolute, out spAppWebUrl) ||
                !(spAppWebUrl.Scheme == Uri.UriSchemeHttp || spAppWebUrl.Scheme == Uri.UriSchemeHttps))
            {
                spAppWebUrl = null;
            }

            // SPLanguage
            string spLanguage = httpRequest.QueryString[SharePointContext.SPLanguageKey];
            if (string.IsNullOrEmpty(spLanguage))
            {
                //return null;
            }

            // SPClientTag
            string spClientTag = httpRequest.QueryString[SharePointContext.SPClientTagKey];
            if (string.IsNullOrEmpty(spClientTag))
            {
                //return null;
            }

            // SPProductNumber
            string spProductNumber = httpRequest.QueryString[SharePointContext.SPProductNumberKey];
            if (string.IsNullOrEmpty(spProductNumber))
            {
                //return null;
            }

            return new SharePointContextParameters(spHostUrl, spAppWebUrl, spLanguage, spClientTag, spProductNumber);
        }

        /// <summary>
        /// Gets the share point context parameters from cache.
        /// </summary>
        /// <returns>the sharepoint context parameters</returns>
        public static SharePointContextParameters GetSharePointContextParametersFromCache()
        {
            var paramCacheID = KnownConstants.SP_CONTEXT_PARAM_CACHE_PREFIX + HttpContext.Current.Session.SessionID;
            return HttpRuntime.Cache[paramCacheID] as SharePointContextParameters;
        }

        /// <summary>
        /// Gets the share point context token from cache.
        /// </summary>
        /// <returns>the token</returns>
        public static string GetSharePointContextTokenFromCache()
        {
            var tokenCacheID = KnownConstants.SP_CONTEXT_TOKEN_CACHE_PREFIX + HttpContext.Current.Session.SessionID;
            return HttpRuntime.Cache[tokenCacheID] as string;
        }
    }
}
