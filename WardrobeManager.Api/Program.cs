#region

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Endpoints;
using WardrobeManager.Api.Middleware;
using WardrobeManager.Api.Repositories;
using WardrobeManager.Api.Repositories.Implementation;
using WardrobeManager.Api.Repositories.Interfaces;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.DTOs;

#endregion

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
builder.Services.AddIdentityCore<User>(options =>
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

builder.Services.AddScoped<IClothingService, ClothingService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddSingleton<IDataDirectoryService, DataDirectoryService>();


// Automapper
builder.Services.AddAutoMapper(cfg => 
{
    cfg.LicenseKey = builder.Configuration["AutoMapper_License_Key"];
    
    // Add your maps here directly
    cfg.CreateMap<Log, LogDTO>().ReverseMap();
    cfg.CreateMap<NewClothingItemDTO, ClothingItem>();
    cfg.CreateMap<ClothingItemDTO, ClothingItem>().ReverseMap();
});

// Entity Services
builder.Services.AddScoped<IGenericRepository<Log>, GenericRepository<Log>>();
builder.Services.AddScoped<IClothingRepository, ClothingRepository>();

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
app.MapIdentityApi<User>();
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
app.UseMiddleware<LoggingMiddleware>();

// Initialize DB (only run if db doesn't exist)
using var scope = app.Services.CreateScope();
await DatabaseInitializer.InitializeAsync(scope);

app.Run();