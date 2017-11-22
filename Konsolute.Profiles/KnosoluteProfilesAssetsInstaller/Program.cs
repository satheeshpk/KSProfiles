using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using System;
using System.Linq;

namespace KnosoluteProfilesAssetsInstaller
{
    class Program
    {
        const string configFilePath = "ProfilesTemplate.xml";

        static void Main(string[] args)
        {
            var inputDetails = GetSiteAndLoginDetails();

            try
            {
                var secureString = new System.Security.SecureString();
                foreach (var c in inputDetails.Item3.ToCharArray()) secureString.AppendChar(c);
                using (var context = new ClientContext(inputDetails.Item1))
                {
                    context.Credentials = new SharePointOnlineCredentials(inputDetails.Item2, secureString);

                    var provider = new XMLFileSystemTemplateProvider(
                        AppDomain.CurrentDomain.BaseDirectory, string.Empty);
                    var template = provider.GetTemplate(configFilePath);

                    template.Connector = provider.Connector;

                    // provision taxonomy
                    var departmentTSetId = AddDepartmentTermSetsAndTerms(context, template.TermGroups);

                    foreach (var field in template.SiteFields)
                    {
                        field.SchemaXml = field.SchemaXml.Replace("{deptermsetid}", $"{{{departmentTSetId.ToString()}}}");
                    }

                    // remove term groups from template
                    template.TermGroups.Clear();

                    context.Web.ApplyProvisioningTemplate(template);

                    // remove all items
                    var list = context.Web.Lists.GetByTitle("Profiles");
                    list.Hidden = true;
                    list.Update();
                    var view = list.Views.GetByTitle("All Items");
                    view.DeleteObject();
                    context.ExecuteQuery();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + " - " + ex.StackTrace);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Adds the term sets and terms.
        /// </summary>
        /// <param name="clientContext">The client context.</param>
        /// <param name="termGroups">The term groups.</param>
        private static Guid AddDepartmentTermSetsAndTerms(ClientContext clientContext, OfficeDevPnP.Core.Framework.Provisioning.Model.TermGroupCollection termGroups)
        {
            var taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
            var termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
            var termGroup = termStore.GetSiteCollectionGroup(clientContext.Site, true);
            termStore.CommitAll();
            var termSets = termGroup.TermSets;
            clientContext.Load(termSets);
            clientContext.ExecuteQuery();
            foreach (var group in termGroups)
            {
                foreach (var termSet in group.TermSets)
                {
                    TermSet tset = null;
                    if (!termSets.Any(x => x.Name.Equals(termSet.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        tset = termGroup.CreateTermSet(termSet.Name, termSet.Id, 1033);
                        termStore.CommitAll();
                        clientContext.ExecuteQuery();
                    }
                    else
                    {
                        tset = termSets.First(x => x.Name.Equals(termSet.Name, StringComparison.OrdinalIgnoreCase));
                    }

                    var terms = tset.Terms;
                    clientContext.Load(terms);
                    clientContext.ExecuteQuery();
                    foreach (var term in termSet.Terms)
                    {
                        if (!terms.Any(x => x.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            var newTerm = tset.CreateTerm(term.Name, 1033, term.Id);
                            termStore.CommitAll();
                            clientContext.ExecuteQuery();
                        }
                    }

                    return tset.Id;
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Gets the site and login details.
        /// </summary>
        /// <returns>the input details</returns>
        private static Tuple<string, string, string> GetSiteAndLoginDetails()
        {
            Console.WriteLine("Enter sharepoint site url");
            string url = Console.ReadLine();

            Console.WriteLine("Enter username");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password");
            string password = Console.ReadLine();

            return new Tuple<string, string, string>(url, username, password);
        }
    }
}
