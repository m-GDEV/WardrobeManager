using System;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Services.Interfaces;


namespace WardrobeManager.Api.Endpoints;

public static class ClothingEndpoints {
    public static void MapClothingEndpoints(this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/clothing").RequireAuthorization();

        group.MapGet("", GetClothing);
        group.MapPost("", AddClothingItem);
        group.MapPut("/{id}", EditClothingItem);
        group.MapDelete("/{id}", DeleteClothingItem);
    }

    // ---------------------
    // Get all clothing items
    // ---------------------
    public static async Task<IResult> GetClothing(
            HttpContext context, IClothingItemService clothingItemService, DatabaseContext _context
            ){
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        var clothes = await clothingItemService.GetAllClothing(user.Id);

        return Results.Ok(clothes);
    }

    // ---------------------
    // Add a new clothing item (these comments are unecessar but i don't want no comments)
    // ---------------------
    public static async Task<IResult> AddClothingItem(
            [FromBody] NewOrEditedClothingItemDTO  newItem, IClothingItemService clothingItemService, ISharedService sharedService, IFileService fileService, IUserService userService, HttpContext context, DatabaseContext _context
            ){
        // if (!sharedService.IsValid(newItem.ClothingItem)) {
        //     return Results.BadRequest("Invalid clothing item");
        // }
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await clothingItemService.AddClothingItem(user.Id, newItem);

        return Results.Created();
    }

    // ---------------------
    // Edit one clothing item
    // ---------------------
    public static async Task<IResult> EditClothingItem(
            int id, [FromBody] NewOrEditedClothingItemDTO editedItem, IClothingItemService clothingItemService, ISharedService sharedService, IFileService fileService, DatabaseContext _context, IUserService userService, HttpContext context
            ){

        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await clothingItemService.UpdateClothingItem(user.Id, id, editedItem);

        return Results.Ok();
    }

    // ---------------------
    // Delete one clothing item
    // ---------------------
    public static async Task<IResult> DeleteClothingItem(
            int itemId, IClothingItemService clothingItemService, HttpContext context, IUserService userService, DatabaseContext _context
            ){
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await clothingItemService.DeleteClothingItem(user.Id,itemId);

        return Results.Ok("Deleted");
    }

}
