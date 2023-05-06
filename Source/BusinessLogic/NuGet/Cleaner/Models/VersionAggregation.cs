using System;
using System.Collections.Generic;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents an aggregation of a version of a package.
    /// </summary>
    internal sealed class VersionAggregation: IVersionAggregation
    {
        /// <inheritdoc />
        public Version? Version {get; set;}

        /// <inheritdoc />
        public IList<IOriginalVersionAggregation> RegularVersions {get;}

        /// <inheritdoc />
        public IList<IOriginalVersionAggregation> PreviewVersions {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionAggregation" /> class.
        /// </summary>
        internal VersionAggregation()
        {
            this.RegularVersions = new List<IOriginalVersionAggregation>(1);
            this.PreviewVersions = new List<IOriginalVersionAggregation>();
        }
    }
}