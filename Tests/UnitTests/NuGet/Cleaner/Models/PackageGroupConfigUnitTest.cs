using e2.Framework;
using e2.Framework.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the unit test of <see cref="PackageGroupConfig" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageGroupConfigUnitTest: UnitTestTemplate
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
        private PackageGroupConfig CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<PackageGroupConfig>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="PackageGroupConfig" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.SourceId" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void SourceId_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.SourceId);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.SourceId" /> works as expected.
        /// </summary>
        [TestMethod]
        public void SourceId_02()
        {
            var instance = this.CreateInstance();

            instance.SourceId = "MySourceId";
            Assert.AreEqual("MySourceId", instance.SourceId);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.ApiKeyId" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void ApiKeyId_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.ApiKeyId);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.ApiKeyId" /> works as expected.
        /// </summary>
        [TestMethod]
        public void ApiKeyId_02()
        {
            var instance = this.CreateInstance();

            instance.ApiKeyId = "MyApiKeyId";
            Assert.AreEqual("MyApiKeyId", instance.ApiKeyId);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.Owner" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void Owner_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.Owner);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.Owner" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Owner_02()
        {
            var instance = this.CreateInstance();

            instance.Owner = "MyOwner";
            Assert.AreEqual("MyOwner", instance.Owner);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.PackageIdPattern" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void PackageIdPattern_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.PackageIdPattern);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.PackageIdPattern" /> works as expected.
        /// </summary>
        [TestMethod]
        public void PackageIdPattern_02()
        {
            var instance = this.CreateInstance();

            instance.PackageIdPattern = "MyPackageIdPattern";
            Assert.AreEqual("MyPackageIdPattern", instance.PackageIdPattern);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.PackageIdMatchMode" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void PackageIdMatchMode_01()
        {
            var instance = this.CreateInstance();
            Assert.AreEqual(default, instance.PackageIdMatchMode);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.PackageIdMatchMode" /> works as expected.
        /// </summary>
        [TestMethod]
        public void PackageIdMatchMode_02()
        {
            var instance = this.CreateInstance();

            instance.PackageIdMatchMode = eCoreStringMatchMode.Like;
            Assert.AreEqual(eCoreStringMatchMode.Like, instance.PackageIdMatchMode);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.PackageCleanupId" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void PackageCleanupId_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.PackageCleanupId);
        }

        /// <summary>
        /// Tests that <see cref="PackageGroupConfig.PackageCleanupId" /> works as expected.
        /// </summary>
        [TestMethod]
        public void PackageCleanupId_02()
        {
            var instance = this.CreateInstance();

            instance.PackageCleanupId = "MyPackageCleanupId";
            Assert.AreEqual("MyPackageCleanupId", instance.PackageCleanupId);
        }
    }
}