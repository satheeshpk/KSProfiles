// <copyright file="ISourceUserProfileRepository.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.Core.Services
{   
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Konsolute.Profiles.Core.Models;

    /// <summary>
    /// Get User profile store contract
    /// </summary>
    public interface ISourceUserProfileRepository
    {
        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns>the groups</returns>
        Task<IList<GroupInfo>> GetAllGroups();

        /// <summary>
        /// Gets the profiles in group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>the user profiles</returns>
        Task<IList<UserProfile>> GetProfilesInGroup(string groupId);

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>true if update is successfull;otherwise false;</returns>
        Task<bool> UpdateProfile(UserProfile userProfile);

        /// <summary>
        /// Authenticates to the source repository
        /// </summary>
        /// <returns>the awaitable task</returns>
        Task Authenticate();
    }
}
