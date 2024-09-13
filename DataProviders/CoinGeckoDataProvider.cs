using CryptoMicroservice.Interfaces;
using CryptoMicroservice.Models;

namespace CryptoMicroservice.DataProviders
{
    public class CoinGeckoDataProvider : ICryptoDataProvider
    {
        public string ProviderName => "CoinGecko";
        private readonly HttpClient _httpClient;
        private readonly ICryptoDataProviderAdapter _adapter;

        public CoinGeckoDataProvider(IHttpClientFactory httpClientFactory, ICryptoDataProviderAdapter adapter)
        {
            _httpClient = httpClientFactory.CreateClient("CoinGecko");
            _adapter = adapter;
        }

        public async Task<Dictionary<string, decimal>> GetCryptoPricesAsync(string[] cryptoIds)
        {
            try
            {
                var url = $"simple/price?ids={string.Join(",", cryptoIds)}&vs_currencies=usd";
                var response = await _httpClient.GetStringAsync(url);
                return _adapter.AdaptPrices(response);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Failed to fetch crypto prices from CoinGecko provider", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                throw new Exception("An error occurred while fetching crypto prices", ex);
            }
          

        }

        public async Task<List<CryptoMarketData>> GetCryptoMarketDataAsync(string[] cryptoIds)
        {
            try
            {
                var url = $"coins/markets?vs_currency=usd&ids={string.Join(",", cryptoIds)}";
                var response = await _httpClient.GetStringAsync(url);
                return _adapter.AdaptMarketData(response);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Failed to fetch crypto prices data from CoinGecko provider", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                throw new Exception("An error occurred while fetching crypto market data", ex);
            }
        }
    }
}
