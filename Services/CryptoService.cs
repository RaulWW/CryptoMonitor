using System.Net.Http.Json;
using System.Text.Json.Serialization;
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
            Console.WriteLine("[INFO] Fetching assets from CoinCap...");
            var result = await TryFetchFromCoinCap();
            if (result.Any()) return result;
            
            Console.WriteLine("[WARN] CoinCap returned empty list. Trying CoinGecko...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] CoinCap failed: {ex.Message}");
        }

        try
        {
            Console.WriteLine("[INFO] Fetching assets from CoinGecko...");
            var result = await TryFetchFromCoinGecko();
            if (result.Any()) return result;

            Console.WriteLine("[WARN] CoinGecko returned empty list. Trying CoinPaprika...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] CoinGecko failed: {ex.Message}");
        }

        try
        {
            Console.WriteLine("[INFO] Fetching assets from CoinPaprika...");
            return await TryFetchFromCoinPaprika();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] All crypto APIs failed. Last exception: {ex.Message}");
            Console.WriteLine($"[DEBUG] StackTrace: {ex.StackTrace}");
            return new List<CryptoAsset>();
        }
    }

    private async Task<List<CryptoAsset>> TryFetchFromCoinCap()
    {
        var url = "https://api.coincap.io/v2/assets?limit=100";
        var response = await _httpClient.GetAsync(url);
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[DEBUG] CoinCap HTTP Error: {response.StatusCode}");
            return new List<CryptoAsset>();
        }

        var result = await response.Content.ReadFromJsonAsync<CoinCapResponse>();
        if (result?.Data == null) return new List<CryptoAsset>();
        return result.Data.Select(CryptoAsset.FromCoinCap).ToList();
    }

    private async Task<List<CryptoAsset>> TryFetchFromCoinGecko()
    {
        // Fallback to CoinGecko Public API
        var url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=100&page=1&sparkline=false";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] CoinGecko HTTP Error: {response.StatusCode} - Body: {body}");
            return new List<CryptoAsset>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<CryptoAsset>>();
        if (result != null)
        {
            foreach (var asset in result) asset.Provider = "CoinGecko";
        }
        return result ?? new List<CryptoAsset>();
    }

    private async Task<List<CryptoAsset>> TryFetchFromCoinPaprika()
    {
        // CoinPaprika has a very easy public API
        var url = "https://api.coinpaprika.com/v1/tickers?limit=100";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[DEBUG] CoinPaprika HTTP Error: {response.StatusCode}");
            return new List<CryptoAsset>();
        }

        var result = await response.Content.ReadFromJsonAsync<List<CoinPaprikaTicker>>();
        return result?.Select(CryptoAsset.FromCoinPaprika).ToList() ?? new List<CryptoAsset>();
    }
}
