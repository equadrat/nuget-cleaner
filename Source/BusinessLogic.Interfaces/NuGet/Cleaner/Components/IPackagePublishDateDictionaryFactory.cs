﻿using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using System;
using System.Diagnostics.Contracts;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This interface describes a factory to create instances of <see cref="IPackagePublishDateDictionary" />.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public interface IPackagePublishDateDictionaryFactory
    {
        /// <summary>
        /// Gets a publish date dictionary.
        /// </summary>
        /// <param name="packageOwnerSnapshot">The package owner snapshot.</param>
        /// <returns>
        /// The publish date dictionary.
        /// </returns>
        [Pure]
        ICoreOwnerToken<IPackagePublishDateDictionary> GetPublishDateDictionary(IPackageOwnerSnapshot packageOwnerSnapshot);
    }
}