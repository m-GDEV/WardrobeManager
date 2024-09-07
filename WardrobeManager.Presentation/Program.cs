using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WardrobeManager.Presentation;
using WardrobeManager.Presentation.Services.Implementation;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Services.Implementation;
using WardrobeManager.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using WardrobeManager.Shared.Models;
using static System.Net.WebRequestMethods;



// No appsettings.json so just doing constants here
string apiEndpoint = "https://localhost:7026";


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


// Auth0 api-auth (user can become authorized to use api)

//builder.Services.AddTransient<CustomAuthorizationMessageHandler>();
//builder.Services.AddHttpClient("WebAPI",
//        client => client.BaseAddress = new Uri(apiEndpoint))
//    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-8.0#custom-authorizationmessagehandler-class
builder.Services.AddTransient<CustomAuthorizationMessageHandler>();
builder.Services.AddHttpClient("WebAPI",
        client => client.BaseAddress = new Uri(apiEndpoint))
.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

//builder.Services.AddHttpClient("ServerAPI",
//      client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
//    .AddHttpMessageHandler<AuthorizationMessageHandler>();
//    //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
//  .CreateClient("ServerAPI"));


builder.Services.AddScoped<IApiService, ApiService>(serviceProvider =>
        {
        //var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("ApiService");

        return new ApiService(apiEndpoint, serviceProvider.GetRequiredService<IHttpClientFactory>());
        });
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();


// Auth0 client-side auth (user can become authenticated)
builder.Services.AddOidcAuthentication(options =>
        {
        builder.Configuration.Bind("Auth0", options.ProviderOptions);
        options.ProviderOptions.ResponseType = "code";
        options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
        });



await builder.Build().RunAsync();
