using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes the configuration of an API key.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IApiKeyConfig
    {
        /// <summary>
        /// Gets the API key identifier.
        /// </summary>
        /// <value>
        /// The API key identifier.
        /// </value>
        string ApiKeyId {get;}

        /// <summary>
        /// Gets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        string ApiKey {get;}
    }
}