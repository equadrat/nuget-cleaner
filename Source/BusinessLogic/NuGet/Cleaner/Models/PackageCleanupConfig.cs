using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the configuration of a package cleanup.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageCleanupConfig: IPackageCleanupConfig
    {
        /// <inheritdoc />
        public string PackageCleanupId {get; set;}

        /// <inheritdoc />
        public int RetainVersions {get; set;}

        /// <inheritdoc />
        public TimeSpan Expiry {get; set;}

        /// <inheritdoc />
        public bool RetainPreviewsOfRegularReleases {get; set;}
    }
}