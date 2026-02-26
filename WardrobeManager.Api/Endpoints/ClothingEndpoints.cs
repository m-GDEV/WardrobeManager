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
    }

    // ---------------------
    // Get all clothing items
    // ---------------------
    public static async Task<IResult> GetClothing(
            HttpContext context, IClothingService clothingService, DatabaseContext _context
            ){
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        var clothes = await clothingService.GetAllClothingAsync(user.Id);

        return Results.Ok(clothes);
    }

}
