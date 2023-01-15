using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using e2.Framework.Components;
using e2.Framework.Exceptions;
using e2.NuGet.Cleaner.Components;
using JetBrains.Annotations;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using ExcludeFromCodeCoverage = System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute;
using INuGetLogger = NuGet.Common.ILogger;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents an accessor for the NuGet API.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    [ExcludeFromCodeCoverage]
    internal sealed class NuGetAccessor: INuGetAccessor
    {
        /// <summary>
        /// The default package source.
        /// </summary>
        private const string DefaultPackageSource = "https://api.nuget.org/v3/index.json";

        /// <summary>
        /// Cleanups the specified accessor.
        /// </summary>
        /// <param name="accessor">The accessor.</param>
        /// <exception cref="System.ArgumentNullException">accessor</exception>
        internal static void Cleanup([NotNull] NuGetAccessor accessor)
        {
#if DEBUG
            if (accessor == null) throw new ArgumentNullException(nameof(accessor));
#endif
            accessor._nuGetProtocolFactory = null;
            accessor._apiKey = null;
        }

        /// <summary>
        /// Gets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [NotNull]
        private string ApiKey => this._apiKey ?? throw new CoreInvalidOperationException(this);

        /// <summary>
        /// Gets the NuGet protocol factory.
        /// </summary>
        /// <value>
        /// The NuGet protocol factory.
        /// </value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [NotNull]
        private SourceRepository NuGetProtocolFactory => this._nuGetProtocolFactory ?? throw new CoreInvalidOperationException(this);

        /// <summary>
        /// The logger.
        /// </summary>
        [NotNull]
        private readonly ILogger _logger;

        /// <summary>
        /// The NuGet logger.
        /// </summary>
        [NotNull]
        private readonly INuGetLogger _nuGetLogger;

        /// <summary>
        /// The package metadata factory.
        /// </summary>
        [NotNull]
        private readonly ICoreIOCInstanceFactory<IPackageMetadata> _packageMetadataFactory;

        /// <summary>
        /// The backing field of <see cref="NuGetProtocolFactory" />.
        /// </summary>
        [CanBeNull]
        private SourceRepository _nuGetProtocolFactory;

        /// <summary>
        /// The backing field of <see cref="ApiKey" />.
        /// </summary>
        [CanBeNull]
        private string _apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetAccessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="nuGetLogger">The NuGet logger.</param>
        /// <param name="packageMetadataFactory">The package metadata factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// logger
        /// or
        /// nuGetLogger
        /// or
        /// packageMetadataFactory
        /// </exception>
        internal NuGetAccessor([NotNull] ILogger logger, [NotNull] INuGetLogger nuGetLogger, [NotNull] ICoreIOCInstanceFactory<IPackageMetadata> packageMetadataFactory)
        {
#if DEBUG
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (nuGetLogger == null) throw new ArgumentNullException(nameof(nuGetLogger));
            if (packageMetadataFactory == null) throw new ArgumentNullException(nameof(packageMetadataFactory));
#endif
            this._logger = logger;
            this._nuGetLogger = nuGetLogger;
            this._packageMetadataFactory = packageMetadataFactory;
        }

        /// <summary>
        /// Initializes the specified package source.
        /// </summary>
        /// <param name="packageSource">The package source.</param>
        /// <param name="apiKey">The API key.</param>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        internal void Init([CanBeNull] string packageSource, [NotNull] string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));

            if (string.IsNullOrWhiteSpace(packageSource)) packageSource = DefaultPackageSource;

            this._nuGetProtocolFactory = Repository.Factory.GetCoreV3(new PackageSource(packageSource)) ?? throw new CoreInvalidOperationException(this);
            this._apiKey = apiKey;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<IPackageMetadata> GetPackagesAsync(string owner, Func<string, bool> packageIdPredicate, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var ownerPackageMetadata in this.QueryOwnerPackages(owner, cancellationToken))
            {
                var ownerPackageIdentity = ownerPackageMetadata.Identity;

                if ((packageIdPredicate != null) && !packageIdPredicate.Invoke(ownerPackageIdentity.Id))
                {
                    this._logger.NuGetQueryPackageVersionsSkip.IfEnabled?.Log((string)null, ownerPackageIdentity.Id);
                    continue;
                }

                await foreach (var packageVersionMetadata in this.QueryPackageVersions(ownerPackageIdentity.Id, cancellationToken))
                {
                    var model = this._packageMetadataFactory.CreateInstance();

                    var packageIdentity = packageVersionMetadata.Identity;

                    model.PackageId = packageIdentity.Id;
                    model.Owners = packageVersionMetadata.Owners;
                    model.IsListed = packageVersionMetadata.IsListed;

                    var deprecationMetadata = await packageVersionMetadata.GetDeprecationMetadataAsync();
                    model.IsDeprecated = (deprecationMetadata != null) && deprecationMetadata.Reasons.Any();

                    model.PublishDate = packageVersionMetadata.Published;

                    if (packageIdentity.HasVersion)
                    {
                        model.Version = packageIdentity.Version.Version;
                        model.OriginalVersion = packageIdentity.Version.OriginalVersion;
                        model.IsPrerelease = packageIdentity.Version.IsPrerelease;
                    }
                    else
                    {
                        model.Version = null;
                        model.OriginalVersion = null;
                        model.IsPrerelease = false;
                    }

                    yield return model;
                }
            }
        }

        /// <inheritdoc />
        public async Task UnlistPackageAsync(string packageId, string originalVersion, CancellationToken cancellationToken)
        {
            if (packageId == null) throw new ArgumentNullException(nameof(packageId));
            if (originalVersion == null) throw new ArgumentNullException(nameof(originalVersion));

            this._logger.NuGetDeletePackageVersionBegin.IfEnabled?.Log((string)null, packageId, originalVersion);

            try
            {
                var resource = this.NuGetProtocolFactory.GetResource<PackageUpdateResource>();
                await resource.Delete(packageId, originalVersion, this.UnlistPackageGetApiKey, this.ConfirmPackageDelete, true, this._nuGetLogger);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this._logger.NuGetDeletePackageVersionFail.IfEnabled?.Log(ex, packageId, originalVersion);
                throw;
            }

            this._logger.NuGetDeletePackageVersionEnd.IfEnabled?.Log((string)null, packageId, originalVersion);
        }

        /// <summary>
        /// Gets the API key to unlist a package.
        /// </summary>
        /// <param name="packageSource">The package identifier.</param>
        /// <returns>
        /// The API key.
        /// </returns>
        [Pure]
        [NotNull]
        private string UnlistPackageGetApiKey(string packageSource)
        {
            this._logger.NuGetDeletePackageVersionApiKeyRequested.IfEnabled?.Log((string)null, packageSource);
            return this.ApiKey;
        }

        /// <summary>
        /// Confirms the package delete.
        /// </summary>
        /// <param name="message">The package identifier.</param>
        /// <returns>
        /// <c>true</c> to confirm; <c>false</c> to refuse.
        /// </returns>
        [Pure]
        private bool ConfirmPackageDelete(string message)
        {
            this._logger.NuGetDeletePackageVersionConfirmation.IfEnabled?.Log(message);
            return true;
        }

        /// <summary>
        /// Queries the owner packages.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The owner package metadata.
        /// </returns>
        [Pure]
        [NotNull]
        private async IAsyncEnumerable<IPackageSearchMetadata> QueryOwnerPackages(string owner, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            this._logger.NuGetQueryPackagesByOwnerBegin.IfEnabled?.Log((string)null, owner);

            var resource = this.NuGetProtocolFactory.GetResource<PackageSearchResource>() ?? throw new CoreTypeNotSupportedException(typeof(PackageSearchResource));
            var searchTerm = $"owner:{owner}";

            const int itemsPerQuery = 20;

            var offset = 0;
            int oldOffset;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                oldOffset = offset;

                var metaDataItems = await resource.SearchAsync(searchTerm, new SearchFilter(true), offset, itemsPerQuery, this._nuGetLogger, CancellationToken.None);
                foreach (var metaDataItem in metaDataItems)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    yield return metaDataItem;
                    offset++;
                }
            }
            while (offset != oldOffset);

            this._logger.NuGetQueryPackagesByOwnerEnd.IfEnabled?.Log($"NumberOfPackages: {offset}", owner, offset.ToString("0", CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Queries the package versions.
        /// </summary>
        /// <param name="packageId">The package identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The package version metadata.
        /// </returns>
        [Pure]
        [NotNull]
        private async IAsyncEnumerable<IPackageSearchMetadata> QueryPackageVersions(string packageId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            this._logger.NuGetQueryPackageVersionsBegin.IfEnabled?.Log((string)null, packageId);

            var resource = this.NuGetProtocolFactory.GetResource<PackageMetadataResource>() ?? throw new CoreTypeNotSupportedException(typeof(PackageMetadataResource));

            var numberOfVersions = 0;

            var metaDataItems = await resource.GetMetadataAsync(packageId, true, true, NullSourceCacheContext.Instance, this._nuGetLogger, cancellationToken);
            foreach (var metaDataItem in metaDataItems)
            {
                yield return metaDataItem;
                numberOfVersions++;
            }

            this._logger.NuGetQueryPackageVersionsEnd.IfEnabled?.Log($"NumberOfVersions: {numberOfVersions}", packageId, numberOfVersions.ToString("0", CultureInfo.CurrentCulture));
        }
    }
}