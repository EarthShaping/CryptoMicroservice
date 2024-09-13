namespace CryptoMicroservice.Adapters
{

    public class BinanceHelper
    {
        // Lazy initialization of the singleton instance
        private static readonly Lazy<BinanceHelper> _instance =   new Lazy<BinanceHelper>(() => new BinanceHelper());

        // Private constructor to prevent instantiation from outside
        private BinanceHelper() { }

        // Public accessor for the singleton instance
        public static BinanceHelper Instance => _instance.Value;

        public string GetCryptoNameFromSymbol(string symbol)
        {
            return symbol switch
            {
                "BTCUSDT" => "Bitcoin",
                "ETHUSDT" => "Ethereum",
                "XRPUSDT" => "Ripple",
                "LTCUSDT" => "Litecoin",
                // Add more mappings as needed
                _ => "Unknown"
            };
        }
        public string GetBinanceSymbol(string cryptoId)
        {
            return cryptoId.ToLower() switch
            {
                "bitcoin" => "BTCUSDT",
                "ethereum" => "ETHUSDT",
                "ripple" => "XRPUSDT",
                "litecoin" => "LTCUSDT",
                // Add more mappings as needed
                _ => throw new ArgumentException($"Unsupported cryptoId: {cryptoId}")
            };
        }
    }
}
