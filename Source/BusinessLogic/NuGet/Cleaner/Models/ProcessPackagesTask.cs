using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using e2.Framework.Components;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.Components;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the task to process the packages.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal sealed class ProcessPackagesTask: IProcessPackagesTask
    {
        /// <summary>
        /// The system time provider.
        /// </summary>
        [NotNull]
        private readonly ICoreSystemTimeProvider _systemTimeProvider;

        /// <summary>
        /// The logger.
        /// </summary>
        [NotNull]
        private readonly ILogger _logger;

        /// <summary>
        /// The NuGet accessor factory.
        /// </summary>
        [NotNull]
        private readonly INuGetAccessorFactory _nuGetAccessorFactory;

        /// <summary>
        /// The configuration snapshot provider.
        /// </summary>
        [NotNull]
        private readonly IConfigSnapshotProvider _configSnapshotProvider;

        /// <summary>
        /// The package aggregator.
        /// </summary>
        [NotNull]
        private readonly IPackageAggregator _packageAggregator;

        /// <summary>
        /// The package publish date dictionary factory.
        /// </summary>
        [NotNull]
        private readonly IPackagePublishDateDictionaryFactory _packagePublishDateDictionaryFactory;

        /// <summary>
        /// The package cleanup action decision maker.
        /// </summary>
        [NotNull]
        private readonly IPackageCleanupActionDecisionMaker _packageCleanupActionDecisionMaker;

        /// <summary>
        /// The metadata set pool.
        /// </summary>
        [NotNull]
        private readonly ICoreInstancePool<HashSet<IPackageMetadata>> _metadataSetPool;

        /// <summary>
        /// The package aggregation list pool.
        /// </summary>
        [NotNull]
        private readonly ICoreInstancePool<List<IPackageAggregation>> _packageAggregationListPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessPackagesTask" /> class.
        /// </summary>
        /// <param name="systemTimeProvider">The system time provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="nuGetAccessorFactory">The NuGet accessor factory.</param>
        /// <param name="configSnapshotProvider">The configuration snapshot provider.</param>
        /// <param name="packageAggregator">The package aggregator.</param>
        /// <param name="packagePublishDateDictionaryFactory">The package publish date dictionary factory.</param>
        /// <param name="packageCleanupActionDecisionMaker">The package cleanup action decision maker.</param>
        /// <param name="metadataSetPool">The metadata set pool.</param>
        /// <param name="packageAggregationListPool">The package aggregation list pool.</param>
        /// <exception cref="System.ArgumentNullException">
        /// systemTimeProvider
        /// or
        /// logger
        /// or
        /// nuGetAccessorFactory
        /// or
        /// configSnapshotProvider
        /// or
        /// packageAggregator
        /// or
        /// packagePublishDateDictionaryFactory
        /// or
        /// packageCleanupActionDecisionMaker
        /// or
        /// metadataSetPool
        /// or
        /// packageAggregationListPool
        /// </exception>
        internal ProcessPackagesTask([NotNull] ICoreSystemTimeProvider systemTimeProvider, [NotNull] ILogger logger, [NotNull] INuGetAccessorFactory nuGetAccessorFactory, [NotNull] IConfigSnapshotProvider configSnapshotProvider, [NotNull] IPackageAggregator packageAggregator, [NotNull] IPackagePublishDateDictionaryFactory packagePublishDateDictionaryFactory, [NotNull] IPackageCleanupActionDecisionMaker packageCleanupActionDecisionMaker, [NotNull] ICoreInstancePool<HashSet<IPackageMetadata>> metadataSetPool, [NotNull] ICoreInstancePool<List<IPackageAggregation>> packageAggregationListPool)
        {
#if DEBUG
            if (systemTimeProvider == null) throw new ArgumentNullException(nameof(systemTimeProvider));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (nuGetAccessorFactory == null) throw new ArgumentNullException(nameof(nuGetAccessorFactory));
            if (configSnapshotProvider == null) throw new ArgumentNullException(nameof(configSnapshotProvider));
            if (packageAggregator == null) throw new ArgumentNullException(nameof(packageAggregator));
            if (packagePublishDateDictionaryFactory == null) throw new ArgumentNullException(nameof(packagePublishDateDictionaryFactory));
            if (packageCleanupActionDecisionMaker == null) throw new ArgumentNullException(nameof(packageCleanupActionDecisionMaker));
            if (metadataSetPool == null) throw new ArgumentNullException(nameof(metadataSetPool));
            if (packageAggregationListPool == null) throw new ArgumentNullException(nameof(packageAggregationListPool));
#endif
            this._systemTimeProvider = systemTimeProvider;
            this._logger = logger;
            this._nuGetAccessorFactory = nuGetAccessorFactory;
            this._configSnapshotProvider = configSnapshotProvider;
            this._packageAggregator = packageAggregator;
            this._packagePublishDateDictionaryFactory = packagePublishDateDictionaryFactory;
            this._packageCleanupActionDecisionMaker = packageCleanupActionDecisionMaker;
            this._metadataSetPool = metadataSetPool;
            this._packageAggregationListPool = packageAggregationListPool;
        }

        /// <inheritdoc />
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            this._logger.ProcessPackagesBegin.IfEnabled?.Log();

            try
            {
                DateTimeOffset now = this._systemTimeProvider.GetCurrentDate();

                var configSnapshot = this._configSnapshotProvider.GetConfigSnapshot();

                foreach (var packageSource in configSnapshot.PackageSources)
                {
                    using (this._nuGetAccessorFactory.GetAccessor(packageSource.Source.PackageSource, packageSource.ApiKey.ApiKey).Deconstruct(out var accessor))
                    {
                        await foreach (var unlistPackageVersion in this.GetDelistingPackages(accessor, packageSource.Owners, now, cancellationToken))
                        {
                            try
                            {
                                await accessor.UnlistPackageAsync(unlistPackageVersion.PackageId, unlistPackageVersion.OriginalVersion, cancellationToken);
                            }
                            catch (OperationCanceledException)
                            {
                                throw;
                            }
                            catch
                            {
                                // Ignored.
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ignored.
            }
            catch (Exception ex)
            {
                this._logger.ProcessPackagesFailed.IfEnabled?.Log(ex);
            }

            this._logger.ProcessPackagesEnd.IfEnabled?.Log();
        }

        /// <summary>
        /// Gets the packages for delisting.
        /// </summary>
        /// <param name="accessor">The accessor.</param>
        /// <param name="owners">The owners.</param>
        /// <param name="now">The current time stamp.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The packages for delisting.
        /// </returns>
        [Pure]
        [NotNull]
        [ItemNotNull]
        private async IAsyncEnumerable<IPackageMetadata> GetDelistingPackages(INuGetAccessor accessor, IReadOnlyList<IPackageOwnerSnapshot> owners, DateTimeOffset now, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var numberOfOwners = owners.Count;
            for (var i = 0; i < numberOfOwners; i++)
            {
                var packageSourceOwner = owners[i];

                await foreach (var packageMetadata in this.GetDelistingPackages(accessor, packageSourceOwner, now, cancellationToken))
                {
                    yield return packageMetadata;
                }
            }
        }

        /// <summary>
        /// Gets the packages for delisting.
        /// </summary>
        /// <param name="accessor">The accessor.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="now">The current time stamp.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The packages for delisting.
        /// </returns>
        [Pure]
        [NotNull]
        [ItemNotNull]
        private async IAsyncEnumerable<IPackageMetadata> GetDelistingPackages(INuGetAccessor accessor, IPackageOwnerSnapshot owner, DateTimeOffset now, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var uniquePackages = this._metadataSetPool.GetInstance();

            try
            {
                await foreach (var packageMetadata in accessor.GetPackagesAsync(owner.Owner, owner.MatchesAnyPackageIdPattern, cancellationToken))
                {
                    uniquePackages.Add(packageMetadata);
                }

                var aggregatedPackages = this._packageAggregationListPool.GetInstance();

                try
                {
                    this.AggregatePackages(owner, uniquePackages, aggregatedPackages);

                    foreach (var packageMetadata in this.GetDelistingPackages(owner, aggregatedPackages, owner.PackageGroups, now, cancellationToken))
                    {
                        yield return packageMetadata;
                    }
                }
                finally
                {
                    this._packageAggregationListPool.Recycle(aggregatedPackages);
                }
            }
            finally
            {
                this._metadataSetPool.Recycle(uniquePackages);
            }
        }

        /// <summary>
        /// Gets the packages for delisting.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="aggregatedPackages">The aggregated packages.</param>
        /// <param name="packageGroups">The package groups.</param>
        /// <param name="now">The current time stamp.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The packages for delisting.
        /// </returns>
        [Pure]
        [NotNull]
        [ItemNotNull]
        private IEnumerable<IPackageMetadata> GetDelistingPackages([NotNull] IPackageOwnerSnapshot owner, [NotNull] IReadOnlyList<IPackageAggregation> aggregatedPackages, [NotNull] IReadOnlyList<IPackageGroupSnapshot> packageGroups, DateTimeOffset now, CancellationToken cancellationToken)
        {
            // Process each aggregated package.
            var numberOfPackageAggregations = aggregatedPackages.Count;
            for (var i = 0; i < numberOfPackageAggregations; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var packageAggregation = aggregatedPackages[i];
                var packageGroupIndex = owner.GetPackageGroupIndex(packageAggregation.PackageId);
                var packageCleanup = packageGroups[packageGroupIndex].PackageCleanup;

                foreach (var packageAggregationAddress in packageAggregation.GetAddresses(includePreviewVersions: true, includeRegularVersions: true))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (this._packageCleanupActionDecisionMaker.GetUnlistVersion(packageAggregation, packageAggregationAddress, packageCleanup, now)) yield return packageAggregation.Resolve(packageAggregationAddress).Package;
                }
            }
        }

        /// <summary>
        /// Aggregates the unique packages.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="uniquePackages">The unique packages.</param>
        /// <param name="aggregatedPackages">The aggregated packages.</param>
        private void AggregatePackages([NotNull] IPackageOwnerSnapshot owner, [NotNull] IEnumerable<IPackageMetadata> uniquePackages, [NotNull] ICollection<IPackageAggregation> aggregatedPackages)
        {
            using (this._packagePublishDateDictionaryFactory.GetPublishDateDictionary(owner).Deconstruct(out var packagePublishDateDictionary))
            {
                this._packageAggregator.Aggregate(uniquePackages, packagePublishDateDictionary, aggregatedPackages);
            }
        }
    }
}