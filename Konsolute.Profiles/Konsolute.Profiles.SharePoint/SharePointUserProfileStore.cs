// <copyright file="SharePointUserProfileStore.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.SharePoint
{
    using System.Collections.Generic;
    using System.Web;
    using Konsolute.Profiles.Core.Models;
    using Konsolute.Profiles.Core.Repositories;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Taxonomy;

    /// <summary>
    /// Sharepoint user profile store
    /// </summary>
    public class SharePointUserProfileStore : ITargetUserProfileRepository
    {   
        /// <summary>
        /// Adds the or update profiles.
        /// </summary>
        /// <param name="profiles">The profiles.</param>
        /// <returns>
        /// true if success;otherwise false
        /// </returns>
        public bool AddOrUpdateProfiles(IList<UserProfile> profiles)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    var profilesList = clientContext.Web.Lists.GetByTitle(KnownConstants.LIST_TITLE_PROFILES);
                    var taxonomyField = this.LoadTaxonomyField(clientContext, profilesList, KnownConstants.FIELD_NAME_PROFILES_DEPARTMENT);
                    var termSet = this.LoadTermSet(clientContext, KnownConstants.TERMSET_NAME_DEPARTMENTS);
                    foreach (var profile in profiles)
                    {
                        this.AddOrUpdateProfile(clientContext, profile, profilesList, taxonomyField, termSet);
                    }
                }
            }

            return true;
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
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    var userProfiles = new List<UserProfile>();
                    var query = new CamlQuery();
                    query.ViewXml = $"<View>" +
                            $"<Query>" +
                                $"<Where>" +
                                    $"<Or>" +
                                        $"<Or>" +
                                            $"<Contains><FieldRef Name='{KnownConstants.FIELD_NAME_PROFILES_DISPLAYNAME}'/><Value Type='Text'>{keyword}</Value></Contains>" +
                                            $"<Contains><FieldRef Name='{KnownConstants.FIELD_NAME_PROFILES_FIRSTNAME}'/><Value Type='Text'>{keyword}</Value></Contains>" +
                                        $"</Or>" +
                                        $"<Contains><FieldRef Name='{KnownConstants.FIELD_NAME_PROFILES_LASTNAME}'/><Value Type='Text'>{keyword}</Value></Contains>" +
                                    $"</Or>" +
                                $"</Where>" +
                            $"</Query>" +
                        $"</View>";

                    var profilesList = clientContext.Web.Lists.GetByTitle(KnownConstants.LIST_TITLE_PROFILES);
                    var taxonomyField = this.LoadTaxonomyField(clientContext, profilesList, KnownConstants.FIELD_NAME_PROFILES_DEPARTMENT);
                    var items = profilesList.GetItems(query);
                    clientContext.Load(items);
                    clientContext.ExecuteQuery();

                    foreach (var item in items)
                    {
                        var profile = new UserProfile
                        {
                            UserId = this.GetValueFromItem(item, KnownConstants.FIELD_NAME_PROFILES_OBJECTID),
                            DisplayName = this.GetValueFromItem(item, KnownConstants.FIELD_NAME_PROFILES_DISPLAYNAME),
                            FirstName = this.GetValueFromItem(item, KnownConstants.FIELD_NAME_PROFILES_FIRSTNAME),
                            LastName = this.GetValueFromItem(item, KnownConstants.FIELD_NAME_PROFILES_LASTNAME),
                            EmailAddress = this.GetValueFromItem(item, KnownConstants.FIELD_NAME_PROFILES_EMAIL),
                            Department = this.GetTaxonomyFieldValue(taxonomyField, item, KnownConstants.FIELD_NAME_PROFILES_DEPARTMENT)
                        };

                        userProfiles.Add(profile);
                    }

                    return userProfiles;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>
        /// true if update is successfull;otherwise false;
        /// </returns>
        public bool UpdateProfile(UserProfile userProfile)
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    var profilesList = clientContext.Web.Lists.GetByTitle(KnownConstants.LIST_TITLE_PROFILES);
                    var taxonomyField = this.LoadTaxonomyField(clientContext, profilesList, KnownConstants.FIELD_NAME_PROFILES_DEPARTMENT);
                    var termSet = this.LoadTermSet(clientContext, KnownConstants.TERMSET_NAME_DEPARTMENTS);

                    this.AddOrUpdateProfile(clientContext, userProfile, profilesList, taxonomyField, termSet);
                }
            }

            return true;
        }

        /// <summary>
        /// Gets all departments.
        /// </summary>
        /// <returns>
        /// the departments
        /// </returns>
        public IList<string> GetAllDepartments()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    var taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
                    var termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
                    var termGroup = termStore.GetSiteCollectionGroup(clientContext.Site, false);
                    var termSet = termGroup.TermSets.GetByName(KnownConstants.TERMSET_NAME_DEPARTMENTS);
                    var terms = termSet.Terms;
                    clientContext.Load(terms);
                    clientContext.ExecuteQuery();
                    var departments = new List<string>();
                    foreach (var term in terms)
                    {
                        departments.Add(term.Name);
                    }

                    return departments;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the value from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>the field value</returns>
        private string GetValueFromItem(ListItem item, string fieldName)
        {
            return item[fieldName] != null ? item[fieldName].ToString() : string.Empty;
        }

        /// <summary>
        /// Gets the taxonomy field value.
        /// </summary>
        /// <param name="taxonomyField">The taxonomy field.</param>
        /// <param name="item">The item.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>the field value</returns>
        private string GetTaxonomyFieldValue(TaxonomyField taxonomyField, ListItem item, string fieldName)
        {
            if(item[fieldName] != null)
            {   
                var taxonomyFieldValue = (TaxonomyFieldValue)item[fieldName];
                return taxonomyFieldValue.Label;
            }

            return string.Empty;
        }

        /// <summary>
        /// Loads the taxonomy field.
        /// </summary>
        /// <param name="clientContext">The client context.</param>
        /// <param name="list">The list.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>the taxonomy field</returns>
        private TaxonomyField LoadTaxonomyField(ClientContext clientContext, List list, string fieldName)
        {
            var spField = list.Fields.GetFieldByInternalName(fieldName);
            var taxonomyField = (TaxonomyField)spField;
            return taxonomyField;
        }

        /// <summary>
        /// Loads the term set.
        /// </summary>
        /// <param name="clientContext">The client context.</param>
        /// <param name="termSetName">Name of the term set.</param>
        /// <returns>the term set</returns>
        private TermSet LoadTermSet(ClientContext clientContext, string termSetName)
        {
            var taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
            var termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
            var termGroup = termStore.GetSiteCollectionGroup(clientContext.Site, false);
            var termSet = termGroup.TermSets.GetByName(termSetName);
            clientContext.Load(termSet);
            clientContext.ExecuteQuery();

            return termSet;
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="clientContext">The client context.</param>
        /// <param name="profile">The profile.</param>
        /// <param name="profilesList">The profiles list.</param>
        /// <param name="departmentField">The department field.</param>
        /// <param name="departmentTermSet">The department term set.</param>
        private void AddOrUpdateProfile(ClientContext clientContext, UserProfile profile, List profilesList, TaxonomyField departmentField, TermSet departmentTermSet)
        {
            Term departmentTerm = null;
            if (!string.IsNullOrEmpty(profile.Department))
            {
                departmentTerm = departmentTermSet.Terms.GetByName(profile.Department);
                clientContext.Load(departmentTerm);
            }

            var query = this.GetCamlQueryForUserId(profile.UserId);
            var items = profilesList.GetItems(query);
            clientContext.Load(items);
            clientContext.ExecuteQuery();
            ListItem listItem = null;
            if (items.Count > 0)
            {
                listItem = items[0];
            }
            else
            {
                listItem = profilesList.AddItem(new ListItemCreationInformation());
                listItem[KnownConstants.FIELD_NAME_PROFILES_OBJECTID] = profile.UserId;
            }

            listItem[KnownConstants.FIELD_NAME_PROFILES_DISPLAYNAME] = profile.DisplayName;
            listItem[KnownConstants.FIELD_NAME_PROFILES_FIRSTNAME] = profile.FirstName;
            listItem[KnownConstants.FIELD_NAME_PROFILES_LASTNAME] = profile.LastName;
            listItem[KnownConstants.FIELD_NAME_PROFILES_EMAIL] = profile.EmailAddress;
            listItem.Update();
            if (departmentTerm != null)
            {
                var taxonomyFieldValue = new TaxonomyFieldValue();
                taxonomyFieldValue.Label = departmentTerm.Name;
                taxonomyFieldValue.TermGuid = departmentTerm.Id.ToString();
                taxonomyFieldValue.WssId = -1;
                departmentField.SetFieldValueByValue(listItem, taxonomyFieldValue);
                listItem.Update();
            }

            clientContext.ExecuteQuery();
        }

        /// <summary>
        /// Gets the caml query for user identifier.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns>the caml query</returns>
        private CamlQuery GetCamlQueryForUserId(string objectId)
        {
            var camlQuery = new CamlQuery();
            camlQuery.ViewXml = $"<View>" +
                $"<Query>" +
                $"<Where>" +
                    $"<Eq>" +
                        $"<FieldRef Name='KSLProfileObjectID' />" +
                        $"<Value Type='Text'>{objectId}</Value>" +
                    $"</Eq>" +
                $"</Where>" +
                $"</Query>" +
                $"<RowLimit>1</RowLimit>" +
                $"</View>";
            return camlQuery;
        }
    }
}
