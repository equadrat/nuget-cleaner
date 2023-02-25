using JetBrains.Annotations;
using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes the configuration of a package cleanup.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageCleanupConfig
    {
        /// <summary>
        /// Gets the package cleanup identifier.
        /// </summary>
        /// <value>
        /// The package cleanup identifier.
        /// </value>
        string PackageCleanupId {get;}

        /// <summary>
        /// Gets the number of versions to retain.
        /// </summary>
        /// <value>
        /// The number of versions to retain.
        /// </value>
        int RetainVersions {get;}

        /// <summary>
        /// Gets the expiry.
        /// </summary>
        /// <value>
        /// The expiry.
        /// </value>
        TimeSpan Expiry {get;}

        /// <summary>
        /// Gets a value indicating whether to retain previews of regular releases.
        /// </summary>
        /// <value>
        /// <c>true</c> to retain previews of regular releases; otherwise, <c>false</c>.
        /// </value>
        bool RetainPreviewsOfRegularReleases {get;}
    }
}