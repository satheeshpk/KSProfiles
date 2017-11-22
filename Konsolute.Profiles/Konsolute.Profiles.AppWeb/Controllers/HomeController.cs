using Konsolute.Profiles.Core.Services;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Konsolute.Profiles.AppWeb.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// The read user profile service
        /// </summary>
        private ISourceUserProfileService readUserProfileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="readUserProfileService">The read user profile service.</param>
        /// <param name="writeUserProfileService">The write user profile service.</param>
        public HomeController(
            ISourceUserProfileService readUserProfileService)
        {
            this.readUserProfileService = readUserProfileService;
        }

        public ActionResult HandleSignIn()
        {
            return View();
        }

        //[SharePointContextFilter]
        //[Authorize]
        public async Task<ActionResult> Index()
        {
            //User spUser = null;

            // make sure that the sharepoint context is persisted
            //var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            //using (var clientContext = spContext.CreateUserClientContextForSPHost())
            //{
            //    if (clientContext != null)
            //    {
            //        spUser = clientContext.Web.CurrentUser;

            //        clientContext.Load(spUser, user => user.Title);

            //        clientContext.ExecuteQuery();

            //        ViewBag.UserName = spUser.Title;
            //    }
            //}
            //if (this.Request.IsAuthenticated)
            //{
            //    var groups = await this.readUserProfileService.GetAllGroups();
            //    var builder = new System.Text.StringBuilder();
            //    foreach (var group in groups)
            //    {
            //        builder.Append($"{group.GroupName},");
            //    }

            //    ViewBag.Groups = builder.ToString().TrimEnd(',');
            //}

            return View();
        }

        public void SignIn()
        {
            if (!this.Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
