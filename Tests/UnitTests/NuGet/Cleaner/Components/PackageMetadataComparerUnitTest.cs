using System;
using System.Diagnostics;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcludeFromCodeCoverage = System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents the unit test of <see cref="PackageMetadataComparer" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackageMetadataComparerUnitTest: UnitTestTemplate
    {
        /// <summary>
        /// Tests to create an instance of <see cref="PackageMetadataComparer" />.
        /// </summary>
        [TestMethod]
        public void Init_01()
        {
            _ = PackageMetadataComparer.Default;
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadataComparer.Compare" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Compare_01()
        {
            var model1 = new PackageMetadata();
            var model2 = new PackageMetadata();

            Assert.AreEqual(0, PackageMetadataComparer.Default.Compare(model1, model2));
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadataComparer.Compare" /> works as expected.
        /// </summary>
        /// <param name="mode">The mode.</param>
        [DataTestMethod]
        [DataRow(-2, DisplayName = "Instance (1)")]
        [DataRow(-1, DisplayName = "Instance (2)")]
        [DataRow(0, DisplayName = "PackageId")]
        [DataRow(1, DisplayName = "Owners")]
        [DataRow(2, DisplayName = "IsListed")]
        [DataRow(3, DisplayName = "IsDeprecated")]
        [DataRow(4, DisplayName = "PublishDate")]
        [DataRow(5, DisplayName = "Version")]
        [DataRow(6, DisplayName = "OriginalVersion")]
        [DataRow(7, DisplayName = "IsPrerelease")]
        public void Compare_02(int mode)
        {
            var model1 = new PackageMetadata();
            var model2 = new PackageMetadata();

            switch (mode)
            {
                case -2:
                {
                    model1 = model2;
                    break;
                }

                case -1:
                {
                    model1 = model2 = null;
                    break;
                }

                case 0:
                {
                    model1.PackageId = model2.PackageId = "m";
                    break;
                }

                case 1:
                {
                    model1.Owners = model2.Owners = "o";
                    break;
                }

                case 2:
                {
                    model1.IsListed = model2.IsListed = true;
                    break;
                }

                case 3:
                {
                    model1.IsDeprecated = model2.IsDeprecated = true;
                    break;
                }

                case 4:
                {
                    model1.PublishDate = model2.PublishDate = DateTimeOffset.UtcNow;
                    break;
                }

                case 5:
                {
                    model1.Version = model2.Version = new Version(1, 2, 3, 4);
                    break;
                }

                case 6:
                {
                    model1.OriginalVersion = model2.OriginalVersion = "v";
                    break;
                }

                case 7:
                {
                    model1.IsPrerelease = model2.IsPrerelease = true;
                    break;
                }
            }

            Assert.AreEqual(0, PackageMetadataComparer.Default.Compare(model1, model2), "Regular order");
            Assert.AreEqual(0, PackageMetadataComparer.Default.Compare(model2, model1), "Inverse order");
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadataComparer.Compare" /> works as expected.
        /// </summary>
        /// <param name="mode">The mode.</param>
        [DataTestMethod]
        [DataRow(-1, DisplayName = "Instance")]
        [DataRow(0, DisplayName = "PackageId")]
        [DataRow(1, DisplayName = "Owners")]
        [DataRow(2, DisplayName = "IsListed")]
        [DataRow(3, DisplayName = "IsDeprecated")]
        [DataRow(4, DisplayName = "PublishDate")]
        [DataRow(5, DisplayName = "Version")]
        [DataRow(6, DisplayName = "OriginalVersion")]
        [DataRow(7, DisplayName = "IsPrerelease (1)")]
        [DataRow(8, DisplayName = "IsPrerelease (2)")]
        public void Compare_03(int mode)
        {
            var model1 = new PackageMetadata();
            var model2 = new PackageMetadata();

            switch (mode)
            {
                case -1:
                {
                    model1 = null;
                    break;
                }

                case 0:
                {
                    model2.PackageId = "m";
                    break;
                }

                case 1:
                {
                    model2.Owners = "o";
                    break;
                }

                case 2:
                {
                    model2.IsListed = true;
                    break;
                }

                case 3:
                {
                    model2.IsDeprecated = true;
                    break;
                }

                case 4:
                {
                    model2.PublishDate = DateTimeOffset.UtcNow;
                    break;
                }

                case 5:
                {
                    model2.Version = new Version(1, 2, 3, 4);
                    break;
                }

                case 6:
                {
                    model2.OriginalVersion = "v";
                    break;
                }

                case 7:
                {
                    model2.IsPrerelease = true;
                    break;
                }

                case 8:
                {
                    model1.IsPrerelease = model2.IsPrerelease = true;
                    model2.OriginalVersion = "v";
                    break;
                }
            }

            Assert.AreNotEqual(0, PackageMetadataComparer.Default.Compare(model1, model2), "Regular order");
            Assert.AreNotEqual(0, PackageMetadataComparer.Default.Compare(model2, model1), "Inverse order");
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadataComparer.Equals(e2.NuGet.Cleaner.Models.IPackageMetadata,e2.NuGet.Cleaner.Models.IPackageMetadata)" /> works as expected.
        /// </summary>
        [TestMethod]
        public void Equals_01()
        {
            var model1 = new PackageMetadata();
            var model2 = new PackageMetadata();

            Assert.IsTrue(PackageMetadataComparer.Default.Equals(model1, model2));
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadataComparer.Equals(e2.NuGet.Cleaner.Models.IPackageMetadata,e2.NuGet.Cleaner.Models.IPackageMetadata)" /> works as expected.
        /// </summary>
        /// <param name="mode">The mode.</param>
        [DataTestMethod]
        [DataRow(0, DisplayName = "Instance")]
        [DataRow(1, DisplayName = "Null")]
        public void Equals_02(int mode)
        {
            var model = mode == 0 ? new PackageMetadata() : null;

            Assert.IsTrue(PackageMetadataComparer.Default.Equals(model, model));
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadataComparer.Equals(e2.NuGet.Cleaner.Models.IPackageMetadata,e2.NuGet.Cleaner.Models.IPackageMetadata)" /> works as expected.
        /// </summary>
        /// <param name="mode">The mode.</param>
        [DataTestMethod]
        [DataRow(-1, DisplayName = "Instance")]
        [DataRow(0, DisplayName = "PackageId")]
        [DataRow(1, DisplayName = "Owners")]
        [DataRow(2, DisplayName = "IsListed")]
        [DataRow(3, DisplayName = "IsDeprecated")]
        [DataRow(4, DisplayName = "PublishDate")]
        [DataRow(5, DisplayName = "Version")]
        [DataRow(6, DisplayName = "OriginalVersion")]
        [DataRow(7, DisplayName = "IsPrerelease")]
        public void Equals_03(int mode)
        {
            var model1 = new PackageMetadata();
            var model2 = new PackageMetadata();

            switch (mode)
            {
                case -1:
                {
                    model1 = null;
                    break;
                }

                case 0:
                {
                    model2.PackageId = "m";
                    break;
                }

                case 1:
                {
                    model2.Owners = "o";
                    break;
                }

                case 2:
                {
                    model2.IsListed = true;
                    break;
                }

                case 3:
                {
                    model2.IsDeprecated = true;
                    break;
                }

                case 4:
                {
                    model2.PublishDate = DateTimeOffset.UtcNow;
                    break;
                }

                case 5:
                {
                    model2.Version = new Version(1, 2, 3, 4);
                    break;
                }

                case 6:
                {
                    model2.OriginalVersion = "v";
                    break;
                }

                case 7:
                {
                    model2.IsPrerelease = true;
                    break;
                }
            }

            Assert.IsFalse(PackageMetadataComparer.Default.Equals(model1, model2), "Regular order");
            Assert.IsFalse(PackageMetadataComparer.Default.Equals(model2, model1), "Inverse order");
        }

        /// <summary>
        /// Tests that <see cref="PackageMetadataComparer.GetHashCode(e2.NuGet.Cleaner.Models.IPackageMetadata)" /> returns the expected result.
        /// </summary>
        [TestMethod]
        public void GetHashCode_01()
        {
            var model1 = new PackageMetadata();
            var model2 = new PackageMetadata();

            var hashCode1 = PackageMetadataComparer.Default.GetHashCode(model1);
            var hashCode2 = PackageMetadataComparer.Default.GetHashCode(model2);

            Assert.AreEqual(hashCode1, hashCode2);
        }
    }
}