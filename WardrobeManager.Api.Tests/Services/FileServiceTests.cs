using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;

namespace WardrobeManager.Api.Tests.Services;

public class FileServiceTests
{
    private Mock<IDataDirectoryService> _mockDataDirectoryService;
    private Mock<IWebHostEnvironment> _mockWebHostEnvironment;
    private Mock<IConfiguration> _mockConfiguration;
    private FileService _service;
    private string _tempDir;
    private string _webRootDir;

    [SetUp]
    public void Setup()
    {
        _mockDataDirectoryService = new Mock<IDataDirectoryService>();
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        _mockConfiguration = new Mock<IConfiguration>();

        // Set up temp directories for file operations
        _tempDir = Path.Combine(Path.GetTempPath(), $"wardrobe-test-{Guid.NewGuid()}");
        _webRootDir = Path.Combine(Path.GetTempPath(), $"wardrobe-webroot-{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempDir);
        Directory.CreateDirectory(Path.Combine(_webRootDir, "images"));

        // Create a fake notfound.jpg so GetImage can fall back to it
        File.WriteAllBytes(Path.Combine(_webRootDir, "images", "notfound.jpg"), new byte[] { 0xFF, 0xD8, 0xFF });

        _mockDataDirectoryService.Setup(s => s.GetUploadsDirectory()).Returns(_tempDir);
        _mockWebHostEnvironment.Setup(e => e.WebRootPath).Returns(_webRootDir);
        _mockConfiguration.Setup(c => c["WM_MAX_IMAGE_UPLOAD_SIZE_IN_MB"]).Returns("5");

        _service = new FileService(_mockDataDirectoryService.Object, _mockWebHostEnvironment.Object, _mockConfiguration.Object);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up temp directories after each test
        if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
        if (Directory.Exists(_webRootDir)) Directory.Delete(_webRootDir, true);
    }

    #region ParseGuid

    [Test]
    public void ParseGuid_WhenGiven_ReturnsStringRepresentation()
    {
        // Arrange
        var guid = new Guid("12345678-1234-1234-1234-123456789012");

        // Act
        var result = _service.ParseGuid(guid);

        // Assert
        result.Should().Be("12345678-1234-1234-1234-123456789012");
    }

    [Test]
    public void ParseGuid_WhenGuidHasCurlyBraces_RemovesCurlyBraces()
    {
        // Arrange
        var guid = new Guid("{12345678-1234-1234-1234-123456789012}");

        // Act
        var result = _service.ParseGuid(guid);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotContain("{");
            result.Should().NotContain("}");
        }
    }

    #endregion

    #region SaveImage

    [Test]
    public async Task SaveImage_WhenGuidIsNull_DoesNotSaveFile()
    {
        // Arrange
        var initialFileCount = Directory.GetFiles(_tempDir).Length;

        // Act
        await _service.SaveImage(null, "base64data");

        // Assert
        Directory.GetFiles(_tempDir).Length.Should().Be(initialFileCount);
    }

    [Test]
    public async Task SaveImage_WhenImageBase64IsEmpty_DoesNotSaveFile()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var initialFileCount = Directory.GetFiles(_tempDir).Length;

        // Act
        await _service.SaveImage(guid, string.Empty);

        // Assert
        Directory.GetFiles(_tempDir).Length.Should().Be(initialFileCount);
    }

    [Test]
    public async Task SaveImage_WhenImageIsValid_SavesFile()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var imageBytes = new byte[] { 1, 2, 3, 4, 5 };
        var base64 = Convert.ToBase64String(imageBytes);

        // Act
        await _service.SaveImage(guid, base64);

        // Assert
        var expectedPath = Path.Combine(_tempDir, _service.ParseGuid(guid));
        File.Exists(expectedPath).Should().BeTrue();
    }

    [Test]
    public async Task SaveImage_WhenImageIsTooLarge_ThrowsException()
    {
        // Arrange
        var guid = Guid.NewGuid();
        // Create image larger than 5 * 1024 bytes
        var largeImageBytes = new byte[6 * 1024];
        var base64 = Convert.ToBase64String(largeImageBytes);

        // Act & Assert
        await _service.Invoking(s => s.SaveImage(guid, base64))
            .Should().ThrowAsync<Exception>()
            .WithMessage("*File size too large*");
    }

    #endregion

    #region GetImage

    [Test]
    public async Task GetImage_WhenFileExists_ReturnsFileBytes()
    {
        // Arrange
        var guid = Guid.NewGuid().ToString();
        var imageBytes = new byte[] { 10, 20, 30 };
        await File.WriteAllBytesAsync(Path.Combine(_tempDir, guid), imageBytes);

        // Act
        var result = await _service.GetImage(guid);

        // Assert
        result.Should().BeEquivalentTo(imageBytes);
    }

    [Test]
    public async Task GetImage_WhenFileDoesNotExist_ReturnsNotFoundImageBytes()
    {
        // Arrange
        var notFoundBytes = new byte[] { 0xFF, 0xD8, 0xFF };
        File.WriteAllBytes(Path.Combine(_webRootDir, "images", "notfound.jpg"), notFoundBytes);

        // Act
        var result = await _service.GetImage("nonexistent-guid");

        // Assert
        result.Should().BeEquivalentTo(notFoundBytes);
    }

    #endregion
}
