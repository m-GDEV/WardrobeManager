#region

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Models;
using AutoMapper;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.StaticResources;

#endregion

namespace WardrobeManager.Api.Endpoints;

public static class ClothingEndpoints
{
    public static void MapClothingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/clothing").RequireAuthorization();

        group.MapGet("", GetClothingAsync);
        group.MapGet("{itemId}", GetClothingItemAsync);
        group.MapPost("/add", AddNewClothingItemAsync);
        group.MapDelete("/delete", DeleteClothingItemAsync);
        // maybe should get a GET request, idc rn
    }

    public static async Task<IResult> GetClothingAsync(
        HttpContext context, IClothingService clothingService
    )
    {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        var clothes = await clothingService.GetAllClothingAsync(user.Id);

        return Results.Ok(clothes);
    }
    
    public static async Task<IResult> GetClothingItemAsync(
        HttpContext context, IClothingService clothingService, [FromQuery] int itemId
    )
    {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        var clothes = await clothingService.GetClothingItemAsync(user.Id, itemId);

        return Results.Ok(clothes);
    }

    public static async Task<IResult> AddNewClothingItemAsync(
        [FromBody] NewClothingItemDTO newClothingItem,
        HttpContext context, IClothingService clothingService, IMapper mapper
    )
    {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        var res = StaticValidators.Validate(newClothingItem);
        if (!res.Success)
        {
            return Results.BadRequest(res.Message);
        }
        await clothingService.AddNewClothingItem(user.Id ,newClothingItem);

        return Results.Ok();
    }
    
    public static async Task<IResult> DeleteClothingItemAsync(
        [FromQuery] int itemId,
        HttpContext context, IClothingService clothingService, IMapper mapper
    )
    {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");
    
        await clothingService.DeleteClothingItem(user.Id ,itemId);

        return Results.Ok();
    }
}