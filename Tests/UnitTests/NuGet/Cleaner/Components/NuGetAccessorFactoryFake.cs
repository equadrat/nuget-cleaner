using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using e2.Framework.Components;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using ExcludeFromCodeCoverage = System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute;

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
        [NotNull]
        private readonly ICoreTokenFactory _tokenFactory;

        /// <summary>
        /// The thread pool.
        /// </summary>
        [NotNull]
        private readonly ICoreThreadPool _threadPool;

        /// <summary>
        /// The repositories ordered by their package source.
        /// </summary>
        [NotNull]
        private readonly ConcurrentDictionary<string, NuGetRepository> _repositoryByPackageSource;

        /// <summary>
        /// The default repository
        /// </summary>
        [NotNull]
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
        internal NuGetAccessorFactoryFake([NotNull] ICoreTokenFactory tokenFactory, [NotNull] ICoreThreadPool threadPool)
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
        ICoreOwnerToken<INuGetAccessor> INuGetAccessorFactory.GetAccessor(string packageSource, string apiKey)
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
        [NotNull]
        internal NuGetRepository GetRepository([CanBeNull] string packageSource)
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
        internal bool TryGetRepository([CanBeNull] string packageSource, out NuGetRepository repository)
        {
            if (string.IsNullOrWhiteSpace(packageSource))
            {
                if (!this._defaultRepository.IsValueCreated)
                {
                    repository = null;
                    return false;
                }

                repository = this._defaultRepository.Value;
                return true;
            }

            return this._repositoryByPackageSource.TryGetValue(packageSource, out repository);
        }

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <param name="packageSource">The package source.</param>
        /// <returns>
        /// The repository.
        /// </returns>
        [Pure]
        [NotNull]
        private NuGetRepository CreateRepository([CanBeNull] string packageSource)
        {
            return new NuGetRepository(this._threadPool, packageSource);
        }
    }
}