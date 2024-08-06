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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// add db context
builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseSqlite("Data Source=database.dat")
        );

// services added by me but created by microsft
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen();
// builder.Services.AddAuthentication();

//builder.WebHost.UseUrls("http://localhost:9865");

// custom services
builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddScoped<IClothingItemService, ClothingItemService>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();

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
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = domain;
//        options.Audience = builder.Configuration["Auth0:Audience"];
//    });


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
        // pass along to controllers
        context.Items["Auth0Id"] = Auth0Id;
        }

        await next(context);
        });



// Initialize DB (only run if db doesn't exist)
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
DatabaseInitializer.Initialize(context);

// ----------------------------------
// Miscellaneous
// ----------------------------------
app.MapGet("/ping", async Task<IResult> (HttpContext context) =>
        {
        return Results.Ok("WardrobeManager API is active");
        });

// ----------------------------------
// All clothing items
// ----------------------------------
app.MapGet("/clothing", async Task<IResult> (HttpContext context, IUserService userService, DatabaseContext con) =>
        {
        // assuming user exists in db because of middleware
        // assuming this exists because the user is logged in
        string Auth0Id = context.Items["Auth0Id"].ToString() ?? string.Empty;
        Debug.Assert(Auth0Id != string.Empty, "Cannot get Auth0Id");

        var user = await userService.GetUserWithClothingItems(Auth0Id);
        Debug.Assert(user != null, "User should not be null");

        return Results.Ok(user.ServerClothingItems);

        }).RequireAuthorization();

// ----------------------------------
// GET, POST, PUT for one clothing item
// ----------------------------------
app.MapPost("/clothingitem",  async Task<IResult> ([FromBody] NewOrEditedClothingItem newItem, IClothingItemService clothingItemService, ISharedService sharedService, IFileService fileService, IUserService userService, HttpContext context, DatabaseContext _context) =>
        {
        if (!sharedService.IsValid(newItem.ClothingItem)) {
        return Results.BadRequest("Invalid clothing item");
        }

        string Auth0Id = context.Items["Auth0Id"].ToString() ?? string.Empty;
        Debug.Assert(Auth0Id != string.Empty, "Cannot get Auth0Id");

        var user = await userService.GetUser(Auth0Id);
        Debug.Assert(user != null, "User should not be null");


        var newClothingItem = new ServerClothingItem
        (
         name: newItem.ClothingItem.Name,
         category: newItem.ClothingItem.Category,
         season: newItem.ClothingItem.Season,
         imageGuid: Guid.NewGuid()
        );

        // decode and save file to place on disk with guid as name
        // make new obj of server clothing item and and use guid from before
        // return "Created"
        await fileService.SaveImage(newClothingItem.ImageGuid, newItem.ImageBase64);

        user.ServerClothingItems.Add(newClothingItem);
        await _context.SaveChangesAsync();


        return Results.Created();

        }).RequireAuthorization();

app.MapPut("/clothingitem", async Task<IResult> ([FromBody] NewOrEditedClothingItem editedItem, IClothingItemService clothingItemService, ISharedService sharedService, IFileService fileService, DatabaseContext _context, IUserService userService, HttpContext context)  =>
        {
        if (!sharedService.IsValid(editedItem.ClothingItem))
        {
        return Results.NotFound("Item cannot be updated. It can either not be found or the provided data is invalid.");
        }

        string Auth0Id = context.Items["Auth0Id"].ToString() ?? string.Empty;
        Debug.Assert(Auth0Id != string.Empty, "Cannot get Auth0Id");

        var user = await userService.GetUser(Auth0Id);
        Debug.Assert(user != null, "User should not be null");

        var dbRecord = await _context.ClothingItems.FindAsync(editedItem.ClothingItem.Id);

        Debug.Assert(dbRecord != null, "If item is being edited old record must exist");
        Debug.Assert(editedItem.ClothingItem.Id != dbRecord.Id, "Edited and Old item IDs should always match");
        Debug.Assert(editedItem.ClothingItem.UserId != dbRecord.UserId, "Edited and Old item user IDs should always match");


        if (editedItem.ImageBase64 != string.Empty)
        {
            // generate new guid since its a new image
            editedItem.ClothingItem.ImageGuid = Guid.NewGuid();
        }

        // decode and save file to place on disk with guid as name
        // make new obj of server clothing item and and use guid from before
        await fileService.SaveImage(editedItem.ClothingItem.ImageGuid, editedItem.ImageBase64);
        await clothingItemService.Update(editedItem.ClothingItem);

        return Results.Ok();
        }).RequireAuthorization();

app.MapDelete("/clothingitem", async Task<IResult> (int Id, IClothingItemService clothingItemService, HttpContext context, IUserService userService, DatabaseContext _context) =>
        {
        if (Id == 0)
        {
        return Results.BadRequest("You must specify a clothing ID");
        }

        string Auth0Id = context.Items["Auth0Id"].ToString() ?? string.Empty;
        Debug.Assert(Auth0Id != string.Empty, "Cannot get Auth0Id");

        var user = await userService.GetUserWithClothingItems(Auth0Id);
        Debug.Assert(user != null, "User should not be null");

        var itemToDelete = user.ServerClothingItems.SingleOrDefault(x => x.Id == Id);
        if (itemToDelete == null) {
        return Results.NotFound("Could not find item to delete");
        }

        _context.ClothingItems.Remove(itemToDelete);
        await _context.SaveChangesAsync();

        // await clothingItemService.Delete(Id);
        return Results.Ok("Deleted");

        }).RequireAuthorization();


// ----------------------------------
// Images
// ----------------------------------

// No authorization for this for right now since doing <img src="addfs" /> does not include the JWT token 
app.MapGet("/img/{*imagePath}", async Task<IResult> (string imagePath, IFileService fileService) =>
        {
        return Results.File(await fileService.GetImage(imagePath));

        // should implement
        //return Results.NotFound("Image not found");

        });




app.Run();
