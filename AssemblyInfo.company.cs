using e2;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;

[assembly: AssemblyCompany(CompanyAssemblyInfo.Name)]
[assembly: AssemblyCopyright(CompanyAssemblyInfo.Copyright)]
[assembly: AssemblyTrademark(CompanyAssemblyInfo.Trademark)]
[assembly: NeutralResourcesLanguage(CompanyAssemblyInfo.NeutralLanguage)]

#if !DisableCompanyAssemblyInfo
namespace e2
{
    /// <summary>
    /// This class provides informations for the assembly.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [DebuggerStepThrough]
    internal static class CompanyAssemblyInfo
    {
        /// <summary>
        /// The copyright year.
        /// </summary>
        private const string CopyrightYear = "2023";

        /// <summary>
        /// The name.
        /// </summary>
        public const string Name = "equadrat";

        /// <summary>
        /// The copyright.
        /// </summary>
        public const string Copyright = "Copyright © " + Name + " 2004 - " + CopyrightYear;

        /// <summary>
        /// The trademark.
        /// </summary>
        public const string Trademark = "";

        /// <summary>
        /// The default CLS compliant status.
        /// </summary>
#if NONCLSCOMPLIANT
        public const bool DefaultClsCompliant = false;
#else
        public const bool DefaultClsCompliant = true;
#endif

        /// <summary>
        /// The neutral language.
        /// </summary>
        public const string NeutralLanguage = "en-US";
    }
}
#endif