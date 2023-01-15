using System;
using System.Diagnostics;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes a provider for the current configuration snapshot.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IConfigSnapshotProvider
    {
        /// <summary>
        /// Gets the configuration snapshot.
        /// </summary>
        /// <returns>
        /// The configuration snapshot.
        /// </returns>
        [Pure]
        [NotNull]
        IConfigSnapshot GetConfigSnapshot();
    }
}