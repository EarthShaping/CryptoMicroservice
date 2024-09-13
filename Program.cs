using CryptoMicroservice.Adapters;
using CryptoMicroservice.DataProviders;
using CryptoMicroservice.Factories;
using CryptoMicroservice.GraphQL;
using CryptoMicroservice.Interfaces;
using System.Net.Http;


var builder = WebApplication.CreateBuilder(args);


// Register HttpClient instances for each provider using IHttpClientFactory.
builder.Services.AddHttpClient("CoinGecko", client =>
{
    client.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
    client.DefaultRequestHeaders.Add("User-Agent", "CryptoMicroservice/1.0");

});

builder.Services.AddHttpClient("Binance", client =>
{
    client.BaseAddress = new Uri("https://api.binance.com/");
    client.DefaultRequestHeaders.Add("User-Agent", "CryptoMicroservice/1.0");
});

// Register Adapters
builder.Services.AddSingleton<ICryptoDataProviderAdapter, CoinGeckoAdapter>();
builder.Services.AddSingleton<ICryptoDataProviderAdapter, BinanceAdapter>();

// Register Data Providers
builder.Services.AddSingleton<ICryptoDataProvider>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var adapter = serviceProvider.GetServices<ICryptoDataProviderAdapter>().First(a => a is CoinGeckoAdapter);
    return new CoinGeckoDataProvider(httpClientFactory, adapter);
});

builder.Services.AddSingleton<ICryptoDataProvider>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var adapter = serviceProvider.GetServices<ICryptoDataProviderAdapter>().First(a => a is BinanceAdapter);
    return new BinanceDataProvider(httpClientFactory, adapter);
});

// Register the Factory
builder.Services.AddSingleton<ICryptoDataProviderFactory, CryptoDataProviderFactory>();

// Register GraphQL services
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<CryptoMarketDataType>();

var app = builder.Build();


// Enable GraphQL middleware
app.MapGraphQL("/graphql");
// Enable Banana Cake Pop middleware
if (app.Environment.IsDevelopment())
{
    app.MapBananaCakePop("/graphql-ui");
}
app.Run();

