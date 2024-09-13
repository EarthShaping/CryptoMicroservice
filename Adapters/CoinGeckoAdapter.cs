using CryptoMicroservice.Interfaces;
using CryptoMicroservice.Models;
using System.Text.Json;

namespace CryptoMicroservice.Adapters
{
    public class CoinGeckoAdapter : ICryptoDataProviderAdapter
    {
        public Dictionary<string, decimal> AdaptPrices(string rawData)
        {
            using var jsonDocument = JsonDocument.Parse(rawData);
            var root = jsonDocument.RootElement;

            var prices = new Dictionary<string, decimal>();

            foreach (var property in root.EnumerateObject())
            {
                var cryptoId = property.Name;
                var usdPrice = property.Value.GetProperty("usd").GetDecimal();
                prices.Add(cryptoId, usdPrice);
            }

            return prices;
        }

        public List<CryptoMarketData> AdaptMarketData(string rawData)
        {
            using var jsonDocument = JsonDocument.Parse(rawData);
            var root = jsonDocument.RootElement;

            var marketDataList = new List<CryptoMarketData>();

            foreach (var item in root.EnumerateArray())
            {
                var marketData = new CryptoMarketData
                {
                    Id = item.GetProperty("id").GetString(),
                    Symbol = item.GetProperty("symbol").GetString(),
                    Name = item.GetProperty("name").GetString(),
                    CurrentPrice = item.GetProperty("current_price").GetDecimal(),
                    MarketCap = item.GetProperty("market_cap").GetDecimal(),
                    Volume = item.GetProperty("total_volume").GetDecimal()
                };

                marketDataList.Add(marketData);
            }

            return marketDataList;
        }
    }
}
