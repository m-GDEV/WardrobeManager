using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Middleware;
using WardrobeManager.Api.Services.Interfaces;

namespace WardrobeManager.Api.Tests.Middleware;

public class UserCreationMiddlewareTests
{
    private Mock<IUserService> _mockUserService;
    private ServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
        var services = new ServiceCollection();
        services.AddSingleton(_mockUserService.Object);
        _serviceProvider = services.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }

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
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };
        _mockUserService.Setup(s => s.GetUser(It.IsAny<string>())).ReturnsAsync((User?)null);

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
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };
        _mockUserService.Setup(s => s.GetUser(It.IsAny<string>())).ReturnsAsync((User?)null);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        capturedContext.Should().BeSameAs(httpContext);
    }

    [Test]
    public async Task InvokeAsync_WhenUserExists_SetsUserOnContext()
    {
        // Arrange
        var user = new User { Id = "test-user-id" };
        _mockUserService.Setup(s => s.GetUser(It.IsAny<string>())).ReturnsAsync(user);

        RequestDelegate next = ctx => Task.CompletedTask;
        var middleware = new UserCreationMiddleware(next);
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        httpContext.Items["user"].Should().BeSameAs(user);
    }

    [Test]
    public async Task InvokeAsync_WhenUserDoesNotExist_DoesNotSetUserOnContext()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetUser(It.IsAny<string>())).ReturnsAsync((User?)null);

        RequestDelegate next = ctx => Task.CompletedTask;
        var middleware = new UserCreationMiddleware(next);
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        httpContext.Items.ContainsKey("user").Should().BeFalse();
    }
}
