using System;
using System.Diagnostics;
using e2.Framework.Components;
using e2.Framework.MemberTemplates;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes the bootstrapper module of the worker.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IWorkerBootstrapperModule
    {
        /// <summary>
        /// Registers the configuration provider.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterConfigProvider([NotNull] ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the NuGet logger.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterNuGetLogger([NotNull] ICoreIOCRegistry registry);

        /// <summary>
        /// Registers the NuGet accessor factory.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void RegisterNuGetAccessorFactory([NotNull] ICoreIOCRegistry registry);

        /// <summary>
        /// Initializes the logging targets.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="lifetimeObjects">The lifetime objects.</param>
        void InitLoggingTargets([NotNull] ICoreIOCFactory factory, [NotNull] ICorePushOne<IDisposable> lifetimeObjects);
    }
}