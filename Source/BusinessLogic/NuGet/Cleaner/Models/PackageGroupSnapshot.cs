using e2.Framework.Models;
using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents a snapshot of a package group.
    /// </summary>
    internal sealed class PackageGroupSnapshot: IPackageGroupSnapshot
    {
        /// <inheritdoc />
        public ICorePredicate<string> PackageIdPattern {get;}

        /// <inheritdoc />
        public IPackageGroupConfig PackageGroup {get;}

        /// <inheritdoc />
        public IPackageCleanupConfig PackageCleanup {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageGroupSnapshot" /> class.
        /// </summary>
        /// <param name="packageIdPattern">The package identifier pattern.</param>
        /// <param name="packageGroup">The package group.</param>
        /// <param name="packageCleanup">The package cleanup.</param>
        /// <exception cref="System.ArgumentNullException">
        /// packageIdPattern
        /// or
        /// packageGroup
        /// or
        /// packageCleanup
        /// </exception>
        internal PackageGroupSnapshot(ICorePredicate<string> packageIdPattern, IPackageGroupConfig packageGroup, IPackageCleanupConfig packageCleanup)
        {
#if DEBUG
            if (packageIdPattern == null) throw new ArgumentNullException(nameof(packageIdPattern));
            if (packageGroup == null) throw new ArgumentNullException(nameof(packageGroup));
            if (packageCleanup == null) throw new ArgumentNullException(nameof(packageCleanup));
#endif
            this.PackageIdPattern = packageIdPattern;
            this.PackageGroup = packageGroup;
            this.PackageCleanup = packageCleanup;
        }
    }
}