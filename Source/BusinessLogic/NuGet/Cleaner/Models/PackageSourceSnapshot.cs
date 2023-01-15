using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents a snapshot of a package source.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal sealed class PackageSourceSnapshot: IPackageSourceSnapshot
    {
        /// <inheritdoc />
        public ISourceConfig Source {get;}

        /// <inheritdoc />
        public IApiKeyConfig ApiKey {get;}

        /// <inheritdoc />
        public IReadOnlyList<IPackageOwnerSnapshot> Owners {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageSourceSnapshot" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="owners">The owners.</param>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// apiKey
        /// or
        /// owners
        /// </exception>
        internal PackageSourceSnapshot([NotNull] ISourceConfig source, [NotNull] IApiKeyConfig apiKey, [NotNull] IReadOnlyList<IPackageOwnerSnapshot> owners)
        {
#if DEBUG
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            if (owners == null) throw new ArgumentNullException(nameof(owners));
#endif
            this.Source = source;
            this.ApiKey = apiKey;
            this.Owners = owners;
        }
    }
}