// <copyright file="ITargetUserProfileService.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Konsolute.Profiles.Core.Models;
    

    /// <summary>
    /// Target user profile service contract
    /// </summary>
    public interface ITargetUserProfileService
    {
        /// <summary>
        /// Adds the or update profiles.
        /// </summary>
        /// <param name="profiles">The profiles.</param>
        /// <returns>true if success;otherwise false</returns>
        bool AddOrUpdateProfiles(IList<UserProfile> profiles);

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>
        /// true if update is successfull;otherwise false;
        /// </returns>
        bool UpdateProfile(UserProfile userProfile);

        /// <summary>
        /// Gets the user profiles.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <returns>
        /// the user profiles
        /// </returns>
        IList<UserProfile> GetUserProfiles(string keyword);

        /// <summary>
        /// Gets all departments.
        /// </summary>
        /// <returns>the departments</returns>
        IList<string> GetAllDepartments();
    }
}
