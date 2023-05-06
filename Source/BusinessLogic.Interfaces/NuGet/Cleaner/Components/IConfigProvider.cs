using e2.Framework.Enums;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes the config provider.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IConfigProvider
    {
        /// <summary>
        /// Occurs when the config has changed.
        /// </summary>
        event EventHandler Changed;

        /// <summary>
        /// Gets the worker process interval.
        /// </summary>
        /// <value>
        /// The worker process interval.
        /// </value>
        TimeSpan? WorkerProcessInterval {get;}

        /// <summary>
        /// Gets the task scheduler operator mode.
        /// </summary>
        /// <value>
        /// The task scheduler operator mode.
        /// </value>
        eCoreTaskSchedulerOperatorMode? TaskSchedulerOperatorMode {get;}

        /// <summary>
        /// Gets the sources.
        /// </summary>
        /// <value>
        /// The sources.
        /// </value>
        IReadOnlyList<ISourceConfig> Sources {get;}

        /// <summary>
        /// Gets the API keys.
        /// </summary>
        /// <value>
        /// The API keys.
        /// </value>
        IReadOnlyList<IApiKeyConfig> ApiKeys {get;}

        /// <summary>
        /// Gets the package cleanups.
        /// </summary>
        /// <value>
        /// The package cleanups.
        /// </value>
        IReadOnlyList<IPackageCleanupConfig> PackageCleanups {get;}

        /// <summary>
        /// Gets the package groups.
        /// </summary>
        /// <value>
        /// The package groups.
        /// </value>
        IReadOnlyList<IPackageGroupConfig> PackageGroups {get;}

        /// <summary>
        /// Enables this instance.
        /// </summary>
        /// <returns>
        /// The token to disable this instance.
        /// </returns>
        [Pure]
        ICoreOwnerToken Enable();
    }
}