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
        
        // var userService = context.RequestServices.GetRequiredService<IUserService>();
        // var loggingService = context.RequestServices.GetRequiredService<ILoggingService>();

        // var log = new Log("Request received:", context.Request.ToString() ?? context.Request.Path, LogType.Info, );
        // await loggingService.CreateDatabaseAndConsoleLog(log);


        // -------------- !!IMPORTANT!! -------------
        // Previously I was using Auth0 as an authentication provider.
        // Coincidentally, Microsoft Identity and Auto0 used the NameIdentifier claim for a user
        // So all the old code still works 100% fine. 
        // For this reason I will not be modifying anything Auth0 at the moment
        // var Auth0Id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // only create the user if the consumer of the api is authorized
        // if (Auth0Id != null)
        // {
        //     await userService.CreateUser(Auth0Id);
        //     Debug.Assert(await userService.DoesUserExist(Auth0Id) == true, "User finding/creating is broken");
        //
        //     var user = await userService.GetUser(Auth0Id);
        //     Debug.Assert(user != null, "At this point in the pipeline user should exist");
        //
        //     // pass along to controllers
        //     context.Items["user"] = user;
        // }

        await _next(context);
    }
}