using e2.Framework.Helpers;
using e2.Framework.MemberTemplates;
using e2.Framework.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents a repository.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class NuGetRepository: IDisposable,
                                           INuGetAccessor,
                                           ICoreClear,
                                           ICoreAdd<IPackageMetadata>
    {
        /// <summary>
        /// Gets the package source.
        /// </summary>
        /// <value>
        /// The package source.
        /// </value>
        internal string? PackageSource {get;}

        /// <summary>
        /// The thread pool.
        /// </summary>
        private readonly ICoreThreadPool _threadPool;

        /// <summary>
        /// The lock.
        /// </summary>
        private readonly ReaderWriterLockSlim _lock;

        /// <summary>
        /// The API keys.
        /// </summary>
        private readonly HashSet<string> _apiKeys;

        /// <summary>
        /// The packages.
        /// </summary>
        private readonly List<IPackageMetadata> _packages;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetRepository" /> class.
        /// </summary>
        /// <param name="threadPool">The thread pool.</param>
        /// <param name="packageSource">The package source.</param>
        /// <exception cref="System.ArgumentNullException">threadPool</exception>
        internal NuGetRepository(ICoreThreadPool threadPool, string? packageSource)
        {
            if (threadPool == null) throw new ArgumentNullException(nameof(threadPool));

            this.PackageSource = packageSource;

            this._threadPool = threadPool;
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this._apiKeys = new HashSet<string>(StringComparer.Ordinal);
            this._packages = new List<IPackageMetadata>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="NuGetRepository" /> class.
        /// </summary>
        ~NuGetRepository()
        {
            this.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._lock.Dispose();

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds an API key.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        internal void AddApiKey(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));

            this._lock.EnterWriteLock();

            try
            {
                this._apiKeys.Add(apiKey);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether the specified API key is valid.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>
        /// <c>true</c> if the specified API key is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        [Pure]
        internal bool IsApiKeyValid(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));

            this._lock.EnterReadLock();

            try
            {
                return this._apiKeys.Contains(apiKey);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<IPackageMetadata> GetPackagesAsync(string owner, Func<string, bool>? packageIdPredicate, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (packageIdPredicate == null) throw new ArgumentNullException(nameof(packageIdPredicate));

            var result = await this._threadPool.RunAsync(
                () =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    List<IPackageMetadata> packages;

                    this._lock.EnterReadLock();

                    try
                    {
                        packages = this._packages
                                       .Where(x => x.Owners == owner)
                                       .Select(x => x.Clone())
                                       .ToList();
                    }
                    finally
                    {
                        this._lock.ExitReadLock();
                    }

                    return packages.Where(x => (x.PackageId != null) && packageIdPredicate.Invoke(x.PackageId));
                });

            foreach (var metadata in result)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return metadata;
            }
        }

        /// <inheritdoc />
        public async Task UnlistPackageAsync(string packageId, string originalVersion, CancellationToken cancellationToken)
        {
            if (packageId == null) throw new ArgumentNullException(nameof(packageId));
            if (originalVersion == null) throw new ArgumentNullException(nameof(originalVersion));

            await this._threadPool.RunAsync(
                () =>
                {
                    this._lock.EnterWriteLock();

                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        foreach (var packageMetadata in this._packages.Where(x => (x.PackageId == packageId) && (x.OriginalVersion == originalVersion)))
                        {
                            packageMetadata.IsListed = false;
                        }
                    }
                    finally
                    {
                        this._lock.ExitWriteLock();
                    }
                });
        }

        /// <inheritdoc />
        public void Clear()
        {
            this._lock.EnterWriteLock();

            try
            {
                this._apiKeys.Clear();
                this._packages.Clear();
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <inheritdoc />
        public void Add(IPackageMetadata item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            this._lock.EnterWriteLock();

            try
            {
                this._packages.Add(item.Clone());
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the packages.
        /// </summary>
        /// <returns>
        /// The packages.
        /// </returns>
        [Pure]
        internal IReadOnlyList<IPackageMetadata> GetPackages()
        {
            this._lock.EnterReadLock();

            try
            {
                return this._packages.Select(x => x.Clone()).AsReadOnly();
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }
    }
}