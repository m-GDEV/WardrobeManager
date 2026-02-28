#region

using System.Diagnostics;
using System.Security.Claims;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Api.Middleware;

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-8.0
public class UserCreationMiddleware
{
    private readonly RequestDelegate _next;

    public UserCreationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userService = context.RequestServices.GetRequiredService<IUserService>();
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        var user = await userService.GetUser(userId);
        if (user != null)
        {
            // pass along to controllers
            context.Items["user"] = user;
        }

        await _next(context);
    }
}