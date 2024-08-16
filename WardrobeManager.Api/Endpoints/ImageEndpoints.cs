using System;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Services.Interfaces;


namespace WardrobeManager.Api.Endpoints;

public static class ImageEndpoints {
    public static void MapImageEndpoints(this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/images").RequireAuthorization();

        group.MapGet("/{id}", GetImage);
    }

    // No authorization for this for right now since doing <img src="addfs" /> does not include the JWT token
    public static async Task<IResult> GetImage(
            string imageGuid, IFileService fileService
            ){
        return Results.File(await fileService.GetImage(imageGuid));

        // should implement - maybe not idk
        //return Results.NotFound("Image not found");
    }
}

