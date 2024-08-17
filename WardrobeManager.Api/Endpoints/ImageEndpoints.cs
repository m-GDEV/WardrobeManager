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
        // No authorization for this for right now since doing <img src="addfs" /> does not include the JWT token
        var group = app.MapGroup("/images");

        group.MapGet("/{id}", GetImage);
    }

    public static async Task<IResult> GetImage(
            string id, IFileService fileService
            ){

        // maybe should implement a 404 response if the image is not found instead of just retruning a 'no image found' png
        return Results.File(await fileService.GetImage(id));
    }
}

