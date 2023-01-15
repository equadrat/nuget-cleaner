using System;
using System.Collections.Generic;
using System.Diagnostics;
using e2.Framework.Components;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a factory to create tasks to process the packages.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ProcessPackagesTaskFactory: IProcessPackagesTaskFactory
    {
        /// <summary>
        /// The token factory.
        /// </summary>
        [NotNull]
        private readonly ICoreTokenFactory _tokenFactory;

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
        /// The process packages task pool.
        /// </summary>
        [NotNull]
        private readonly ICoreInstancePool<ProcessPackagesTask> _processPackagesTaskPool;

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
        /// Initializes a new instance of the <see cref="ProcessPackagesTaskFactory" /> class.
        /// </summary>
        /// <param name="tokenFactory">The token factory.</param>
        /// <param name="systemTimeProvider">The system time provider.</param>
        /// <param name="instancePoolFactory">The instance pool factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="nuGetAccessorFactory">The nu get accessor factory.</param>
        /// <param name="configSnapshotProvider">The configuration snapshot provider.</param>
        /// <param name="packageAggregator">The package aggregator.</param>
        /// <param name="packagePublishDateDictionaryFactory">The package publish date dictionary factory.</param>
        /// <param name="packageCleanupActionDecisionMaker">The package cleanup action decision maker.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tokenFactory
        /// or
        /// systemTimeProvider
        /// or
        /// instancePoolFactory
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
        /// </exception>
        public ProcessPackagesTaskFactory([NotNull] ICoreTokenFactory tokenFactory, [NotNull] ICoreSystemTimeProvider systemTimeProvider, [NotNull] ICoreInstancePoolFactory instancePoolFactory, [NotNull] ILogger logger, [NotNull] INuGetAccessorFactory nuGetAccessorFactory, [NotNull] IConfigSnapshotProvider configSnapshotProvider, [NotNull] IPackageAggregator packageAggregator, [NotNull] IPackagePublishDateDictionaryFactory packagePublishDateDictionaryFactory, [NotNull] IPackageCleanupActionDecisionMaker packageCleanupActionDecisionMaker)
        {
            if (tokenFactory == null) throw new ArgumentNullException(nameof(tokenFactory));
            if (systemTimeProvider == null) throw new ArgumentNullException(nameof(systemTimeProvider));
            if (instancePoolFactory == null) throw new ArgumentNullException(nameof(instancePoolFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (nuGetAccessorFactory == null) throw new ArgumentNullException(nameof(nuGetAccessorFactory));
            if (configSnapshotProvider == null) throw new ArgumentNullException(nameof(configSnapshotProvider));
            if (packageAggregator == null) throw new ArgumentNullException(nameof(packageAggregator));
            if (packagePublishDateDictionaryFactory == null) throw new ArgumentNullException(nameof(packagePublishDateDictionaryFactory));
            if (packageCleanupActionDecisionMaker == null) throw new ArgumentNullException(nameof(packageCleanupActionDecisionMaker));

            this._tokenFactory = tokenFactory;
            this._systemTimeProvider = systemTimeProvider;
            this._logger = logger;
            this._nuGetAccessorFactory = nuGetAccessorFactory;
            this._configSnapshotProvider = configSnapshotProvider;
            this._packageAggregator = packageAggregator;
            this._packagePublishDateDictionaryFactory = packagePublishDateDictionaryFactory;
            this._packageCleanupActionDecisionMaker = packageCleanupActionDecisionMaker;

            this._processPackagesTaskPool = instancePoolFactory.CreateInstancePool(this.CreateProcessPackagesTask);
            this._metadataSetPool = instancePoolFactory.CreateInstancePool(() => new HashSet<IPackageMetadata>(PackageMetadataComparer.Default), cleanup: x => x.Clear());
            this._packageAggregationListPool = instancePoolFactory.CreateInstancePool<List<IPackageAggregation>>(cleanup: x => x.Clear());
        }

        /// <inheritdoc />
        public ICoreOwnerToken<IProcessPackagesTask> GetTask()
        {
            var task = this._processPackagesTaskPool.GetInstance();
            return this._tokenFactory.CreateRelayToken<IProcessPackagesTask, ProcessPackagesTask>(task, this.ReleaseTask);
        }

        /// <summary>
        /// Releases the task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <exception cref="System.ArgumentNullException">task</exception>
        private void ReleaseTask([NotNull] ProcessPackagesTask task)
        {
            this._processPackagesTaskPool.Recycle(task);
        }

        /// <summary>
        /// Creates a process packages task.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [Pure]
        [NotNull]
        private ProcessPackagesTask CreateProcessPackagesTask()
        {
            return new ProcessPackagesTask(this._systemTimeProvider, this._logger, this._nuGetAccessorFactory, this._configSnapshotProvider, this._packageAggregator, this._packagePublishDateDictionaryFactory, this._packageCleanupActionDecisionMaker, this._metadataSetPool, this._packageAggregationListPool);
        }
    }
}