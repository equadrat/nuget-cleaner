using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the package metadata.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageMetadata: IPackageMetadata
    {
        /// <inheritdoc />
        public string? PackageId {get; set;}

        /// <inheritdoc />
        public string? Owners {get; set;}

        /// <inheritdoc />
        public bool IsListed {get; set;}

        /// <inheritdoc />
        public bool IsDeprecated {get; set;}

        /// <inheritdoc />
        public DateTimeOffset? PublishDate {get; set;}

        /// <inheritdoc />
        public Version? Version {get; set;}

        /// <inheritdoc />
        public string? OriginalVersion {get; set;}

        /// <inheritdoc />
        public bool IsPrerelease {get; set;}

        /// <inheritdoc />
        public IPackageMetadata Clone()
        {
            return (IPackageMetadata)this.MemberwiseClone();
        }
    }
}