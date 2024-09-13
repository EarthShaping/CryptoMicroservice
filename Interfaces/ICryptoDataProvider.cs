using CryptoMicroservice.Models;

namespace CryptoMicroservice.Interfaces
{
    public interface ICryptoDataProvider
    {
        string ProviderName { get; }
        Task<Dictionary<string, decimal>> GetCryptoPricesAsync(string[] cryptoIds);
        Task<List<CryptoMarketData>> GetCryptoMarketDataAsync(string[] cryptoIds);
    }
}
