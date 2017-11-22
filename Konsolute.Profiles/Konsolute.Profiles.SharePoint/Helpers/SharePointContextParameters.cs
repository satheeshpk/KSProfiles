// <copyright file="SharePointContextParameters.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.SharePoint
{
    using System;

    /// <summary>
    /// Sharepoint context parameters
    /// </summary>
    public class SharePointContextParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointContextParameters"/> class.
        /// </summary>
        /// <param name="spHostUrl">The sp host URL.</param>
        /// <param name="spAppWebUrl">The sp application web URL.</param>
        /// <param name="spLanguage">The sp language.</param>
        /// <param name="spClientTag">The sp client tag.</param>
        /// <param name="spProductNumber">The sp product number.</param>
        public SharePointContextParameters(
            Uri spHostUrl,
            Uri spAppWebUrl,
            string spLanguage,
            string spClientTag,
            string spProductNumber)
        {
            this.SPHostUrl = spHostUrl;
            this.SPAppWebUrl = spAppWebUrl;
            this.SPLanguage = spLanguage;
            this.SPClientTag = spClientTag;
            this.SPProductNumber = spProductNumber;
        }

        /// <summary>
        /// Gets the sp host URL.
        /// </summary>
        /// <value>
        /// The sp host URL.
        /// </value>
        public Uri SPHostUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sp application web URL.
        /// </summary>
        /// <value>
        /// The sp application web URL.
        /// </value>
        public Uri SPAppWebUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sp language.
        /// </summary>
        /// <value>
        /// The sp language.
        /// </value>
        public string SPLanguage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sp client tag.
        /// </summary>
        /// <value>
        /// The sp client tag.
        /// </value>
        public string SPClientTag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sp product number.
        /// </summary>
        /// <value>
        /// The sp product number.
        /// </value>
        public string SPProductNumber
        {
            get;
            private set;
        }
    }
}
