using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes an accessor for the NuGet API.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface INuGetAccessor
    {
        /// <summary>
        /// Gets the packages of an owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="packageIdPredicate">The package identifier predicate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The packages.
        /// </returns>
        [Pure]
        [NotNull]
        [ItemNotNull]
        IAsyncEnumerable<IPackageMetadata> GetPackagesAsync([NotNull] string owner, [CanBeNull] Func<string, bool> packageIdPredicate, CancellationToken cancellationToken);

        /// <summary>
        /// Unlists a package.
        /// </summary>
        /// <param name="packageId">The package identifier.</param>
        /// <param name="originalVersion">The original version.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The state of the asynchronous process.
        /// </returns>
        [NotNull]
        Task UnlistPackageAsync([NotNull] string packageId, [NotNull] string originalVersion, CancellationToken cancellationToken);
    }
}