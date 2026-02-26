using FluentAssertions;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Tests.Models;

public class NotificationMessageTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void NotificationMessage_InitializeWithProperties_HasPropertiesSet()
    {
        // Arrange
        var message = "something!";
        var type = NotificationType.Success;
        // Act
        var model = new NotificationMessage(message, type);
        // Assert
        model.Message.Should().Be(message);
        model.Type.Should().Be(type);
        model.CreationDate.Date.Should().Be(DateTime.UtcNow.Date);
    }
}