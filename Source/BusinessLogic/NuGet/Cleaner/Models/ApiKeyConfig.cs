using System;

namespace e2.NuGet.Cleaner.Models
{
    /// <summary>
    /// This class represents the configuration of an API key.
    /// </summary>
    [CLSCompliant(ProductAssemblyInfo.ClsCompliant)]
    public sealed class ApiKeyConfig: IApiKeyConfig
    {
        /// <inheritdoc />
        public string? ApiKeyId {get; set;}

        /// <inheritdoc />
        public string? ApiKey {get; set;}
    }
}