using System;
using WardrobeManager.Api.Database;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Services.Interfaces;


namespace WardrobeManager.Api.Endpoints;

public static class MiscEndpoints {
    public static void MapMiscEndpoints(this IEndpointRouteBuilder app) {
        // Since these are misc. authorization might not be necessary
        app.MapGet("/ping", Ping);
    }

    public static IResult Ping(HttpContext context) {
        if (context.User.Identity?.IsAuthenticated == true) {
            return Results.Ok("Authenticated: WardrobeManager API is active.");
        }
        return Results.Ok("Unauthenticated: WardrobeManager API is active.");
    }
}

