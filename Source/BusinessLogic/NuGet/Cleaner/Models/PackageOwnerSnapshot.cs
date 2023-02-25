using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents a snapshot of a package owner.
    /// </summary>
    internal sealed class PackageOwnerSnapshot: IPackageOwnerSnapshot
    {
        /// <summary>
        /// Gets the package group index.
        /// </summary>
        /// <param name="packageGroups">The package groups.</param>
        /// <param name="packageId">The package identifier.</param>
        /// <returns>
        /// The package group index or <c>-1</c> if the package identifier doesn't exist.
        /// </returns>
        [Pure]
        private static int GetPackageGroupIndex([NotNull] IReadOnlyList<IPackageGroupSnapshot> packageGroups, [NotNull] string packageId)
        {
            var numberOfPackageGroups = packageGroups.Count;
            for (var i = 0; i < numberOfPackageGroups; i++)
            {
                if (packageGroups[i].PackageIdPattern.IsMatch(packageId)) return i;
            }

            return -1;
        }

        /// <inheritdoc />
        public string Owner {get;}

        /// <inheritdoc />
        public IReadOnlyList<IPackageGroupSnapshot> PackageGroups {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageOwnerSnapshot" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="packageGroups">The package groups.</param>
        /// <exception cref="System.ArgumentNullException">
        /// owner
        /// or
        /// packageGroups
        /// </exception>
        internal PackageOwnerSnapshot([NotNull] string owner, [NotNull] IReadOnlyList<IPackageGroupSnapshot> packageGroups)
        {
#if DEBUG
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (packageGroups == null) throw new ArgumentNullException(nameof(packageGroups));
#endif
            this.Owner = owner;
            this.PackageGroups = packageGroups;
        }

        /// <inheritdoc />
        public int GetPackageGroupIndex(string packageId)
        {
            if (packageId == null) throw new ArgumentNullException(nameof(packageId));

            return GetPackageGroupIndex(this.PackageGroups, packageId);
        }

        /// <inheritdoc />
        public bool MatchesAnyPackageIdPattern(string packageId)
        {
            if (packageId == null) throw new ArgumentNullException(nameof(packageId));

            return GetPackageGroupIndex(this.PackageGroups, packageId) != -1;
        }
    }
}