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

    public static CryptoAsset FromCoinCap(CoinCapAsset cap)
    {
        var symbol = cap.Symbol.ToLower();
        return new CryptoAsset
        {
            Id = cap.Id,
            Symbol = cap.Symbol,
            Name = cap.Name,
            MarketCapRank = int.TryParse(cap.Rank, out var r) ? r : 0,
            CurrentPrice = decimal.TryParse(cap.PriceUsd, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var p) ? p : 0,
            MarketCap = long.TryParse(cap.MarketCapUsd.Split('.')[0], out var m) ? m : 0,
            PriceChange24h = double.TryParse(cap.ChangePercent24Hr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var c) ? c : 0,
            Image = $"https://assets.coincap.io/assets/icons/{symbol}@2x.png",
            PriceChange1h = null, // Not available in basic assets list
            PriceChange7d = null  // Not available in basic assets list
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
