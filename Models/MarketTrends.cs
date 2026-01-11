namespace CryptoMonitor.Models;

public class MarketTrend
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Impact { get; set; } = string.Empty; // Positive, Negative, Neutral
    public DateTime Date { get; set; } = DateTime.Now;
    public List<string> Tags { get; set; } = new();
}

public class MacroInsight
{
    public string GlobalContext { get; set; } = string.Empty;
    public string DailyOutlook { get; set; } = string.Empty;
    public Dictionary<string, string> KeyMetrics { get; set; } = new();
    public string FearAndGreedValue { get; set; } = "50";
    public string FearAndGreedLabel { get; set; } = "Neutral";
}
