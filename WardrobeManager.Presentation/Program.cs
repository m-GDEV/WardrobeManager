using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WardrobeManager.Presentation;
using WardrobeManager.Presentation.Services.Implementation;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Services.Implementation;
using WardrobeManager.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Shared.Models;
using static System.Net.WebRequestMethods;





var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Blazor WASM apps cannot read environment variables as they are not executed on the server
// Configuration is setup by default to read from (in order of precedence) Environment Variables, appsettings.json
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
string BackendUrl = builder.Configuration["WM_BACKEND_URL"] ?? throw new Exception("WM_BACKEND_URL: configuration value not set");

Console.WriteLine("Hello man" + BackendUrl);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Kind of like HttpClient middleware that adds a cookie to the request 
builder.Services.AddTransient<CookieHandler>();

// Setup authorization
builder.Services.AddAuthorizationCore();

// Custom service you can use to perform auth actions (login, logout, etc)
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// I think this associates the interface of the previous Service with the default AuthenticationStateProvider
builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// set base address for default host
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(BackendUrl) });

// configure client for auth interactions
builder.Services.AddHttpClient("Auth", opt => 
        opt.BaseAddress = new Uri(BackendUrl))
    .AddHttpMessageHandler<CookieHandler>();

// My Services
builder.Services.AddScoped<IApiService, ApiService>(sp => new ApiService(BackendUrl, sp.GetRequiredService<IHttpClientFactory>()));
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();



await builder.Build().RunAsync();
