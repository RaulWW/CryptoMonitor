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
            var url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=100&page=1&sparkline=false&price_change_percentage=1h,24h,7d";
            var result = await _httpClient.GetFromJsonAsync<List<CryptoAsset>>(url);
            return result ?? new List<CryptoAsset>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching crypto data: {ex.Message}");
            return new List<CryptoAsset>();
        }
    }
}
