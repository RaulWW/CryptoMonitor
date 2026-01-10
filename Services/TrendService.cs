using CryptoMonitor.Models;

namespace CryptoMonitor.Services;

public interface ITrendService
{
    Task<MacroInsight> GetMacroInsightsAsync();
    Task<List<MarketTrend>> GetCurrentTrendsAsync();
}

public class TrendService : ITrendService
{
    // Simulation of an AI-driven or News-aggregated data source
    public async Task<MacroInsight> GetMacroInsightsAsync()
    {
        await Task.Delay(100); // Simulate network
        return new MacroInsight
        {
            GlobalContext = "Os mercados globais observam de perto os dados de inflação dos EUA e as movimentações do FED. A liquidez global mostra sinais de recuperação, favorecendo ativos de risco.",
            DailyOutlook = "Tendência de acumulação para o Bitcoin após suporte em níveis chave. O interesse institucional em ETFs continua sendo o principal driver de volume.",
            KeyMetrics = new Dictionary<string, string>
            {
                { "DXY (Dólar Index)", "102.5 (Estável)" },
                { "SPX (S&P 500)", "Subindo" },
                { "Dominância BTC", "52.4%" }
            }
        };
    }

    public async Task<List<MarketTrend>> GetCurrentTrendsAsync()
    {
        await Task.Delay(100);
        return new List<MarketTrend>
        {
            new MarketTrend 
            { 
                Title = "Halving Impact", 
                Summary = "A redução na emissão de novos Bitcoins começa a pressionar a oferta nas exchanges.", 
                Impact = "Positive",
                Tags = new List<string> { "BTC", "On-chain" }
            },
            new MarketTrend 
            { 
                Title = "Regulação Europeia (MiCA)", 
                Summary = "Novas regras trazem clareza institucional, facilitando a entrada de grandes fundos no mercado cripto.", 
                Impact = "Neutral",
                Tags = new List<string> { "Regulação", "Europa" }
            },
            new MarketTrend 
            { 
                Title = "Expansão de Layer 2", 
                Summary = "Ethereum L2s como Arbitrum e Base atingem recordes de transações diárias.", 
                Impact = "Positive",
                Tags = new List<string> { "ETH", "L2" }
            }
        };
    }
}
