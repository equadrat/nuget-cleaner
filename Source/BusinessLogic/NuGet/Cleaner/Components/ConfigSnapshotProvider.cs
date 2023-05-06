using e2.NuGet.Cleaner.Models;
using System;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a provider for the current configuration snapshot.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ConfigSnapshotProvider: IConfigSnapshotProvider
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The configuration provider.
        /// </summary>
        private readonly IConfigProvider _configProvider;

        /// <summary>
        /// The configuration snapshot factory.
        /// </summary>
        private readonly IConfigSnapshotFactory _configSnapshotFactory;

        /// <summary>
        /// The configuration snapshot.
        /// </summary>
        private volatile IConfigSnapshot? _configSnapshot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSnapshotProvider" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configProvider">The configuration provider.</param>
        /// <param name="configSnapshotFactory">The configuration snapshot factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// logger
        /// or
        /// configProvider
        /// or
        /// configSnapshotFactory
        /// </exception>
        public ConfigSnapshotProvider(ILogger logger, IConfigProvider configProvider, IConfigSnapshotFactory configSnapshotFactory)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (configProvider == null) throw new ArgumentNullException(nameof(configProvider));
            if (configSnapshotFactory == null) throw new ArgumentNullException(nameof(configSnapshotFactory));

            this._logger = logger;
            this._configProvider = configProvider;
            this._configSnapshotFactory = configSnapshotFactory;

            this._configSnapshot = null;

            this._configProvider.Changed += this.ConfigProvider_Changed;
        }

        /// <inheritdoc />
        public IConfigSnapshot GetConfigSnapshot()
        {
            var configSnapshot = this._configSnapshot;
            if (configSnapshot != null) return configSnapshot;

            this._configSnapshot = configSnapshot = this._configSnapshotFactory.CreateSnapshot(this._configProvider);

            this._logger.ProcessPackagesConfigLoaded.IfEnabled?.Log(configSnapshot.GetLoggingOutput());

            return this._configSnapshot;
        }

        /// <summary>
        /// Handles the Changed event of the ConfigProvider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ConfigProvider_Changed(object sender, EventArgs e)
        {
            this._configSnapshot = null;
        }
    }
}