namespace CryptoMicroservice.Interfaces
{
    public interface ICryptoDataProviderFactory
    {
        ICryptoDataProvider GetProvider(string providerName);
    }
}
