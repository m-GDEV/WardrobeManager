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

#endregion

namespace WardrobeManager.Api.Endpoints;

public static class ClothingEndpoints
{
    public static void MapClothingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/clothing").RequireAuthorization();

        group.MapGet("", GetClothing);
        group.MapPost("/add", AddNewClothingItem);
        group.MapPost("/delete", RemoveClothingItem);
        // maybe should get a GET request, idc rn
    }

    // ---------------------
    // Get all clothing items
    // ---------------------
    public static async Task<IResult> GetClothing(
        HttpContext context, IClothingService clothingService
    )
    {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        var clothes = await clothingService.GetAllClothingAsync(user.Id);

        return Results.Ok(clothes);
    }

    public static async Task<IResult> AddNewClothingItem(
        [FromBody] NewClothingItemDTO newNewClothingItem,
        HttpContext context, IClothingService clothingService, IMapper mapper
    )
    {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await clothingService.AddNewClothingItem(user.Id ,newNewClothingItem);

        return Results.Ok();
    }
    
    public static async Task<IResult> RemoveClothingItem(
        [FromBody] int itemId,
        HttpContext context, IClothingService clothingService, IMapper mapper
    )
    {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await clothingService.RemoveClothingItem(user.Id ,itemId);

        return Results.Ok();
    }
}