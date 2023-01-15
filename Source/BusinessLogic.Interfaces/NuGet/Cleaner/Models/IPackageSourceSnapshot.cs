using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes a snapshot of a package source.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageSourceSnapshot
    {
        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        [NotNull]
        ISourceConfig Source {get;}

        /// <summary>
        /// Gets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        [NotNull]
        IApiKeyConfig ApiKey {get;}

        /// <summary>
        /// Gets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        [NotNull]
        IReadOnlyList<IPackageOwnerSnapshot> Owners {get;}
    }
}