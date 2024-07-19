using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Services.Interfaces;
using WardrobeManager.Api.Database.Services.Implementation;
using WardrobeManager.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Services.Interfaces;
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
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddScoped<IClothingItemService, ClothingItemService>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<IFileService, FileService>();

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

// Initialize DB (only run if db doesn't exist)
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
DatabaseInitializer.Initialize(context);

// ----------------------------------
// All clothing items
// ----------------------------------
app.MapGet("/ping", async Task<IResult> (HttpContext context) =>
{

    //var userId = context.User.FindFirst()
    var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    string resp = "";
    foreach (var iden in context.User.Identities)
    {
        resp += $"{iden.Name} - {iden.Label}\n";
    }

    var f = context.User;

    resp += $"{DateTime.UtcNow} Service is active - user: {userId}";

    return Results.Ok(resp);
}).RequireAuthorization();

app.MapGet("/createuser", async Task<IResult> (HttpContext context) =>
{
    var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    
    if (userId == null)
    {
        return Results.NotFound("No user ID");
    }


}).RequireAuthorization();



app.MapGet("/clothing", [Authorize] async Task<Array> (IClothingItemService clothingItemService) =>
{
    var clothes = await clothingItemService.GetClothes();

    return clothes.ToArray();   
}).WithName("Get Clothes").WithOpenApi();

// ----------------------------------
// GET, POST, PUT for one clothing item
// ----------------------------------
app.MapDelete("/clothingitem", [Authorize] async Task<IResult> (int? Id, IClothingItemService clothingItemService) =>
{
    if (Id == null || Id == 0)
    {
        return Results.BadRequest("You must specify a clothing ID");
    }

    int properId = Id.Value;

    try
    {
        await clothingItemService.Delete(properId);
        return Results.Ok("Delete");
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex); // change this later, don't wanna expose exception details
    }
}).WithName("Delete clothing item").WithOpenApi();


app.MapPut("/clothingitem", [Authorize] async Task<IResult> ([FromBody] NewOrEditedClothingItem editedItem, IClothingItemService clothingItemService, ISharedService sharedService, IFileService fileService) =>
{
    if (!sharedService.IsValid(editedItem.ClothingItem))
    {
        return Results.NotFound("Item cannot be updated. It can either not be found or the provided data is invalid.");
    }


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
}).WithName("Edit an existing clothing item").WithOpenApi();

app.MapPost("/clothingitem", [Authorize] async Task<IResult> ([FromBody] NewOrEditedClothingItem newItem, IClothingItemService clothingItemService, ISharedService sharedService, IFileService fileService) => 
{
    if (!sharedService.IsValid(newItem.ClothingItem)) {
        return Results.BadRequest("Invalid clothing item");
    }

    var newClothingItem = new ServerClothingItem
    (
        name: newItem.ClothingItem.Name,
        category: newItem.ClothingItem.Category,
        imageGuid: Guid.NewGuid()
    );

    // decode and save file to place on disk with guid as name
    // make new obj of server clothing item and and use guid from before
    // return "Created"
    await fileService.SaveImage(newClothingItem.ImageGuid, newItem.ImageBase64);

    await clothingItemService.Add(newClothingItem);


    return Results.Created();

}).WithName("Create Clothing Item").WithOpenApi();


// ----------------------------------
// Images
// ----------------------------------
app.MapGet("/img/{*imagePath}", [Authorize] async Task<IResult> (string imagePath, IFileService fileService) =>
{
    return Results.File(await fileService.GetImage(imagePath));
    
    // should implement
    //return Results.NotFound("Image not found");

}).WithName("Get Image").WithOpenApi();




app.Run();