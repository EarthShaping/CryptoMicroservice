using CryptoMicroservice.Interfaces;
using CryptoMicroservice.Models;

namespace CryptoMicroservice.GraphQL
{
    public class Query
    {
        private readonly ICryptoDataProviderFactory _providerFactory;

        public Query(ICryptoDataProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
        }

        [GraphQLName("cryptoPrices")]
        public async Task<Dictionary<string, decimal>> GetCryptoPrices(string[] cryptoIds, string provider)
        {
            var dataProvider = _providerFactory.GetProvider(provider);
            return await dataProvider.GetCryptoPricesAsync(cryptoIds);
        }
        [GraphQLName("cryptoMarketData")]
        public async Task<List<CryptoMarketData>> GetCryptoMarketData(string[] cryptoIds, string provider)
        {
            var dataProvider = _providerFactory.GetProvider(provider);
            return await dataProvider.GetCryptoMarketDataAsync(cryptoIds);
        }
    }
}
