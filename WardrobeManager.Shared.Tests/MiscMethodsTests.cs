using FluentAssertions;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Shared.Tests;

public class MiscMethodsTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GenerateRandomId_DefaultScenario_CorrectFormat()
    {
        // Arrange
        // Act
        var randomId = MiscMethods.GenerateRandomId();
        // Assert
        randomId.Length.Should().Be(10);
        randomId.Should().StartWith("id");
    }
}