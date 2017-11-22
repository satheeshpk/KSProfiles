// <copyright file="UserProfileService.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AzureAD
{   
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Konsolute.Profiles.Core.Models;
    using Konsolute.Profiles.Core.Services;

    /// <summary>
    /// User Profile Service
    /// </summary>
    public class UserProfileService : ISourceUserProfileService
    {
        /// <summary>
        /// The read user profile repository
        /// </summary>
        private ISourceUserProfileRepository sourceUserProfileRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileService" /> class.
        /// </summary>
        /// <param name="sourceUserProfileRepository">The source user profile repository.</param>
        public UserProfileService(
            ISourceUserProfileRepository sourceUserProfileRepository)
        {
            this.sourceUserProfileRepository = sourceUserProfileRepository;
        }

        /// <summary>
        /// Authenticates to the source service
        /// </summary>
        /// <returns>
        /// the awaitable task
        /// </returns>
        public async Task Authenticate()
        {
            await this.sourceUserProfileRepository.Authenticate();
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns>
        /// the groups
        /// </returns>
        public async Task<IList<GroupInfo>> GetAllGroups()
        {
            return await this.sourceUserProfileRepository.GetAllGroups();
        }

        /// <summary>
        /// Gets the profiles in group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>
        /// the user profiles
        /// </returns>
        public async Task<IList<UserProfile>> GetProfilesInGroup(string groupId)
        {
            return await this.sourceUserProfileRepository.GetProfilesInGroup(groupId);
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>
        /// true if update is successfull;otherwise false;
        /// </returns>
        public async Task<bool> UpdateProfile(UserProfile userProfile)
        {
            return await this.sourceUserProfileRepository.UpdateProfile(userProfile);
        }
    }
}
