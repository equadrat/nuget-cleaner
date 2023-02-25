using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes the task to process the packages.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IProcessPackagesTask
    {
        /// <summary>
        /// Runs the task.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The state of the asynchronous process.</returns>
        [NotNull]
        Task RunAsync(CancellationToken cancellationToken);
    }
}