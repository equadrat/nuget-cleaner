using System;
using System.Diagnostics;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes a factory to create snapshots of the configuration.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IConfigSnapshotFactory
    {
        /// <summary>
        /// Creates a snapshot.
        /// </summary>
        /// <param name="configProvider">The configuration provider.</param>
        /// <returns>
        /// The snapshot.
        /// </returns>
        [Pure]
        [NotNull]
        IConfigSnapshot CreateSnapshot([NotNull] IConfigProvider configProvider);
    }
}