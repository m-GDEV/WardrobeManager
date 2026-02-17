#region

using System.Diagnostics;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Api.Endpoints;

public static class ActionEndpoints {
    public static void MapActionEndpoints(this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/actions").RequireAuthorization();

        group.MapGet("/wear/{id}", WearClothing);
        group.MapGet("/wash/{id}", WashClothing);
    }

    public static async Task<IResult> WearClothing
        (
         int id, HttpContext context, IClothingItemService clothingItemService
        ){
            User? user = context.Items["user"] as User;
            Debug.Assert(user != null, "Cannot get user");

            await clothingItemService.CallMethodOnClothingItem(user.PrimaryKeyId, id, ActionType.Wear);

            return Results.Ok("Item Worn");
        }
    public static async Task<IResult> WashClothing
        (
         int id, HttpContext context, IClothingItemService clothingItemService
        ){
            User? user = context.Items["user"] as User;
            Debug.Assert(user != null, "Cannot get user");

            await clothingItemService.CallMethodOnClothingItem(user.PrimaryKeyId, id, ActionType.Wash);
            return Results.Ok("Item Worn");
        }
}
