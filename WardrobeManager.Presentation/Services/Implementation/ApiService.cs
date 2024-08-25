using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using WardrobeManager.Presentation.Pages;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace WardrobeManager.Presentation.Services.Implementation;

public class ApiService : IAsyncDisposable, IApiService
{
    private readonly string _apiEndpoint;
    private readonly HttpClient _httpClient;

    public ApiService(string apiEndpoint, IHttpClientFactory factory) {
        _apiEndpoint = apiEndpoint;
        _httpClient = factory.CreateClient("WebAPI");
    }

    public async Task<List<ServerClothingItem>?> GetClothing()
    {
        var clothing = await _httpClient.GetFromJsonAsync<List<ServerClothingItem>>( "/clothing");
        return clothing;
    }

    public async Task Add(NewOrEditedClothingItemDTO clothing)
    {
        var serialized = JsonSerializer.Serialize(clothing);
        var de_serialized = JsonSerializer.Deserialize<NewOrEditedClothingItemDTO>(serialized);




        var res = await _httpClient.PostAsJsonAsync<NewOrEditedClothingItemDTO>( "/clothingitem", clothing);
        res.EnsureSuccessStatusCode();
    }

    public async Task Update(NewOrEditedClothingItemDTO clothing)
    {
        var res = await _httpClient.PutAsJsonAsync<NewOrEditedClothingItemDTO>( "/clothingitem", clothing);
        res.EnsureSuccessStatusCode();
    }

    public async Task Delete(ServerClothingItem clothing)
    {
        var res = await _httpClient.DeleteAsync( "/clothingitem?id={clothing.Id}");
        res.EnsureSuccessStatusCode();
    }

    [Authorize]
    public async Task<bool> IsUserInitialized() {
        // Exists in DB, not in auth0
        var res = await _httpClient.GetAsync( "/userexists");

        if (res.StatusCode == HttpStatusCode.OK) {
            return true;
        }
        else if (res.StatusCode == HttpStatusCode.NotFound) {
            return false;
        }
        else {
            throw new Exception("Issue with request: " + res);
        }
    }

    [Authorize]
    public async Task CreateUser() {
        var res = await _httpClient.GetAsync("/createuser");

        if (res.StatusCode != HttpStatusCode.Created) {
            throw new Exception("Could not create user on db: " + res);
        }
    }


    public async ValueTask DisposeAsync()
    {
        _httpClient.Dispose();
    }
}
