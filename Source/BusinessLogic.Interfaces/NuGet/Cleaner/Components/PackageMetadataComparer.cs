using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a comparer for <see cref="IPackageMetadata" />.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageMetadataComparer: EqualityComparer<IPackageMetadata>,
                                                 IComparer<IPackageMetadata>
    {
        /// <summary>
        /// Gets the default instance.
        /// </summary>
        /// <value>
        /// The default instance.
        /// </value>
        [NotNull]
        public static new PackageMetadataComparer Default {get;}

        /// <summary>
        /// The default string comparer.
        /// </summary>
        [NotNull]
        public static readonly StringComparer DefaultStringComparer;

        /// <summary>
        /// The version comparer.
        /// </summary>
        [NotNull]
        public static readonly Comparer<Version> VersionComparer;

        /// <summary>
        /// The date time offset comparer.
        /// </summary>
        [NotNull]
        public static readonly Comparer<DateTimeOffset?> DateTimeOffsetComparer;

        /// <summary>
        /// Initializes static members of the <see cref="PackageMetadataComparer" /> class.
        /// </summary>
        static PackageMetadataComparer()
        {
            Default = new PackageMetadataComparer();
            DefaultStringComparer = StringComparer.Ordinal;
            VersionComparer = Comparer<Version>.Default;
            DateTimeOffsetComparer = Comparer<DateTimeOffset?>.Default;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="PackageMetadataComparer" /> class from being created.
        /// </summary>
        private PackageMetadataComparer()
        {
        }

        /// <inheritdoc />
        public int Compare(IPackageMetadata x, IPackageMetadata y)
        {
            if (x == null) return y == null ? 0 : -1;
            if (y == null) return 1;

            if (ReferenceEquals(x, y)) return 0;

            var result = DefaultStringComparer.Compare(x.PackageId, y.PackageId);
            if (result != 0) return result;

            result = VersionComparer.Compare(x.Version, y.Version);
            if (result != 0) return result;

            result = DateTimeOffsetComparer.Compare(x.PublishDate, y.PublishDate);
            if (result != 0) return result;

            if (x.IsPrerelease)
            {
                if (y.IsPrerelease)
                {
                    result = DefaultStringComparer.Compare(x.OriginalVersion, y.OriginalVersion);
                    if (result != 0) return result;
                }
                else return -1;
            }
            else if (y.IsPrerelease) return 1;
            else
            {
                result = DefaultStringComparer.Compare(x.OriginalVersion, y.OriginalVersion);
                if (result != 0) return result;
            }

            if (x.IsListed)
            {
                if (!y.IsListed) return 1;
            }
            else if (y.IsListed) return -1;

            if (x.IsDeprecated)
            {
                if (!y.IsDeprecated) return 1;
            }
            else if (y.IsDeprecated) return -1;

            return DefaultStringComparer.Compare(x.Owners, y.Owners);
        }

        /// <inheritdoc />
        public override bool Equals(IPackageMetadata x, IPackageMetadata y)
        {
            if (x == null) return y == null;
            if (y == null) return false;

            if (ReferenceEquals(x, y)) return true;

            return DefaultStringComparer.Equals(x.PackageId, y.PackageId)
                   && DefaultStringComparer.Equals(x.Owners, y.Owners)
                   && (x.IsListed == y.IsListed)
                   && (x.IsDeprecated == y.IsDeprecated)
                   && (x.PublishDate == y.PublishDate)
                   && (x.Version == y.Version)
                   && DefaultStringComparer.Equals(x.OriginalVersion, y.OriginalVersion)
                   && (x.IsPrerelease == y.IsPrerelease);
        }

        /// <inheritdoc />
        public override int GetHashCode(IPackageMetadata obj)
        {
            return HashCode.Combine(
                obj.PackageId,
                obj.Owners,
                obj.IsListed,
                obj.IsDeprecated,
                obj.PublishDate,
                obj.Version,
                obj.OriginalVersion,
                obj.IsPrerelease);
        }
    }
}