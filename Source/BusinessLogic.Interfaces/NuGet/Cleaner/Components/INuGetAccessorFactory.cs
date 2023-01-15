using System;
using System.Diagnostics;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes a factory to create instances of <see cref="INuGetAccessor" />.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface INuGetAccessorFactory
    {
        /// <summary>
        /// Gets the accessor.
        /// </summary>
        /// <param name="packageSource">The package source.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>
        /// The accessor.
        /// </returns>
        [Pure]
        [NotNull]
        ICoreOwnerToken<INuGetAccessor> GetAccessor([CanBeNull] string packageSource, [NotNull] string apiKey);
    }
}