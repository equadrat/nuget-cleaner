using e2.Framework.Components;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a factory to create snapshots of the configuration.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ConfigSnapshotFactory: IConfigSnapshotFactory
    {
        /// <summary>
        /// The string pattern predicate factory.
        /// </summary>
        [NotNull]
        private readonly ICoreStringPatternPredicateFactory _stringPatternPredicateFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSnapshotFactory" /> class.
        /// </summary>
        /// <param name="stringPatternPredicateFactory">The string pattern predicate factory.</param>
        /// <exception cref="System.ArgumentNullException">stringPatternPredicateFactory</exception>
        public ConfigSnapshotFactory([NotNull] ICoreStringPatternPredicateFactory stringPatternPredicateFactory)
        {
            if (stringPatternPredicateFactory == null) throw new ArgumentNullException(nameof(stringPatternPredicateFactory));

            this._stringPatternPredicateFactory = stringPatternPredicateFactory;
        }

        /// <inheritdoc />
        public IConfigSnapshot CreateSnapshot(IConfigProvider configProvider)
        {
            if (configProvider == null) throw new ArgumentNullException(nameof(configProvider));

            var sources = configProvider.Sources
                                        .Where(x => !string.IsNullOrWhiteSpace(x.SourceId))
                                        .GroupBy(x => x.SourceId, PackageMetadataComparer.DefaultStringComparer)
                                        .ToDictionary(x => x.Key, x => x.First(), PackageMetadataComparer.DefaultStringComparer)
                                        .AsReadOnly();

            var apiKeys = configProvider.ApiKeys
                                        .Where(x => !string.IsNullOrWhiteSpace(x.ApiKeyId))
                                        .GroupBy(x => x.ApiKeyId, PackageMetadataComparer.DefaultStringComparer)
                                        .ToDictionary(x => x.Key, x => x.First(), PackageMetadataComparer.DefaultStringComparer)
                                        .AsReadOnly();

            var packageCleanups = configProvider.PackageCleanups
                                                .Where(x => !string.IsNullOrWhiteSpace(x.PackageCleanupId))
                                                .GroupBy(x => x.PackageCleanupId, PackageMetadataComparer.DefaultStringComparer)
                                                .ToDictionary(x => x.Key, x => x.First(), PackageMetadataComparer.DefaultStringComparer)
                                                .AsReadOnly();

            var packageGroups = configProvider.PackageGroups
                                              .Where(x => !string.IsNullOrWhiteSpace(x.SourceId) && !string.IsNullOrWhiteSpace(x.ApiKeyId) && !string.IsNullOrWhiteSpace(x.PackageCleanupId))
                                              .Select(
                                                  (x, i) =>
                                                  {
                                                      sources.TryGetValue(x.SourceId, out var source);
                                                      apiKeys.TryGetValue(x.ApiKeyId, out var apiKey);
                                                      packageCleanups.TryGetValue(x.PackageCleanupId, out var packageCleanup);
                                                      return (Index: i, PackageGroup: x, Source: source, ApiKey: apiKey, PackageCleanup: packageCleanup);
                                                  })
                                              // ReSharper disable MergeIntoPattern
                                              .Where(x => (x.Source != null) && (x.ApiKey != null) && (x.PackageCleanup != null))
                                              // ReSharper restore MergeIntoPattern
                                              .GroupBy(x => (x.Source, x.ApiKey, x.PackageGroup.Owner), x => (x.PackageGroup, x.Index, PackageIdPattern: this._stringPatternPredicateFactory.Create(x.PackageGroup.PackageIdPattern, x.PackageGroup.PackageIdMatchMode), x.PackageCleanup))
                                              .Select(x => (x.Key.Source, x.Key.ApiKey, x.Key.Owner, PackageGroups: x.OrderBy(y => y.Index).Select(y => new PackageGroupSnapshot(y.PackageIdPattern, y.PackageGroup, y.PackageCleanup)).AsReadOnly()))
                                              .AsReadOnly()
                                              .GroupBy(x => (x.Source, x.ApiKey), x => new PackageOwnerSnapshot(x.Owner, x.PackageGroups))
                                              .Select(x => new PackageSourceSnapshot(x.Key.Source, x.Key.ApiKey, x.AsReadOnly()))
                                              .AsReadOnly();

            return new ConfigSnapshot(sources, apiKeys, packageCleanups, packageGroups);
        }
    }
}