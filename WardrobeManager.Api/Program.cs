using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Services.Interfaces;
using WardrobeManager.Api.Database.Services.Implementation;
using WardrobeManager.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Services.Interfaces;
using WardrobeManager.Shared.Exceptions;
using System.Diagnostics;
using WardrobeManager.Shared.Services.Implementation;
using WardrobeManager.Api.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using WardrobeManager.Api;


var builder = WebApplication.CreateBuilder(args);

// add db context
builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseSqlite("Data Source=database.dat")
        );

// services added by me but created by microsft
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger can use JWT Bearer auth
builder.Services.AddSwaggerGen(opt =>
        {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
                });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                new OpenApiSecurityScheme
                {
                Reference = new OpenApiReference
                {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
                }
                },
                new string[]{}
                }
                });
        });

// custom services
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<DatabaseContext>();

builder.Services.AddScoped<IClothingItemService, ClothingItemService>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoggingService, LoggingSerivce>();

// auth0
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
            {
            c.Authority = domain;
            c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = domain,
            };
            });

builder.Services.AddAuthorization(o =>
        {
        o.AddPolicy("clothing:read-write", p => p.
                RequireAuthenticatedUser().
                RequireClaim("scope", "clothing:read-write"));
        });

builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
}

// auth0
app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();
app.UseExceptionHandler();


// Mapping custom endpoints
app.MapUserEndpoints();
app.MapClothingEndpoints();
app.MapImageEndpoints();
app.MapMiscEndpoints();
app.MapActionEndpoints();


// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-8.0
// middleware (convert into class when you clean up this file)
app.Use(async (HttpContext context, RequestDelegate next) => {

        var userService = context.RequestServices.GetRequiredService<IUserService>();


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
var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
DatabaseInitializer.Initialize(context);

app.Run();
