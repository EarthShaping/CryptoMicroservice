using CryptoMicroservice.Models;
using HotChocolate.Types;

namespace CryptoMicroservice.GraphQL
{
    public class CryptoMarketDataType : ObjectType<CryptoMarketData>
    {
        protected override void Configure(IObjectTypeDescriptor<CryptoMarketData> descriptor)
        {
            descriptor.Field(f => f.Id).Type<NonNullType<StringType>>();
            descriptor.Field(f => f.Name).Type<StringType>();
            descriptor.Field(f => f.Symbol).Type<StringType>();
            descriptor.Field(f => f.CurrentPrice).Type<DecimalType>();
            descriptor.Field(f => f.MarketCap).Type<DecimalType>();
            descriptor.Field(f => f.Volume).Type<DecimalType>();
        }
    }
}
