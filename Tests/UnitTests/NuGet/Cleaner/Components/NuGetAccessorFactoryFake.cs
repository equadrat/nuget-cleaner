using e2.Framework.Components;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a fake implementation of <see cref="INuGetAccessorFactory" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class NuGetAccessorFactoryFake: INuGetAccessorFactory
    {
        /// <summary>
        /// The token factory.
        /// </summary>
        private readonly ICoreOwnerTokenFactory _tokenFactory;

        /// <summary>
        /// The thread pool.
        /// </summary>
        private readonly ICoreThreadPool _threadPool;

        /// <summary>
        /// The repositories ordered by their package source.
        /// </summary>
        private readonly ConcurrentDictionary<string, NuGetRepository> _repositoryByPackageSource;

        /// <summary>
        /// The default repository
        /// </summary>
        private readonly Lazy<NuGetRepository> _defaultRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetAccessorFactoryFake" /> class.
        /// </summary>
        /// <param name="tokenFactory">The token factory.</param>
        /// <param name="threadPool">The thread pool.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tokenFactory
        /// or
        /// threadPool
        /// </exception>
        internal NuGetAccessorFactoryFake(ICoreOwnerTokenFactory tokenFactory, ICoreThreadPool threadPool)
        {
#if DEBUG
            if (tokenFactory == null) throw new ArgumentNullException(nameof(tokenFactory));
            if (threadPool == null) throw new ArgumentNullException(nameof(threadPool));
#endif
            this._tokenFactory = tokenFactory;
            this._threadPool = threadPool;
            this._repositoryByPackageSource = new ConcurrentDictionary<string, NuGetRepository>(StringComparer.Ordinal);
            this._defaultRepository = new Lazy<NuGetRepository>(() => this.CreateRepository(null));
        }

        /// <inheritdoc />
        ICoreOwnerToken<INuGetAccessor> INuGetAccessorFactory.GetAccessor(string? packageSource, string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));

            var accessor = new NuGetAccessorFake(this, packageSource, apiKey);
            return this._tokenFactory.CreateDummyToken<INuGetAccessor>(accessor);
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <param name="packageSource">The package source.</param>
        /// <returns>
        /// The repository.
        /// </returns>
        [Pure]
        internal NuGetRepository GetRepository(string? packageSource)
        {
            if (string.IsNullOrWhiteSpace(packageSource)) return this._defaultRepository.Value;
            return this._repositoryByPackageSource.GetOrAdd(packageSource, this.CreateRepository);
        }

        /// <summary>
        /// Tries to get the repository.
        /// </summary>
        /// <param name="packageSource">The package source.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="repository" /> exists; otherwise, <c>false</c>.
        /// </returns>
        internal bool TryGetRepository(string? packageSource, out NuGetRepository repository)
        {
            if (string.IsNullOrWhiteSpace(packageSource))
            {
                if (!this._defaultRepository.IsValueCreated)
                {
                    repository = null!;
                    return false;
                }

                repository = this._defaultRepository.Value;
                return true;
            }

            return this._repositoryByPackageSource.TryGetValue(packageSource, out repository!);
        }

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <param name="packageSource">The package source.</param>
        /// <returns>
        /// The repository.
        /// </returns>
        [Pure]
        private NuGetRepository CreateRepository(string? packageSource)
        {
            return new NuGetRepository(this._threadPool, packageSource);
        }
    }
}