using e2.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the unit test of <see cref="PackageCleanupConfig" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageCleanupConfigUnitTest: UnitTestTemplate
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
        private PackageCleanupConfig CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<PackageCleanupConfig>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="PackageCleanupConfig" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupConfig.PackageCleanupId" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void PackageCleanupId_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.PackageCleanupId);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupConfig.PackageCleanupId" /> works as expected.
        /// </summary>
        [TestMethod]
        public void PackageCleanupId_02()
        {
            var instance = this.CreateInstance();

            instance.PackageCleanupId = "MyPackageCleanupId";
            Assert.AreEqual("MyPackageCleanupId", instance.PackageCleanupId);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupConfig.RetainVersions" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void RetainVersions_01()
        {
            var instance = this.CreateInstance();
            Assert.AreEqual(0, instance.RetainVersions);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupConfig.RetainVersions" /> works as expected.
        /// </summary>
        [TestMethod]
        public void RetainVersions_02()
        {
            var instance = this.CreateInstance();

            instance.RetainVersions = 5;
            Assert.AreEqual(5, instance.RetainVersions);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupConfig.Expiry" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void Expiry_01()
        {
            var instance = this.CreateInstance();
            Assert.AreEqual(TimeSpan.Zero, instance.Expiry);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupConfig.Expiry" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Expiry_02()
        {
            var instance = this.CreateInstance();

            instance.Expiry = TimeSpan.FromDays(30);
            Assert.AreEqual(TimeSpan.FromDays(30), instance.Expiry);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupConfig.RetainPreviewsOfRegularReleases" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void RetainPreviewsOfRegularReleases_01()
        {
            var instance = this.CreateInstance();
            Assert.IsFalse(instance.RetainPreviewsOfRegularReleases);
        }

        /// <summary>
        /// Tests that <see cref="PackageCleanupConfig.RetainPreviewsOfRegularReleases" /> works as expected.
        /// </summary>
        [TestMethod]
        public void RetainPreviewsOfRegularReleases_02()
        {
            var instance = this.CreateInstance();

            instance.RetainPreviewsOfRegularReleases = true;
            Assert.IsTrue(instance.RetainPreviewsOfRegularReleases);
        }
    }
}