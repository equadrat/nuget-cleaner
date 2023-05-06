using e2.Framework.Components;
using e2.Framework.Delegates;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the bootstrapper for unit tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public class UnitTestBootstrapperModule: CoreBootstrapperModule
    {
        /// <inheritdoc />
        protected override void RegisterDependencyModules(ICoreBootstrapperModuleRegistry moduleRegistry)
        {
            if (moduleRegistry == null) throw new ArgumentNullException(nameof(moduleRegistry));

            moduleRegistry.RegisterBusinessLogicModule()
                          .TestFramework().RegisterBaseModule();
        }

        /// <inheritdoc />
        protected override IEnumerable<procBootstrapperRegister> GetRegisterMethods()
        {
            yield return this.RegisterConfigProvider;
            yield return this.RegisterNuGetAccessorFactory;
            yield return this.RegisterHostApplicationLifetime;
        }

        /// <inheritdoc />
        protected override IEnumerable<procBootstrapperInitialize> GetInitializeMethods()
        {
            yield break;
        }

        /// <summary>
        /// Registers the configuration provider.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public virtual void RegisterConfigProvider(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (registry.CanGetInstanceOf<IConfigProvider>()) return;

            if (!registry.CanGetInstanceOf<ConfigProviderFake>()) registry.Register<ConfigProviderFake>().AsSingleton();
            registry.Register<IConfigProvider>().AsRouteTo<ConfigProviderFake>();
        }

        /// <summary>
        /// Registers the NuGet accessor factory.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <exception cref="System.ArgumentNullException">registry</exception>
        public virtual void RegisterNuGetAccessorFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (registry.CanGetInstanceOf<INuGetAccessorFactory>()) return;

            if (!registry.CanGetInstanceOf<NuGetAccessorFactoryFake>()) registry.Register<NuGetAccessorFactoryFake>().AsSingleton();
            registry.Register<INuGetAccessorFactory>().AsRouteTo<NuGetAccessorFactoryFake>();
        }

        /// <summary>
        /// Registers the host application lifetime.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <exception cref="System.ArgumentNullException">registry</exception>
        public virtual void RegisterHostApplicationLifetime(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (registry.CanGetInstanceOf<IHostApplicationLifetime>()) return;

            if (!registry.CanGetInstanceOf<HostApplicationLifetimeFake>()) registry.Register<HostApplicationLifetimeFake>().AsSingleton();
            registry.Register<IHostApplicationLifetime>().AsRouteTo<HostApplicationLifetimeFake>();
        }
    }
}