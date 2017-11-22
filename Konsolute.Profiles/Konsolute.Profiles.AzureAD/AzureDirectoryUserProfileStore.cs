// <copyright file="AzureDirectoryUserProfileStore.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AzureAD
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.ActiveDirectory.GraphClient;
    using Konsolute.Profiles.Core;
    using Konsolute.Profiles.Core.Models;
    using Konsolute.Profiles.Core.Services;
    using Konsolute.Profiles.AzureAD.Utilities;
    using Konsolute.Profiles.AzureAD.Authentication;

    /// <summary>
    /// Azure directory user profile store
    /// </summary>
    public class AzureDirectoryUserProfileStore : ISourceUserProfileRepository
    {
        /// <summary>
        /// The configuration provider
        /// </summary>
        private IConfigurationProvider configurationProvider;

        /// <summary>
        /// The tenant identifier
        /// </summary>
        private string tenantId;

        /// <summary>
        /// The resource URL
        /// </summary>
        private string resourceUrl;

        /// <summary>
        /// The authentication helper
        /// </summary>
        private AuthenticationHelper authHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDirectoryUserProfileStore"/> class.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        public AzureDirectoryUserProfileStore(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
            this.tenantId = this.configurationProvider.GetConfigurationValue(KnownConstants.TenantId);
            this.resourceUrl = this.configurationProvider.GetConfigurationValue(KnownConstants.ResourceUrl);
            this.authHelper = new AuthenticationHelper(this.configurationProvider);
        }

        /// <summary>
        /// Authenticates to the source repository
        /// </summary>
        /// <returns>
        /// the awaitable task
        /// </returns>
        public async Task Authenticate()
        {
            await this.authHelper.EnsureValidAccessToken();
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns>
        /// the groups
        /// </returns>
        async Task<IList<GroupInfo>> ISourceUserProfileRepository.GetAllGroups()
        {
            await this.authHelper.EnsureValidAccessToken();
            var allGroups = new List<GroupInfo>();
            var client = new ADClientHelper().GetActiveDirectoryClient(
                this.authHelper,
                this.tenantId,
                this.resourceUrl);
            var pagedCollection = await client.Groups.ExecuteAsync();
            do
            {
                List<IGroup> groups = pagedCollection.CurrentPage.ToList();
                foreach (IGroup group in groups)
                {
                    var groupObj = (Group)group;
                    allGroups.Add(new GroupInfo
                    {
                        GroupId = groupObj.ObjectId,
                        GroupName = groupObj.DisplayName
                    });
                }
                pagedCollection = pagedCollection.GetNextPageAsync().Result;
            } while (pagedCollection != null);

            return allGroups;
        }

        /// <summary>
        /// Gets the profiles in group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>
        /// the user profiles
        /// </returns>
        async Task<IList<UserProfile>> ISourceUserProfileRepository.GetProfilesInGroup(string groupId)
        {
            await this.authHelper.EnsureValidAccessToken();
            var users = new List<UserProfile>();
            var client = new ADClientHelper().GetActiveDirectoryClient(
               this.authHelper,
               this.tenantId,
               this.resourceUrl);
            var group = await client.Groups.GetByObjectId(groupId).ExecuteAsync();
            var groupFetcher = group as IGroupFetcher;
            var pagedCollection = await groupFetcher.Members.ExecuteAsync();
            do
            {
                List<IDirectoryObject> directoryObjects = pagedCollection.CurrentPage.ToList();
                foreach (IDirectoryObject directoryObject in directoryObjects)
                {
                    if (directoryObject is User)
                    {
                        var userObj = (User)directoryObject;
                        users.Add(new UserProfile
                        {
                            UserId = userObj.ObjectId,
                            DisplayName = userObj.DisplayName,
                            FirstName = userObj.GivenName,
                            LastName = userObj.Surname,
                            EmailAddress = userObj.Mail,
                            Department = userObj.Department
                        });
                    }
                }
                pagedCollection = await pagedCollection.GetNextPageAsync();
            } while (pagedCollection != null);

            return users;
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>
        /// true if update is successfull;otherwise false;
        /// </returns>
        async Task<bool> ISourceUserProfileRepository.UpdateProfile(UserProfile userProfile)
        {
            await this.authHelper.EnsureValidAccessToken();
            var client = new ADClientHelper().GetActiveDirectoryClient(
               this.authHelper,
               this.tenantId,
               this.resourceUrl);
            var user = await client.Users.GetByObjectId(userProfile.UserId)
                .ExecuteAsync();
            if (user != null)
            {
                user.DisplayName = userProfile.DisplayName;
                user.Surname = userProfile.LastName;
                user.GivenName = userProfile.FirstName;
                user.Department = userProfile.Department;
                await user.UpdateAsync();
            }

            return true;
        }
    }
}
