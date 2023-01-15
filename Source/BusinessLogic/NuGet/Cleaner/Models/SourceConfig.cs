using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the configuration of a source.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class SourceConfig: ISourceConfig
    {
        /// <inheritdoc />
        public string SourceId {get; set;}

        /// <inheritdoc />
        public string PackageSource {get; set;}
    }
}