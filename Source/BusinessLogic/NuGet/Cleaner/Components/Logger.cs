using e2.Framework.Components;
using e2.Framework.Enums;
using JetBrains.Annotations;
using System;
using ExcludeFromCodeCoverage = System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the logger.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class Logger: ILogger
    {
        /// <inheritdoc />
        public ICoreLoggingEvent ProcessPackagesBegin {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent ProcessPackagesFailed {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent ProcessPackagesEnd {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent ProcessPackagesConfigLoaded {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetQueryPackagesByOwnerBegin {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetQueryPackagesByOwnerNextPage {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetQueryPackagesByOwnerEnd {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetQueryPackageVersionsSkip {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetQueryPackageVersionsBegin {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetQueryPackageVersionsEnd {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetDeletePackageVersionBegin {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetDeletePackageVersionApiKeyRequested {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetDeletePackageVersionConfirmation {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetDeletePackageVersionFail {get;}

        /// <inheritdoc />
        public ICoreLoggingEvent NuGetDeletePackageVersionEnd {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="loggingContext">The logging context.</param>
        /// <exception cref="System.ArgumentNullException">loggingContext</exception>
        public Logger([NotNull] ICoreLoggingContext loggingContext)
        {
            if (loggingContext == null) throw new ArgumentNullException(nameof(loggingContext));

            var product = loggingContext.RegisterProduct("NuGet.Cleaner", throwErrorIfDuplicateAndDifferent: false);

            this.ProcessPackagesBegin = product.RegisterEvent("PROCESS_PACKAGES_BEGIN", eCoreLoggingLevel.Information);
            this.ProcessPackagesFailed = product.RegisterEvent("PROCESS_PACKAGES_FAIL", eCoreLoggingLevel.Error);
            this.ProcessPackagesEnd = product.RegisterEvent("PROCESS_PACKAGES_END", eCoreLoggingLevel.Information);
            this.ProcessPackagesConfigLoaded = product.RegisterEvent("PROCESS_PACKAGES_CONFIG_LOAD", eCoreLoggingLevel.Information);
            this.NuGetQueryPackagesByOwnerBegin = product.RegisterEvent("QUERY_OWNER_PACKAGES_BEGIN", eCoreLoggingLevel.Debug);
            this.NuGetQueryPackagesByOwnerNextPage = product.RegisterEvent("QUERY_OWNER_PACKAGES_NEXTPAGE", eCoreLoggingLevel.Debug);
            this.NuGetQueryPackagesByOwnerEnd = product.RegisterEvent("QUERY_OWNER_PACKAGES_END", eCoreLoggingLevel.Debug);
            this.NuGetQueryPackageVersionsSkip = product.RegisterEvent("QUERY_PACKAGE_VERSIONS_SKIP", eCoreLoggingLevel.Debug);
            this.NuGetQueryPackageVersionsBegin = product.RegisterEvent("QUERY_PACKAGE_VERSIONS_BEGIN", eCoreLoggingLevel.Debug);
            this.NuGetQueryPackageVersionsEnd = product.RegisterEvent("QUERY_PACKAGE_VERSIONS_END", eCoreLoggingLevel.Debug);
            this.NuGetDeletePackageVersionBegin = product.RegisterEvent("DELETE_PACKAGE_BEGIN", eCoreLoggingLevel.Information);
            this.NuGetDeletePackageVersionApiKeyRequested = product.RegisterEvent("DELETE_PACKAGE_API_KEY_REQUESTED", eCoreLoggingLevel.Debug);
            this.NuGetDeletePackageVersionConfirmation = product.RegisterEvent("DELETE_PACKAGE_CONFIRM", eCoreLoggingLevel.Debug);
            this.NuGetDeletePackageVersionFail = product.RegisterEvent("DELETE_PACKAGE_FAIL", eCoreLoggingLevel.Error);
            this.NuGetDeletePackageVersionEnd = product.RegisterEvent("DELETE_PACKAGE_END", eCoreLoggingLevel.Information);
        }
    }
}