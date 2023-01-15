using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents a snapshots of the configuration.
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal sealed class ConfigSnapshot: IConfigSnapshot
    {
        /// <summary>
        /// Formats a text whether the instance is active or inactive.
        /// </summary>
        /// <param name="isActive"><c>true</c> if the instance is active.</param>
        /// <returns>
        /// The text.
        /// </returns>
        [Pure]
        [NotNull]
        private static string FormatActive(bool isActive)
        {
            return isActive ? "active" : "inactive";
        }

        /// <summary>
        /// Writes the items.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="writer">The writer.</param>
        /// <param name="items">The items.</param>
        /// <param name="activeItems">The active items.</param>
        /// <param name="textSelector">The text selector.</param>
        /// <param name="title">The title.</param>
        /// <param name="header">The header.</param>
        private static void WriteItems<TKey, TItem>([NotNull] IndentedTextWriter writer, [NotNull] IReadOnlyDictionary<TKey, TItem> items, [NotNull] ICollection<TItem> activeItems, Func<TItem, string> textSelector, [NotNull] string title, [NotNull] string header)
            where TItem: class
        {
            writer.WriteLine($"{title}:");

            writer.Indent++;

            if (items.Count == 0)
            {
                writer.WriteLine("none");
            }
            else
            {
                foreach (var item in items.Values.OrderBy(textSelector))
                {
                    writer.WriteLine($"{header}: \"{textSelector.Invoke(item)}\" ({FormatActive(activeItems.Contains(item))})");
                }
            }

            writer.Indent--;
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, ISourceConfig> Sources {get;}

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IApiKeyConfig> ApiKeys {get;}

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IPackageCleanupConfig> PackageCleanups {get;}

        /// <inheritdoc />
        public IReadOnlyList<IPackageSourceSnapshot> PackageSources {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSnapshot" /> class.
        /// </summary>
        /// <param name="sources">The sources.</param>
        /// <param name="apiKeys">The API keys.</param>
        /// <param name="packageCleanups">The package cleanups.</param>
        /// <param name="packageSources">The package sources.</param>
        /// <exception cref="System.ArgumentNullException">
        /// sources
        /// or
        /// apiKeys
        /// or
        /// packageCleanups
        /// or
        /// packageSources
        /// </exception>
        internal ConfigSnapshot([NotNull] IReadOnlyDictionary<string, ISourceConfig> sources, [NotNull] IReadOnlyDictionary<string, IApiKeyConfig> apiKeys, [NotNull] IReadOnlyDictionary<string, IPackageCleanupConfig> packageCleanups, [NotNull] IReadOnlyList<IPackageSourceSnapshot> packageSources)
        {
#if DEBUG
            if (sources == null) throw new ArgumentNullException(nameof(sources));
            if (apiKeys == null) throw new ArgumentNullException(nameof(apiKeys));
            if (packageCleanups == null) throw new ArgumentNullException(nameof(packageCleanups));
            if (packageSources == null) throw new ArgumentNullException(nameof(packageSources));
#endif
            this.Sources = sources;
            this.ApiKeys = apiKeys;
            this.PackageCleanups = packageCleanups;
            this.PackageSources = packageSources;
        }

        /// <inheritdoc />
        public string GetLoggingOutput()
        {
            var activeSources = new HashSet<ISourceConfig>();
            var activeApiKeys = new HashSet<IApiKeyConfig>();
            var activePackageCleanups = new HashSet<IPackageCleanupConfig>();

            const string tabString = "  ";
            var sbPackageGroups = new StringBuilder();

            using (var writer = new IndentedTextWriter(new StringWriter(sbPackageGroups), tabString))
            {
                this.WritePackageGroups(writer, activeSources, activeApiKeys, activePackageCleanups);
            }

            var sbResult = new StringBuilder();

            using (var writer = new IndentedTextWriter(new StringWriter(sbResult), tabString))
            {
                this.WriteSources(writer, activeSources);
                this.WriteApiKeys(writer, activeApiKeys);
                this.WritePackageCleanups(writer, activePackageCleanups);
            }

            sbResult.Append(sbPackageGroups);

            return sbResult.ToString();
        }

        /// <summary>
        /// Writes the package groups.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="activeSources">The active sources.</param>
        /// <param name="activeApiKeys">The active API keys.</param>
        /// <param name="activePackageCleanups">The active package cleanups.</param>
        private void WritePackageGroups([NotNull] IndentedTextWriter writer, [NotNull] HashSet<ISourceConfig> activeSources, [NotNull] HashSet<IApiKeyConfig> activeApiKeys, [NotNull] HashSet<IPackageCleanupConfig> activePackageCleanups)
        {
            writer.WriteLine("PackageGroups:");

            writer.Indent++;

            if (this.PackageSources.Count == 0)
            {
                writer.Write("none");
            }
            else
            {
                var isFirst = true;

                foreach (var packageSource in this.PackageSources)
                {
                    activeSources.Add(packageSource.Source);
                    activeApiKeys.Add(packageSource.ApiKey);

                    foreach (var owner in packageSource.Owners)
                    {
                        foreach (var packageGroup in owner.PackageGroups)
                        {
                            if (isFirst) isFirst = false;
                            else writer.WriteLine();

                            activePackageCleanups.Add(packageGroup.PackageCleanup);

                            writer.WriteLine($"PackageIdPattern: \"{packageGroup.PackageGroup.PackageIdPattern}\"");

                            writer.Indent++;

                            writer.WriteLine($"PackageIdMatchMode: {packageGroup.PackageGroup.PackageIdMatchMode}");
                            writer.WriteLine($"Owner: \"{owner.Owner}\"");
                            writer.WriteLine($"SourceId: \"{packageSource.Source.SourceId}\"");
                            writer.WriteLine($"ApiKeyId: \"{packageSource.ApiKey.ApiKeyId}\"");
                            writer.Write($"PackageCleanupId: \"{packageGroup.PackageCleanup.PackageCleanupId}\"");

                            writer.Indent--;
                        }
                    }
                }
            }

            writer.Indent--;
        }

        /// <summary>
        /// Writes the sources.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="activeSources">The active sources.</param>
        private void WriteSources([NotNull] IndentedTextWriter writer, [NotNull] HashSet<ISourceConfig> activeSources)
        {
            WriteItems(writer, this.Sources, activeSources, x => x.SourceId, "Sources", "SourceId");
        }

        /// <summary>
        /// Writes the API keys.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="activeApiKeys">The active API keys.</param>
        private void WriteApiKeys([NotNull] IndentedTextWriter writer, [NotNull] HashSet<IApiKeyConfig> activeApiKeys)
        {
            WriteItems(writer, this.ApiKeys, activeApiKeys, x => x.ApiKeyId, "ApiKeys", "ApiKeyId");
        }

        /// <summary>
        /// Writes the package cleanups.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="activePackageCleanups">The active package cleanups.</param>
        private void WritePackageCleanups([NotNull] IndentedTextWriter writer, [NotNull] HashSet<IPackageCleanupConfig> activePackageCleanups)
        {
            WriteItems(writer, this.PackageCleanups, activePackageCleanups, x => x.PackageCleanupId, "PackageCleanups", "PackageCleanupId");
        }
    }
}