using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Tests.Models;

public class ResultTests
{
    [Test]
    public void Result_WhenCreatedWithSuccessTrue_HasCorrectValues()
    {
        // Arrange & Act
        var result = new Result<string>("hello", true);

        // Assert
        using (new AssertionScope())
        {
            result.Data.Should().Be("hello");
            result.Success.Should().BeTrue();
            result.Message.Should().Be(string.Empty);
        }
    }

    [Test]
    public void Result_WhenCreatedWithSuccessFalseAndMessage_HasCorrectValues()
    {
        // Arrange & Act
        var result = new Result<int>(0, false, "Something went wrong");

        // Assert
        using (new AssertionScope())
        {
            result.Data.Should().Be(0);
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Something went wrong");
        }
    }

    [Test]
    public void Result_WhenPropertiesAreMutated_ReflectsChanges()
    {
        // Arrange
        var result = new Result<bool>(false, false, "Initial")
        {
            // Act
            Success = true,
            Message = "Updated",
            Data = true
        };

        // Assert
        using (new AssertionScope())
        {
            result.Data.Should().BeTrue();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Updated");
        }
    }
}
