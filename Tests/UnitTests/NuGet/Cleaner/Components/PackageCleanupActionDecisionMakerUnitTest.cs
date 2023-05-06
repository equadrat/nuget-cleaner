using e2.Framework;
using e2.NuGet.Cleaner.Helpers;
using e2.NuGet.Cleaner.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the unit test of <see cref="PackageCleanupActionDecisionMaker" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageCleanupActionDecisionMakerUnitTest: UnitTestTemplate
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
        private PackageCleanupActionDecisionMaker CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<PackageCleanupActionDecisionMaker>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="PackageCleanupActionDecisionMaker" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupActionDecisionMaker.GetUnlistVersion" /> returns the expected results when expiring by time only.
        /// </summary>
        [TestMethod]
        public void GetUnlistVersion_01()
        {
            var instance = this.CreateInstance();

            var accessorFactoryFake = this.Factory.GetInstanceOf<NuGetAccessorFactoryFake>();
            accessorFactoryFake.ApplyTestConfig1();

            var aggregator = this.Factory.GetInstanceOf<IPackageAggregator>();

            var repository = accessorFactoryFake.GetRepository("SomeUrl");

            var packagePublishDateDictionary = Mock.Create<IPackagePublishDateDictionary>(Behavior.Strict);
            packagePublishDateDictionary.Arrange(x => x.GetPublishDate(Arg.AnyString, Arg.IsAny<Version>(), Arg.AnyString, Arg.IsAny<DateTimeOffset?>())).Returns<string, Version, string, DateTimeOffset?>((_, _, _, publishDate) => publishDate);

            var packageAggregations = new List<IPackageAggregation>();
            aggregator.Aggregate(repository.GetPackages().Where(x => x.PackageId == "My.Packages.A"), packagePublishDateDictionary, packageAggregations);
            var packageAggregation = packageAggregations.Single();

            var cleanupConfig = new PackageCleanupConfig { RetainVersions = 0, Expiry = TimeSpan.FromDays(11) };

            var expectedResults = new[]
            {
                true,  // 1.0.0.0
                true,  // 1.1.0.0-pre1
                true,  // 1.1.0.0-pre2
                true,  // 1.1.0.0
                true,  // 1.2.0.0-pre1
                false, // 1.2.0.0
                false  // 1.3.0.0-pre1
            };

            var actualResults = packageAggregation.GetAddresses(includeRegularVersions: true, includePreviewVersions: true)
                                                  .Select(x => instance.GetUnlistVersion(packageAggregation, x, cleanupConfig, new DateTimeOffset(2000, 2, 1, 0, 0, 0, TimeSpan.Zero)))
                                                  .ToList();

            CollectionAssert.AreEqual(expectedResults, actualResults);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupActionDecisionMaker.GetUnlistVersion" /> returns the expected results when expiring by retain count only.
        /// </summary>
        [TestMethod]
        public void GetUnlistVersion_02()
        {
            var instance = this.CreateInstance();

            var accessorFactoryFake = this.Factory.GetInstanceOf<NuGetAccessorFactoryFake>();
            accessorFactoryFake.ApplyTestConfig1();

            var aggregator = this.Factory.GetInstanceOf<IPackageAggregator>();

            var repository = accessorFactoryFake.GetRepository("SomeUrl");

            var packagePublishDateDictionary = Mock.Create<IPackagePublishDateDictionary>(Behavior.Strict);
            packagePublishDateDictionary.Arrange(x => x.GetPublishDate(Arg.AnyString, Arg.IsAny<Version>(), Arg.AnyString, Arg.IsAny<DateTimeOffset?>())).Returns<string, Version, string, DateTimeOffset?>((_, _, _, publishDate) => publishDate);

            var packageAggregations = new List<IPackageAggregation>();
            aggregator.Aggregate(repository.GetPackages().Where(x => x.PackageId == "My.Packages.A"), packagePublishDateDictionary, packageAggregations);
            var packageAggregation = packageAggregations.Single();

            var cleanupConfig = new PackageCleanupConfig { RetainVersions = 2, Expiry = TimeSpan.Zero };

            var expectedResults = new[]
            {
                true,  // 1.0.0.0
                true,  // 1.1.0.0-pre1
                true,  // 1.1.0.0-pre2
                false, // 1.1.0.0
                true,  // 1.2.0.0-pre1
                false, // 1.2.0.0
                false  // 1.3.0.0-pre1
            };

            var actualResults = packageAggregation.GetAddresses(includeRegularVersions: true, includePreviewVersions: true)
                                                  .Select(x => instance.GetUnlistVersion(packageAggregation, x, cleanupConfig, new DateTimeOffset(2000, 2, 1, 0, 0, 0, TimeSpan.Zero)))
                                                  .ToList();

            CollectionAssert.AreEqual(expectedResults, actualResults);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupActionDecisionMaker.GetUnlistVersion" /> returns the expected results when expiring by time only and retaining the preview version.
        /// </summary>
        [TestMethod]
        public void GetUnlistVersion_03()
        {
            var instance = this.CreateInstance();

            var accessorFactoryFake = this.Factory.GetInstanceOf<NuGetAccessorFactoryFake>();
            accessorFactoryFake.ApplyTestConfig1();

            var aggregator = this.Factory.GetInstanceOf<IPackageAggregator>();

            var repository = accessorFactoryFake.GetRepository("SomeUrl");

            var packagePublishDateDictionary = Mock.Create<IPackagePublishDateDictionary>(Behavior.Strict);
            packagePublishDateDictionary.Arrange(x => x.GetPublishDate(Arg.AnyString, Arg.IsAny<Version>(), Arg.AnyString, Arg.IsAny<DateTimeOffset?>())).Returns<string, Version, string, DateTimeOffset?>((_, _, _, publishDate) => publishDate);

            var packageAggregations = new List<IPackageAggregation>();
            aggregator.Aggregate(repository.GetPackages().Where(x => (x.PackageId == "My.Packages.A")), packagePublishDateDictionary, packageAggregations);
            var packageAggregation = packageAggregations.Single();

            var cleanupConfig = new PackageCleanupConfig { RetainVersions = 0, Expiry = TimeSpan.FromDays(11), RetainPreviewsOfRegularReleases = true };

            var expectedResults = new[]
            {
                true,  // 1.0.0.0
                true,  // 1.1.0.0-pre1
                true,  // 1.1.0.0-pre2
                true,  // 1.1.0.0
                false, // 1.2.0.0-pre1
                false, // 1.2.0.0
                false  // 1.3.0.0-pre1
            };

            var actualResults = packageAggregation.GetAddresses(includeRegularVersions: true, includePreviewVersions: true)
                                                  .Select(x => instance.GetUnlistVersion(packageAggregation, x, cleanupConfig, new DateTimeOffset(2000, 2, 1, 0, 0, 0, TimeSpan.Zero)))
                                                  .ToList();

            CollectionAssert.AreEqual(expectedResults, actualResults);
        }
    }
}