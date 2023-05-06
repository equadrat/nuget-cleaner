using e2.Framework.Components;
using System;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes the business logic bootstrapper module.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IBusinessLogicBootstrapperModule
    {
        /// <summary>
        /// Registers the worker service.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterWorkerService(ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the process packages task factory.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterProcessPackagesTaskFactory(ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the logger.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterLogger(ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the configuration snapshot factory.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterConfigSnapshotFactory(ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the configuration snapshot provider.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterConfigSnapshotProvider(ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the package aggregator.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterPackageAggregator(ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the package publish date dictionary factory.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterPackagePublishDateDictionaryFactory(ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the package cleanup action decision maker.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterPackageCleanupActionDecisionMaker(ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the models.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterModels(ICoreIOCRegistry registry);
    }
}