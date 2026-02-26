using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using WardrobeManager.Api.Services.Implementation;

namespace WardrobeManager.Api.Tests.Services;

public class DataDirectoryServiceTests
{
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<IWebHostEnvironment> _mockWebHostEnvironment;
    private DataDirectoryService _service;
    private string _tempBaseDir;

    [SetUp]
    public void Setup()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

        _tempBaseDir = Path.Combine(Path.GetTempPath(), $"wardrobe-data-test-{Guid.NewGuid()}");

        // Use Production environment so the DataDirectory is used directly as-is
        _mockWebHostEnvironment.Setup(e => e.EnvironmentName).Returns("Production");
        _mockConfiguration.Setup(c => c["WM_DATA_DIRECTORY"]).Returns(_tempBaseDir);

        _service = new DataDirectoryService(_mockConfiguration.Object, _mockWebHostEnvironment.Object);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_tempBaseDir)) Directory.Delete(_tempBaseDir, true);
    }

    [Test]
    public void GetBaseDataDirectory_WhenCalled_CreatesAndReturnsDirectory()
    {
        // Act
        var result = _service.GetBaseDataDirectory();

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be(_tempBaseDir);
            Directory.Exists(result).Should().BeTrue();
        }
    }

    [Test]
    public void GetDatabaseDirectory_WhenCalled_CreatesAndReturnsDbSubdirectory()
    {
        // Act
        var result = _service.GetDatabaseDirectory();

        // Assert
        using (new AssertionScope())
        {
            result.Should().EndWith("db");
            result.Should().StartWith(_tempBaseDir);
            Directory.Exists(result).Should().BeTrue();
        }
    }

    [Test]
    public void GetImagesDirectory_WhenCalled_CreatesAndReturnsImagesSubdirectory()
    {
        // Act
        var result = _service.GetImagesDirectory();

        // Assert
        using (new AssertionScope())
        {
            result.Should().EndWith("images");
            result.Should().StartWith(_tempBaseDir);
            Directory.Exists(result).Should().BeTrue();
        }
    }

    [Test]
    public void GetUploadsDirectory_WhenCalled_CreatesAndReturnsUploadsSubdirectory()
    {
        // Act
        var result = _service.GetUploadsDirectory();

        // Assert
        using (new AssertionScope())
        {
            result.Should().EndWith("uploads");
            result.Should().StartWith(_tempBaseDir);
            Directory.Exists(result).Should().BeTrue();
        }
    }

    [Test]
    public void DataDirectoryService_WhenDataDirectoryNotConfigured_ThrowsException()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["WM_DATA_DIRECTORY"]).Returns((string?)null);

        // Act & Assert
        var action = () => new DataDirectoryService(mockConfig.Object, _mockWebHostEnvironment.Object);
        action.Should().Throw<Exception>().WithMessage("*configuration value not set*");
    }
}
