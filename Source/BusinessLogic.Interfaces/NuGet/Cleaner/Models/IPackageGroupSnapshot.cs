using System;
using System.Diagnostics;
using e2.Framework.Models;
using JetBrains.Annotations;

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
        [NotNull]
        ICorePredicate<string> PackageIdPattern {get;}

        /// <summary>
        /// Gets the package group.
        /// </summary>
        /// <value>
        /// The package group.
        /// </value>
        [NotNull]
        IPackageGroupConfig PackageGroup {get;}

        /// <summary>
        /// Gets the package cleanup.
        /// </summary>
        /// <value>
        /// The package cleanup.
        /// </value>
        [NotNull]
        IPackageCleanupConfig PackageCleanup {get;}
    }
}