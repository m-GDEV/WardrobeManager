using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Moq.Protected;
using WardrobeManager.Presentation.Services.Implementation;
using WardrobeManager.Presentation.Services.Interfaces;

namespace WardrobeManager.Presentation.Tests;

public class ApiServiceTests
{
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly string _apiEndPoint = "http://localhost:3000";
    private IApiService _apiService;
    private Mock<HttpMessageHandler> _mockHandler;

    [SetUp]
    public void Setup()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHandler = new Mock<HttpMessageHandler>();
        // Create a real HttpClient that uses your Mock Handler
        var httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri(_apiEndPoint)
        };

        // Tell the mock factory to return this client when "Auth" is requested
        _mockHttpClientFactory
            .Setup(f => f.CreateClient("Auth"))
            .Returns(httpClient);

        _apiService = new ApiService(_apiEndPoint, _mockHttpClientFactory.Object);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _apiService.DisposeAsync();
    }

    [Test]
    public async Task DoesAdminUserExists_Does_FunctionsCorrectly()
    {
        // Arrange
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(true)
        };
        // 2. Setup the Handler to return our mockResponse
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get &&
                    req.RequestUri != null && 
                    req.RequestUri.AbsolutePath.Contains("/does-admin-user-exist")
                ), 
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);
        // Act
        var res = await _apiService.DoesAdminUserExist();
        // Assert
        _mockHttpClientFactory.Verify(s => s.CreateClient("Auth"), Times.Once);
        res.Should().Be(true);
    }

    [Test]
    public async Task DoesAdminUserExists_DoesNotExist_FunctionsCorrectly()
    {
        // Arrange
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };
        // 2. Setup the Handler to return our mockResponse
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get &&
                    req.RequestUri != null && 
                    req.RequestUri.AbsolutePath.Contains("/does-admin-user-exist")
                    ), 
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);
        // Act
        // Assert
        await _apiService.Invoking(s => s.DoesAdminUserExist()).Should().ThrowAsync<HttpRequestException>();
        _mockHttpClientFactory.Verify(s => s.CreateClient("Auth"), Times.Once);
    }

    [Test]
    public async Task CheckApiConnection_WhenApiIsUp_ReturnsTrue()
    {
        // Arrange
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri != null &&
                    req.RequestUri.AbsolutePath.Contains("/ping")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _apiService.CheckApiConnection();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task CheckApiConnection_WhenApiIsDown_ReturnsFalse()
    {
        // Arrange - simulate a connection failure
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        // Act
        var result = await _apiService.CheckApiConnection();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task AddLog_WhenCalled_PostsToAddLogEndpoint()
    {
        // Arrange
        var logDto = new WardrobeManager.Shared.DTOs.LogDTO
        {
            Title = "Test",
            Description = "Test description",
            Type = WardrobeManager.Shared.Enums.LogType.Info,
            Origin = WardrobeManager.Shared.Enums.LogOrigin.Frontend
        };
        var mockResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri != null &&
                    req.RequestUri.AbsolutePath.Contains("/add-log")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _apiService.AddLog(logDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task CreateAdminUserIfMissing_WhenSuccessful_ReturnsTrueWithMessage()
    {
        // Arrange
        var credentials = new WardrobeManager.Shared.Models.AdminUserCredentials
        {
            email = "admin@test.com",
            password = "SecurePass1!"
        };
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created,
            Content = new StringContent(string.Empty)
        };
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri != null &&
                    req.RequestUri.AbsolutePath.Contains("/create-admin-user-if-missing")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _apiService.CreateAdminUserIfMissing(credentials);

        // Assert
        using (new FluentAssertions.Execution.AssertionScope())
        {
            result.Item1.Should().BeTrue();
            result.Item2.Should().Be("Admin user created!");
        }
    }

    [Test]
    public async Task CreateAdminUserIfMissing_WhenAdminAlreadyExists_ReturnsFalseWithMessage()
    {
        // Arrange
        var credentials = new WardrobeManager.Shared.Models.AdminUserCredentials
        {
            email = "admin@test.com",
            password = "SecurePass1!"
        };
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Conflict,
            Content = new StringContent("Admin user already exists!")
        };
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri != null &&
                    req.RequestUri.AbsolutePath.Contains("/create-admin-user-if-missing")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _apiService.CreateAdminUserIfMissing(credentials);

        // Assert
        using (new FluentAssertions.Execution.AssertionScope())
        {
            result.Item1.Should().BeFalse();
            result.Item2.Should().Be("Admin user already exists!");
        }
    }
}