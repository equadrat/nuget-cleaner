using e2.Framework;
using e2.NuGet.Cleaner.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the unit test of <see cref="ConfigSnapshotFactory" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ConfigSnapshotFactoryUnitTest: UnitTestTemplate
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
        private ConfigSnapshotFactory CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<ConfigSnapshotFactory>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="ConfigSnapshotFactory" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="ConfigSnapshotFactory.CreateSnapshot" /> returns a snapshot.
        /// </summary>
        [TestMethod]
        public void CreateSnapshot_01()
        {
            var instance = this.CreateInstance();
            var configProvider = this.Factory.GetInstanceOf<IConfigProvider>();
            Assert.IsNotNull(instance.CreateSnapshot(configProvider));
        }

        /// <summary>
        /// Tests that <see cref="ConfigSnapshotFactory.CreateSnapshot" /> returns the expected snapshot.
        /// </summary>
        [TestMethod]
        public void CreateSnapshot_GetLoggingOutput_01()
        {
            var instance = this.CreateInstance();
            var configProvider = this.Factory.GetInstanceOf<ConfigProviderFake>();

            configProvider.ApplyTestConfig1();

            const string expectedLoggingOutput = "Sources:\r\n" +
                                                 "  SourceId: \"DefaultSource\" (active)\r\n" +
                                                 "  SourceId: \"OtherSource\" (inactive)\r\n" +
                                                 "ApiKeys:\r\n" +
                                                 "  ApiKeyId: \"DefaultApiKey\" (active)\r\n" +
                                                 "  ApiKeyId: \"OtherApiKey\" (inactive)\r\n" +
                                                 "PackageCleanups:\r\n" +
                                                 "  PackageCleanupId: \"DefaultCleanup\" (active)\r\n" +
                                                 "  PackageCleanupId: \"OtherCleanup\" (inactive)\r\n" +
                                                 "PackageGroups:\r\n" +
                                                 "  PackageIdPattern: \"My.Packages*\"\r\n" +
                                                 "    PackageIdMatchMode: Like\r\n" +
                                                 "    Owner: \"Me\"\r\n" +
                                                 "    SourceId: \"DefaultSource\"\r\n" +
                                                 "    ApiKeyId: \"DefaultApiKey\"\r\n" +
                                                 "    PackageCleanupId: \"DefaultCleanup\"";

            var actualLoggingOutput = instance.CreateSnapshot(configProvider).GetLoggingOutput();

            Assert.AreEqual(expectedLoggingOutput, actualLoggingOutput);
        }
    }
}