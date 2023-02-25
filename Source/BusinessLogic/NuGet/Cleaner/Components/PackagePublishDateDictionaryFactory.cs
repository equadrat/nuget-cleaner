using e2.Framework.Components;
using e2.Framework.Models;
using e2.NuGet.Cleaner.Models;
using JetBrains.Annotations;
using System;

namespace e2.NuGet.Cleaner.Components
{
    /// <summary>
    /// This class represents a factory to create instances of <see cref="IPackagePublishDateDictionary" />.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class PackagePublishDateDictionaryFactory: IPackagePublishDateDictionaryFactory
    {
        /// <summary>
        /// Creates a dictionary.
        /// </summary>
        /// <returns>
        /// The dictionary.
        /// </returns>
        [Pure]
        [NotNull]
        private static PackagePublishDateDictionary CreateDictionary()
        {
            return new PackagePublishDateDictionary();
        }

        /// <summary>
        /// The token factory.
        /// </summary>
        [NotNull]
        private readonly ICoreTokenFactory _tokenFactory;

        /// <summary>
        /// The dictionary pool.
        /// </summary>
        [NotNull]
        private readonly ICoreInstancePool<PackagePublishDateDictionary> _dictionaryPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackagePublishDateDictionaryFactory" /> class.
        /// </summary>
        /// <param name="tokenFactory">The token factory.</param>
        /// <param name="instancePoolFactory">The instance pool factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tokenFactory
        /// or
        /// instancePoolFactory
        /// </exception>
        public PackagePublishDateDictionaryFactory([NotNull] ICoreTokenFactory tokenFactory, [NotNull] ICoreInstancePoolFactory instancePoolFactory)
        {
            if (tokenFactory == null) throw new ArgumentNullException(nameof(tokenFactory));
            if (instancePoolFactory == null) throw new ArgumentNullException(nameof(instancePoolFactory));

            this._tokenFactory = tokenFactory;

            this._dictionaryPool = instancePoolFactory.CreateInstancePool(CreateDictionary, cleanup: PackagePublishDateDictionary.Cleanup);
        }

        /// <inheritdoc />
        public ICoreOwnerToken<IPackagePublishDateDictionary> GetPublishDateDictionary(IPackageOwnerSnapshot packageOwnerSnapshot)
        {
            if (packageOwnerSnapshot == null) throw new ArgumentNullException(nameof(packageOwnerSnapshot));

            var dictionary = this._dictionaryPool.GetInstance();
            dictionary.Init(packageOwnerSnapshot);

            return this._tokenFactory.CreateRelayToken<IPackagePublishDateDictionary, PackagePublishDateDictionary>(dictionary, this.ReleaseDictionary);
        }

        /// <summary>
        /// Releases the dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        private void ReleaseDictionary([NotNull] PackagePublishDateDictionary dictionary)
        {
            if (!this._dictionaryPool.Recycle(dictionary)) PackagePublishDateDictionary.Cleanup(dictionary);
        }
    }
}