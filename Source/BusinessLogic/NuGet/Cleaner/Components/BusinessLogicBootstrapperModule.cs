using System;
using System.Collections.Generic;
using System.Diagnostics;
using e2.Framework.Attributes;
using e2.Framework.Components;
using e2.Framework.Delegates;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the business logic bootstrapper module.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    [CoreImplicitBootstrapperModuleInterface(typeof(IBusinessLogicBootstrapperModule))]
    public class BusinessLogicBootstrapperModule: CoreBootstrapperModule,
                                                  IBusinessLogicBootstrapperModule
    {
        /// <inheritdoc />
        protected override void RegisterDependencyModules(ICoreBootstrapperModuleRegistry moduleRegistry)
        {
            if (moduleRegistry == null) throw new ArgumentNullException(nameof(moduleRegistry));

            moduleRegistry.RegisterCoreFrameworkModule();

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
        public virtual void RegisterWorkerService([NotNull] ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<IWorkerService>()) registry.Register<IWorkerService>().AsSingletonOf<WorkerService>();
        }

        /// <inheritdoc />
        public virtual void RegisterProcessPackagesTaskFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<IProcessPackagesTaskFactory>()) registry.Register<IProcessPackagesTaskFactory>().AsSingletonOf<ProcessPackagesTaskFactory>();
        }

        /// <inheritdoc />
        public virtual void RegisterLogger(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<ILogger>()) registry.Register<ILogger>().AsSingletonOf<Logger>();
        }

        /// <inheritdoc />
        public virtual void RegisterConfigSnapshotFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<IConfigSnapshotFactory>()) registry.Register<IConfigSnapshotFactory>().AsSingletonOf<ConfigSnapshotFactory>();
        }

        /// <inheritdoc />
        public virtual void RegisterConfigSnapshotProvider(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<IConfigSnapshotProvider>()) registry.Register<IConfigSnapshotProvider>().AsSingletonOf<ConfigSnapshotProvider>();
        }

        /// <inheritdoc />
        public virtual void RegisterPackageAggregator(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<IPackageAggregator>()) registry.Register<IPackageAggregator>().AsSingletonOf<PackageAggregator>();
        }

        /// <inheritdoc />
        public virtual void RegisterPackagePublishDateDictionaryFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<IPackagePublishDateDictionaryFactory>()) registry.Register<IPackagePublishDateDictionaryFactory>().AsSingletonOf<PackagePublishDateDictionaryFactory>();
        }

        /// <inheritdoc />
        public virtual void RegisterPackageCleanupActionDecisionMaker(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<IPackageCleanupActionDecisionMaker>()) registry.Register<IPackageCleanupActionDecisionMaker>().AsSingletonOf<PackageCleanupActionDecisionMaker>();
        }

        /// <inheritdoc />
        public virtual void RegisterModels(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanCreateInstanceOf<IPackageMetadata>()) registry.Register<IPackageMetadata>().AsInstancePerCallOf<PackageMetadata>();
            if (!registry.CanCreateInstanceOf<IPackageAggregation>()) registry.Register<IPackageAggregation>().AsInstancePerCallOf<PackageAggregation>();
            if (!registry.CanCreateInstanceOf<IVersionAggregation>()) registry.Register<IVersionAggregation>().AsInstancePerCallOf<VersionAggregation>();
            if (!registry.CanCreateInstanceOf<IOriginalVersionAggregation>()) registry.Register<IOriginalVersionAggregation>().AsInstancePerCallOf<OriginalVersionAggregation>();
        }
    }
}