using CryptoMonitor.Models;

namespace CryptoMonitor.Services;

public interface ITrendService
{
    Task<MacroInsight> GetMacroInsightsAsync();
    Task<List<MarketTrend>> GetCurrentTrendsAsync();
}

public class TrendService : ITrendService
{
    private readonly ILanguageService _lang;

    public TrendService(ILanguageService lang)
    {
        _lang = lang;
    }

    public async Task<MacroInsight> GetMacroInsightsAsync()
    {
        await Task.Delay(50); // Simulate network
        return new MacroInsight
        {
            GlobalContext = _lang.T("MacroDescription"),
            DailyOutlook = _lang.T("DailyOutlookDesc"),
            KeyMetrics = new Dictionary<string, string>
            {
                { _lang.T("Indicator1"), _lang.T("Indicator1Val") },
                { _lang.T("Indicator2"), _lang.T("Indicator2Val") },
                { _lang.T("Indicator3"), _lang.T("Indicator3Val") }
            }
        };
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
