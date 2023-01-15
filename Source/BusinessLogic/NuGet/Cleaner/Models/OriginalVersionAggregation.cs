using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents an aggregation of an original version of a package.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal sealed class OriginalVersionAggregation: IOriginalVersionAggregation
    {
        /// <inheritdoc />
        public string OriginalVersion {get; set;}

        /// <inheritdoc />
        public DateTimeOffset? PublishDate {get; set;}

        /// <inheritdoc />
        public IList<IPackageMetadata> Packages {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="OriginalVersionAggregation" /> class.
        /// </summary>
        internal OriginalVersionAggregation()
        {
            this.Packages = new List<IPackageMetadata>();
        }
    }
}