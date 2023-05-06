using e2.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the unit test of <see cref="ApiKeyConfig" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ApiKeyConfigUnitTest: UnitTestTemplate
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
        private ApiKeyConfig CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<ApiKeyConfig>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="ApiKeyConfig" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="ApiKeyConfig.ApiKeyId" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void ApiKeyId_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.ApiKeyId);
        }

        /// <summary>
        /// Tests that <see cref="ApiKeyConfig.ApiKeyId" /> works as expected.
        /// </summary>
        [TestMethod]
        public void ApiKeyId_02()
        {
            var instance = this.CreateInstance();

            instance.ApiKeyId = "MyApiKeyId";
            Assert.AreEqual("MyApiKeyId", instance.ApiKeyId);
        }

        /// <summary>
        /// Tests that <see cref="ApiKeyConfig.ApiKey" /> returns the expected default value.
        /// </summary>
        [TestMethod]
        public void ApiKey_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNull(instance.ApiKey);
        }

        /// <summary>
        /// Tests that <see cref="ApiKeyConfig.ApiKey" /> works as expected.
        /// </summary>
        [TestMethod]
        public void ApiKey_02()
        {
            var instance = this.CreateInstance();

            instance.ApiKey = "MyApiKey";
            Assert.AreEqual("MyApiKey", instance.ApiKey);
        }
    }
}