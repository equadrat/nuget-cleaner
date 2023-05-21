using e2.Framework.Components;
using e2.Framework.Delegates;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.Components;
using e2.NuGet.Cleaner.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace e2.NuGet.Cleaner.BootstrapperModules
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

            moduleRegistry.TestFramework().RegisterBaseModule()
                          .RegisterBusinessLogicModule();
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

            var registrationSuccessor = registry.TryRegister<IConfigProvider>();
            if (registrationSuccessor == null) return;

            registry.TryRegister<ConfigProviderFake>()?.AsSingleton();
            registrationSuccessor.AsRouteTo<ConfigProviderFake>();
        }

        /// <summary>
        /// Registers the NuGet accessor factory.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <exception cref="System.ArgumentNullException">registry</exception>
        public virtual void RegisterNuGetAccessorFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            var registrationSuccessor = registry.TryRegister<INuGetAccessorFactory>();
            if (registrationSuccessor == null) return;

            registry.TryRegister<NuGetAccessorFactoryFake>()?.AsSingleton();
            registrationSuccessor.AsRouteTo<NuGetAccessorFactoryFake>();
        }

        /// <summary>
        /// Registers the host application lifetime.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <exception cref="System.ArgumentNullException">registry</exception>
        public virtual void RegisterHostApplicationLifetime(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            var registrationSuccessor = registry.TryRegister<IHostApplicationLifetime>();
            if (registrationSuccessor == null) return;

            registry.TryRegister<HostApplicationLifetimeFake>()?.AsSingleton();
            registrationSuccessor.AsRouteTo<HostApplicationLifetimeFake>();
        }
    }
}