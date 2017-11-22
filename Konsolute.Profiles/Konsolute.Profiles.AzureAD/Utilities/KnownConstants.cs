// <copyright file="KnownConstants.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AzureAD.Utilities
{
    /// <summary>
    /// Known constants
    /// </summary>
    internal class KnownConstants
    {
        /// <summary>
        /// The resource URL
        /// </summary>
        internal const string ResourceUrl = "ida:GraphUrl";

        /// <summary>
        /// The client identifier
        /// </summary>
        internal const string ClientId = "ida:ClientId";

        /// <summary>
        /// The application key
        /// </summary>
        internal const string AppKey = "ida:AppKey";

        /// <summary>
        /// The tenant identifier
        /// </summary>
        internal const string TenantId = "ida:TenantId";

        /// <summary>
        /// The authentication string
        /// </summary>
        internal const string AuthString = "ida:Auth";

        /// <summary>
        /// The tenant
        /// </summary>
        internal const string Tenant = "ida:Tenant";

        /// <summary>
        /// The client secret
        /// </summary>
        internal const string ClientSecret = "ida:ClientSecret";

        /// <summary>
        /// The session token key
        /// </summary>
        internal const string SessionTokenKey = "OID_TOKEN";
    }
}
