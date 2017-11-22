using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Web.Mvc;
using Konsolute.Profiles.AzureAD.Authentication.Web;
using Konsolute.Profiles.AzureAD.OpenIDConnect;

[assembly: OwinStartup(typeof(Konsolute.Profiles.AppWeb.Startup))]

namespace Konsolute.Profiles.AppWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {   
            var clientId = ConfigurationManager.AppSettings["ida:ClientId"];
            var appKey = ConfigurationManager.AppSettings["ida:AppKey"];
            var aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
            var tenant = ConfigurationManager.AppSettings["ida:Tenant"];
            var redirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
            var authority = string.Format(aadInstance, tenant);
            var graphResourceId = ConfigurationManager.AppSettings["ida:GraphUrl"];

            //app.UseAzureOpenIDConnect(clientId, appKey, authority, redirectUri, graphResourceId);
        }
    }
}
