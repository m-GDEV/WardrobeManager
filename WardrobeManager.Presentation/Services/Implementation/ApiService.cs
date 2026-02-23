#region

using System.Net.Http.Json;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Presentation.Services.Implementation;

public class ApiService : IAsyncDisposable, IApiService
{
    private readonly string _apiEndpoint;
    private readonly HttpClient _httpClient;

    public ApiService(string apiEndpoint, IHttpClientFactory factory)
    {
        _apiEndpoint = apiEndpoint;
        _httpClient = factory.CreateClient("Auth");
    }

    public async Task<bool> CheckApiConnection()
    {
        // Probably not the best way since this would require wrapping each method in a try/catch but it works for now
        try
        {
            HttpResponseMessage con = await _httpClient.GetAsync("/ping");
            return con.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<HttpResponseMessage> AddLog(LogDTO log)
    {
        HttpResponseMessage con = await _httpClient.PostAsJsonAsync("/add-log", log);
        return con;
    }

    public async Task<bool> DoesAdminUserExist()
    {
        var res = await _httpClient.GetAsync("/does-admin-user-exist");
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<(bool, string)> CreateAdminUserIfMissing(AdminUserCredentials credentials)
    {
        var res = await _httpClient.PostAsJsonAsync("/create-admin-user-if-missing", credentials);

        // Not ensuring success status code cus this could fail and it would be not a big deal
        // For some reason its saying the string might be null. The endpoint is currently setup to return a string so I'm throwing an exception if it doesn't
        var content = await res.Content.ReadAsStringAsync();

        // When returning the "Created" http code nothing is returned in body
        if (content == string.Empty)
        {
            content = "Admin user created!";
        }

        return (res.IsSuccessStatusCode, content);
    }

    public async ValueTask DisposeAsync()
    {
        _httpClient.Dispose();
    }
}