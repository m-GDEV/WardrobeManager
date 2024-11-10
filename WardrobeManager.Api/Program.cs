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

// add db context
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite("Data Source=database.db"));

// Add identify and opt-in to endpoints 
builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    // this maps some Identity API endpoints like '/register' check Swagger for all of them
    .AddApiEndpoints(); 

// Add a CORS policy for the client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:5001", 
                builder.Configuration["FrontendUrl"] ?? "https://localhost:5002"])
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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


// Mapping custom endpoints
app.MapUserEndpoints();
app.MapClothingEndpoints();
app.MapImageEndpoints();
app.MapMiscEndpoints();
app.MapActionEndpoints();


// middleware (convert into class when you clean up this file)
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-8.0
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    var userService = context.RequestServices.GetRequiredService<IUserService>();

    
    // -------------- !!IMPORTANT!! -------------
    // Previously I was using Auth0 as an authentication provider.
    // Coincidentally, Microsoft Identity and Auto0 used the NameIdentifier claim for a user
    // So all the old code still works 100% fine. 
    // For this reason I will not be modifying anything Auth0 at the moment
    var Auth0Id = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    // only create the user if the consumer of the api is authorized
    if (Auth0Id != null)
    {
        await userService.CreateUser(Auth0Id);
        Debug.Assert(await userService.DoesUserExist(Auth0Id) == true, "User finding/creating is broken");

        var user = await userService.GetUser(Auth0Id);
        Debug.Assert(user != null, "At this point in the pipeline user should exist");

        // pass along to controllers
        context.Items["user"] = user;
    }

    await next(context);
});


// Initialize DB (only run if db doesn't exist)
using var scope = app.Services.CreateScope();
await DatabaseInitializer.Initialize(scope);

// auth endpoints (remove later)

// Provide an end point to clear the cookie for logout
// For more information on the logout endpoint and antiforgery, see:
// https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity#antiforgery-support
app.MapPost("/logout", async (SignInManager<AppUser> signInManager, [FromBody] object empty) =>
{
    if (empty is not null)
    {
        await signInManager.SignOutAsync();

        return Results.Ok();
    }

    return Results.Unauthorized();
}).RequireAuthorization();

app.UseHttpsRedirection();

app.MapGet("/roles", (ClaimsPrincipal user) =>
{
    if (user.Identity is not null && user.Identity.IsAuthenticated)
    {
        var identity = (ClaimsIdentity)user.Identity;
        var roles = identity.FindAll(identity.RoleClaimType)
            .Select(c => 
                new
                {
                    c.Issuer, 
                    c.OriginalIssuer, 
                    c.Type, 
                    c.Value, 
                    c.ValueType
                });

        return TypedResults.Json(roles);
    }

    return Results.Unauthorized();
}).RequireAuthorization();

app.MapPost("/data-processing-1", ([FromBody] FilterModel model) =>
    Results.Text($"{model.SearchQuery.Length} characters (naughty boy"))
    .RequireAuthorization();

app.MapPost("/data-processing-2", ([FromBody] FilterModel model) =>
    Results.Text($"{model.SearchQuery.Length} characters (naughty boy"))
    .RequireAuthorization(policy => policy.RequireRole("Admin"));

app.Run();