using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Middleware;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Middleware;

public class GlobalExceptionHandlerTests
{
    private Mock<ILoggingService> _mockLoggingService;
    private FakeLogger<GlobalExceptionHandler> _fakeLogger;
    private Mock<IServiceScopeFactory> _mockScopeFactory;

    [SetUp]
    public void Setup()
    {
        _mockLoggingService = new Mock<ILoggingService>();
        _fakeLogger = new FakeLogger<GlobalExceptionHandler>();

        // Wire up: ScopeFactory → Scope → ServiceProvider → LoggingService
        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider
            .Setup(sp => sp.GetService(typeof(ILoggingService)))
            .Returns(_mockLoggingService.Object);

        var mockScope = new Mock<IServiceScope>();
        mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);

        _mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockScopeFactory.Setup(f => f.CreateScope()).Returns(mockScope.Object);
    }

    [Test]
    public async Task TryHandleAsync_WhenExceptionOccurs_LogsToDatabase()
    {
        // Arrange
        var handler = new GlobalExceptionHandler(_fakeLogger, _mockScopeFactory.Object);
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        var exception = new Exception("Test error message");

        // Act
        await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        _mockLoggingService.Verify(s =>
            s.CreateDatabaseAndConsoleLog(It.Is<Log>(l =>
                l.Type == LogType.UncaughtException &&
                l.Origin == LogOrigin.Backend &&
                l.Title == "Test error message")), Times.Once);
    }

    [Test]
    public async Task TryHandleAsync_WhenExceptionOccurs_SetsResponseStatusTo500()
    {
        // Arrange
        var handler = new GlobalExceptionHandler(_fakeLogger, _mockScopeFactory.Object);
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        // Act
        await handler.TryHandleAsync(httpContext, new Exception("Error"), CancellationToken.None);

        // Assert
        httpContext.Response.StatusCode.Should().Be(500);
    }

    [Test]
    public async Task TryHandleAsync_WhenExceptionOccurs_ReturnsTrue()
    {
        // Arrange
        var handler = new GlobalExceptionHandler(_fakeLogger, _mockScopeFactory.Object);
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        // Act
        var result = await handler.TryHandleAsync(httpContext, new Exception("Error"), CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
