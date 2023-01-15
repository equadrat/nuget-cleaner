using System;
using System.Diagnostics;
using e2.Framework.Components;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes the logger.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface ILogger
    {
        /// <summary>
        /// Gets the logging event when starting to process the packages.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent ProcessPackagesBegin {get;}

        /// <summary>
        /// Gets the logging event when processing the packages failed.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent ProcessPackagesFailed {get;}

        /// <summary>
        /// Gets the logging event when finished to process the packages.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent ProcessPackagesEnd {get;}

        /// <summary>
        /// Gets the logging event when the config was loaded.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent ProcessPackagesConfigLoaded {get;}

        /// <summary>
        /// Gets the logging event when starting to query the packages of a specific owner.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetQueryPackagesByOwnerBegin {get;}

        /// <summary>
        /// Gets the logging event when quering the next packages page of a specific owner.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetQueryPackagesByOwnerNextPage {get;}

        /// <summary>
        /// Gets the logging event when finished to query the packages of a specific owner.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetQueryPackagesByOwnerEnd {get;}

        /// <summary>
        /// Gets the logging event when skipping to query the versions of a specific package.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetQueryPackageVersionsSkip {get;}

        /// <summary>
        /// Gets the logging event when starting to query the versions of a specific package.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetQueryPackageVersionsBegin {get;}

        /// <summary>
        /// Gets the logging event when finished to query the versions of a specific package.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetQueryPackageVersionsEnd {get;}

        /// <summary>
        /// Gets the logging event when starting to delete a package.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetDeletePackageVersionBegin {get;}

        /// <summary>
        /// Gets the logging event when the API key is requested to delete a package.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetDeletePackageVersionApiKeyRequested {get;}

        /// <summary>
        /// Gets the logging event when handling the confirmation to delete a package.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetDeletePackageVersionConfirmation {get;}

        /// <summary>
        /// Gets the logging event when failed to delete a package.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetDeletePackageVersionFail {get;}

        /// <summary>
        /// Gets the logging event when finished to delete a package.
        /// </summary>
        /// <value>
        /// The logging event.
        /// </value>
        [NotNull]
        ICoreLoggingEvent NuGetDeletePackageVersionEnd {get;}
    }
}