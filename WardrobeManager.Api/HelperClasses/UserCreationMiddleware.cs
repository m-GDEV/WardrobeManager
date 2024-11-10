using System.Diagnostics;
using WardrobeManager.Api.Services.Interfaces;

namespace WardrobeManager.Api;

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


        // -------------- !!IMPORTANT!! -------------
        // Previously I was using Auth0 as an authentication provider.
        // Coincidentally, Microsoft Identity and Auto0 used the NameIdentifier claim for a user
        // So all the old code still works 100% fine. 
        // For this reason I will not be modifying anything Auth0 at the moment
        var Auth0Id = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // only create the user if the consumer of the api is authorized
        if (Auth0Id != null)
        {
            await userService.CreateUser(Auth0Id);
            Debug.Assert(await userService.DoesUserExist(Auth0Id) == true, "User finding/creating is broken");

            var user = await userService.GetUser(Auth0Id);
            Debug.Assert(user != null, "At this point in the pipeline user should exist");

            // pass along to controllers
            context.Items["user"] = user;
        }

        await _next(context);
    }
}