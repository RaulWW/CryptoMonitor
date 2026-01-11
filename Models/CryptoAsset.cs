using System.Text.Json.Serialization;

namespace CryptoMonitor.Models;

public class CryptoAsset
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("current_price")]
    public decimal CurrentPrice { get; set; }

    [JsonPropertyName("market_cap")]
    public long MarketCap { get; set; }

    [JsonPropertyName("market_cap_rank")]
    public int MarketCapRank { get; set; }

    [JsonPropertyName("price_change_percentage_24h")]
    public double PriceChange24h { get; set; }

    [JsonPropertyName("price_change_percentage_1h_in_currency")]
    public double? PriceChange1h { get; set; }

    [JsonPropertyName("price_change_percentage_7d_in_currency")]
    public double? PriceChange7d { get; set; }

    public string Provider { get; set; } = "CoinGecko"; // Default

    public static CryptoAsset FromCoinCap(CoinCapAsset cap)
    {
        var symbol = (cap.Symbol ?? "").ToLower();
        return new CryptoAsset
        {
            Id = cap.Id ?? "",
            Symbol = cap.Symbol ?? "",
            Name = cap.Name ?? "",
            Provider = "CoinCap",
            MarketCapRank = int.TryParse(cap.Rank, out var r) ? r : 0,
            CurrentPrice = decimal.TryParse(cap.PriceUsd, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var p) ? p : 0,
            MarketCap = long.TryParse((cap.MarketCapUsd ?? "").Split('.')[0], out var m) ? m : 0,
            PriceChange24h = double.TryParse(cap.ChangePercent24Hr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var c) ? c : 0,
            Image = $"https://assets.coincap.io/assets/icons/{symbol}@2x.png",
            PriceChange1h = null,
            PriceChange7d = null
        };
    }

    public static CryptoAsset FromCoinPaprika(CoinPaprikaTicker ticker)
    {
        var quote = ticker.Quotes != null && ticker.Quotes.TryGetValue("USD", out var q) ? q : new CoinPaprikaQuote();
        return new CryptoAsset
        {
            Id = ticker.Id ?? "",
            Symbol = ticker.Symbol ?? "",
            Name = ticker.Name ?? "",
            Provider = "CoinPaprika",
            MarketCapRank = ticker.Rank,
            CurrentPrice = quote.Price,
            MarketCap = quote.MarketCap,
            PriceChange24h = quote.PercentChange24h,
            Image = $"https://static.coinpaprika.com/coin/{ticker.Id}/logo.png",
            PriceChange1h = null,
            PriceChange7d = null
        };
    }
}

public class CoinCapAsset
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("rank")]
    public string Rank { get; set; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("marketCapUsd")]
    public string MarketCapUsd { get; set; } = string.Empty;

    [JsonPropertyName("priceUsd")]
    public string PriceUsd { get; set; } = string.Empty;

    [JsonPropertyName("changePercent24Hr")]
    public string ChangePercent24Hr { get; set; } = string.Empty;
}

public class CoinCapResponse
{
    [JsonPropertyName("data")]
    public List<CoinCapAsset> Data { get; set; } = new();
}

public class CoinPaprikaTicker
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("symbol")] public string Symbol { get; set; } = "";
    [JsonPropertyName("rank")] public int Rank { get; set; }
    [JsonPropertyName("quotes")] public Dictionary<string, CoinPaprikaQuote> Quotes { get; set; } = new();
}

public class CoinPaprikaQuote
{
    [JsonPropertyName("price")] public decimal Price { get; set; }
    [JsonPropertyName("market_cap")] public long MarketCap { get; set; }
    [JsonPropertyName("percent_change_24h")] public double PercentChange24h { get; set; }
}
