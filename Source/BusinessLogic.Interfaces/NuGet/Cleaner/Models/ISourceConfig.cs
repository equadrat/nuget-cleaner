using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes the configuration of a source.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface ISourceConfig
    {
        /// <summary>
        /// Gets the source identifier.
        /// </summary>
        /// <value>
        /// The source identifier.
        /// </value>
        string SourceId {get;}

        /// <summary>
        /// Gets the package source.
        /// </summary>
        /// <value>
        /// The package source.
        /// </value>
        string PackageSource {get;}
    }
}