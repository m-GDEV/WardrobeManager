using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.DTOs;

namespace WardrobeManager.Api.Endpoints;

public static class MiscEndpoints {
    public static void MapMiscEndpoints(this IEndpointRouteBuilder app) {
        // Since these are misc. authorization might not be necessary
        app.MapGet("/ping", Ping);
        app.MapGet("/", Ping); // don't want blank response on /
        app.MapPost("/add-log", AddLog); 
    }

    public static IResult Ping(HttpContext context) {
        if (context.User.Identity?.IsAuthenticated == true) {
            return Results.Ok("Authenticated: WardrobeManager API is active.");
        }
        return Results.Ok("Unauthenticated: WardrobeManager API is active.");
    }

    public static async Task<IResult> AddLog([FromBody] LogDTO givenLog, ILoggingService loggingService, IMapper mapper)
    {
        try
        {
            Log log = mapper.Map<Log>(givenLog);
            await loggingService.CreateDatabaseAndConsoleLog(log);
            return Results.Ok();
        }
        catch
        {
            return Results.Problem("Could not create log.");
        }
    }
}

