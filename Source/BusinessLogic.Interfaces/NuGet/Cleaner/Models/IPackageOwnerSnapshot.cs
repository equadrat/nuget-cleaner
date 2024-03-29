﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes a snapshot of a package owner.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageOwnerSnapshot
    {
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        string Owner {get;}

        /// <summary>
        /// Gets the package groups.
        /// </summary>
        /// <value>
        /// The package groups.
        /// </value>
        IReadOnlyList<IPackageGroupSnapshot> PackageGroups {get;}

        /// <summary>
        /// Gets the package group index.
        /// </summary>
        /// <param name="packageId">The package identifier.</param>
        /// <returns>
        /// The package group index or <c>-1</c> if the package identifier doesn't exist.
        /// </returns>
        [Pure]
        int GetPackageGroupIndex(string packageId);

        /// <summary>
        /// Determines whether the package identifier matcheses any package identifier pattern.
        /// </summary>
        /// <param name="packageId">The package identifier.</param>
        /// <returns>
        /// <c>true</c> if the package identifier matches a package identifier pattern.
        /// </returns>
        [Pure]
        bool MatchesAnyPackageIdPattern(string packageId);
    }
}