using e2.Framework;
using e2.NuGet.Cleaner.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

// ReSharper disable once UseObjectOrCollectionInitializer
#pragma warning disable IDE0028

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the unit test of <see cref="ConfigSnapshotProvider" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ConfigSnapshotProviderUnitTest: UnitTestTemplate
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
        private ConfigSnapshotProvider CreateInstance(NCObject? _ = null, bool inconclusive = true)
        {
            try
            {
                return this.Factory.CreateCustomInstanceOf<ConfigSnapshotProvider>();
            }
            catch
            {
                if (inconclusive) Assert.Inconclusive(nameof(this.Init_01));
                throw;
            }
        }

        /// <summary>
        /// Tests to create an instance of <see cref="ConfigSnapshotProvider" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = this.CreateInstance(inconclusive: false);
        }

        /// <summary>
        /// Tests that <see cref="ConfigSnapshotProvider.GetConfigSnapshot" /> returns a snapshot.
        /// </summary>
        [TestMethod]
        public void GetConfigSnapshot_01()
        {
            var instance = this.CreateInstance();
            Assert.IsNotNull(instance.GetConfigSnapshot());
        }

        /// <summary>
        /// Tests that <see cref="ConfigSnapshotProvider.GetConfigSnapshot" /> returns the same snapshot again until the config changes.
        /// </summary>
        [TestMethod]
        public void GetConfigSnapshot_02()
        {
            var instance = this.CreateInstance();
            var configProvider = this.Factory.GetInstanceOf<ConfigProviderFake>();

            var snapshots = new List<IConfigSnapshot>();

            snapshots.Add(instance.GetConfigSnapshot());
            snapshots.Add(instance.GetConfigSnapshot());
            configProvider.OnChanged();
            snapshots.Add(instance.GetConfigSnapshot());
            snapshots.Add(instance.GetConfigSnapshot());

            Assert.AreSame(snapshots[0], snapshots[1]);
            Assert.AreNotSame(snapshots[1], snapshots[2]);
            Assert.AreSame(snapshots[2], snapshots[3]);
        }
    }
}