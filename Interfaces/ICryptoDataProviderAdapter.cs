using CryptoMicroservice.Models;

namespace CryptoMicroservice.Interfaces
{
    public interface ICryptoDataProviderAdapter
    {
        Dictionary<string, decimal> AdaptPrices(string rawData);
        List<CryptoMarketData> AdaptMarketData(string rawData);
    }
}
