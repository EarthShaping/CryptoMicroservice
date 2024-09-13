using CryptoMicroservice.Interfaces;

namespace CryptoMicroservice.Factories
{
    public class CryptoDataProviderFactory : ICryptoDataProviderFactory
    {
        private readonly IDictionary<string, ICryptoDataProvider> _providers;

        public CryptoDataProviderFactory(IEnumerable<ICryptoDataProvider> providers)
        {
            _providers = providers.ToDictionary(p => p.ProviderName, StringComparer.OrdinalIgnoreCase);
        }

        public ICryptoDataProvider GetProvider(string providerName)
        {
            if (_providers.TryGetValue(providerName, out var provider))
            {
                return provider;
            }

            throw new ArgumentException($"Provider '{providerName}' not found.");
        }
    }
}
