using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;

namespace WardrobeManager.Api.Tests.Services;

public class FileServiceTests
{
    private Mock<IDataDirectoryService> _mockDataDirectoryService;
    private Mock<IWebHostEnvironment> _mockWebHostEnvironment;
    private Mock<IConfiguration> _mockConfiguration;
    private FakeLogger<FileService> _fakeLogger;
    private FileService _service;
    private string _tempDir;
    private string _tempDeletedDir;
    private string _webRootDir;

    [SetUp]
    public void Setup()
    {
        _mockDataDirectoryService = new Mock<IDataDirectoryService>();
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        _mockConfiguration = new Mock<IConfiguration>();
        _fakeLogger = new FakeLogger<FileService>();

        _tempDir = Path.Combine(Path.GetTempPath(), $"wardrobe-test-{Guid.NewGuid()}");
        _tempDeletedDir = Path.Combine(Path.GetTempPath(), $"wardrobe-deleted-{Guid.NewGuid()}");
        _webRootDir = Path.Combine(Path.GetTempPath(), $"wardrobe-webroot-{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempDir);
        Directory.CreateDirectory(_tempDeletedDir);
        Directory.CreateDirectory(Path.Combine(_webRootDir, "images"));

        File.WriteAllBytes(Path.Combine(_webRootDir, "images", "notfound.jpg"), new byte[] { 0xFF, 0xD8, 0xFF });

        _mockDataDirectoryService.Setup(s => s.GetUploadsDirectory()).Returns(_tempDir);
        _mockDataDirectoryService.Setup(s => s.GetDeletedUploadsDirectory()).Returns(_tempDeletedDir);
        _mockWebHostEnvironment.Setup(e => e.WebRootPath).Returns(_webRootDir);
        _mockConfiguration.Setup(c => c["WM_MAX_IMAGE_UPLOAD_SIZE_IN_MB"]).Returns("5");

        _service = new FileService(_mockDataDirectoryService.Object, _mockWebHostEnvironment.Object, _fakeLogger, _mockConfiguration.Object);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
        if (Directory.Exists(_tempDeletedDir)) Directory.Delete(_tempDeletedDir, true);
        if (Directory.Exists(_webRootDir)) Directory.Delete(_webRootDir, true);
    }

    #region ParseGuid

    [Test]
    public void ParseGuid_WhenGiven_ReturnsStringRepresentation()
    {
        var guid = new Guid("12345678-1234-1234-1234-123456789012");
        var result = _service.ParseGuid(guid);
        result.Should().Be("12345678-1234-1234-1234-123456789012");
    }

    [Test]
    public void ParseGuid_WhenGuidHasCurlyBraces_RemovesCurlyBraces()
    {
        var guid = new Guid("{12345678-1234-1234-1234-123456789012}");
        var result = _service.ParseGuid(guid);
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
        var initialFileCount = Directory.GetFiles(_tempDir).Length;
        await _service.SaveImage(null, "base64data");
        Directory.GetFiles(_tempDir).Length.Should().Be(initialFileCount);
    }

    [Test]
    public async Task SaveImage_WhenImageBase64IsEmpty_DoesNotSaveFile()
    {
        var guid = Guid.NewGuid();
        var initialFileCount = Directory.GetFiles(_tempDir).Length;
        await _service.SaveImage(guid, string.Empty);
        Directory.GetFiles(_tempDir).Length.Should().Be(initialFileCount);
    }

    [Test]
    public async Task SaveImage_WhenImageIsValid_SavesFile()
    {
        var guid = Guid.NewGuid();
        var imageBytes = new byte[] { 1, 2, 3, 4, 5 };
        var base64 = Convert.ToBase64String(imageBytes);
        await _service.SaveImage(guid, base64);
        var expectedPath = Path.Combine(_tempDir, _service.ParseGuid(guid));
        File.Exists(expectedPath).Should().BeTrue();
    }

    [Test]
    public async Task SaveImage_WhenImageIsTooLarge_ThrowsException()
    {
        var guid = Guid.NewGuid();
        // Create image larger than 5MB (the configured limit: 5 * 1024 * 1024 bytes)
        var largeImageBytes = new byte[6 * 1024 * 1024];
        var base64 = Convert.ToBase64String(largeImageBytes);
        await _service.Invoking(s => s.SaveImage(guid, base64))
            .Should().ThrowAsync<Exception>()
            .WithMessage("*File size too large*");
    }

    #endregion

    #region GetImage

    [Test]
    public async Task GetImage_WhenFileExists_ReturnsFileBytes()
    {
        var guid = Guid.NewGuid().ToString();
        var imageBytes = new byte[] { 10, 20, 30 };
        await File.WriteAllBytesAsync(Path.Combine(_tempDir, guid), imageBytes);
        var result = await _service.GetImage(guid);
        result.Should().BeEquivalentTo(imageBytes);
    }

    [Test]
    public async Task GetImage_WhenFileDoesNotExist_ReturnsNotFoundImageBytes()
    {
        var notFoundBytes = new byte[] { 0xFF, 0xD8, 0xFF };
        File.WriteAllBytes(Path.Combine(_webRootDir, "images", "notfound.jpg"), notFoundBytes);
        var result = await _service.GetImage("nonexistent-guid");
        result.Should().BeEquivalentTo(notFoundBytes);
    }

    #endregion

    #region DeleteImage

    [Test]
    public async Task DeleteImage_WhenFileExists_MovesToDeletedDirectory()
    {
        var guid = Guid.NewGuid();
        var guidString = _service.ParseGuid(guid);
        var sourceFile = Path.Combine(_tempDir, guidString);
        File.WriteAllBytes(sourceFile, new byte[] { 1, 2, 3 });

        await _service.DeleteImage(guid);

        using (new AssertionScope())
        {
            File.Exists(sourceFile).Should().BeFalse();
            File.Exists(Path.Combine(_tempDeletedDir, guidString)).Should().BeTrue();
        }
    }

    [Test]
    public async Task DeleteImage_WhenFileDoesNotExist_LogsError()
    {
        var guid = Guid.NewGuid();
        await _service.DeleteImage(guid);
        _fakeLogger.Collector.LatestRecord.Level.Should().Be(Microsoft.Extensions.Logging.LogLevel.Error);
    }

    #endregion
}
