using JetBrains.Annotations;
using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the address of an aggregated package.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public readonly struct PackageAggregationAddress
    {
        /// <summary>
        /// Gets the version index.
        /// </summary>
        /// <value>
        /// The version index.
        /// </value>
        public int VersionIndex {get;}

        /// <summary>
        /// Gets a value indicating whether this instance is a preview version.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a preview version; otherwise, <c>false</c>.
        /// </value>
        public bool IsPreviewVersion {get;}

        /// <summary>
        /// Gets the original version index.
        /// </summary>
        /// <value>
        /// The original version index.
        /// </value>
        public int OriginalVersionIndex {get;}

        /// <summary>
        /// Gets the package index.
        /// </summary>
        /// <value>
        /// The package index.
        /// </value>
        public int PackageIndex {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageAggregationAddress" /> struct.
        /// </summary>
        /// <param name="versionIndex">The version index.</param>
        /// <param name="isPreviewVersion">if set to <c>true</c> [is preview version].</param>
        /// <param name="originalVersionIndex">The original version index.</param>
        /// <param name="packageIndex">The package index.</param>
        public PackageAggregationAddress(int versionIndex, bool isPreviewVersion, int originalVersionIndex, int packageIndex)
        {
            this.VersionIndex = versionIndex;
            this.IsPreviewVersion = isPreviewVersion;
            this.OriginalVersionIndex = originalVersionIndex;
            this.PackageIndex = packageIndex;
        }
    }
}