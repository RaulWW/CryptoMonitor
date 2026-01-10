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
            var url = "https://api.coincap.io/v2/assets?limit=100";
            var result = await _httpClient.GetFromJsonAsync<CoinCapResponse>(url);
            
            if (result?.Data == null) return new List<CryptoAsset>();

            return result.Data.Select(CryptoAsset.FromCoinCap).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching crypto data from CoinCap: {ex.Message}");
            return new List<CryptoAsset>();
        }
    }
}
