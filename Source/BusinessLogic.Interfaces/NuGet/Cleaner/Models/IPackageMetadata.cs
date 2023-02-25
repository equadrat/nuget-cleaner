using e2.Framework.MemberTemplates;
using JetBrains.Annotations;
using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This interface describes the package metadata.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackageMetadata: ICoreCloneable<IPackageMetadata>
    {
        /// <summary>
        /// Gets or sets the package identifier.
        /// </summary>
        /// <value>
        /// The package identifier.
        /// </value>
        string PackageId {get; set;}

        /// <summary>
        /// Gets or sets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        string Owners {get; set;}

        /// <summary>
        /// Gets or sets a value indicating whether this instance is listed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is listed; otherwise, <c>false</c>.
        /// </value>
        bool IsListed {get; set;}

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deprecated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deprecated; otherwise, <c>false</c>.
        /// </value>
        bool IsDeprecated {get; set;}

        /// <summary>
        /// Gets or sets the publish date.
        /// </summary>
        /// <value>
        /// The publish date.
        /// </value>
        DateTimeOffset? PublishDate {get; set;}

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        Version Version {get; set;}

        /// <summary>
        /// Gets or sets the original version.
        /// </summary>
        /// <value>
        /// The original version.
        /// </value>
        string OriginalVersion {get; set;}

        /// <summary>
        /// Gets or sets a value indicating whether this instance is prerelease.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is prerelease; otherwise, <c>false</c>.
        /// </value>
        bool IsPrerelease {get; set;}
    }
}