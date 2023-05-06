using e2.Framework.Components;
using e2.Framework.Enums;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a fake implementation of <see cref="IConfigProvider" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ConfigProviderFake: IConfigProvider
    {
        #region event Changed
        /// <inheritdoc />
        public event EventHandler Changed
        {
            add => this._changed.AddHandler(value);
            remove => this._changed.RemoveHandler(value);
        }

        /// <summary>
        /// The backing field of <see cref="Changed" />.
        /// </summary>
        private readonly ICoreEvent _changed;
        #endregion

        /// <inheritdoc />
        public TimeSpan? WorkerProcessInterval {get; set;}

        /// <inheritdoc />
        public eCoreTaskSchedulerOperatorMode? TaskSchedulerOperatorMode {get; set;}

        /// <inheritdoc cref="IConfigProvider.Sources" />
        public List<ISourceConfig> Sources {get;}

        /// <inheritdoc />
        IReadOnlyList<ISourceConfig> IConfigProvider.Sources => this.Sources.AsReadOnly();

        /// <inheritdoc cref="IConfigProvider.ApiKeys" />
        public List<IApiKeyConfig> ApiKeys {get;}

        /// <inheritdoc />
        IReadOnlyList<IApiKeyConfig> IConfigProvider.ApiKeys => this.ApiKeys.AsReadOnly();

        /// <inheritdoc cref="PackageCleanups" />
        public List<IPackageCleanupConfig> PackageCleanups {get;}

        /// <inheritdoc />
        IReadOnlyList<IPackageCleanupConfig> IConfigProvider.PackageCleanups => this.PackageCleanups.AsReadOnly();

        /// <inheritdoc cref="IConfigProvider.PackageGroups" />
        public List<IPackageGroupConfig> PackageGroups {get;}

        /// <inheritdoc />
        IReadOnlyList<IPackageGroupConfig> IConfigProvider.PackageGroups => this.PackageGroups;

        /// <summary>
        /// The token factory.
        /// </summary>
        private readonly ICoreOwnerTokenFactory _tokenFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigProviderFake" /> class.
        /// </summary>
        /// <param name="tokenFactory">The token factory.</param>
        /// <param name="eventFactory">The event factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tokenFactory
        /// or
        /// eventFactory
        /// </exception>
        public ConfigProviderFake(ICoreOwnerTokenFactory tokenFactory, ICoreEventFactory eventFactory)
        {
            if (tokenFactory == null) throw new ArgumentNullException(nameof(tokenFactory));
            if (eventFactory == null) throw new ArgumentNullException(nameof(eventFactory));

            this._changed = eventFactory.CreateEvent();

            this.Sources = new List<ISourceConfig>();
            this.ApiKeys = new List<IApiKeyConfig>();
            this.PackageCleanups = new List<IPackageCleanupConfig>();
            this.PackageGroups = new List<IPackageGroupConfig>();

            this._tokenFactory = tokenFactory;
        }

        /// <inheritdoc />
        public ICoreOwnerToken Enable()
        {
            return this._tokenFactory.CreateDummyToken();
        }

        /// <summary>
        /// Raises the <see cref="Changed" /> event.
        /// </summary>
        internal void OnChanged()
        {
            this._changed.Invoke(this, EventArgs.Empty);
        }
    }
}