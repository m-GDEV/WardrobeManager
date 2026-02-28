using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Moq.Protected;
using WardrobeManager.Presentation.Services.Interfaces;

namespace WardrobeManager.Presentation.Tests.Identity;

/// <summary>
/// Tests for CustomHttpMessageHandler (which replaced the old CookieHandler).
/// CustomHttpMessageHandler adds X-Requested-With header, includes browser credentials,
/// and reports HTTP errors through INotificationService.
/// </summary>
public class CookieHandlerTests
{
    private Mock<INotificationService> _mockNotificationService;
    private Mock<HttpMessageHandler> _mockInnerHandler;
    private CustomHttpMessageHandler _handler;
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _mockNotificationService = new Mock<INotificationService>();
        _mockInnerHandler = new Mock<HttpMessageHandler>();

        _handler = new CustomHttpMessageHandler(_mockNotificationService.Object)
        {
            InnerHandler = _mockInnerHandler.Object
        };

        _httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri("https://example.com")
        };
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
        _handler.Dispose();
    }

    [Test]
    public async Task SendAsync_WhenCalled_AddsXRequestedWithHeader()
    {
        // Arrange
        HttpRequestMessage? capturedRequest = null;
        _mockInnerHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        // Act
        await _httpClient.GetAsync("/test");

        // Assert
        using (new AssertionScope())
        {
            capturedRequest.Should().NotBeNull();
            capturedRequest!.Headers.Should().ContainKey("X-Requested-With");
            capturedRequest.Headers.GetValues("X-Requested-With").Should().Contain("XMLHttpRequest");
        }
    }

    [Test]
    public async Task SendAsync_WhenCalled_ForwardsRequestToInnerHandler()
    {
        // Arrange
        _mockInnerHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        // Act
        await _httpClient.GetAsync("/test");

        // Assert
        _mockInnerHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task SendAsync_WhenResponseIsNotSuccess_NotifiesNotificationService()
    {
        // Arrange
        _mockInnerHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        // Act
        await _httpClient.GetAsync("/test");

        // Assert
        _mockNotificationService.Verify(
            s => s.AddNotification(It.Is<string>(m => m.Contains("500")), It.IsAny<WardrobeManager.Shared.Enums.NotificationType>()),
            Times.Once);
    }

    [Test]
    public async Task SendAsync_WhenSuccessfulResponse_DoesNotCallNotificationService()
    {
        // Arrange
        _mockInnerHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        // Act
        await _httpClient.GetAsync("/test");

        // Assert
        _mockNotificationService.Verify(
            s => s.AddNotification(It.IsAny<string>(), It.IsAny<WardrobeManager.Shared.Enums.NotificationType>()),
            Times.Never);
    }
}
