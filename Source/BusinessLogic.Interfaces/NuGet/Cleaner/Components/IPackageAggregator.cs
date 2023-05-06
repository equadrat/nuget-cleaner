using e2.NuGet.Cleaner.Models;
using System;
using System.Collections.Generic;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes an aggregator for packages.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageAggregator
    {
        /// <summary>
        /// Aggregates the specified metadata.
        /// </summary>
        /// <param name="packageMetadata">The package metadata.</param>
        /// <param name="packagePublishDateDictionary">The package publish date dictionary.</param>
        /// <param name="aggregatedPackages">The aggregated packages.</param>
        void Aggregate(IEnumerable<IPackageMetadata> packageMetadata, IPackagePublishDateDictionary packagePublishDateDictionary, ICollection<IPackageAggregation> aggregatedPackages);
    }
}