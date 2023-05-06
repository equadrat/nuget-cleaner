using e2.Framework.Enums;
using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes the configuration of a package group.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageGroupConfig
    {
        /// <summary>
        /// Gets the source identifier.
        /// </summary>
        /// <value>
        /// The source identifier.
        /// </value>
        string? SourceId {get;}

        /// <summary>
        /// Gets the API key identifier.
        /// </summary>
        /// <value>
        /// The API key identifier.
        /// </value>
        string? ApiKeyId {get;}

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        string? Owner {get;}

        /// <summary>
        /// Gets the package identifier pattern.
        /// </summary>
        /// <value>
        /// The package identifier pattern.
        /// </value>
        string? PackageIdPattern {get;}

        /// <summary>
        /// Gets the package identifier match mode.
        /// </summary>
        /// <value>
        /// The package identifier match mode.
        /// </value>
        eCoreStringMatchMode PackageIdMatchMode {get;}

        /// <summary>
        /// Gets the package cleanup identifier.
        /// </summary>
        /// <value>
        /// The package cleanup identifier.
        /// </value>
        string? PackageCleanupId {get;}
    }
}