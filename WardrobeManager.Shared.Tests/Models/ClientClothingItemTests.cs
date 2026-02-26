using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Tests.Models;

public class ClientClothingItemTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ClientClothingItem_InitializeWithProperties_HasPropertiesSet()
    {
        // Arrange
        const string name = "My Blue T-Shirt";
        const ClothingCategory category = ClothingCategory.TShirt;
        const string imageBase64 = "base64encodedstring";

        // Act
        var item = new ClientClothingItem(name, category, imageBase64);

        // Assert
        using (new AssertionScope())
        {
            item.Name.Should().Be(name);
            item.Category.Should().Be(category);
            item.ImageBase64.Should().Be(imageBase64);
        }
    }

    [Test]
    public void ClientClothingItem_WhenCreated_HasDefaultIdOfZero()
    {
        // Arrange
        // Act
        var item = new ClientClothingItem("Jeans", ClothingCategory.Jeans, string.Empty);

        // Assert
        item.Id.Should().Be(0);
    }

    [Test]
    public void ClientClothingItem_WhenIdIsSet_ReturnsSetId()
    {
        // Arrange
        var item = new ClientClothingItem("Sweater", ClothingCategory.Sweater, string.Empty);

        // Act
        item.Id = 42;

        // Assert
        item.Id.Should().Be(42);
    }
}
