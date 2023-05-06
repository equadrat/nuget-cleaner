using e2.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the unit test of <see cref="SourceConfig" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class SourceConfigUnitTest: UnitTestTemplate
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
        private SourceConfig CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<SourceConfig>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="SourceConfig" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="SourceConfig.SourceId" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void SourceId_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.SourceId);
        }

        /// <summary>
        /// Tests that <see cref="SourceConfig.SourceId" /> works as expected.
        /// </summary>
        [TestMethod]
        public void SourceId_02()
        {
            var instance = this.CreateInstance();

            instance.SourceId = "MySourceId";
            Assert.AreEqual("MySourceId", instance.SourceId);
        }

        /// <summary>
        /// Tests that <see cref="SourceConfig.PackageSource" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void PackageSource_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.PackageSource);
        }

        /// <summary>
        /// Tests that <see cref="SourceConfig.PackageSource" /> works as expected.
        /// </summary>
        [TestMethod]
        public void PackageSource_02()
        {
            var instance = this.CreateInstance();

            instance.PackageSource = "MyPackageSource";
            Assert.AreEqual("MyPackageSource", instance.PackageSource);
        }
    }
}