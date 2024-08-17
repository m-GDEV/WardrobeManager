
using System;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Services.Interfaces;


namespace WardrobeManager.Api.Endpoints;

public static class UserEndpoints {
    public static void MapUserEndpoints(this IEndpointRouteBuilder app) {
        var group = app.MapGroup("/user").RequireAuthorization();

        group.MapGet("", GetUser);
        group.MapPut("", EditUser);
        group.MapDelete("", DeleteUser);
    }

    public static async Task<IResult> GetUser(
            IUserService userService, HttpContext context
            ){
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        return Results.Ok(user);
    }

    public static async Task<IResult> EditUser(
            [FromBody] EditedUserDTO editedUser,  IUserService userService, HttpContext context
            ) {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await userService.UpdateUser(user.Id, editedUser);

        return Results.Ok("Updated");
    }
    public static async Task<IResult> DeleteUser(
            IUserService userService, HttpContext context
            ) {
        User? user = context.Items["user"] as User;
        Debug.Assert(user != null, "Cannot get user");

        await userService.DeleteUser(user.Id);

        return Results.Ok("Deleted");
    }
}

