using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using e2.Framework;
using e2.NuGet.Cleaner.Helpers;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using ExcludeFromCodeCoverage = System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the unit test of <see cref="PackageAggregator" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageAggregatorUnitTest: UnitTestTemplate
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
        private PackageAggregator CreateInstance([CanBeNull] NCObject _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<PackageAggregator>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="PackageAggregator" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="PackageAggregator.Aggregate" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Aggregate_01()
        {
            var instance = this.CreateInstance();

            var accessorFactory = this.Factory.GetInstanceOf<NuGetAccessorFactoryFake>();
            accessorFactory.ApplyTestConfig1();
            var packages = accessorFactory.GetRepository("SomeUrl").GetPackages();

            var publishDateDictionary = Mock.Create<IPackagePublishDateDictionary>(Behavior.Strict);
            publishDateDictionary.Arrange(x => x.GetPublishDate(Arg.AnyString, Arg.IsAny<Version>(), Arg.AnyString, Arg.IsAny<DateTimeOffset?>())).Returns<string, Version, string, DateTimeOffset?>((_, _, _, publishDate) => publishDate);

            var aggregatedPackages = new List<IPackageAggregation>();
            instance.Aggregate(packages, publishDateDictionary, aggregatedPackages);

            Assert.AreEqual(2, aggregatedPackages.Count);
            Assert.AreEqual("My.Packages.A", aggregatedPackages[0].PackageId);

            Assert.AreEqual(4, aggregatedPackages[0].Versions.Count);

            Assert.AreEqual(new Version(1, 0, 0, 0), aggregatedPackages[0].Versions[0].Version);
            Assert.AreEqual(0, aggregatedPackages[0].Versions[0].PreviewVersions.Count);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[0].RegularVersions.Count);
            Assert.AreEqual("1.0.0.0", aggregatedPackages[0].Versions[0].RegularVersions[0].OriginalVersion);
            Assert.AreEqual(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), aggregatedPackages[0].Versions[0].RegularVersions[0].PublishDate);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[0].RegularVersions[0].Packages.Count);
            Assert.AreSame(packages.First(x => (x.PackageId == "My.Packages.A") && (x.OriginalVersion == "1.0.0.0")), aggregatedPackages[0].Versions[0].RegularVersions[0].Packages[0]);

            Assert.AreEqual(new Version(1, 1, 0, 0), aggregatedPackages[0].Versions[1].Version);
            Assert.AreEqual(2, aggregatedPackages[0].Versions[1].PreviewVersions.Count);
            Assert.AreEqual("1.1.0.0-pre1", aggregatedPackages[0].Versions[1].PreviewVersions[0].OriginalVersion);
            Assert.AreEqual(new DateTimeOffset(2000, 1, 10, 0, 0, 0, TimeSpan.Zero), aggregatedPackages[0].Versions[1].PreviewVersions[0].PublishDate);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[1].PreviewVersions[0].Packages.Count);
            Assert.AreSame(packages.First(x => (x.PackageId == "My.Packages.A") && (x.OriginalVersion == "1.1.0.0-pre1")), aggregatedPackages[0].Versions[1].PreviewVersions[0].Packages[0]);
            Assert.AreEqual("1.1.0.0-pre2", aggregatedPackages[0].Versions[1].PreviewVersions[1].OriginalVersion);
            Assert.AreEqual(new DateTimeOffset(2000, 1, 20, 0, 0, 0, TimeSpan.Zero), aggregatedPackages[0].Versions[1].PreviewVersions[1].PublishDate);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[1].PreviewVersions[1].Packages.Count);
            Assert.AreSame(packages.First(x => (x.PackageId == "My.Packages.A") && (x.OriginalVersion == "1.1.0.0-pre2")), aggregatedPackages[0].Versions[1].PreviewVersions[1].Packages[0]);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[1].RegularVersions.Count);
            Assert.AreEqual("1.1.0.0", aggregatedPackages[0].Versions[1].RegularVersions[0].OriginalVersion);
            Assert.AreEqual(new DateTimeOffset(2000, 1, 21, 0, 0, 0, TimeSpan.Zero), aggregatedPackages[0].Versions[1].RegularVersions[0].PublishDate);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[1].RegularVersions[0].Packages.Count);
            Assert.AreSame(packages.First(x => (x.PackageId == "My.Packages.A") && (x.OriginalVersion == "1.1.0.0")), aggregatedPackages[0].Versions[1].RegularVersions[0].Packages[0]);

            Assert.AreEqual(new Version(1, 2, 0, 0), aggregatedPackages[0].Versions[2].Version);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[2].PreviewVersions.Count);
            Assert.AreEqual("1.2.0.0-pre1", aggregatedPackages[0].Versions[2].PreviewVersions[0].OriginalVersion);
            Assert.AreEqual(new DateTimeOffset(2000, 1, 22, 0, 0, 0, TimeSpan.Zero), aggregatedPackages[0].Versions[2].PreviewVersions[0].PublishDate);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[2].PreviewVersions[0].Packages.Count);
            Assert.AreSame(packages.First(x => (x.PackageId == "My.Packages.A") && (x.OriginalVersion == "1.2.0.0-pre1")), aggregatedPackages[0].Versions[2].PreviewVersions[0].Packages[0]);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[2].RegularVersions.Count);
            Assert.AreEqual("1.2.0.0", aggregatedPackages[0].Versions[2].RegularVersions[0].OriginalVersion);
            Assert.AreEqual(new DateTimeOffset(2000, 1, 30, 0, 0, 0, TimeSpan.Zero), aggregatedPackages[0].Versions[2].RegularVersions[0].PublishDate);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[2].RegularVersions[0].Packages.Count);
            Assert.AreSame(packages.First(x => (x.PackageId == "My.Packages.A") && (x.OriginalVersion == "1.2.0.0")), aggregatedPackages[0].Versions[2].RegularVersions[0].Packages[0]);

            Assert.AreEqual(new Version(1, 3, 0, 0), aggregatedPackages[0].Versions[3].Version);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[3].PreviewVersions.Count);
            Assert.AreEqual("1.3.0.0-pre1", aggregatedPackages[0].Versions[3].PreviewVersions[0].OriginalVersion);
            Assert.AreEqual(new DateTimeOffset(2000, 1, 31, 0, 0, 0, TimeSpan.Zero), aggregatedPackages[0].Versions[3].PreviewVersions[0].PublishDate);
            Assert.AreEqual(1, aggregatedPackages[0].Versions[3].PreviewVersions[0].Packages.Count);
            Assert.AreSame(packages.First(x => (x.PackageId == "My.Packages.A") && (x.OriginalVersion == "1.3.0.0-pre1")), aggregatedPackages[0].Versions[3].PreviewVersions[0].Packages[0]);
            Assert.AreEqual(0, aggregatedPackages[0].Versions[3].RegularVersions.Count);

            Assert.AreEqual("OtherPackage", aggregatedPackages[1].PackageId);

            Assert.AreEqual(1, aggregatedPackages[1].Versions.Count);

            Assert.AreEqual(new Version(0, 0, 0, 0), aggregatedPackages[1].Versions[0].Version);
            Assert.AreEqual(0, aggregatedPackages[1].Versions[0].PreviewVersions.Count);
            Assert.AreEqual(1, aggregatedPackages[1].Versions[0].RegularVersions.Count);
            Assert.AreEqual("0.0.0.0", aggregatedPackages[1].Versions[0].RegularVersions[0].OriginalVersion);
            Assert.AreEqual(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), aggregatedPackages[1].Versions[0].RegularVersions[0].PublishDate);
            Assert.AreEqual(1, aggregatedPackages[1].Versions[0].RegularVersions[0].Packages.Count);
            Assert.AreSame(packages.First(x => (x.PackageId == "OtherPackage") && (x.OriginalVersion == "0.0.0.0")), aggregatedPackages[1].Versions[0].RegularVersions[0].Packages[0]);
        }
    }
}