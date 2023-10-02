using e2;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: AssemblyCulture(ProductAssemblyInfo.Culture)]
[assembly: CLSCompliant(ProductAssemblyInfo.ClsCompliant)]

[assembly: AssemblyTitle(ProductAssemblyInfo.Title)]
[assembly: AssemblyProduct(ProductAssemblyInfo.Product)]
[assembly: AssemblyVersion(ProductAssemblyInfo.Version)]
[assembly: AssemblyInformationalVersion(ProductAssemblyInfo.Version)]
[assembly: AssemblyFileVersion(ProductAssemblyInfo.FileVersion)]
[assembly: AssemblyConfiguration(ProductAssemblyInfo.Configuration)]

#if !DisableProductAssemblyInfo
namespace e2
{
    /// <summary>
    /// This class provides informations for the assembly.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [DebuggerStepThrough]
    internal static class ProductAssemblyInfo
    {
        /// <summary>
        /// The title.
        /// </summary>
        public const string Title = CompanyAssemblyInfo.Name + " " + Product;

        /// <summary>
        /// The product.
        /// </summary>
        public const string Product = "NuGet Cleaner";

        /// <summary>
        /// The culture.
        /// </summary>
        public const string Culture = "";

        /// <summary>
        /// The version.
        /// </summary>
        /// <remarks>
        /// Use Major.Minor.Revision.
        /// </remarks>
        public const string Version = "1.0.5";

        /// <summary>
        /// The build identifier.
        /// </summary>
        /// <remarks>
        /// The build identifier will be set by the automated build process.
        /// </remarks>
        private const string BuildId = "";

        /// <summary>
        /// The file version.
        /// </summary>
        public const string FileVersion = Version + BuildId;

        /// <summary>
        /// The CLS compliant status.
        /// </summary>
        public const bool ClsCompliant = CompanyAssemblyInfo.DefaultClsCompliant;

        /// <summary>
        /// The configuration.
        /// </summary>
#if DEBUG
        public const string Configuration = "DEBUG";
#else
        public const string Configuration = "RELEASE";
#endif
    }
}
#endif