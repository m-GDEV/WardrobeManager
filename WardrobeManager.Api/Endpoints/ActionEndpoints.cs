using System;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Services.Interfaces;


namespace WardrobeManager.Api.Endpoints;

public static class ActionEndpoints {
    public static void MapActionEndpoints(this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/actions").RequireAuthorization();

        group.MapGet("/wear/{id}", WearClothing);
        group.MapGet("/wash/{id}", WashClothing);
    }

    public static async Task<IResult> WearClothing
        (
         int itemId, HttpContext context, IClothingItemService clothingItemService
        ){
            User? user = context.Items["user"] as User;
            Debug.Assert(user != null, "Cannot get user");

            await clothingItemService.CallMethodOnClothingItem(user.Id, itemId, ActionType.Wear);

            return Results.Ok("Item Worn");
        }
    public static async Task<IResult> WashClothing
        (
         int itemId, HttpContext context, IClothingItemService clothingItemService
        ){
            User? user = context.Items["user"] as User;
            Debug.Assert(user != null, "Cannot get user");

            await clothingItemService.CallMethodOnClothingItem(user.Id, itemId, ActionType.Wash);
            return Results.Ok("Item Worn");
        }
}
