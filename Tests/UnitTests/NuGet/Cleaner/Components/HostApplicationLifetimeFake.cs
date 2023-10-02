using e2.Framework.Components;
using e2.Framework.Exceptions;
using e2.Framework.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a fake implementation of <see cref="IHostApplicationLifetime" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class HostApplicationLifetimeFake: IHostApplicationLifetime
    {
        /// <inheritdoc />
        CancellationToken IHostApplicationLifetime.ApplicationStarted => throw new CoreMemberAccessNotSupportedException(this);

        /// <inheritdoc />
        CancellationToken IHostApplicationLifetime.ApplicationStopping => throw new CoreMemberAccessNotSupportedException(this);

        /// <inheritdoc />
        CancellationToken IHostApplicationLifetime.ApplicationStopped => throw new CoreMemberAccessNotSupportedException(this);

        /// <summary>
        /// Gets the application stop request wait handle.
        /// </summary>
        /// <value>
        /// The application stop request wait handle.
        /// </value>
        internal CoreWaitHandle ApplicationStopRequestWaitHandle {get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="HostApplicationLifetimeFake" /> class.
        /// </summary>
        /// <param name="lockFactory">The lock factory.</param>
        /// <exception cref="System.ArgumentNullException">lockFactory</exception>
        public HostApplicationLifetimeFake(ICoreLockFactory lockFactory)
        {
            if (lockFactory == null) throw new ArgumentNullException(nameof(lockFactory));

            this.ApplicationStopRequestWaitHandle = lockFactory.CreateWaitHandle();
        }

        /// <inheritdoc />
        public void StopApplication()
        {
            this.ApplicationStopRequestWaitHandle.Set();
        }
    }
}