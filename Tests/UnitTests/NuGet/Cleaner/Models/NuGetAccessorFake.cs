using e2.Framework;
using e2.NuGet.Cleaner.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents a fake implementation of <see cref="INuGetAccessor" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class NuGetAccessorFake: INuGetAccessor
    {
        /// <summary>
        /// The owner.
        /// </summary>
        private readonly NuGetAccessorFactoryFake _owner;

        /// <summary>
        /// The package source.
        /// </summary>
        private readonly string? _packageSource;

        /// <summary>
        /// The API key.
        /// </summary>
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetAccessorFake" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="packageSource">The package source.</param>
        /// <param name="apiKey">The API key.</param>
        /// <exception cref="System.ArgumentNullException">
        /// owner
        /// or
        /// apiKey
        /// </exception>
        internal NuGetAccessorFake(NuGetAccessorFactoryFake owner, string? packageSource, string apiKey)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));

            this._owner = owner;
            this._packageSource = packageSource;
            this._apiKey = apiKey;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<IPackageMetadata> GetPackagesAsync(string owner, Func<string, bool>? packageIdPredicate, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (packageIdPredicate == null) throw new ArgumentNullException(nameof(packageIdPredicate));

            if (!this.TryGetRepository(out var repository, out var exception)) throw exception;

            await foreach (var packageMetadata in repository.GetPackagesAsync(owner, packageIdPredicate, cancellationToken))
            {
                yield return packageMetadata;
            }
        }

        /// <inheritdoc />
        public async Task UnlistPackageAsync(string packageId, string originalVersion, CancellationToken cancellationToken)
        {
            if (packageId == null) throw new ArgumentNullException(nameof(packageId));
            if (originalVersion == null) throw new ArgumentNullException(nameof(originalVersion));

            if (!this.TryGetRepository(out var repository, out var exception, validateApiKey: true)) throw exception;

            await repository.UnlistPackageAsync(packageId, originalVersion, cancellationToken);
        }

        /// <summary>
        /// Tries to get the repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="_">unused</param>
        /// <param name="validateApiKey"><c>true</c> to validate the API key.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="validateApiKey" /> exists; otherwise, <c>false</c>.
        /// </returns>
        // ReSharper disable once UnusedParameter.Local
        private bool TryGetRepository(out NuGetRepository repository, out Exception exception, NCObject? _ = null, bool validateApiKey = false)
        {
            if (!this._owner.TryGetRepository(this._packageSource, out repository))
            {
                exception = new HttpRequestException("PackageSource not found.", null, HttpStatusCode.NotFound);
                return false;
            }

            if (validateApiKey && !repository.IsApiKeyValid(this._apiKey))
            {
                exception = new HttpRequestException("Access denied.", null, HttpStatusCode.Forbidden);
                return false;
            }

            exception = null!;
            return true;
        }
    }
}