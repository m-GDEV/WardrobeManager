using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Middleware;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Middleware;

public class LoggingMiddlewareTests
{
    private Mock<ILoggingService> _mockLoggingService;
    private bool _nextWasCalled;
    private RequestDelegate _next;

    [SetUp]
    public void Setup()
    {
        _mockLoggingService = new Mock<ILoggingService>();
        _nextWasCalled = false;
        _next = ctx =>
        {
            _nextWasCalled = true;
            return Task.CompletedTask;
        };
    }

    [Test]
    public async Task InvokeAsync_WhenCalled_LogsRequestAndCallsNext()
    {
        // Arrange
        var middleware = new LoggingMiddleware(_next);

        var services = new ServiceCollection();
        services.AddSingleton(_mockLoggingService.Object);
        var httpContext = new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider()
        };

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        using (new AssertionScope())
        {
            _mockLoggingService.Verify(s =>
                s.CreateDatabaseAndConsoleLog(It.Is<Log>(l =>
                    l.Type == LogType.RequestLog && l.Origin == LogOrigin.Backend)), Times.Once);
            _nextWasCalled.Should().BeTrue();
        }
    }

    [Test]
    public async Task InvokeAsync_WhenCalled_AlwaysCallsNextMiddleware()
    {
        // Arrange
        var middleware = new LoggingMiddleware(_next);

        var services = new ServiceCollection();
        services.AddSingleton(_mockLoggingService.Object);
        var httpContext = new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider()
        };

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        _nextWasCalled.Should().BeTrue();
    }
}
