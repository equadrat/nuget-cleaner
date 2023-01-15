using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes the worker service.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IWorkerService: IHostedService
    {
    }
}