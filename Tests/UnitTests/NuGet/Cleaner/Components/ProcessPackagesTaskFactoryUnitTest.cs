using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using e2.Framework;
using e2.Framework.Components;
using e2.Framework.Helpers;
using e2.NuGet.Cleaner.Helpers;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcludeFromCodeCoverage = System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the unit test of <see cref="ProcessPackagesTaskFactory" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ProcessPackagesTaskFactoryUnitTest: UnitTestTemplate
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
        [NotNull]
        // ReSharper disable once UnusedParameter.Local
        private ProcessPackagesTaskFactory CreateInstance([CanBeNull] NCObject _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<ProcessPackagesTaskFactory>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="ProcessPackagesTaskFactory" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="ProcessPackagesTaskFactory.GetTask" /> returns an instance.
        /// </summary>
        [TestMethod]
        public void GetTask_01()
        {
            var instance = this.CreateInstance();

            using (var taskToken = instance.GetTask())
            {
                Assert.IsNotNull(taskToken);
                Assert.IsNotNull(taskToken.Instance);
            }
        }

        /// <summary>
        /// Tests that <see cref="ProcessPackagesTaskFactory.GetTask" />.<see cref="IProcessPackagesTask.RunAsync" /> works as expected.
        /// </summary>
        [TestMethod]
        public void GetTask_RunAsync_01()
        {
            var instance = this.CreateInstance();

            var systemTimeProvider = this.Factory.GetInstanceOf<ICoreSystemTimeProvider>();

            var configProvider = this.Factory.GetInstanceOf<ConfigProviderFake>();
            configProvider.ApplyTestConfig1();

            var accessorFactory = this.Factory.GetInstanceOf<NuGetAccessorFactoryFake>();
            accessorFactory.ApplyTestConfig1(systemTimeProvider.GetCurrentDate());

            var repository = accessorFactory.GetRepository("SomeUrl");

            using (instance.GetTask().Deconstruct(out var task))
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
            {
                Assert.IsTrue(task.RunAsync(cts.Token).Wait(TimeSpan.FromSeconds(1.5)), "Timeout");
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