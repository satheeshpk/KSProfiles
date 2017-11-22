// <copyright file="ProfileController.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Konsolute.Profiles.Core.Services;
    using Konsolute.Profiles.Core.Models;
    using System;
    using System.Linq;
    using Konsolute.Profiles.Web.Models;

    /// <summary>
    /// Profile controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class ProfileController : Controller
    {
        /// <summary>
        /// The source user profile service
        /// </summary>
        private ISourceUserProfileService sourceUserProfileService;

        /// <summary>
        /// The target user profile service
        /// </summary>
        private ITargetUserProfileService targetUserProfileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="sourceUserProfileService">The source user profile service.</param>
        public ProfileController(
            ISourceUserProfileService sourceUserProfileService,
            ITargetUserProfileService targetUserProfileService)
        {
            this.sourceUserProfileService = sourceUserProfileService;
            this.targetUserProfileService = targetUserProfileService;

            this.sourceUserProfileService.Authenticate();
        }

        /// <summary>
        /// Groups action
        /// </summary>
        /// <returns>the group action</returns>
        public async Task<ActionResult> Groups()
        {
            var groups = await this.sourceUserProfileService.GetAllGroups();
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
            try
            {
                var profiles = await this.sourceUserProfileService.GetProfilesInGroup(groupId);
                this.targetUserProfileService.AddOrUpdateProfiles(profiles);
                return true;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Profiles action.
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// search and return the profiles.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <returns>the profiles</returns>
        [HttpGet]
        public ActionResult Search(string keyword)
        {
            var profiles = this.targetUserProfileService.GetUserProfiles(keyword);
            return Json(profiles, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the specified user identifier.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>
        /// true if update successfull;otherwise false
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> Update(UserProfile profile)
        {
            try
            {
                await this.sourceUserProfileService.UpdateProfile(profile);
                this.targetUserProfileService.UpdateProfile(profile);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all departments.
        /// </summary>
        /// <returns>the action result</returns>
        [HttpGet]
        public ActionResult Departments()
        {
            var departments = this.targetUserProfileService.GetAllDepartments();
            var retDepartments = departments.Select(x => new DepartmentValue
            {
                Value = x
            }).ToList();

            return Json(retDepartments, JsonRequestBehavior.AllowGet);
        }
    }
}