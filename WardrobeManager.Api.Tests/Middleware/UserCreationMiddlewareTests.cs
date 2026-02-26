using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WardrobeManager.Api.Middleware;

namespace WardrobeManager.Api.Tests.Middleware;

public class UserCreationMiddlewareTests
{
    [Test]
    public async Task InvokeAsync_WhenCalled_AlwaysCallsNext()
    {
        // Arrange
        var nextWasCalled = false;
        RequestDelegate next = ctx =>
        {
            nextWasCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new UserCreationMiddleware(next);
        var httpContext = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        nextWasCalled.Should().BeTrue();
    }

    [Test]
    public async Task InvokeAsync_WhenCalled_PassesContextToNext()
    {
        // Arrange
        HttpContext? capturedContext = null;
        RequestDelegate next = ctx =>
        {
            capturedContext = ctx;
            return Task.CompletedTask;
        };
        var middleware = new UserCreationMiddleware(next);
        var httpContext = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        capturedContext.Should().BeSameAs(httpContext);
    }
}
