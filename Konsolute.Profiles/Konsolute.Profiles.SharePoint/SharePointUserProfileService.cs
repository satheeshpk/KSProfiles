// <copyright file="SharePointUserProfileService.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.SharePoint
{
    using System.Collections.Generic;
    using Konsolute.Profiles.Core.Models;
    using Konsolute.Profiles.Core.Services;
    using Konsolute.Profiles.Core.Repositories;

    /// <summary>
    /// Sharepoint user profile service
    /// </summary>
    /// <seealso cref="Konsolute.Profiles.Core.Services.ITargetUserProfileService" />
    public class SharePointUserProfileService : ITargetUserProfileService
    {
        /// <summary>
        /// The target user profile repository
        /// </summary>
        private ITargetUserProfileRepository targetUserProfileRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointUserProfileService" /> class.
        /// </summary>
        /// <param name="targetUserProfileRepository">The target user profile repository.</param>
        public SharePointUserProfileService(ITargetUserProfileRepository targetUserProfileRepository)
        {
            this.targetUserProfileRepository = targetUserProfileRepository;
        }

        /// <summary>
        /// Adds the or update profiles.
        /// </summary>
        /// <param name="profiles">The profiles.</param>
        /// <returns>
        /// true if success;otherwise false
        /// </returns>
        public bool AddOrUpdateProfiles(IList<UserProfile> profiles)
        {
            return this.targetUserProfileRepository.AddOrUpdateProfiles(profiles);
        }

        /// <summary>
        /// Gets the user profiles.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <returns>
        /// the user profiles
        /// </returns>
        public IList<UserProfile> GetUserProfiles(string keyword)
        {
            return this.targetUserProfileRepository.GetUserProfiles(keyword);
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>the user profile</returns>
        public bool UpdateProfile(UserProfile userProfile)
        {
            return this.targetUserProfileRepository.UpdateProfile(userProfile);
        }

        /// <summary>
        /// Gets all departments.
        /// </summary>
        /// <returns>the departments</returns>
        public IList<string> GetAllDepartments()
        {
            return this.targetUserProfileRepository.GetAllDepartments();
        }
    }
}
