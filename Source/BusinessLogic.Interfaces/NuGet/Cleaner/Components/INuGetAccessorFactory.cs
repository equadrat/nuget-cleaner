using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using System;
using System.Diagnostics.Contracts;

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
        ICoreOwnerToken<INuGetAccessor> GetAccessor(string? packageSource, string apiKey);
    }
}