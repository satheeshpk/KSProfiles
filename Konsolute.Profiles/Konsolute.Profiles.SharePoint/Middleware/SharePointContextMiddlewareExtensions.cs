// <copyright file="SharePointContextMiddlewareExtensions.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.SharePoint
{
    using Owin;

    /// <summary>
    /// Sharepoint context middleware extensions
    /// </summary>
    public static class SharePointContextMiddlewareExtensions
    {
        /// <summary>
        /// Users the share point application token handler.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>the application</returns>
        public static IAppBuilder UseSharePointAppTokenHandler(this IAppBuilder app)
        {
            app.Use<SharePointAppTokenHandler>();
            return app;
        }
    }
}
