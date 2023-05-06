using e2.Framework.Components;
using e2.Framework.Helpers;
using e2.Framework.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the worker service.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class WorkerService: IWorkerService
    {
        #region property TimerToken
        /// <summary>
        /// Sets the timer token.
        /// </summary>
        /// <value>
        /// The timer token.
        /// </value>
        private ICoreOwnerToken? TimerToken
        {
            set
            {
                var oldValue = Interlocked.Exchange(ref this._timerToken, value);
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
        /// The backing field of <see cref="TimerToken" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICoreOwnerToken? _timerToken;
        #endregion

        #region property CancellationToken
        /// <summary>
        /// Sets the cancellation token.
        /// </summary>
        /// <value>
        /// The cancellation token.
        /// </value>
        private ICoreOwnerToken? CancellationToken
        {
            set
            {
                var oldValue = Interlocked.Exchange(ref this._cancellationToken, value);
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
        /// The backing field of <see cref="CancellationToken" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICoreOwnerToken? _cancellationToken;
        #endregion

        /// <summary>
        /// The token factory.
        /// </summary>
        private readonly ICoreOwnerTokenFactory _tokenFactory;

        /// <summary>
        /// The task scheduler.
        /// </summary>
        private readonly ICoreTaskScheduler _taskScheduler;

        /// <summary>
        /// The host application lifetime.
        /// </summary>
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        /// <summary>
        /// The configuration provider.
        /// </summary>
        private readonly IConfigProvider _configProvider;

        /// <summary>
        /// The process packages task factory.
        /// </summary>
        private readonly IProcessPackagesTaskFactory _processPackagesTaskFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerService" /> class.
        /// </summary>
        /// <param name="tokenFactory">The token factory.</param>
        /// <param name="taskScheduler">The task scheduler.</param>
        /// <param name="hostApplicationLifetime">The host application lifetime.</param>
        /// <param name="configProvider">The configuration provider.</param>
        /// <param name="processPackagesTaskFactory">The process packages task factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tokenFactory
        /// or
        /// taskScheduler
        /// or
        /// hostApplicationLifetime
        /// or
        /// logger
        /// or
        /// configProvider
        /// or
        /// processPackagesTaskFactory
        /// </exception>
        public WorkerService(ICoreOwnerTokenFactory tokenFactory, ICoreTaskScheduler taskScheduler, IHostApplicationLifetime hostApplicationLifetime, IConfigProvider configProvider, IProcessPackagesTaskFactory processPackagesTaskFactory)
        {
            if (tokenFactory == null) throw new ArgumentNullException(nameof(tokenFactory));
            if (taskScheduler == null) throw new ArgumentNullException(nameof(taskScheduler));
            if (hostApplicationLifetime == null) throw new ArgumentNullException(nameof(hostApplicationLifetime));
            if (configProvider == null) throw new ArgumentNullException(nameof(configProvider));
            if (processPackagesTaskFactory == null) throw new ArgumentNullException(nameof(processPackagesTaskFactory));

            this._tokenFactory = tokenFactory;
            this._taskScheduler = taskScheduler;
            this._hostApplicationLifetime = hostApplicationLifetime;
            this._configProvider = configProvider;
            this._processPackagesTaskFactory = processPackagesTaskFactory;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._configProvider.Changed += this.ConfigProvider_Changed;
            this.SyncTaskSchedulerOperatorMode();

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            this.CancellationToken = this._tokenFactory.CreateRelayToken(cts.Cancel);

            var processInterval = this._configProvider.WorkerProcessInterval;
            var runOnce = !processInterval.HasValue;

            this.TimerToken = this._taskScheduler.Register(
                () => this.WorkAsync(runOnce, token),
                interval: processInterval,
                rescheduleCounter: runOnce ? 0 : null,
                waitOnDispose: true);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this._configProvider.Changed -= this.ConfigProvider_Changed;

            var token = Interlocked.Exchange(ref this._timerToken, null);
            if (token != null) await token.DisposeAsync();

            token = Interlocked.Exchange(ref this._cancellationToken, null);
            if (token != null) await token.DisposeAsync();
        }

        /// <summary>
        /// Works the specified cancellation token.
        /// </summary>
        /// <param name="runOnce"><c>true</c> to run only once; <c>false</c> to run until the worker gets cancelled.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task WorkAsync(bool runOnce, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            try
            {
                await this.ProcessPackagesAsync(cancellationToken);
            }
            finally
            {
                if (runOnce)
                {
#pragma warning disable CS4014
                    // ReSharper disable once MethodSupportsCancellation
                    Task.Run(this._hostApplicationLifetime.StopApplication).ConfigureAwait(false);
#pragma warning restore CS4014
                }
            }
        }

        /// <summary>
        /// Processes the packages asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task ProcessPackagesAsync(CancellationToken cancellationToken)
        {
            using (this._processPackagesTaskFactory.GetTask().Deconstruct(out var task))
            {
                await task.RunAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Handles the Changed event of the ConfigProvider.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ConfigProvider_Changed(object sender, EventArgs e)
        {
            this.SyncTaskSchedulerOperatorMode();
        }

        /// <summary>
        /// Synchronizes the task scheduler operator mode.
        /// </summary>
        private void SyncTaskSchedulerOperatorMode()
        {
            this._taskScheduler.OperatorMode = this._configProvider.TaskSchedulerOperatorMode ?? default;
        }
    }
}