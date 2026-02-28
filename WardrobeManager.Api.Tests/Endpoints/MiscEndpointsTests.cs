using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Security.Claims;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Endpoints;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Endpoints;

public class MiscEndpointsTests
{
    private Mock<ILoggingService> _mockLoggingService;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _mockLoggingService = new Mock<ILoggingService>();
        _mockMapper = new Mock<IMapper>();
    }

    #region Ping

    [Test]
    public void Ping_WhenUserIsAuthenticated_ReturnsAuthenticatedMessage()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var claims = new[] { new Claim(ClaimTypes.Name, "test@test.com") };
        var identity = new ClaimsIdentity(claims, "TestScheme"); // non-null AuthType means authenticated
        httpContext.User = new ClaimsPrincipal(identity);

        // Act
        var result = MiscEndpoints.Ping(httpContext);

        // Assert
        var valueResult = result as IValueHttpResult;
        using (new AssertionScope())
        {
            valueResult.Should().NotBeNull();
            valueResult!.Value.Should().Be("Authenticated: WardrobeManager API is active.");
        }
    }

    [Test]
    public void Ping_WhenUserIsNotAuthenticated_ReturnsUnauthenticatedMessage()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        // DefaultHttpContext has an anonymous user by default

        // Act
        var result = MiscEndpoints.Ping(httpContext);

        // Assert
        var valueResult = result as IValueHttpResult;
        using (new AssertionScope())
        {
            valueResult.Should().NotBeNull();
            valueResult!.Value.Should().Be("Unauthenticated: WardrobeManager API is active.");
        }
    }

    #endregion

    #region AddLogAsync

    [Test]
    public async Task AddLogAsync_WhenCalled_MapsLogDtoAndCreatesLog()
    {
        // Arrange
        var logDto = new LogDTO
        {
            Title = "Test Log",
            Description = "Test description",
            Type = LogType.Info,
            Origin = LogOrigin.Frontend
        };
        var mappedLog = new Log("Test Log", "Test description", LogType.Info, LogOrigin.Frontend);
        _mockMapper.Setup(m => m.Map<Log>(logDto)).Returns(mappedLog);

        // Act
        var result = await MiscEndpoints.AddLogAsync(logDto, _mockLoggingService.Object, _mockMapper.Object);

        // Assert
        _mockMapper.Verify(m => m.Map<Log>(logDto), Times.Once);
        _mockLoggingService.Verify(s => s.CreateDatabaseAndConsoleLog(mappedLog), Times.Once);
        result.Should().BeOfType<Ok>();
    }

    #endregion
}
