using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Services.Interfaces;
using WardrobeManager.Api.Database.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// add db context
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=database.dat")
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// custom
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddScoped<IClothingItemService, ClothingItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Initialize DB (only run if db doesn't exist)
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
DatabaseInitializer.Initialize(context);


app.MapGet("/clothingitems", async Task<Array> (IClothingItemService clothingItemService) =>
{
    var clothes = await clothingItemService.GetClothes();

    return clothes.ToArray();   
}).WithName("Get Clothes").WithOpenApi();



app.Run();