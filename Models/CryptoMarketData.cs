namespace CryptoMicroservice.Models
{
    public class CryptoMarketData
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Volume { get; set; }
    }
}
