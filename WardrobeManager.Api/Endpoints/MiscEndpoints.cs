using System;
using WardrobeManager.Api.Database;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Services.Interfaces;


namespace WardrobeManager.Api.Endpoints;

public static class MiscEndpoints {
    public static void MapMiscEndpoints(this IEndpointRouteBuilder app) {
        // Since these are misc. authorization might not be necessary
        app.MapGet("/ping", Ping);
        app.MapGet("/", Ping); // don't want blank response on /
    }

    public static IResult Ping(HttpContext context) {
        if (context.User.Identity?.IsAuthenticated == true) {
            return Results.Ok("Authenticated: WardrobeManager API is active.");
        }
        return Results.Ok("Unauthenticated: WardrobeManager API is active.");
    }
}

