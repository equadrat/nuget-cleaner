using System;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes a dictionary to provide/track the publish dates for packages.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackagePublishDateDictionary
    {
        /// <summary>
        /// Gets the publish date for the specified package identifier and version.
        /// </summary>
        /// <param name="packageId">The package identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="originalVersion">The original version.</param>
        /// <param name="publishDate">The publish date.</param>
        /// <returns>
        /// The publish date.
        /// </returns>
        [Pure]
        DateTimeOffset? GetPublishDate(string packageId, Version version, string originalVersion, DateTimeOffset? publishDate);
    }
}