// <copyright file="GroupInfo.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.Core.Models
{
    using System;

    /// <summary>
    /// Group information
    /// </summary>
    public class GroupInfo
    {
        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        public string GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string GroupName
        {
            get;
            set;
        }
    }
}
