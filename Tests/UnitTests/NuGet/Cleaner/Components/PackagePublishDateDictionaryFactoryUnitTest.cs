using e2.Framework;
using e2.NuGet.Cleaner.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the unit test of <see cref="PackagePublishDateDictionaryFactory" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackagePublishDateDictionaryFactoryUnitTest: UnitTestTemplate
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
        private PackagePublishDateDictionaryFactory CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<PackagePublishDateDictionaryFactory>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="PackagePublishDateDictionaryFactory" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="PackagePublishDateDictionaryFactory.GetPublishDateDictionary" /> returns an instance.
        /// </summary>
        [TestMethod]
        public void GetPublishDateDictionary_01()
        {
            var instance = this.CreateInstance();

            var configProvider = this.Factory.GetInstanceOf<ConfigProviderFake>();
            configProvider.ApplyTestConfig1();

            var configSnapshotFactory = this.Factory.GetInstanceOf<IConfigSnapshotFactory>();
            var configSnapshot = configSnapshotFactory.CreateSnapshot(configProvider);

            using (var token = instance.GetPublishDateDictionary(configSnapshot.PackageSources[0].Owners[0]))
            {
                Assert.IsNotNull(token);
                Assert.IsNotNull(token.Instance);

                Assert.AreEqual(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), token.Instance.GetPublishDate("My.Packages.A", new Version(1, 0, 0, 0), "1.0.0.0", new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)));
            }
        }
    }
}