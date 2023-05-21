using e2.Framework.Components;
using e2.Framework.Fluent;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.BootstrapperModules;
using e2.NuGet.Cleaner.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace e2.NuGet.Cleaner
{
    /// <summary>
    /// This class represents the entry point of the application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            var bootstrapper = new CoreBootstrapper();
            bootstrapper.ModuleRegistry.RegisterModule<WorkerBootstrapperModule>();

            ICoreBootstrapperStartupFactorySelector bootstrapperStartup = null!;

            var host = Host.CreateDefaultBuilder(args)
                           .UseWindowsService()
                           .UseSystemd()
                           .ConfigureLogging(
                               logging =>
                               {
                                   logging.ClearProviders().AddConsole();
                               })
                           .ConfigureServices(
                               services =>
                               {
                                   services.UseCoreLogging();
                                   bootstrapperStartup = bootstrapper.Startup().Register(services);
                                   services.AddHostedService(ServiceProviderServiceExtensions.GetRequiredService<IWorkerService>);
                               })
                           .Build();

            using (bootstrapperStartup.Init(host.Services))
            {
                var configProvider = host.Services.GetRequiredService<IConfigProvider>();
                using (configProvider.Enable())
                {
                    host.Run();
                }
            }
        }
    }
}