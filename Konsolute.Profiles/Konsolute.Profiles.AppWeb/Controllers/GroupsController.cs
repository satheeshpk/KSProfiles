using Konsolute.Profiles.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Konsolute.Profiles.AppWeb.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class GroupsController : Controller
    {
        /// <summary>
        /// The read user profile service
        /// </summary>
        private ISourceUserProfileService readUserProfileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupsController"/> class.
        /// </summary>
        /// <param name="readUserProfileService">The read user profile service.</param>
        public GroupsController(ISourceUserProfileService readUserProfileService)
        {
            this.readUserProfileService = readUserProfileService;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            var groups = await this.readUserProfileService.GetAllGroups();
            return View("Groups", groups);
        }

        /// <summary>
        /// Synchronizes the profiles.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SyncProfiles(string groupId)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            var profiles = await this.readUserProfileService.GetProfilesInGroup(groupId);
            return true;
        }
    }
}