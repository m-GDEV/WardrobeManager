#region

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Api.Endpoints;

public static class ClothingEndpoints {
    public static void MapClothingEndpoints(this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/clothing").RequireAuthorization();

        group.MapGet("", GetClothing);
        // maybe should get a GET request, idc rn
        group.MapPost("/filtered", GetFilteredClothing);
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

        var clothes = await clothingItemService.GetAllClothing(user.PrimaryKeyId);

        return Results.Ok(clothes);
    }

    // ---------------------
    // Get (but its a POST method) all clothing items - with filters applied
    // ---------------------
    public static async Task<IResult> GetFilteredClothing(
            [FromBody] FilterModel model, HttpContext context, IClothingItemService clothingItemService, DatabaseContext _context
            ){
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        var clothes = await clothingItemService.GetFilteredClothing(user.PrimaryKeyId, model);

        return Results.Ok(clothes);
    }

    // ---------------------
    // Add a new clothing item (these comments are unecessar but i don't want no comments)
    // ---------------------
    public static async Task<IResult> AddClothingItem(
            [FromBody] NewOrEditedClothingItemDTO  newItem, IClothingItemService clothingItemService, IFileService fileService, IUserService userService, HttpContext context, DatabaseContext _context
            ){
        // if (!sharedService.IsValid(newItem.ClothingItem)) {
        //     return Results.BadRequest("Invalid clothing item");
        // }
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await clothingItemService.AddClothingItem(user.PrimaryKeyId, newItem);

        return Results.Created();
    }

    // ---------------------
    // Edit one clothing item
    // ---------------------
    public static async Task<IResult> EditClothingItem(
            int id, [FromBody] NewOrEditedClothingItemDTO editedItem, IClothingItemService clothingItemService, IFileService fileService, DatabaseContext _context, IUserService userService, HttpContext context
            ){

        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await clothingItemService.UpdateClothingItem(user.PrimaryKeyId, id, editedItem);

        return Results.Ok();
    }

    // ---------------------
    // Delete one clothing item
    // ---------------------
    public static async Task<IResult> DeleteClothingItem(
            int id, IClothingItemService clothingItemService, HttpContext context, IUserService userService, DatabaseContext _context
            ){
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await clothingItemService.DeleteClothingItem(user.PrimaryKeyId,id);

        return Results.Ok("Deleted");
    }

}
