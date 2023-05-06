using e2.Framework.Models;
using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes a snapshot of a package group.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageGroupSnapshot
    {
        /// <summary>
        /// Gets the package identifier pattern.
        /// </summary>
        /// <value>
        /// The package identifier pattern.
        /// </value>
        ICorePredicate<string> PackageIdPattern {get;}

        /// <summary>
        /// Gets the package group.
        /// </summary>
        /// <value>
        /// The package group.
        /// </value>
        IPackageGroupConfig PackageGroup {get;}

        /// <summary>
        /// Gets the package cleanup.
        /// </summary>
        /// <value>
        /// The package cleanup.
        /// </value>
        IPackageCleanupConfig PackageCleanup {get;}
    }
}