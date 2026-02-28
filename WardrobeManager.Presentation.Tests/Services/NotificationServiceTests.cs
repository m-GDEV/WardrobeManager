using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Presentation.Services.Implementation;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Tests.Services;

public class NotificationServiceTests
{
    private NotificationService _service;
    private int _onChangeCallCount;

    [SetUp]
    public void Setup()
    {
        _service = new NotificationService();
        _onChangeCallCount = 0;
        _service.OnChange += () => _onChangeCallCount++;
    }

    #region AddNotification (string only)

    [Test]
    public void AddNotification_StringOnly_AddsNotificationWithInfoType()
    {
        // Arrange
        const string message = "Test message";

        // Act
        _service.AddNotification(message);

        // Assert
        using (new AssertionScope())
        {
            _service.Notifications.Should().HaveCount(1);
            _service.Notifications[0].Message.Should().Be(message);
            _service.Notifications[0].Type.Should().Be(NotificationType.Info);
        }
    }

    #endregion

    #region AddNotification (string + type)

    [Test]
    public void AddNotification_WithErrorType_SetsCorrectType()
    {
        // Arrange
        const string message = "Error occurred";

        // Act
        _service.AddNotification(message, NotificationType.Error);

        // Assert
        using (new AssertionScope())
        {
            _service.Notifications.Should().HaveCount(1);
            _service.Notifications[0].Message.Should().Be(message);
            _service.Notifications[0].Type.Should().Be(NotificationType.Error);
        }
    }

    [Test]
    public void AddNotification_WithSuccessType_SetsCorrectType()
    {
        // Arrange

        // Act
        _service.AddNotification("Success!", NotificationType.Success);

        // Assert
        _service.Notifications[0].Type.Should().Be(NotificationType.Success);
    }

    [Test]
    public void AddNotification_MultipleNotifications_AllAppearInList()
    {
        // Arrange

        // Act
        _service.AddNotification("First", NotificationType.Info);
        _service.AddNotification("Second", NotificationType.Warning);
        _service.AddNotification("Third", NotificationType.Error);

        // Assert
        _service.Notifications.Should().HaveCount(3);
    }

    #endregion

    #region RemoveNotification

    [Test]
    public void RemoveNotification_WhenNotificationExists_RemovesItFromList()
    {
        // Arrange
        _service.AddNotification("To be removed", NotificationType.Info);
        var notificationToRemove = _service.Notifications[0];

        // Act
        _service.RemoveNotification(notificationToRemove);

        // Assert
        _service.Notifications.Should().BeEmpty();
    }

    [Test]
    public void RemoveNotification_WhenOneOfMany_RemovesOnlyTargetNotification()
    {
        // Arrange
        _service.AddNotification("Keep this", NotificationType.Info);
        _service.AddNotification("Remove this", NotificationType.Error);
        var toRemove = _service.Notifications[1];

        // Act
        _service.RemoveNotification(toRemove);

        // Assert
        using (new AssertionScope())
        {
            _service.Notifications.Should().HaveCount(1);
            _service.Notifications[0].Message.Should().Be("Keep this");
        }
    }

    #endregion

    #region OnChange event

    [Test]
    public void AddNotification_WhenCalled_FiresOnChangeEvent()
    {
        // Arrange

        // Act
        _service.AddNotification("Event test");

        // Assert
        _onChangeCallCount.Should().Be(1);
    }

    [Test]
    public void RemoveNotification_WhenCalled_FiresOnChangeEvent()
    {
        // Arrange
        _service.AddNotification("Test");
        var notification = _service.Notifications[0];
        var countBeforeRemoval = _onChangeCallCount;

        // Act
        _service.RemoveNotification(notification);

        // Assert
        _onChangeCallCount.Should().Be(countBeforeRemoval + 1);
    }

    #endregion

    #region Notifications property

    [Test]
    public void Notifications_WhenEmpty_ReturnsEmptyList()
    {
        // Act
        var result = _service.Notifications;

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Notifications_ReturnsACopy_NotTheOriginalList()
    {
        // Arrange
        _service.AddNotification("Test");

        // Act
        var list1 = _service.Notifications;
        var list2 = _service.Notifications;

        // Assert
        list1.Should().NotBeSameAs(list2); // each call returns a new list copy
    }

    #endregion
}
