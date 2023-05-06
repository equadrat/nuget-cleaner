using e2.Framework.Enums;
using e2.NuGet.Cleaner.Components;
using e2.NuGet.Cleaner.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace e2.NuGet.Cleaner.Helpers
{
    /// <summary>
    /// This class provides helper functions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class TestDataExtensions
    {
        /// <summary>
        /// Applies the test config 1.
        /// </summary>
        /// <param name="configProvider">The configuration provider.</param>
        /// <exception cref="System.ArgumentNullException">configProvider</exception>
        internal static void ApplyTestConfig1(this ConfigProviderFake configProvider)
        {
            if (configProvider == null) throw new ArgumentNullException(nameof(configProvider));

            configProvider.Sources.Add(new SourceConfig { SourceId = "DefaultSource", PackageSource = "SomeUrl" });
            configProvider.Sources.Add(new SourceConfig { SourceId = "OtherSource", PackageSource = "SomeOtherUrl" });

            configProvider.ApiKeys.Add(new ApiKeyConfig { ApiKeyId = "DefaultApiKey", ApiKey = "MyApiKey" });
            configProvider.ApiKeys.Add(new ApiKeyConfig { ApiKeyId = "OtherApiKey", ApiKey = "SomeOtherApiKey" });

            configProvider.PackageCleanups.Add(new PackageCleanupConfig { PackageCleanupId = "DefaultCleanup", RetainVersions = 2, Expiry = TimeSpan.FromDays(10), RetainPreviewsOfRegularReleases = false });
            configProvider.PackageCleanups.Add(new PackageCleanupConfig { PackageCleanupId = "OtherCleanup", RetainVersions = 0, Expiry = TimeSpan.Zero, RetainPreviewsOfRegularReleases = false });

            configProvider.PackageGroups.Add(new PackageGroupConfig { SourceId = "DefaultSource", ApiKeyId = "DefaultApiKey", PackageCleanupId = "DefaultCleanup", Owner = "Me", PackageIdPattern = "My.Packages*", PackageIdMatchMode = eCoreStringMatchMode.Like });
        }

        /// <summary>
        /// Applies the test config 1.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="now">The current date/time.</param>
        /// <exception cref="System.ArgumentNullException">factory</exception>
        internal static void ApplyTestConfig1(this NuGetAccessorFactoryFake factory, DateTimeOffset? now = null)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var refDate = now ?? new DateTimeOffset(2000, 2, 1, 0, 0, 0, TimeSpan.Zero);

            var repository = factory.GetRepository("SomeUrl");

            repository.Clear();

            repository.AddApiKey("MyApiKey");

            repository.Add(new PackageMetadata { PackageId = "My.Packages.A", Version = new Version(1, 0, 0, 0), OriginalVersion = "1.0.0.0", Owners = "Me", IsListed = true, PublishDate = refDate - TimeSpan.FromDays(31) });
            repository.Add(new PackageMetadata { PackageId = "My.Packages.A", Version = new Version(1, 1, 0, 0), OriginalVersion = "1.1.0.0-pre1", Owners = "Me", IsListed = true, PublishDate = refDate - TimeSpan.FromDays(22), IsPrerelease = true });
            repository.Add(new PackageMetadata { PackageId = "My.Packages.A", Version = new Version(1, 1, 0, 0), OriginalVersion = "1.1.0.0-pre2", Owners = "Me", IsListed = true, PublishDate = refDate - TimeSpan.FromDays(12), IsPrerelease = true });
            repository.Add(new PackageMetadata { PackageId = "My.Packages.A", Version = new Version(1, 1, 0, 0), OriginalVersion = "1.1.0.0", Owners = "Me", IsListed = true, PublishDate = refDate - TimeSpan.FromDays(11) });
            repository.Add(new PackageMetadata { PackageId = "My.Packages.A", Version = new Version(1, 2, 0, 0), OriginalVersion = "1.2.0.0-pre1", Owners = "Me", IsListed = true, PublishDate = refDate - TimeSpan.FromDays(10), IsPrerelease = true });
            repository.Add(new PackageMetadata { PackageId = "My.Packages.A", Version = new Version(1, 2, 0, 0), OriginalVersion = "1.2.0.0", Owners = "Me", IsListed = true, PublishDate = refDate - TimeSpan.FromDays(2) });
            repository.Add(new PackageMetadata { PackageId = "My.Packages.A", Version = new Version(1, 3, 0, 0), OriginalVersion = "1.3.0.0-pre1", Owners = "Me", IsListed = true, PublishDate = refDate - TimeSpan.FromDays(1), IsPrerelease = true });

            repository.Add(new PackageMetadata { PackageId = "OtherPackage", Version = new Version(0, 0, 0, 0), OriginalVersion = "0.0.0.0", Owners = "Me", IsListed = true, PublishDate = refDate - TimeSpan.FromDays(31) });
        }
    }
}