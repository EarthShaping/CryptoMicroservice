using CryptoMicroservice.Interfaces;
using CryptoMicroservice.Models;
using System.Text.Json;

namespace CryptoMicroservice.DataProviders
{
    public class BinanceDataProvider : ICryptoDataProvider
    {
        public string ProviderName => "Binance";
        private readonly HttpClient _httpClient;
        private readonly ICryptoDataProviderAdapter _adapter;

        public BinanceDataProvider(IHttpClientFactory httpClientFactory, ICryptoDataProviderAdapter adapter)
        {
            _httpClient = httpClientFactory.CreateClient("Binance");
            _adapter = adapter;
        }
        public async Task<Dictionary<string, decimal>> GetCryptoPricesAsync(string[] cryptoIds)
        {
            try
            {
                // Map cryptoIds to Binance symbols
                var binanceSymbols = cryptoIds.Select(id => Adapters.BinanceHelper.Instance.GetBinanceSymbol(id)).ToList();
                // Fetch all prices from Binance API
                var response = await _httpClient.GetStringAsync("api/v3/ticker/price");

                // Parse the JSON response
                using var jsonDocument = JsonDocument.Parse(response);
                var root = jsonDocument.RootElement;

                // Filter the JSON elements to include only the requested binanceSymbols
                var filteredElements = root.EnumerateArray()
                    .Where(item =>
                    {
                        var symbol = item.GetProperty("symbol").GetString().ToLower();
                        return binanceSymbols.Contains(symbol, StringComparer.OrdinalIgnoreCase);
                    })
                    .ToList();

                // Serialize the filtered elements back to JSON string
                var filteredResponse = JsonSerializer.Serialize(filteredElements);

                // Pass the filtered JSON to the adapter
                var prices = _adapter.AdaptPrices(filteredResponse);

                return prices;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Failed to fetch data from Binance provider", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                throw new Exception("An error occurred while fetching crypto prices", ex);
            }
        }
      
        public async Task<List<CryptoMarketData>> GetCryptoMarketDataAsync(string[] cryptoIds)
        {
            // Binance does not accept multiple symbols in a single request for market data
            var tasks = new List<Task<CryptoMarketData>>();

            foreach (var cryptoId in cryptoIds)
            {
                tasks.Add(GetMarketDataForSymbolAsync(cryptoId));
            }

            // Wait for all tasks to complete
            var marketDataArray = await Task.WhenAll(tasks);

            return marketDataArray.ToList();
        }

        private async Task<CryptoMarketData> GetMarketDataForSymbolAsync(string cryptoId)
        {
            try
            {
                var symbol = Adapters.BinanceHelper.Instance.GetBinanceSymbol(cryptoId);
                var url = $"api/v3/ticker/24hr?symbol={symbol}";
                var response = await _httpClient.GetStringAsync(url);

                // Use the adapter to adapt the raw market data
                var marketData = _adapter.AdaptMarketData(response);

                // Since AdaptMarketData returns a list, but we only have one item, get the first item
                return marketData.FirstOrDefault();
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Failed to fetch market data for symbol {cryptoId} from Binance provider", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                throw new Exception($"An error occurred while fetching market data for symbol {cryptoId}", ex);
            }
        }
      
    }
}
