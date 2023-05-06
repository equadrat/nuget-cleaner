using e2.Framework;
using e2.Framework.Components;
using e2.NuGet.Cleaner.Components;
using System;
using System.Diagnostics.CodeAnalysis;

namespace e2.NuGet.Cleaner
{
    /// <summary>
    /// This class represents a template for unit tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public abstract class UnitTestTemplate: MSTestTemplate
    {
        /// <inheritdoc />
        protected override void RegisterDependencyModules(ICoreBootstrapperModuleRegistry moduleRegistry)
        {
            if (moduleRegistry == null) throw new ArgumentNullException(nameof(moduleRegistry));

            moduleRegistry.RegisterModule<UnitTestBootstrapperModule>();
        }
    }
}