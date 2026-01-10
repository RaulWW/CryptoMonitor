using System.Net.Http.Json;
using CryptoMonitor.Models;

namespace CryptoMonitor.Services;

public interface ICryptoService
{
    Task<List<CryptoAsset>> GetTop100AssetsAsync();
}

public class CryptoService : ICryptoService
{
    private readonly HttpClient _httpClient;

    public CryptoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoMonitorApp");
    }

    public async Task<List<CryptoAsset>> GetTop100AssetsAsync()
    {
        try
        {
            return await TryFetchFromCoinCap();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CRITICAL] CoinCap unreachable: {ex.Message}. Trying CoinGecko fallback...");
            return await TryFetchFromCoinGecko();
        }
    }

    private async Task<List<CryptoAsset>> TryFetchFromCoinCap()
    {
        var url = "https://api.coincap.io/v2/assets?limit=100";
        var result = await _httpClient.GetFromJsonAsync<CoinCapResponse>(url);
        if (result?.Data == null) return new List<CryptoAsset>();
        return result.Data.Select(CryptoAsset.FromCoinCap).ToList();
    }

    private async Task<List<CryptoAsset>> TryFetchFromCoinGecko()
    {
        try
        {
            // Fallback to CoinGecko Public API
            var url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=100&page=1&sparkline=false";
            var result = await _httpClient.GetFromJsonAsync<List<CryptoAsset>>(url);
            return result ?? new List<CryptoAsset>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CRITICAL] All crypto APIs failed: {ex.Message}");
            return new List<CryptoAsset>();
        }
    }
}
