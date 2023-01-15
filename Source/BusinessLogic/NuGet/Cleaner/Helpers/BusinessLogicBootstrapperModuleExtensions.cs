using System;
using System.Diagnostics;
using e2.Framework.Components;
using e2.NuGet.Cleaner.Components;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Helpers
{
    /// <summary>
    /// This class provides helper methods.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public static class BusinessLogicBootstrapperModuleExtensions
    {
        /// <summary>
        /// Registers the business logic bootstrapper module.
        /// </summary>
        /// <param name="moduleRegistry">The module registry.</param>
        /// <returns>
        /// The module registry.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">moduleRegistry</exception>
        public static ICoreBootstrapperModuleRegistry RegisterBusinessLogicModule([NotNull] this ICoreBootstrapperModuleRegistry moduleRegistry)
        {
            if (moduleRegistry == null) throw new ArgumentNullException(nameof(moduleRegistry));

            return moduleRegistry.RegisterModule<BusinessLogicBootstrapperModule, IBusinessLogicBootstrapperModule>();
        }
    }
}