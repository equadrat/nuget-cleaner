using System;
using System.Diagnostics;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes a factory to create tasks to process the packages.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IProcessPackagesTaskFactory
    {
        /// <summary>
        /// Creates a task to process the packages.
        /// </summary>
        /// <returns>
        /// The token to release the task.
        /// </returns>
        [Pure]
        [NotNull]
        ICoreOwnerToken<IProcessPackagesTask> GetTask();
    }
}