using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Tests.DTOs;

public class LogDTOTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void LogDTO_InitializeWithProperties_HasPropertiesSet()
    {
        // Arrange
        const string title = "System Error";
        const string description = "Database connection failed";
        const LogType type = LogType.Error;
        const LogOrigin origin = LogOrigin.Backend;
        var created = DateTime.UtcNow;

        // Act
        var dto = new LogDTO
        {
            Title = title,
            Description = description,
            Type = type,
            Origin = origin,
            Created = created
        };

        // Assert
        using (new AssertionScope())
        {
            dto.Title.Should().Be(title);
            dto.Description.Should().Be(description);
            dto.Type.Should().Be(type);
            dto.Origin.Should().Be(origin);
            dto.Created.Should().Be(created);
        }
    }

    [Test]
    public void LogDTO_WhenTypeIsInfo_HasInfoType()
    {
        // Arrange
        // Act
        var dto = new LogDTO
        {
            Title = "Info event",
            Description = "Something informational",
            Type = LogType.Info,
            Origin = LogOrigin.Frontend
        };

        // Assert
        dto.Type.Should().Be(LogType.Info);
    }
}
