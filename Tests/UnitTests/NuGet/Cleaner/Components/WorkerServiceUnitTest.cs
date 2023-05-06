using e2.Framework;
using e2.Framework.Components;
using e2.NuGet.Cleaner.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the unit test of <see cref="WorkerService" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class WorkerServiceUnitTest: UnitTestTemplate
    {
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="_">unused</param>
        /// <param name="inconclusive"><c>true</c> to let the test fail as inconclusive.</param>
        /// <returns>
        /// The instance.
        /// </returns>
        [Pure]
        // ReSharper disable once UnusedParameter.Local
        private WorkerService CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<WorkerService>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="WorkerService" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="WorkerService" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Execute_01()
        {
            var instance = this.CreateInstance();

            var hostApplicationLifetime = this.Factory.GetInstanceOf<HostApplicationLifetimeFake>();
            var systemTimeProvider = this.Factory.GetInstanceOf<ICoreSystemTimeProvider>();

            var configProvider = this.Factory.GetInstanceOf<ConfigProviderFake>();
            configProvider.ApplyTestConfig1();

            var accessorFactory = this.Factory.GetInstanceOf<NuGetAccessorFactoryFake>();
            accessorFactory.ApplyTestConfig1(systemTimeProvider.GetCurrentDate());

            var repository = accessorFactory.GetRepository("SomeUrl");

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
            {
                Assert.IsTrue(instance.StartAsync(cts.Token).Wait(TimeSpan.FromSeconds(1.5)), "Timeout (start)");
            }

            Assert.IsTrue(hostApplicationLifetime.ApplicationStopRequestWaitHandle.Wait(TimeSpan.FromSeconds(2)), "Timeout (execute)");

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
            {
                Assert.IsTrue(instance.StopAsync(cts.Token).Wait(TimeSpan.FromSeconds(1.5)), "Timeout (stop)");
            }

            var expectedPackages = new[]
            {
                ("My.Packages.A", "1.0.0.0", false),
                ("My.Packages.A", "1.1.0.0-pre1", false),
                ("My.Packages.A", "1.1.0.0-pre2", false),
                ("My.Packages.A", "1.1.0.0", true),
                ("My.Packages.A", "1.2.0.0-pre1", false),
                ("My.Packages.A", "1.2.0.0", true),
                ("My.Packages.A", "1.3.0.0-pre1", true),
                ("OtherPackage", "0.0.0.0", true),
            };

            var actualPackages = repository.GetPackages()
                                           .Select(x => (x.PackageId, x.OriginalVersion, x.IsListed))
                                           .ToArray();

            CollectionAssert.AreEquivalent(expectedPackages, actualPackages);
        }
    }
}