// <copyright file="WebConfigConfigurationProvider.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.AppWeb
{
    using Konsolute.Profiles.Core;
    using System.Configuration;

    /// <summary>
    /// Web.config configuration provider
    /// </summary>
    public class WebConfigConfigurationProvider : IConfigurationProvider
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="configrationKey">The configration key.</param>
        /// <returns>
        /// the configuration value
        /// </returns>
        public string GetConfigurationValue(string configrationKey)
        {
            return ConfigurationManager.AppSettings[configrationKey];
        }
    }
}