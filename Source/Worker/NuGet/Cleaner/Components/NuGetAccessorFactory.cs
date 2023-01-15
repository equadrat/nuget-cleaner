﻿using System;
using System.Diagnostics;
using e2.Framework.Components;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using ExcludeFromCodeCoverage = System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute;
using INuGetLogger = NuGet.Common.ILogger;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a factory to create instances of <see cref="INuGetAccessor" />.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    [ExcludeFromCodeCoverage]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class NuGetAccessorFactory: INuGetAccessorFactory
    {
        /// <summary>
        /// The token factory.
        /// </summary>
        [NotNull]
        private readonly ICoreTokenFactory _tokenFactory;

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
        /// The accessor instance pool.
        /// </summary>
        [NotNull]
        private readonly ICoreInstancePool<NuGetAccessor> _accessorInstancePool;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetAccessorFactory" /> class.
        /// </summary>
        /// <param name="tokenFactory">The token factory.</param>
        /// <param name="instancePoolFactory">The instance pool factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="nuGetLogger">The nu get logger.</param>
        /// <param name="packageMetadataFactory">The package metadata factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tokenFactory
        /// or
        /// instancePoolFactory
        /// or
        /// logger
        /// or
        /// nuGetLogger
        /// or
        /// packageMetadataFactory
        /// </exception>
        public NuGetAccessorFactory([NotNull] ICoreTokenFactory tokenFactory, [NotNull] ICoreInstancePoolFactory instancePoolFactory, [NotNull] ILogger logger, [NotNull] INuGetLogger nuGetLogger, [NotNull] ICoreIOCInstanceFactory<IPackageMetadata> packageMetadataFactory)
        {
            if (tokenFactory == null) throw new ArgumentNullException(nameof(tokenFactory));
            if (instancePoolFactory == null) throw new ArgumentNullException(nameof(instancePoolFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (nuGetLogger == null) throw new ArgumentNullException(nameof(nuGetLogger));
            if (packageMetadataFactory == null) throw new ArgumentNullException(nameof(packageMetadataFactory));

            this._tokenFactory = tokenFactory;
            this._logger = logger;
            this._nuGetLogger = nuGetLogger;
            this._packageMetadataFactory = packageMetadataFactory;

            this._accessorInstancePool = instancePoolFactory.CreateInstancePool(this.CreateAccessor, cleanup: NuGetAccessor.Cleanup);
        }

        /// <inheritdoc />
        public ICoreOwnerToken<INuGetAccessor> GetAccessor(string packageSource, string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));

            var accessor = this._accessorInstancePool.GetInstance();
            accessor.Init(packageSource, apiKey);

            return this._tokenFactory.CreateRelayToken<INuGetAccessor, NuGetAccessor>(accessor, this.ReleaseAccessor);
        }

        /// <summary>
        /// Releases the accessor.
        /// </summary>
        /// <param name="accessor">The accessor.</param>
        private void ReleaseAccessor([NotNull] NuGetAccessor accessor)
        {
            if (!this._accessorInstancePool.Recycle(accessor)) NuGetAccessor.Cleanup(accessor);
        }

        /// <summary>
        /// Creates an accessor.
        /// </summary>
        /// <returns>
        /// The accessor.
        /// </returns>
        [Pure]
        [NotNull]
        private NuGetAccessor CreateAccessor()
        {
            return new NuGetAccessor(this._logger, this._nuGetLogger, this._packageMetadataFactory);
        }
    }
}