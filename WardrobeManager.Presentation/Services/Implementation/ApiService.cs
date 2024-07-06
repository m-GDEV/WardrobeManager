using System.Net.Http.Json;
using System.Text.Json;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Implementation;

public class ApiService(string apiEndpoint) : IAsyncDisposable, IApiService
{
    private readonly string _apiEndpoint = apiEndpoint;
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<List<ClothingItem>?> GetClothing()
    {
        var clothing = await _httpClient.GetFromJsonAsync<List<ClothingItem>>(_apiEndpoint + "/clothing");
        return clothing;
    }

    public async ValueTask DisposeAsync()
    {
        _httpClient.Dispose();
    }
}
