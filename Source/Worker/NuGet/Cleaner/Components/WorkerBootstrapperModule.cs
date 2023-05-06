using e2.Framework.Components;
using e2.Framework.Delegates;
using e2.Framework.Helpers;
using e2.Framework.MemberTemplates;
using e2.NuGet.Cleaner.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using INuGetLogger = NuGet.Common.ILogger;
using NuGetNullLogger = NuGet.Common.NullLogger;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the bootstrapper module of the worker.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public class WorkerBootstrapperModule: CoreBootstrapperModule,
                                           IWorkerBootstrapperModule
    {
        /// <inheritdoc />
        protected override void RegisterDependencyModules(ICoreBootstrapperModuleRegistry moduleRegistry)
        {
            if (moduleRegistry == null) throw new ArgumentNullException(nameof(moduleRegistry));

            moduleRegistry.CoreFramework().RegisterMicrosoftExtensionsLoggingModule()
                          .CoreFramework().RegisterAppSettingsConfigurationModule()
                          .RegisterBusinessLogicModule();

            base.RegisterDependencyModules(moduleRegistry);
        }

        /// <inheritdoc />
        protected override IEnumerable<procBootstrapperRegister> GetRegisterMethods()
        {
            yield return this.RegisterConfigProvider;
            yield return this.RegisterNuGetLogger;
            yield return this.RegisterNuGetAccessorFactory;
        }

        /// <inheritdoc />
        protected override IEnumerable<procBootstrapperInitialize> GetInitializeMethods()
        {
            yield return this.InitLoggingTargets;
        }

        /// <inheritdoc />
        public virtual void RegisterConfigProvider(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<IConfigProvider>()) registry.Register<IConfigProvider>().AsSingletonOf<ConfigProvider>();
        }

        /// <inheritdoc />
        public virtual void RegisterNuGetLogger(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<INuGetLogger>()) registry.Register<INuGetLogger>().AsSingletonOf(NuGetNullLogger.Instance);
        }

        /// <inheritdoc />
        public virtual void RegisterNuGetAccessorFactory(ICoreIOCRegistry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            if (!registry.CanGetInstanceOf<INuGetAccessorFactory>()) registry.Register<INuGetAccessorFactory>().AsSingletonOf<NuGetAccessorFactory>();
        }

        /// <inheritdoc />
        public virtual void InitLoggingTargets(ICoreIOCFactory factory, ICorePushOne<IDisposable> lifetimeObjects)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (lifetimeObjects == null) throw new ArgumentNullException(nameof(lifetimeObjects));

            var loggingContext = factory.GetInstanceOf<ICoreLoggingContext>();
#if DEBUG
            if (!loggingContext.GetAttachedLoggingTargets(CoreLoggingTargetRoles.Trace).Any(x => x is not CoreMicrosoftExtensionsLoggingTarget))
            {
                var loggingTarget = factory.CreateCustomInstanceOf<CoreLoggingTraceLogTarget>();
                lifetimeObjects.Push(loggingContext.Attach(loggingTarget));
            }
#endif

            if (!loggingContext.GetAttachedLoggingTargets(CoreLoggingTargetRoles.File).Any(x => x is not CoreMicrosoftExtensionsLoggingTarget))
            {
                var baseFileName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);

                switch (Path.GetExtension(baseFileName).ToUpperInvariant())
                {
                    case ".EXE":
                    case ".DLL":
                    {
                        baseFileName = Path.GetFileNameWithoutExtension(baseFileName);
                        break;
                    }
                }

                var logFileName = $"{baseFileName}.log";
                var currentDirectory = Environment.CurrentDirectory;
                var logFilePath = Path.Combine(currentDirectory, logFileName);

                var loggingTextFileTarget = CoreLoggingTextFileTarget.Create(factory, logFilePath, maxBackupFiles: 10, maxFileSize: 128 * 1024 * 1024);
                lifetimeObjects.Push(loggingContext.Attach(loggingTextFileTarget));
            }
        }
    }
}