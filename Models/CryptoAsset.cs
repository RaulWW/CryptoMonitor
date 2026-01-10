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
}
