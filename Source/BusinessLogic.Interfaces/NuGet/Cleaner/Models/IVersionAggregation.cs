using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes an aggregation of a version of a package.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IVersionAggregation
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        Version Version {get; set;}

        /// <summary>
        /// Gets the regular versions.
        /// </summary>
        /// <value>
        /// The regular versions.
        /// </value>
        [NotNull]
        IList<IOriginalVersionAggregation> RegularVersions {get;}

        /// <summary>
        /// Gets the preview versions.
        /// </summary>
        /// <value>
        /// The preview versions.
        /// </value>
        [NotNull]
        IList<IOriginalVersionAggregation> PreviewVersions {get;}
    }
}