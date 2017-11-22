// <copyright file="ITargetUserProfileRepository.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Konsolute.Profiles.Core.Models;

    /// <summary>
    /// Target user profile repository contract
    /// </summary>
    public interface ITargetUserProfileRepository
    {
        /// <summary>
        /// Adds the or update profiles.
        /// </summary>
        /// <param name="profiles">The profiles.</param>
        /// <returns>true if success;otherwise false</returns>
        bool AddOrUpdateProfiles(IList<UserProfile> profiles);

        /// <summary>
        /// Gets the user profiles.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <returns>
        /// the user profiles
        /// </returns>
        IList<UserProfile> GetUserProfiles(string keyword);

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>
        /// true if update is successfull;otherwise false;
        /// </returns>
        bool UpdateProfile(UserProfile userProfile);

        /// <summary>
        /// Gets all departments.
        /// </summary>
        /// <returns>the departments</returns>
        IList<string> GetAllDepartments();
    }
}
