using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Moq.Protected;
using System.Net;
using WardrobeManager.Presentation.Identity;

namespace WardrobeManager.Presentation.Tests.Identity;

public class CookieHandlerTests
{
    private Mock<HttpMessageHandler> _mockInnerHandler;
    private CookieHandler _cookieHandler;
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _mockInnerHandler = new Mock<HttpMessageHandler>();
        _mockInnerHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        _cookieHandler = new CookieHandler { InnerHandler = _mockInnerHandler.Object };
        _httpClient = new HttpClient(_cookieHandler)
        {
            BaseAddress = new Uri("https://example.com")
        };
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
        _cookieHandler.Dispose();
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
                ItExpr.IsAny<CancellationToken>()
            )
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
        // Act
        var response = await _httpClient.GetAsync("/test");

        // Assert
        _mockInnerHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Test]
    public async Task SendAsync_WhenCalled_ReturnsResponseFromInnerHandler()
    {
        // Arrange
        _mockInnerHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created));

        // Act
        var response = await _httpClient.GetAsync("/test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
