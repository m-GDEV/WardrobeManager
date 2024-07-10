using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Services.Interfaces;
using WardrobeManager.Api.Database.Services.Implementation;
using WardrobeManager.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Services.Interfaces;
using WardrobeManager.Shared.Services.Implementation;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// add db context
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=database.dat")
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.WebHost.UseUrls("http://localhost:9865");

// custom
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddScoped<IClothingItemService, ClothingItemService>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<IFileService, FileService>();
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

app.UseHttpsRedirection();

// Initialize DB (only run if db doesn't exist)
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
DatabaseInitializer.Initialize(context);


app.MapGet("/clothing", async Task<Array> (IClothingItemService clothingItemService) =>
{
    var clothes = await clothingItemService.GetClothes();

    return clothes.ToArray();   
}).WithName("Get Clothes").WithOpenApi();


app.MapDelete("/clothingitem", async Task<IResult> (int? Id, IClothingItemService clothingItemService) =>
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


app.MapPut("/clothingitem", async Task<IResult> ([FromBody] NewOrEditedClothingItem editedItem, IClothingItemService clothingItemService, ISharedService sharedService, IFileService fileService) =>
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

app.MapPost("/clothingitem", async Task<IResult> ([FromBody] NewOrEditedClothingItem newItem, IClothingItemService clothingItemService, ISharedService sharedService, IFileService fileService) => 
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


app.MapGet("/img/{*imagePath}", async Task<IResult> (string imagePath, IFileService fileService) =>
{
    return Results.File(await fileService.GetImage(imagePath));
    
    // should implement
    //return Results.NotFound("Image not found");

}).WithName("Get Image").WithOpenApi();

app.Run();