#region

using System.Diagnostics;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Api.Endpoints;

public static class ActionEndpoints
{
    public static void MapActionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/actions").RequireAuthorization();
    }
}