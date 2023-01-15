using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes an aggregation of an original version of a package.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IOriginalVersionAggregation
    {
        /// <summary>
        /// Gets or sets the original version.
        /// </summary>
        /// <value>
        /// The original version.
        /// </value>
        string OriginalVersion {get; set;}

        /// <summary>
        /// Gets or sets the publish date.
        /// </summary>
        /// <value>
        /// The publish date.
        /// </value>
        DateTimeOffset? PublishDate {get; set;}

        /// <summary>
        /// Gets the packages.
        /// </summary>
        /// <value>
        /// The packages.
        /// </value>
        [NotNull]
        IList<IPackageMetadata> Packages {get;}
    }
}