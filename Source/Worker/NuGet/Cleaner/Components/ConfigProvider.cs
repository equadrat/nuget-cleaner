using e2.Framework.Components;
using e2.Framework.Enums;
using e2.Framework.Helpers;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the config provider.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ConfigProvider: IConfigProvider
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

        #region property EnableCounter
        /// <summary>
        /// Gets or sets the enable counter.
        /// </summary>
        /// <value>
        /// The enable counter.
        /// </value>
        private int EnableCounter
        {
            get => this._enableCounter;
            set
            {
                var oldValue = this._enableCounter;
                if (oldValue == value) return;

                this._enableCounter = value;
                this.OnEnableCounterChanged(value);
            }
        }

        /// <summary>
        /// The backing field of <see cref="EnableCounter" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _enableCounter;
        #endregion

        #region property IsEnabled
        /// <summary>
        /// Sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        private bool IsEnabled
        {
            set
            {
                var oldValue = this._isEnabled;
                if (oldValue == value) return;

                this._isEnabled = value;
                this.OnIsEnabledChanged(value);
            }
        }

        /// <summary>
        /// The backing field of <see cref="IsEnabled" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isEnabled;
        #endregion

        #region property ChangeCallbackToken
        /// <summary>
        /// Sets the change callback token.
        /// </summary>
        /// <value>
        /// The change callback token.
        /// </value>
        private IDisposable? ChangeCallbackToken
        {
            set
            {
                var oldValue = Interlocked.Exchange(ref this._changeCallbackToken, value);
                if (oldValue == null) return;

                try
                {
                    oldValue.Dispose();
                }
                catch
                {
                    // Ignored.
                }
            }
        }

        /// <summary>
        /// The backing field of <see cref="ChangeCallbackToken" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDisposable? _changeCallbackToken;
        #endregion

        /// <inheritdoc />
        public TimeSpan? WorkerProcessInterval => this._configuration.GetSection("Worker").GetValue<TimeSpan?>("ProcessInterval");

        /// <inheritdoc />
        public eCoreTaskSchedulerOperatorMode? TaskSchedulerOperatorMode => this._configuration.GetSection("TaskScheduler").GetValue<eCoreTaskSchedulerOperatorMode?>("OperatorMode");

        /// <inheritdoc />
        public IReadOnlyList<ISourceConfig> Sources => this._configuration.GetSection("Sources").GetChildren().Select(x => x.Get<SourceConfig>()!).AsReadOnly();

        /// <inheritdoc />
        public IReadOnlyList<IApiKeyConfig> ApiKeys => this._configuration.GetSection("ApiKeys").GetChildren().Select(x => x.Get<ApiKeyConfig>()!).AsReadOnly();

        /// <inheritdoc />
        public IReadOnlyList<IPackageCleanupConfig> PackageCleanups => this._configuration.GetSection("PackageCleanups").GetChildren().Select(x => x.Get<PackageCleanupConfig>()!).AsReadOnly();

        /// <inheritdoc />
        public IReadOnlyList<IPackageGroupConfig> PackageGroups => this._configuration.GetSection("PackageGroups").GetChildren().Select(x => x.Get<PackageGroupConfig>()!).AsReadOnly();

        /// <summary>
        /// The token factory.
        /// </summary>
        private readonly ICoreOwnerTokenFactory _tokenFactory;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The lock.
        /// </summary>
        private readonly CoreExclusiveLockSlim _lock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigProvider" /> class.
        /// </summary>
        /// <param name="tokenFactory">The token factory.</param>
        /// <param name="eventFactory">The event factory.</param>
        /// <param name="lockFactory">The lock factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tokenFactory
        /// or
        /// eventFactory
        /// or
        /// lockFactory
        /// or
        /// configuration
        /// </exception>
        public ConfigProvider(ICoreOwnerTokenFactory tokenFactory, ICoreEventFactory eventFactory, ICoreLockFactory lockFactory, IConfiguration configuration)
        {
            if (tokenFactory == null) throw new ArgumentNullException(nameof(tokenFactory));
            if (eventFactory == null) throw new ArgumentNullException(nameof(eventFactory));
            if (lockFactory == null) throw new ArgumentNullException(nameof(lockFactory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            this._changed = eventFactory.CreateEvent();

            this._tokenFactory = tokenFactory;
            this._configuration = configuration;
            this._lock = lockFactory.CreateExclusiveLock();
        }

        /// <summary>
        /// Raises the <see cref="Changed" /> event.
        /// </summary>
        private void OnChanged()
        {
            this._changed.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when <see cref="EnableCounter" /> changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        private void OnEnableCounterChanged(int newValue)
        {
            this.IsEnabled = newValue != 0;
        }

        /// <summary>
        /// Called when <see cref="IsEnabled" /> changed.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        private void OnIsEnabledChanged(bool newValue)
        {
            this.ChangeCallbackToken = newValue
                ? this._configuration.GetReloadToken().RegisterChangeCallback(this.Configuration_Changed, null)
                : null;
        }

        /// <summary>
        /// Handles the changed event of the Configuration.
        /// </summary>
        /// <param name="state">The state.</param>
        private void Configuration_Changed(object? state)
        {
            this.OnChanged();
        }

        /// <inheritdoc />
        public ICoreOwnerToken Enable()
        {
            this._lock.EnterLock();

            try
            {
                this.EnableCounter++;
            }
            finally
            {
                this._lock.ExitLock();
            }

            return this._tokenFactory.CreateRelayToken(this.Disable);
        }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        private void Disable()
        {
            this._lock.EnterLock();

            try
            {
                this.EnableCounter--;
            }
            finally
            {
                this._lock.ExitLock();
            }
        }
    }
}