using System;
using System.Collections.Generic;

namespace CryptoMonitor.Services;

public enum Language { ENG, PT }

public interface ILanguageService
{
    Language CurrentLanguage { get; }
    void SetLanguage(Language language);
    string T(string key);
    event Action OnLanguageChanged;
}

public class LanguageService : ILanguageService
{
    public Language CurrentLanguage { get; private set; } = Language.ENG;
    public event Action? OnLanguageChanged;

    private readonly Dictionary<Language, Dictionary<string, string>> _translations = new()
    {
        [Language.ENG] = new()
        {
            ["AppTitle"] = "Market Sentinel",
            ["AppSubtitle"] = "Top 100 Crypto Monitoring & Macro Intelligence",
            ["MacroTitle"] = "Global Macro Landscape",
            ["MacroDescription"] = "Global markets are closely watching US inflation data and FED movements. Global liquidity shows signs of recovery, favoring risk assets.",
            ["DailyOutlook"] = "Daily Outlook",
            ["DailyOutlookDesc"] = "Bitcoin accumulation trend after support at key levels. Institutional interest in ETFs remains the primary volume driver.",
            ["KeyIndicators"] = "Key Indicators",
            ["TrendsTitle"] = "Market Trends",
            ["FilterPlaceholder"] = "Filter by name or symbol...",
            ["UpdateBtn"] = "Update Data",
            ["Syncing"] = "Syncing with the market...",
            ["Rank"] = "#",
            ["Asset"] = "Asset",
            ["Price"] = "Price (USD)",
            ["1h"] = "1h",
            ["24h"] = "24h",
            ["7d"] = "7d",
            ["MarketCap"] = "Market Cap",
            ["ImpactPositive"] = "Positive",
            ["ImpactNeutral"] = "Neutral",
            ["ImpactNegative"] = "Negative",
            ["Trend1Title"] = "Halving Impact",
            ["Trend1Summary"] = "Reduced Bitcoin issuance begins to pressure supply on exchanges.",
            ["Trend2Title"] = "European Regulation (MiCA)",
            ["Trend2Summary"] = "New rules bring institutional clarity, facilitating early large funds into the crypto market.",
            ["Trend3Title"] = "Layer 2 Expansion",
            ["Trend3Summary"] = "Ethereum L2s like Arbitrum and Base reach record daily transactions.",
            ["Indicator1"] = "DXY (Dollar Index)",
            ["Indicator2"] = "SPX (S&P 500)",
            ["Indicator3"] = "BTC Dominance",
            ["Indicator1Val"] = "102.5 (Stable)",
            ["Indicator2Val"] = "Rising",
            ["Indicator3Val"] = "52.4%"
        },
        [Language.PT] = new()
        {
            ["AppTitle"] = "Market Sentinel",
            ["AppSubtitle"] = "Monitoramento de Top 100 Cryptos e Inteligência Macro",
            ["MacroTitle"] = "Panorama Macro Mundial",
            ["MacroDescription"] = "Os mercados globais observam de perto os dados de inflação dos EUA e as movimentações do FED. A liquidez global mostra sinais de recuperação, favorecendo ativos de risco.",
            ["DailyOutlook"] = "Resumo Diário",
            ["DailyOutlookDesc"] = "Tendência de acumulação para o Bitcoin após suporte em níveis chave. O interesse institucional em ETFs continua sendo o principal driver de volume.",
            ["KeyIndicators"] = "Indicadores Chave",
            ["TrendsTitle"] = "Tendências de Mercado",
            ["FilterPlaceholder"] = "Filtrar por nome ou símbolo...",
            ["UpdateBtn"] = "Atualizar Dados",
            ["Syncing"] = "Sincronizando com o mercado...",
            ["Rank"] = "#",
            ["Asset"] = "Ativo",
            ["Price"] = "Preço (USD)",
            ["1h"] = "1h",
            ["24h"] = "24h",
            ["7d"] = "7d",
            ["MarketCap"] = "Market Cap",
            ["ImpactPositive"] = "Positivo",
            ["ImpactNeutral"] = "Neutro",
            ["ImpactNegative"] = "Negativo",
            ["Trend1Title"] = "Impacto do Halving",
            ["Trend1Summary"] = "A redução na emissão de novos Bitcoins começa a pressionar a oferta nas exchanges.",
            ["Trend2Title"] = "Regulação Europeia (MiCA)",
            ["Trend2Summary"] = "Novas regras trazem clareza institucional, facilitando a entrada de grandes fundos no mercado cripto.",
            ["Trend3Title"] = "Expansão de Layer 2",
            ["Trend3Summary"] = "Ethereum L2s como Arbitrum e Base atingem recordes de transações diárias.",
            ["Indicator1"] = "DXY (Índice Dólar)",
            ["Indicator2"] = "SPX (S&P 500)",
            ["Indicator3"] = "Dominância BTC",
            ["Indicator1Val"] = "102.5 (Estável)",
            ["Indicator2Val"] = "Subindo",
            ["Indicator3Val"] = "52.4%"
        }
    };

    public void SetLanguage(Language language)
    {
        if (CurrentLanguage != language)
        {
            CurrentLanguage = language;
            OnLanguageChanged?.Invoke();
        }
    }

    public string T(string key)
    {
        if (_translations[CurrentLanguage].TryGetValue(key, out var translation))
        {
            return translation;
        }
        return key;
    }
}
