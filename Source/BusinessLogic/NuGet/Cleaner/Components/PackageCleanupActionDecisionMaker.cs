using e2.Framework.Exceptions;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a decision-maker for the cleanup action of a package.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageCleanupActionDecisionMaker: IPackageCleanupActionDecisionMaker
    {
        /// <summary>
        /// Determines whether the specified package is listed.
        /// </summary>
        /// <param name="packageMetadata">The package metadata.</param>
        /// <returns>
        /// <c>true</c> if the specified package is listed; otherwise, <c>false</c>.
        /// </returns>
        [Pure]
        private static bool IsListed([NotNull] IPackageMetadata packageMetadata)
        {
            return packageMetadata.IsListed;
        }

        /// <summary>
        /// Determines whether the specified package is deprecated or has expired.
        /// </summary>
        /// <param name="originalVersionAggregation">The original version aggregation.</param>
        /// <param name="packageMetadata">The package metadata.</param>
        /// <param name="cleanupConfig">The cleanup configuration.</param>
        /// <param name="now">The now.</param>
        /// <returns>
        /// <c>true</c> if the specified package is deprecated or has expired; otherwise, <c>false</c>.
        /// </returns>
        [Pure]
        private static bool IsDeprecatedOrHasExpired([NotNull] IOriginalVersionAggregation originalVersionAggregation, [NotNull] IPackageMetadata packageMetadata, [NotNull] IPackageCleanupConfig cleanupConfig, DateTimeOffset now)
        {
            if (packageMetadata.IsDeprecated) return true;

            var publishDate = originalVersionAggregation.PublishDate;
            if (!publishDate.HasValue) return false;

            var expiryDate = publishDate.Value + cleanupConfig.Expiry;
            return expiryDate <= now;
        }

        /// <summary>
        /// Gets the number of newer regular packages.
        /// </summary>
        /// <param name="packageAggregation">The package aggregation.</param>
        /// <param name="packageAggregationAddress">The package aggregation address.</param>
        /// <returns>
        /// The number of newer regular packages.
        /// </returns>
        /// <exception cref="e2.Framework.Exceptions.CoreArgumentOutOfRangeException">packageAggregationAddress</exception>
        [Pure]
        private static int GetNumberOfNewerRegularPackages([NotNull] IPackageAggregation packageAggregation, PackageAggregationAddress packageAggregationAddress)
        {
#if DEBUG
            if (packageAggregationAddress.IsPreviewVersion) throw new CoreArgumentOutOfRangeException(nameof(packageAggregationAddress), packageAggregationAddress);
#endif
            var numberOfNewerRegularVersions = 0;

            var packageVersionIndex = packageAggregationAddress.VersionIndex;
            var regularVersionIndex = packageAggregationAddress.OriginalVersionIndex;
            var packageIndex = packageAggregationAddress.PackageIndex;

            var numberOfPackageVersions = packageAggregation.Versions.Count;
            for (var i = packageVersionIndex; i < numberOfPackageVersions; i++)
            {
                var packageVersion = packageAggregation.Versions[i];

                if (i == packageVersionIndex)
                {
                    numberOfNewerRegularVersions += packageVersion.RegularVersions.Skip(regularVersionIndex).SelectMany(x => x.Packages.Skip(packageIndex + 1)).Count(IsListed);
                }
                else
                {
                    numberOfNewerRegularVersions += packageVersion.RegularVersions.SelectMany(x => x.Packages).Count(IsListed);
                }
            }

            return numberOfNewerRegularVersions;
        }

        /// <summary>
        /// Gets the preview state.
        /// </summary>
        /// <param name="packageAggregation">The package aggregation.</param>
        /// <param name="packageAggregationAddress">The package aggregation address.</param>
        /// <returns>
        /// The preview state.
        /// </returns>
        /// <exception cref="e2.Framework.Exceptions.CoreArgumentOutOfRangeException">packageAggregationAddress</exception>
        [Pure]
        private static (int NewerPreviewPackages, int NewerRegularPackages) GetPreviewState(IPackageAggregation packageAggregation, PackageAggregationAddress packageAggregationAddress)
        {
#if DEBUG
            if (!packageAggregationAddress.IsPreviewVersion) throw new CoreArgumentOutOfRangeException(nameof(packageAggregationAddress), packageAggregationAddress);
#endif
            var newerPreviewPackages = 0;
            var newerRegularPackages = 0;

            var packageVersionIndex = packageAggregationAddress.VersionIndex;
            var previewVersionIndex = packageAggregationAddress.OriginalVersionIndex;
            var packageIndex = packageAggregationAddress.PackageIndex;

            var numberOfPackageVersions = packageAggregation.Versions.Count;
            for (var i = packageVersionIndex; i < numberOfPackageVersions; i++)
            {
                var packageVersion = packageAggregation.Versions[i];

                if (i == packageVersionIndex)
                {
                    newerPreviewPackages += packageVersion.PreviewVersions.Skip(previewVersionIndex).SelectMany(x => x.Packages.Skip(packageIndex + 1)).Count(IsListed);
                }
                else
                {
                    newerPreviewPackages += packageVersion.PreviewVersions.SelectMany(x => x.Packages).Count(IsListed);
                }

                newerRegularPackages += packageVersion.RegularVersions.SelectMany(x => x.Packages).Count(IsListed);
            }

            return (newerPreviewPackages, newerRegularPackages);
        }

        /// <inheritdoc />
        public bool GetUnlistVersion(IPackageAggregation packageAggregation, PackageAggregationAddress packageAggregationAddress, IPackageCleanupConfig cleanupConfig, DateTimeOffset now)
        {
            if (packageAggregation == null) throw new ArgumentNullException(nameof(packageAggregation));
            if (cleanupConfig == null) throw new ArgumentNullException(nameof(cleanupConfig));

            var (_, originalVersion, package) = packageAggregation.Resolve(packageAggregationAddress);

            bool result;

            if (packageAggregationAddress.IsPreviewVersion)
            {
                // Unlist preview version if:
                // - package is ((deprecated) or (has expired)) and there is a newer listed (preview) of the same version available.
                // - there is a listed preview of a new version available.
                // - there is a newer listed regular version available.
                if (!IsListed(package)) return false;

                var (newerPreviewPackages, newerRegularPackages) = GetPreviewState(packageAggregation, packageAggregationAddress);

                result = (IsDeprecatedOrHasExpired(originalVersion, package, cleanupConfig, now) && ((newerRegularPackages != 0) || (newerPreviewPackages >= cleanupConfig.RetainVersions)))
                         || (!cleanupConfig.RetainPreviewsOfRegularReleases && (newerRegularPackages != 0));
            }
            else
            {
                // Unlist regular version if:
                // - package is ((deprecated) or (has expired)) and there is a newer listed version available.

                if (!IsListed(package)) return false;
                if (!IsDeprecatedOrHasExpired(originalVersion, package, cleanupConfig, now)) return false;

                var numberOfNewerRegularVersions = GetNumberOfNewerRegularPackages(packageAggregation, packageAggregationAddress);
                result = numberOfNewerRegularVersions >= cleanupConfig.RetainVersions;
            }

            return result;
        }
    }
}