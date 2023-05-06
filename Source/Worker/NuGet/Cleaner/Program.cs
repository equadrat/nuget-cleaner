using e2.Framework.Components;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.Components;
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
            var registry = new CoreIOCRegistry();
            var factory = registry.Factory;

            var bootstrapper = factory.CreateInstanceOf<ICoreBootstrapper>();
            bootstrapper.ModuleRegistry.RegisterModule<WorkerBootstrapperModule>();

            using (bootstrapper.Startup(registry))
            {
                var host = Host.CreateDefaultBuilder(args)
                               .UseServiceProviderFactory(registry)
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
                                   })
                               .Build();

                var configProvider = factory.GetInstanceOf<IConfigProvider>();
                using (configProvider.Enable())
                {
                    host.Run();
                }
            }
        }
    }
}