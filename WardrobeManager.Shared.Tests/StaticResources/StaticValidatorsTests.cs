using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Shared.Tests.StaticResources;

public class StaticValidatorsTests
{
    #region NewClothingItemDTO

    [Test]
    public void Validate_NewClothingItemDTO_WithValidName_ReturnsSuccess()
    {
        // Arrange
        var dto = new NewClothingItemDTO { Name = "My Jeans", Category = ClothingCategory.Jeans };

        // Act
        var result = StaticValidators.Validate(dto);

        // Assert
        using (new AssertionScope())
        {
            result.Success.Should().BeTrue();
            result.Message.Should().Be(string.Empty);
        }
    }

    [Test]
    public void Validate_NewClothingItemDTO_WithEmptyName_ReturnsFailure()
    {
        // Arrange
        var dto = new NewClothingItemDTO { Name = "", Category = ClothingCategory.Jeans };

        // Act
        var result = StaticValidators.Validate(dto);

        // Assert
        using (new AssertionScope())
        {
            result.Success.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }
    }

    [Test]
    public void Validate_NewClothingItemDTO_WithTooLongName_ReturnsFailure()
    {
        // Arrange
        var dto = new NewClothingItemDTO { Name = new string('a', 51), Category = ClothingCategory.Jeans };

        // Act
        var result = StaticValidators.Validate(dto);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Test]
    public void Validate_NewClothingItemDTO_WithNameAtMaxLength_ReturnsSuccess()
    {
        // Arrange
        var dto = new NewClothingItemDTO { Name = new string('a', 50), Category = ClothingCategory.Jeans };

        // Act
        var result = StaticValidators.Validate(dto);

        // Assert
        result.Success.Should().BeTrue();
    }

    #endregion

    #region Null input

    [Test]
    public void Validate_WhenInputIsNull_ReturnsFailure()
    {
        // Act
        var result = StaticValidators.Validate<NewClothingItemDTO>(null!);

        // Assert
        using (new AssertionScope())
        {
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("null");
        }
    }

    #endregion

    #region Type without registered validator

    [Test]
    public void Validate_TypeWithNoRegisteredValidator_ReturnsSuccessByDefault()
    {
        // Arrange - EditedUserDTO has no registered validator
        var dto = new EditedUserDTO("Name", "pic");

        // Act
        var result = StaticValidators.Validate(dto);

        // Assert
        result.Success.Should().BeTrue();
    }

    #endregion
}
