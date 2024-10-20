﻿using System.Net.Http.Json;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using WardrobeManager.Presentation.Pages;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Misc;
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

    public async Task<HttpResponseMessage> CheckApiConnection()
    {
        HttpResponseMessage con = await _httpClient.GetAsync("/ping");
        return con;
    }

    public async Task<List<ServerClothingItem>?> GetClothing()
    {
        var clothing = await _httpClient.GetFromJsonAsync<List<ServerClothingItem>>( "/clothing");
        return clothing;
    }

    public async Task<List<ServerClothingItem>?> GetFilteredClothing(FilterModel model)
    {
        var res = await _httpClient.PostAsJsonAsync<FilterModel>( "/clothing/filtered", model);
        res.EnsureSuccessStatusCode();
        var clothing = await res.Content.ReadFromJsonAsync<List<ServerClothingItem>?>();
        return clothing;
    }

    public async Task Add(NewOrEditedClothingItemDTO clothing)
    {
        var res = await _httpClient.PostAsJsonAsync<NewOrEditedClothingItemDTO>( "/clothing", clothing);
        res.EnsureSuccessStatusCode();
    }

    public async Task Update(NewOrEditedClothingItemDTO clothing)
    {
        var res = await _httpClient.PutAsJsonAsync<NewOrEditedClothingItemDTO>( "/clothing", clothing);
        res.EnsureSuccessStatusCode();
    }

    public async Task Delete(ServerClothingItem clothing)
    {
        var res = await _httpClient.DeleteAsync($"/clothing/{clothing.Id}");
        res.EnsureSuccessStatusCode();
    }
    public async Task Wear(ServerClothingItem clothing)
    {
        var res = await _httpClient.GetAsync($"/actions/wear/{clothing.Id}");
        res.EnsureSuccessStatusCode();
    }
    public async Task Wash(ServerClothingItem clothing)
    {
        var res = await _httpClient.GetAsync($"/actions/wash/{clothing.Id}");
        res.EnsureSuccessStatusCode();
    }

    public async ValueTask DisposeAsync()
    {
        _httpClient.Dispose();
    }
}
