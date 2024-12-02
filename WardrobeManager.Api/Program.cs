using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Services.Interfaces;
using WardrobeManager.Shared.Exceptions;
using System.Diagnostics;
using System.Security.Claims;
using WardrobeManager.Shared.Services.Implementation;
using WardrobeManager.Api.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WardrobeManager.Api;
using WardrobeManager.Api.Database.Models;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Configuration is setup by default to read from (in order of precedence) Environment Variables, appsettings.json
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
string BackendUrl = builder.Configuration["WM_BACKEND_URL"] ?? throw new Exception("BackendUrl: configuration value not set");
string FrontendUrl = builder.Configuration["WM_FRONTEND_URL"] ?? throw new Exception("Frontend: configuration value not set");

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
// Configure app cookie (THIS ALLOWS THE BACKEND AND FRONTEND RUN ON DIFFERENT DOMAINS)
//
// The default values, which are appropriate for hosting the Backend and
// BlazorWasmAuth apps on the same domain, are Lax and SameAsRequest. 
// For more information on these settings, see:
// https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity#cross-domain-hosting-same-site-configuration
/*
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
*/
builder.Services.AddAuthorizationBuilder();

// Add identify and opt-in to endpoints 
builder.Services.AddIdentityCore<AppUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    // this maps some Identity API endpoints like '/register' check Swagger for all of them
    .AddApiEndpoints();

// Add a CORS policy for the client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([
                BackendUrl,
                FrontendUrl
            ])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

// services added by me but created by Microsfot
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger can use JWT Bearer auth
builder.Services.AddSwaggerGen();

// Custom Services Added by Musa
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<DatabaseContext>();

builder.Services.AddScoped<IClothingItemService, ClothingItemService>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoggingService, LoggingSerivce>();
builder.Services.AddSingleton<IDataDirectoryService, DataDirectoryService>();

// add db context
builder.Services.AddDbContext<DatabaseContext>((serviceProvider, options) =>
{
    var dataDirectoryService = serviceProvider.GetRequiredService<IDataDirectoryService>();
    var dbPath = Path.Combine(dataDirectoryService.GetDatabaseDirectory(), "database.db");
    options.UseSqlite($"Data Source={dbPath}");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adds a bunch of auto generated Identity-related routes (/register, /login, etc)
app.MapIdentityApi<AppUser>();
app.UseCors("wasm");

app.UseHttpsRedirection();
app.UseExceptionHandler();

// Enable authentication and authorization after CORS Middleware
// processing (UseCors) in case the Authorization Middleware tries
// to initiate a challenge before the CORS Middleware has a chance
// to set the appropriate headers.
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


// Mapping custom endpoints
app.MapUserEndpoints();
app.MapClothingEndpoints();
app.MapImageEndpoints();
app.MapMiscEndpoints();
app.MapActionEndpoints();
app.MapIdentityEndpoints();

// Custom Middleware
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-8.0
app.UseMiddleware<UserCreationMiddleware>();

// Initialize DB (only run if db doesn't exist)
using var scope = app.Services.CreateScope();
await DatabaseInitializer.InitializeAsync(scope);

app.Run();