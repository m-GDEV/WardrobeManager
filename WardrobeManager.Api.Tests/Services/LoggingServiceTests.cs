using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories;
using WardrobeManager.Api.Repositories.Interfaces;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Services;

public class LoggingServiceTests
{
    private Mock<IGenericRepository<Log>> _mockRepo;
    private FakeLogger<LoggingService> _fakeLogger;
    private LoggingService _service;
    
    [SetUp]
    public void Setup()
    {
        // Re-initialize the mocks so they have 0 recorded calls for every test
        _mockRepo = new Mock<IGenericRepository<Log>>();
        _fakeLogger = new FakeLogger<LoggingService>();
        
        // Inject the fresh mocks
        _service = new LoggingService(_mockRepo.Object, _fakeLogger);
    }

    [Test]
    public async Task CreateDatabaseAndConsoleLog_WhenError_LogsCorrectLevel()
    {
        // Arrange
        var log = new Log
        (
            "Database Crash",
            "Connection timed out",
            LogType.Error,
            LogOrigin.Unknown
        );

        // Act
        await _service.CreateDatabaseAndConsoleLog(log);

        // Assert - 1. Repository logic (Using Moq)
        _mockRepo.Verify(r => r.CreateAsync(log), Times.Once);
        _mockRepo.Verify(r => r.SaveAsync(), Times.Once);

        // Assert - 2. Logging logic (Using FakeLogger)
        var latestLog = _fakeLogger.Collector.LatestRecord;

        using (new AssertionScope())
        {
            latestLog.Should().NotBeNull();
            latestLog.Level.Should().Be(LogLevel.Error);
            latestLog.Message.Should().Contain("Database Crash");
        }
    }
    
    [Test]
    public async Task CreateDatabaseAndConsoleLog_WhenInformation_LogsCorrectLevel()
    {
        // Arrange
        var log = new Log
        (
            "Database Crash",
            "Connection timed out",
            LogType.Info,
            LogOrigin.Unknown
        );

        // Act
        await _service.CreateDatabaseAndConsoleLog(log);

        // Assert - 1. Repository logic (Using Moq)
        _mockRepo.Verify(r => r.CreateAsync(log), Times.Once);
        _mockRepo.Verify(r => r.SaveAsync(), Times.Once);

        // Assert - 2. Logging logic (Using FakeLogger)
        var latestLog = _fakeLogger.Collector.LatestRecord;

        using (new AssertionScope())
        {
            latestLog.Should().NotBeNull();
            latestLog.Level.Should().Be(LogLevel.Information);
            latestLog.Message.Should().Contain("Database Crash");
        }
    }
}