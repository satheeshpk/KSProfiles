// <copyright file="Startup.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

[assembly: Microsoft.Owin.OwinStartup(typeof(Konsolute.Profiles.Web.Startup))]
namespace Konsolute.Profiles.Web
{   
    using Owin;
    using Konsolute.Profiles.SharePoint;

    /// <summary>
    /// Owin start up
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            app.UseSharePointAppTokenHandler();
        }
    }
}
