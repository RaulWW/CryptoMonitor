using System.Text.Json.Serialization;
using CryptoMonitor.Models;

namespace CryptoMonitor.Services;

public interface ITrendService
{
    Task<MacroInsight> GetMacroInsightsAsync();
    Task<List<MarketTrend>> GetCurrentTrendsAsync();
}

public class TrendService : ITrendService
{
    private readonly HttpClient _http;
    private readonly ILanguageService _lang;

    public TrendService(HttpClient http, ILanguageService lang)
    {
        _http = http;
        _lang = lang;
        _http.DefaultRequestHeaders.Add("User-Agent", "CryptoMonitorApp");
    }

    private class FnGResponse { [JsonPropertyName("data")] public List<FnGData> Data { get; set; } = new(); }
    private class FnGData { [JsonPropertyName("value")] public string Value { get; set; } = ""; [JsonPropertyName("value_classification")] public string Label { get; set; } = ""; }

    private class GlobalDataResponse { [JsonPropertyName("data")] public GlobalData Data { get; set; } = new(); }
    private class GlobalData 
    { 
        [JsonPropertyName("market_cap_percentage")] public Dictionary<string, double> CapPercentages { get; set; } = new();
        [JsonPropertyName("total_market_cap")] public Dictionary<string, double> TotalCap { get; set; } = new();
    }

    public async Task<MacroInsight> GetMacroInsightsAsync()
    {
        var insight = new MacroInsight
        {
            GlobalContext = _lang.T("MacroDescription"),
            DailyOutlook = _lang.T("DailyOutlookDesc"),
            KeyMetrics = new Dictionary<string, string>()
        };

        try 
        {
            // 1. Fetch Fear & Greed
            var fng = await _http.GetFromJsonAsync<FnGResponse>("https://api.alternative.me/fng/?limit=1");
            if (fng?.Data?.Any() == true)
            {
                insight.FearAndGreedValue = fng.Data[0].Value;
                insight.FearAndGreedLabel = fng.Data[0].Label;
            }
        }
        catch (Exception ex) { Console.WriteLine($"[ERROR] FNG API: {ex.Message}"); }

        try
        {
            // 2. Fetch Global Data
            var global = await _http.GetFromJsonAsync<GlobalDataResponse>("https://api.coingecko.com/api/v3/global");
            if (global?.Data != null)
            {
                if (global.Data.CapPercentages.TryGetValue("btc", out var btcDom))
                    insight.KeyMetrics["BTC Dominance"] = $"{btcDom:N1}%";
                
                if (global.Data.CapPercentages.TryGetValue("eth", out var ethDom))
                    insight.KeyMetrics["ETH Dominance"] = $"{ethDom:N1}%";

                if (global.Data.TotalCap.TryGetValue("usd", out var totalCap))
                    insight.KeyMetrics["Total Market Cap"] = $"${(totalCap / 1e12):N2}T";
            }
        }
        catch (Exception ex) { Console.WriteLine($"[ERROR] Global API: {ex.Message}"); }

        // Fallback or extra hardcoded if APIs fail
        if (!insight.KeyMetrics.ContainsKey("BTC Dominance")) insight.KeyMetrics["BTC Dominance"] = "52.4%";
        
        return insight;
    }

    public async Task<List<MarketTrend>> GetCurrentTrendsAsync()
    {
        await Task.Delay(50);
        return new List<MarketTrend>
        {
            new MarketTrend 
            { 
                Title = _lang.T("Trend1Title"), 
                Summary = _lang.T("Trend1Summary"), 
                Impact = _lang.T("ImpactPositive"),
                Tags = new List<string> { "BTC", "On-chain" }
            },
            new MarketTrend 
            { 
                Title = _lang.T("Trend2Title"), 
                Summary = _lang.T("Trend2Summary"), 
                Impact = _lang.T("ImpactNeutral"),
                Tags = new List<string> { "Regulação", "Europa" }
            },
            new MarketTrend 
            { 
                Title = _lang.T("Trend3Title"), 
                Summary = _lang.T("Trend3Summary"), 
                Impact = _lang.T("ImpactPositive"),
                Tags = new List<string> { "ETH", "L2" }
            }
        };
    }
}
