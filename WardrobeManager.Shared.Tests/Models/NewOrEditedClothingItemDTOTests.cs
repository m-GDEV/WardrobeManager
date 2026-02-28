using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Tests.Models;

/// <summary>
/// Tests for NewClothingItemDTO (formerly NewOrEditedClothingItemDTO).
/// The old NewOrEditedClothingItemDTO was split into NewClothingItemDTO (add) and EditedUserDTO (edit user).
/// </summary>
public class NewOrEditedClothingItemDTOTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void NewClothingItemDTO_DefaultConstructor_HasExpectedDefaults()
    {
        // Act
        var dto = new NewClothingItemDTO();

        // Assert
        using (new AssertionScope())
        {
            dto.Should().NotBeNull();
            dto.Name.Should().Be(string.Empty);
            dto.Category.Should().Be(ClothingCategory.None);
            dto.Size.Should().Be(ClothingSize.NotSpecified);
            dto.ImageBase64.Should().BeNull();
        }
    }

    [Test]
    public void NewClothingItemDTO_WhenPropertiesAreSet_ReflectsValues()
    {
        // Arrange
        var dto = new NewClothingItemDTO
        {
            // Act
            Name = "My T-Shirt",
            Category = ClothingCategory.TShirt,
            Size = ClothingSize.Medium,
            ImageBase64 = "base64data"
        };

        // Assert
        using (new AssertionScope())
        {
            dto.Name.Should().Be("My T-Shirt");
            dto.Category.Should().Be(ClothingCategory.TShirt);
            dto.Size.Should().Be(ClothingSize.Medium);
            dto.ImageBase64.Should().Be("base64data");
        }
    }

    [Test]
    public void NewClothingItemDTO_ImageBase64_CanBeSetToNull()
    {
        // Arrange
        var dto = new NewClothingItemDTO { ImageBase64 = "some-base64" };

        // Act
        dto.ImageBase64 = null;

        // Assert
        dto.ImageBase64.Should().BeNull();
    }
}
