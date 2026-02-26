using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Tests.Models;

public class NewOrEditedClothingItemDTOTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void NewOrEditedClothingItemDTO_InitializeWithProperties_HasPropertiesSet()
    {
        // Arrange
        const string name = "Favourite Green T-Shirt";
        const ClothingCategory category = ClothingCategory.TShirt;
        const Season season = Season.Fall;
        const bool favourited = true;
        const WearLocation wearLocation = WearLocation.HomeAndOutside;
        const int desiredTimesWornBeforeWash = 5;
        const string imageBase64 = "base64image";

        // Act
        var dto = new NewOrEditedClothingItemDTO(name, category, season, favourited, wearLocation, desiredTimesWornBeforeWash, imageBase64);

        // Assert
        using (new AssertionScope())
        {
            dto.Name.Should().Be(name);
            dto.Category.Should().Be(category);
            dto.Season.Should().Be(season);
            dto.Favourited.Should().Be(favourited);
            dto.WearLocation.Should().Be(wearLocation);
            dto.DesiredTimesWornBeforeWash.Should().Be(desiredTimesWornBeforeWash);
            dto.ImageBase64.Should().Be(imageBase64);
        }
    }

    [Test]
    public void NewOrEditedClothingItemDTO_WhenPropertiesAreUpdated_ReflectsChanges()
    {
        // Arrange
        var dto = new NewOrEditedClothingItemDTO("Original", ClothingCategory.TShirt, Season.Fall, false, WearLocation.Home, 3, string.Empty);

        // Act
        dto.Name = "Updated";
        dto.Favourited = true;

        // Assert
        using (new AssertionScope())
        {
            dto.Name.Should().Be("Updated");
            dto.Favourited.Should().BeTrue();
        }
    }
}
