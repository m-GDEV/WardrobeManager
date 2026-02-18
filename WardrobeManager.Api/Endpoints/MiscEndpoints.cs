using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Interfaces;

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

    public static async Task<IResult> AddLog([FromBody] Log givenLog, ILoggingService loggingService)
    {
        try
        {
            await loggingService.CreateDatabaseAndConsoleLog(givenLog);
            return Results.Ok();
        }
        catch
        {
            return Results.Problem("Could not create log.");
        }
    }
}

