
using CryptoMicroservice.Interfaces;
using CryptoMicroservice.Models;
using System.Text.Json;

namespace CryptoMicroservice.Adapters
{
    public class BinanceAdapter : ICryptoDataProviderAdapter
    {
        public Dictionary<string, decimal> AdaptPrices(string rawData)
        {
            using var jsonDocument = JsonDocument.Parse(rawData);
            var root = jsonDocument.RootElement;

            var prices = new Dictionary<string, decimal>();

            foreach (var item in root.EnumerateArray())
            {
                var symbol = item.GetProperty("symbol").GetString().ToLower();
                var price =  Convert.ToDecimal( item.GetProperty("price").GetString());
                prices.Add(symbol, price);
            }

            return prices;
        }

        public List<CryptoMarketData> AdaptMarketData(string rawData)
        {
            var marketDataList = new List<CryptoMarketData>();

            using var jsonDocument = JsonDocument.Parse(rawData);
            var root = jsonDocument.RootElement;

            // The response is a single JSON object
            var marketData = new CryptoMarketData
            {
                Id = root.GetProperty("symbol").GetString(), // Binance does not provide an ID
                Symbol = root.GetProperty("symbol").GetString(),
                Name = BinanceHelper.Instance.GetCryptoNameFromSymbol(root.GetProperty("symbol").GetString()),
                CurrentPrice = Convert.ToDecimal( root.GetProperty("lastPrice").GetString()),
                MarketCap = 0, // Binance's endpoint does not provide market cap
                Volume = Convert.ToDecimal(root.GetProperty("volume").GetString())
            };

            marketDataList.Add(marketData);

            return marketDataList;
        }
       
    }
}
