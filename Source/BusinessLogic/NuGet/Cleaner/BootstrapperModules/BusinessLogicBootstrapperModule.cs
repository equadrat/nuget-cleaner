using e2.Framework.Attributes;
using e2.Framework.Components;
using e2.Framework.Delegates;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.Components;
using e2.NuGet.Cleaner.Models;
using System;
using System.Collections.Generic;

namespace e2.NuGet.Cleaner.BootstrapperModules
{
    /// <summary>
    /// This class represents the business logic bootstrapper module.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    [CoreImplicitBootstrapperModuleInterface(typeof(IBusinessLogicBootstrapperModule))]
    public class BusinessLogicBootstrapperModule: CoreBootstrapperModule,
                                                  IBusinessLogicBootstrapperModule
    {
        /// <inheritdoc />
        protected override void RegisterDependencyModules(ICoreBootstrapperModuleRegistry moduleRegistry)
        {
            if (moduleRegistry == null) throw new ArgumentNullException(nameof(moduleRegistry));

            moduleRegistry.CoreFramework().RegisterBaseModule();

            base.RegisterDependencyModules(moduleRegistry);
        }

        /// <inheritdoc />
        protected override IEnumerable<procBootstrapperRegister> GetRegisterMethods()
        {
            yield return this.RegisterWorkerService;
            yield return this.RegisterProcessPackagesTaskFactory;
            yield return this.RegisterLogger;
            yield return this.RegisterConfigSnapshotFactory;
            yield return this.RegisterConfigSnapshotProvider;
            yield return this.RegisterPackageAggregator;
            yield return this.RegisterPackagePublishDateDictionaryFactory;
            yield return this.RegisterPackageCleanupActionDecisionMaker;
            yield return this.RegisterModels;
        }

        /// <inheritdoc />
        protected override IEnumerable<procBootstrapperInitialize> GetInitializeMethods()
        {
            yield break;
        }

        /// <inheritdoc />
        public virtual void RegisterWorkerService(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<IWorkerService>()?.AsSingletonOf<WorkerService>();
        }

        /// <inheritdoc />
        public virtual void RegisterProcessPackagesTaskFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<IProcessPackagesTaskFactory>()?.AsSingletonOf<ProcessPackagesTaskFactory>();
        }

        /// <inheritdoc />
        public virtual void RegisterLogger(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<ILogger>()?.AsSingletonOf<Logger>();
        }

        /// <inheritdoc />
        public virtual void RegisterConfigSnapshotFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<IConfigSnapshotFactory>()?.AsSingletonOf<ConfigSnapshotFactory>();
        }

        /// <inheritdoc />
        public virtual void RegisterConfigSnapshotProvider(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<IConfigSnapshotProvider>()?.AsSingletonOf<ConfigSnapshotProvider>();
        }

        /// <inheritdoc />
        public virtual void RegisterPackageAggregator(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<IPackageAggregator>()?.AsSingletonOf<PackageAggregator>();
        }

        /// <inheritdoc />
        public virtual void RegisterPackagePublishDateDictionaryFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<IPackagePublishDateDictionaryFactory>()?.AsSingletonOf<PackagePublishDateDictionaryFactory>();
        }

        /// <inheritdoc />
        public virtual void RegisterPackageCleanupActionDecisionMaker(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<IPackageCleanupActionDecisionMaker>()?.AsSingletonOf<PackageCleanupActionDecisionMaker>();
        }

        /// <inheritdoc />
        public virtual void RegisterModels(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.TryRegister<IPackageMetadata>()?.AsInstancePerCallOf<PackageMetadata>();
            registry.TryRegister<IPackageAggregation>()?.AsInstancePerCallOf<PackageAggregation>();
            registry.TryRegister<IVersionAggregation>()?.AsInstancePerCallOf<VersionAggregation>();
            registry.TryRegister<IOriginalVersionAggregation>()?.AsInstancePerCallOf<OriginalVersionAggregation>();
        }
    }
}