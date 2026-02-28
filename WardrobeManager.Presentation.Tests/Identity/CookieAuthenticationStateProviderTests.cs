using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Identity.Models;

namespace WardrobeManager.Presentation.Tests.Identity;

public class CookieAuthenticationStateProviderTests
{
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private Mock<HttpMessageHandler> _mockHandler;
    private CookieAuthenticationStateProvider _provider;

    [SetUp]
    public void Setup()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHandler = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri("https://localhost:5000")
        };

        _mockHttpClientFactory.Setup(f => f.CreateClient("Auth")).Returns(httpClient);
        _provider = new CookieAuthenticationStateProvider(_mockHttpClientFactory.Object);
    }

    #region RegisterAsync

    [Test]
    public async Task RegisterAsync_WhenSuccessful_ReturnsSucceededResult()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.AbsolutePath.Contains("register")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        // Act
        var result = await _provider.RegisterAsync("test@test.com", "password");

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task RegisterAsync_WhenFailed_ReturnsFailedResultWithErrors()
    {
        // Arrange
        var errorBody = """{"errors":{"DuplicateEmail":["Email already taken"]}}""";
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.AbsolutePath.Contains("register")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(errorBody, System.Text.Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _provider.RegisterAsync("test@test.com", "password");

        // Assert
        using (new AssertionScope())
        {
            result.Succeeded.Should().BeFalse();
            result.ErrorList.Should().Contain("Email already taken");
        }
    }

    [Test]
    public async Task RegisterAsync_WhenExceptionThrown_ReturnsFailedResult()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        // Act
        var result = await _provider.RegisterAsync("test@test.com", "password");

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    #endregion

    #region LoginAsync

    [Test]
    public async Task LoginAsync_WhenSuccessful_ReturnsSucceededResult()
    {
        // Arrange
        // Login returns 200 - subsequent GetAuthenticationState call returns 401 (not our concern here)
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.AbsolutePath.Contains("login")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        // GetAuthenticationStateAsync will be called after login - return 401 to keep test simple
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        // Act
        var result = await _provider.LoginAsync("test@test.com", "password");

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Test]
    public async Task LoginAsync_WhenFailed_ReturnsFailedResult()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.AbsolutePath.Contains("login")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        // Act
        var result = await _provider.LoginAsync("test@test.com", "wrongpassword");

        // Assert
        using (new AssertionScope())
        {
            result.Succeeded.Should().BeFalse();
            result.ErrorList.Should().Contain("Invalid email and/or password.");
        }
    }

    [Test]
    public async Task LoginAsync_WhenExceptionThrown_ReturnsFailedResult()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        // Act
        var result = await _provider.LoginAsync("test@test.com", "password");

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    #endregion

    #region LogoutAsync

    [Test]
    public async Task LogoutAsync_WhenSuccessful_ReturnsTrue()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.AbsolutePath.Contains("logout")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        // GetAuthenticationStateAsync will also be called 
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        // Act
        var result = await _provider.LogoutAsync();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task LogoutAsync_WhenFailed_ReturnsFalse()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.AbsolutePath.Contains("logout")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        // Act
        var result = await _provider.LogoutAsync();

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region GetAuthenticationStateAsync

    [Test]
    public async Task GetAuthenticationStateAsync_WhenNotAuthenticated_ReturnsAnonymousUser()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        // Act
        var state = await _provider.GetAuthenticationStateAsync();

        // Assert
        using (new AssertionScope())
        {
            state.Should().NotBeNull();
            state.User.Identity?.IsAuthenticated.Should().BeFalse();
        }
    }

    [Test]
    public async Task GetAuthenticationStateAsync_WhenAuthenticated_ReturnsAuthenticatedUser()
    {
        // Arrange
        var userInfo = new UserInfo
        {
            Email = "test@test.com",
            IsEmailConfirmed = true
        };
        var userInfoJson = JsonSerializer.Serialize(userInfo, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        // manage/info returns user info
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.AbsolutePath.Contains("manage/info")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(userInfoJson, System.Text.Encoding.UTF8, "application/json")
            });

        // roles returns empty array
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.AbsolutePath.Contains("roles")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]", System.Text.Encoding.UTF8, "application/json")
            });

        // Act
        var state = await _provider.GetAuthenticationStateAsync();

        // Assert
        using (new AssertionScope())
        {
            state.Should().NotBeNull();
            state.User.Identity?.IsAuthenticated.Should().BeTrue();
            state.User.Identity?.Name.Should().Be("test@test.com");
        }
    }

    #endregion
}
