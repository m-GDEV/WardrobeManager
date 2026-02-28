using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using WardrobeManager.Api.Endpoints;
using WardrobeManager.Api.Services.Interfaces;

namespace WardrobeManager.Api.Tests.Endpoints;

public class ImageEndpointsTests
{
    private Mock<IFileService> _mockFileService;

    [SetUp]
    public void Setup()
    {
        _mockFileService = new Mock<IFileService>();
    }

    [Test]
    public async Task GetImage_WhenCalled_ReturnsFileFromService()
    {
        // Arrange
        var imageId = "test-image-guid";
        var imageBytes = new byte[] { 0xFF, 0xD8, 0xFF };
        _mockFileService.Setup(s => s.GetImage(imageId)).ReturnsAsync(imageBytes);

        // Act
        var result = await ImageEndpoints.GetImage(imageId, _mockFileService.Object);

        // Assert
        _mockFileService.Verify(s => s.GetImage(imageId), Times.Once);
        result.Should().NotBeNull();
    }
}
