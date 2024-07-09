using System.Net.Http.Json;
using System.Text.Json;
using WardrobeManager.Presentation.Pages;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Implementation;

public class ApiService(string apiEndpoint) : IAsyncDisposable, IApiService
{
    private readonly string _apiEndpoint = apiEndpoint;
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<List<ServerClothingItem>?> GetClothing()
    {
        var clothing = await _httpClient.GetFromJsonAsync<List<ServerClothingItem>>(_apiEndpoint + "/clothing");
        return clothing;
    }

    public async Task Add(ClientClothingItem clothing)
    {
        var res = await _httpClient.PostAsJsonAsync<ClientClothingItem>(_apiEndpoint + "/clothingitem", clothing);
        res.EnsureSuccessStatusCode();
    }

    public async Task Update(ClientClothingItem clothing)
    {
        var res = await _httpClient.PutAsJsonAsync<ClientClothingItem>(_apiEndpoint + "/clothingitem", clothing);
        res.EnsureSuccessStatusCode();
    }

    public async Task Delete(ServerClothingItem clothing)
    {
        var res = await _httpClient.DeleteAsync(_apiEndpoint + $"/clothingitem?id={clothing.Id}");
        res.EnsureSuccessStatusCode();
    }

    public async ValueTask DisposeAsync()
    {
        _httpClient.Dispose();
    }
}
