#region

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Api.Endpoints;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/logout", LogoutAsync).RequireAuthorization();
        app.MapGet("/roles", RolesAsync).RequireAuthorization();

        // Onboarding
        app.MapGet("/does-admin-user-exist", DoesAdminUserExist);
        app.MapPost("/create-admin-user-if-missing", CreateAdminIfMissing);
    }

    // Provide an end point to clear the cookie for logout
    // For more information on the logout endpoint and antiforgery, see:
    // https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity#antiforgery-support
    public static async Task<IResult> LogoutAsync(SignInManager<User> signInManager, [FromBody] object empty)
    {
        if (empty is not null)
        {
            await signInManager.SignOutAsync();

            return Results.Ok();
        }

        return Results.Unauthorized();
    }

    public static async Task<IResult> RolesAsync(ClaimsPrincipal user)
    {
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var identity = (ClaimsIdentity)user.Identity;
            var roles = identity.FindAll(identity.RoleClaimType)
                .Select(c =>
                    new
                    {
                        c.Issuer,
                        c.OriginalIssuer,
                        c.Type,
                        c.Value,
                        c.ValueType
                    });

            return TypedResults.Json(roles);
        }

        return Results.Unauthorized();
    }

    // These methods are called by the frontend during the onboarding process
    public static async Task<IResult> DoesAdminUserExist(IUserService userService)
    {
        return TypedResults.Ok(await userService.DoesAdminUserExist());
    }

    public static async Task<IResult> CreateAdminIfMissing(IUserService userService,
        [FromBody] AdminUserCredentials credentials)
    {
        var res = await userService.CreateAdminIfMissing(credentials.email, credentials.password);

        if (res.Item1)
        {
            return Results.Created();
        }
        else
        {
            return Results.Conflict(res.Item2);
        }
    }
}