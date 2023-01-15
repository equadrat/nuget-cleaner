using System;
using System.Collections.Generic;
using System.Diagnostics;
using e2.Framework;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents an aggregation of the versions of a package.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal sealed class PackageAggregation: IPackageAggregation
    {
        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="versionIndex">The version index.</param>
        /// <param name="isPreviewVersion"><c>true</c> if the address represents a preview version.</param>
        /// <param name="originalVersions">The original versions.</param>
        /// <returns>
        /// The addresses.
        /// </returns>
        [Pure]
        [NotNull]
        private static IEnumerable<PackageAggregationAddress> GetAddresses(int versionIndex, bool isPreviewVersion, [NotNull] IList<IOriginalVersionAggregation> originalVersions)
        {
            var numberOfOriginalVersions = originalVersions.Count;
            for (var originalVersionIndex = 0; originalVersionIndex < numberOfOriginalVersions; originalVersionIndex++)
            {
                var originalVersion = originalVersions[originalVersionIndex];
                var packages = originalVersion.Packages;
                var numberOfPackages = packages.Count;
                for (var packageIndex = 0; packageIndex < numberOfPackages; packageIndex++)
                {
                    yield return new PackageAggregationAddress(versionIndex, isPreviewVersion, originalVersionIndex, packageIndex);
                }
            }
        }

        /// <inheritdoc />
        public string PackageId {get; set;}

        /// <inheritdoc />
        public IList<IVersionAggregation> Versions {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageAggregation" /> class.
        /// </summary>
        public PackageAggregation()
        {
            this.Versions = new List<IVersionAggregation>();
        }

        /// <inheritdoc />
        public IEnumerable<PackageAggregationAddress> GetAddresses(NCObject _ = null, bool includePreviewVersions = false, bool includeRegularVersions = false)
        {
            if (!includeRegularVersions && !includePreviewVersions) yield break;

            var numberOfVersions = this.Versions.Count;
            for (var versionIndex = 0; versionIndex < numberOfVersions; versionIndex++)
            {
                var version = this.Versions[versionIndex];

                if (includePreviewVersions)
                {
                    const bool isPreviewVersion = true;
                    foreach (var address in GetAddresses(versionIndex, isPreviewVersion, version.PreviewVersions))
                    {
                        yield return address;
                    }
                }

                if (includeRegularVersions)
                {
                    const bool isPreviewVersion = false;
                    foreach (var address in GetAddresses(versionIndex, isPreviewVersion, version.RegularVersions))
                    {
                        yield return address;
                    }
                }
            }
        }

        /// <inheritdoc />
        public (IVersionAggregation Version, IOriginalVersionAggregation OriginalVersion, IPackageMetadata Package) Resolve(PackageAggregationAddress address)
        {
            var version = this.Versions[address.VersionIndex];
            var originalVersions = address.IsPreviewVersion ? version.PreviewVersions : version.RegularVersions;
            var originalVersion = originalVersions[address.OriginalVersionIndex];
            var package = originalVersion.Packages[address.PackageIndex];
            return (version, originalVersion, package);
        }
    }
}