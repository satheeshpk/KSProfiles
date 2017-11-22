// <copyright file="IConfigurationProvider.cs" company="Konsolute">
//     Copyright (c) 2017 Konsolute. All rights Reserved.
// </copyright>
// <author>Satheesh</author>

namespace Konsolute.Profiles.Core
{
    /// <summary>
    /// Configuration provider contract
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="configrationKey">The configration key.</param>
        /// <returns>
        /// the configuration value
        /// </returns>
        string GetConfigurationValue(string configrationKey);
    }
}
