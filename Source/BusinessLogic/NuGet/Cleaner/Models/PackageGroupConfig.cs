using e2.Framework.Enums;
using JetBrains.Annotations;
using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the configuration of a package group.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageGroupConfig: IPackageGroupConfig
    {
        /// <inheritdoc />
        public string SourceId {get; set;}

        /// <inheritdoc />
        public string ApiKeyId {get; set;}

        /// <inheritdoc />
        public string Owner {get; set;}

        /// <inheritdoc />
        public string PackageIdPattern {get; set;}

        /// <inheritdoc />
        public eCoreStringMatchMode PackageIdMatchMode {get; set;}

        /// <inheritdoc />
        public string PackageCleanupId {get; set;}
    }
}