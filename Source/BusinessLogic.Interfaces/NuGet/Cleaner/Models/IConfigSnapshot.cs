using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes a snapshots of the configuration.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IConfigSnapshot
    {
        /// <summary>
        /// Gets the sources.
        /// </summary>
        /// <value>
        /// The sources.
        /// </value>
        [NotNull]
        IReadOnlyDictionary<string, ISourceConfig> Sources {get;}

        /// <summary>
        /// Gets the API keys.
        /// </summary>
        /// <value>
        /// The API keys.
        /// </value>
        [NotNull]
        IReadOnlyDictionary<string, IApiKeyConfig> ApiKeys {get;}

        /// <summary>
        /// Gets the package cleanups.
        /// </summary>
        /// <value>
        /// The package cleanups.
        /// </value>
        [NotNull]
        IReadOnlyDictionary<string, IPackageCleanupConfig> PackageCleanups {get;}

        /// <summary>
        /// Gets the package sources.
        /// </summary>
        /// <value>
        /// The package sources.
        /// </value>
        [NotNull]
        IReadOnlyList<IPackageSourceSnapshot> PackageSources {get;}

        /// <summary>
        /// Gets the logging output.
        /// </summary>
        /// <returns>
        /// The logging output.
        /// </returns>
        [Pure]
        [NotNull]
        string GetLoggingOutput();
    }
}