using e2.NuGet.Cleaner.Models;
using System;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes a decision-maker for the cleanup action of a package.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageCleanupActionDecisionMaker
    {
        /// <summary>
        /// Gets a value indicating whether to unlist a package version.
        /// </summary>
        /// <param name="packageAggregation">The package aggregation.</param>
        /// <param name="packageAggregationAddress">The package aggregation address.</param>
        /// <param name="cleanupConfig">The cleanup configuration.</param>
        /// <param name="now">The current date/time.</param>
        /// <returns>
        /// <c>true</c> to unlist the package version; <c>false</c> otherwise.
        /// </returns>
        [Pure]
        bool GetUnlistVersion(IPackageAggregation packageAggregation, PackageAggregationAddress packageAggregationAddress, IPackageCleanupConfig cleanupConfig, DateTimeOffset now);
    }
}