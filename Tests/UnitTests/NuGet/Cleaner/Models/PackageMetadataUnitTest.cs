using System;
using System.Diagnostics;
using e2.Framework;
using e2.NuGet.Cleaner.Components;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcludeFromCodeCoverage = System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the unit test of <see cref="PackageMetadata" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageMetadataUnitTest: UnitTestTemplate
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
        private PackageMetadata CreateInstance([CanBeNull] NCObject _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<PackageMetadata>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="PackageMetadata" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.PackageId" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void PackageId_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.PackageId);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.PackageId" /> works as expected.
        /// </summary>
        [TestMethod]
        public void PackageId_02()
        {
            var instance = this.CreateInstance();

            instance.PackageId = "MyPackageId";
            Assert.AreEqual("MyPackageId", instance.PackageId);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.Owners" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void Owners_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.Owners);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.Owners" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Owners_02()
        {
            var instance = this.CreateInstance();

            instance.Owners = "MyOwners";
            Assert.AreEqual("MyOwners", instance.Owners);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.IsListed" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void IsListed_01()
        {
            var instance = this.CreateInstance();
            Assert.IsFalse(instance.IsListed);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.IsListed" /> works as expected.
        /// </summary>
        [TestMethod]
        public void IsListed_02()
        {
            var instance = this.CreateInstance();

            instance.IsListed = true;
            Assert.IsTrue(instance.IsListed);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.IsDeprecated" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void IsDeprecated_01()
        {
            var instance = this.CreateInstance();
            Assert.IsFalse(instance.IsDeprecated);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.IsDeprecated" /> works as expected.
        /// </summary>
        [TestMethod]
        public void IsDeprecated_02()
        {
            var instance = this.CreateInstance();

            instance.IsDeprecated = true;
            Assert.IsTrue(instance.IsDeprecated);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.PublishDate" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void PublishDate_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.PublishDate);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.PublishDate" /> works as expected.
        /// </summary>
        [TestMethod]
        public void PublishDate_02()
        {
            var instance = this.CreateInstance();

            var expectedValue = DateTimeOffset.UtcNow;
            instance.PublishDate = expectedValue;
            Assert.AreEqual(expectedValue, instance.PublishDate);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.Version" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void Version_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.Version);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.Version" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Version_02()
        {
            var instance = this.CreateInstance();

            instance.Version = new Version(1, 2, 3, 4);
            Assert.AreEqual(new Version(1, 2, 3, 4), instance.Version);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.OriginalVersion" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void OriginalVersion_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.OriginalVersion);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.OriginalVersion" /> works as expected.
        /// </summary>
        [TestMethod]
        public void OriginalVersion_02()
        {
            var instance = this.CreateInstance();

            instance.OriginalVersion = "1.2.3.4-beta";
            Assert.AreEqual("1.2.3.4-beta", instance.OriginalVersion);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.IsPrerelease" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void IsPrerelease_01()
        {
            var instance = this.CreateInstance();
            Assert.IsFalse(instance.IsPrerelease);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.IsPrerelease" /> works as expected.
        /// </summary>
        [TestMethod]
        public void IsPrerelease_02()
        {
            var instance = this.CreateInstance();

            instance.IsPrerelease = true;
            Assert.IsTrue(instance.IsPrerelease);
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadata.Clone" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Clone_01()
        {
            var instance = this.CreateInstance();
            var otherInstance = instance.Clone();
            Assert.AreNotSame(instance, otherInstance);
            Assert.AreEqual(instance, otherInstance, PackageMetadataComparer.Default);
        }
    }
}