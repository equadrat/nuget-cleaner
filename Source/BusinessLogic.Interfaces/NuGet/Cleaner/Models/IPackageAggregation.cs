using e2.Framework;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes an aggregation of the versions of a package.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageAggregation
    {
        /// <summary>
        /// Gets the package identifier.
        /// </summary>
        /// <value>
        /// The package identifier.
        /// </value>
        string PackageId {get; set;}

        /// <summary>
        /// Gets the versions.
        /// </summary>
        /// <value>
        /// The versions.
        /// </value>
        [NotNull]
        IList<IVersionAggregation> Versions {get;}

        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="_">unused</param>
        /// <param name="includePreviewVersions"><c>true</c> to include preview versions.</param>
        /// <param name="includeRegularVersions"><c>true</c> to include regular versions.</param>
        /// <returns>
        /// The addresses.
        /// </returns>
        [Pure]
        [NotNull]
        IEnumerable<PackageAggregationAddress> GetAddresses([CanBeNull] NCObject _ = null, bool includePreviewVersions = false, bool includeRegularVersions = false);

        /// <summary>
        /// Resolves the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>
        /// The resolved package.
        /// </returns>
        [Pure]
        (IVersionAggregation Version, IOriginalVersionAggregation OriginalVersion, IPackageMetadata Package) Resolve(PackageAggregationAddress address);
    }
}