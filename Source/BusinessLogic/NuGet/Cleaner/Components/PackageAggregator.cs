using System;
using System.Collections.Generic;
using System.Diagnostics;
using e2.Framework.Components;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents an aggregator for packages.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageAggregator: IPackageAggregator
    {
        /// <summary>
        /// Creates a level0 dictionary.
        /// </summary>
        /// <returns>
        /// The level0 dictionary.
        /// </returns>
        [Pure]
        [NotNull]
        private static Dictionary<string, IPackageAggregation> CreateLevel0Dictionary()
        {
            return new Dictionary<string, IPackageAggregation>(PackageMetadataComparer.DefaultStringComparer);
        }

        /// <summary>
        /// Cleanups the level0 dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        private static void CleanupLevel0Dictionary([NotNull] Dictionary<string, IPackageAggregation> dictionary)
        {
            dictionary.Clear();
        }

        /// <summary>
        /// The collection helper.
        /// </summary>
        [NotNull]
        private readonly ICoreCollectionHelper _collectionHelper;

        /// <summary>
        /// The package aggregation instance factory.
        /// </summary>
        [NotNull]
        private readonly ICoreIOCInstanceFactory<IPackageAggregation> _packageAggregationInstanceFactory;

        /// <summary>
        /// The version aggregation instance factory.
        /// </summary>
        [NotNull]
        private readonly ICoreIOCInstanceFactory<IVersionAggregation> _versionAggregationInstanceFactory;

        /// <summary>
        /// The original version aggregation instance factory.
        /// </summary>
        [NotNull]
        private readonly ICoreIOCInstanceFactory<IOriginalVersionAggregation> _originalVersionAggregationInstanceFactory;

        /// <summary>
        /// The level0 pool.
        /// </summary>
        [NotNull]
        private readonly ICoreInstancePool<Dictionary<string, IPackageAggregation>> _level0Pool;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageAggregator" /> class.
        /// </summary>
        /// <param name="collectionHelper">The collection helper.</param>
        /// <param name="instancePoolFactory">The instance pool factory.</param>
        /// <param name="packageAggregationInstanceFactory">The package aggregation instance factory.</param>
        /// <param name="versionAggregationInstanceFactory">The version aggregation instance factory.</param>
        /// <param name="originalVersionAggregationInstanceFactory">The original version aggregation instance factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// collectionHelper
        /// or
        /// instancePoolFactory
        /// or
        /// packageAggregationInstanceFactory
        /// or
        /// versionAggregationInstanceFactory
        /// or
        /// originalVersionAggregationInstanceFactory
        /// </exception>
        public PackageAggregator([NotNull] ICoreCollectionHelper collectionHelper, [NotNull] ICoreInstancePoolFactory instancePoolFactory, [NotNull] ICoreIOCInstanceFactory<IPackageAggregation> packageAggregationInstanceFactory, [NotNull] ICoreIOCInstanceFactory<IVersionAggregation> versionAggregationInstanceFactory, [NotNull] ICoreIOCInstanceFactory<IOriginalVersionAggregation> originalVersionAggregationInstanceFactory)
        {
            if (collectionHelper == null) throw new ArgumentNullException(nameof(collectionHelper));
            if (instancePoolFactory == null) throw new ArgumentNullException(nameof(instancePoolFactory));
            if (packageAggregationInstanceFactory == null) throw new ArgumentNullException(nameof(packageAggregationInstanceFactory));
            if (versionAggregationInstanceFactory == null) throw new ArgumentNullException(nameof(versionAggregationInstanceFactory));
            if (originalVersionAggregationInstanceFactory == null) throw new ArgumentNullException(nameof(originalVersionAggregationInstanceFactory));

            this._collectionHelper = collectionHelper;
            this._packageAggregationInstanceFactory = packageAggregationInstanceFactory;
            this._versionAggregationInstanceFactory = versionAggregationInstanceFactory;
            this._originalVersionAggregationInstanceFactory = originalVersionAggregationInstanceFactory;

            this._level0Pool = instancePoolFactory.CreateInstancePool(CreateLevel0Dictionary, cleanup: CleanupLevel0Dictionary);
        }

        /// <inheritdoc />
        public void Aggregate(IEnumerable<IPackageMetadata> packageMetadata, IPackagePublishDateDictionary packagePublishDateDictionary, ICollection<IPackageAggregation> aggregatedPackages)
        {
            if (packageMetadata == null) throw new ArgumentNullException(nameof(packageMetadata));
            if (packagePublishDateDictionary == null) throw new ArgumentNullException(nameof(packagePublishDateDictionary));
            if (aggregatedPackages == null) throw new ArgumentNullException(nameof(aggregatedPackages));

            var packageAggregationByPackageId = this._level0Pool.GetInstance();

            try
            {
                foreach (var metadata in packageMetadata)
                {
                    if ((metadata.Version == null) || (metadata.OriginalVersion == null)) continue;

                    // Get the package aggregation.
                    IPackageAggregation packageAggregation;
                    if (!packageAggregationByPackageId.TryGetValue(metadata.PackageId, out var packageAggregationEntry))
                    {
                        packageAggregation = this._packageAggregationInstanceFactory.CreateInstance();
                        packageAggregation.PackageId = metadata.PackageId;

                        packageAggregationByPackageId.Add(metadata.PackageId, packageAggregation);
                        aggregatedPackages.Add(packageAggregation);
                    }
                    else
                    {
                        packageAggregation = packageAggregationEntry;
                    }

                    // Get or add the version.
                    var versions = packageAggregation.Versions;

                    IVersionAggregation versionAggregation;
                    var versionIndex = this._collectionHelper.BinarySearch(versions, metadata.Version, x => x.Version, PackageMetadataComparer.VersionComparer);
                    if (versionIndex < 0)
                    {
                        versionIndex = ~versionIndex;

                        versionAggregation = this._versionAggregationInstanceFactory.CreateInstance();
                        versionAggregation.Version = metadata.Version;

                        versions.Insert(versionIndex, versionAggregation);
                    }
                    else
                    {
                        versionAggregation = versions[versionIndex];
                    }

                    // Get or add the original version.
                    var originalVersions = metadata.IsPrerelease
                        ? versionAggregation.PreviewVersions
                        : versionAggregation.RegularVersions;

                    IOriginalVersionAggregation originalVersionAggregation;
                    var originalVersionIndex = this._collectionHelper.BinarySearch(originalVersions, metadata.OriginalVersion, x => x.OriginalVersion, PackageMetadataComparer.DefaultStringComparer);
                    if (originalVersionIndex < 0)
                    {
                        originalVersionIndex = ~originalVersionIndex;

                        originalVersionAggregation = this._originalVersionAggregationInstanceFactory.CreateInstance();
                        originalVersionAggregation.OriginalVersion = metadata.OriginalVersion;

                        originalVersions.Insert(originalVersionIndex, originalVersionAggregation);
                    }
                    else
                    {
                        originalVersionAggregation = originalVersions[originalVersionIndex];
                    }

                    // Update the publish date.
                    var publishDate = packagePublishDateDictionary.GetPublishDate(metadata.PackageId, metadata.Version, metadata.OriginalVersion, metadata.PublishDate);
                    originalVersionAggregation.PublishDate = publishDate;

                    // Add the package.
                    var packages = originalVersionAggregation.Packages;
                    var packageIndex = this._collectionHelper.BinarySearch(packages, metadata, PackageMetadataComparer.Default);
                    if (packageIndex >= 0) continue;
                    packageIndex = ~packageIndex;

                    packages.Insert(packageIndex, metadata);
                }
            }
            finally
            {
                this._level0Pool.Recycle(packageAggregationByPackageId);
            }
        }
    }
}