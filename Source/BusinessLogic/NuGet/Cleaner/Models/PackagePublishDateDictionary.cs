using e2.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents a dictionary to provide/track the publish dates for packages.
    /// </summary>
    internal sealed class PackagePublishDateDictionary: IPackagePublishDateDictionary
    {
        /// <summary>
        /// Cleanups the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <exception cref="System.ArgumentNullException">dictionary</exception>
        internal static void Cleanup(PackagePublishDateDictionary dictionary)
        {
#if DEBUG
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
#endif
            dictionary._publishDateByVersion.Clear();
            dictionary._packageOwnerSnapshot = null;
        }

        /// <summary>
        /// Gets the package owner snapshot.
        /// </summary>
        /// <value>
        /// The package owner snapshot.
        /// </value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IPackageOwnerSnapshot PackageOwnerSnapshot => this._packageOwnerSnapshot ?? throw new CoreInvalidOperationException(this);

        /// <summary>
        /// The publish dates ordered by their package and version.
        /// </summary>
        private readonly Dictionary<(int PackageGroupIndex, Version Version, string OriginalVersion), DateTimeOffset> _publishDateByVersion;

        /// <summary>
        /// The backing field of <see cref="PackageOwnerSnapshot" />.
        /// </summary>
        private IPackageOwnerSnapshot? _packageOwnerSnapshot;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackagePublishDateDictionary" /> class.
        /// </summary>
        internal PackagePublishDateDictionary()
        {
            this._publishDateByVersion = new Dictionary<(int PackageGroupIndex, Version Version, string OriginalVersion), DateTimeOffset>();
            this._packageOwnerSnapshot = null;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="packageOwnerSnapshot">The package owner snapshot.</param>
        /// <exception cref="System.ArgumentNullException">packageOwnerSnapshot</exception>
        internal void Init(IPackageOwnerSnapshot packageOwnerSnapshot)
        {
#if DEBUG
            if (packageOwnerSnapshot == null) throw new ArgumentNullException(nameof(packageOwnerSnapshot));
#endif
            this._packageOwnerSnapshot = packageOwnerSnapshot;
        }

        /// <inheritdoc />
        public DateTimeOffset? GetPublishDate(string packageId, Version version, string originalVersion, DateTimeOffset? publishDate)
        {
            if (packageId == null) throw new ArgumentNullException(nameof(packageId));
            if (version == null) throw new ArgumentNullException(nameof(version));
            if (originalVersion == null) throw new ArgumentNullException(nameof(originalVersion));

            var packageGroupIndex = this.PackageOwnerSnapshot.GetPackageGroupIndex(packageId);
            if (packageGroupIndex == -1) throw new CoreKeyNotFoundException(packageId);

            var key = (packageGroupIndex, version, originalVersion);

            if (!publishDate.HasValue) return this._publishDateByVersion.TryGetValue(key, out var tmp) ? tmp : null;
            if (!this._publishDateByVersion.TryGetValue(key, out var result) || (publishDate.Value > result)) this._publishDateByVersion[key] = result = publishDate.Value;

            return result;
        }
    }
}