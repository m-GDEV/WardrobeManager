using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Shared.Tests.StaticResources;

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

    [Test]
    public void GenerateRandomId_CalledTwice_ProducesUniqueIds()
    {
        // Arrange
        // Act
        var id1 = MiscMethods.GenerateRandomId();
        var id2 = MiscMethods.GenerateRandomId();
        // Assert
        id1.Should().NotBe(id2);
    }

    #region GetEmoji(ClothingCategory)

    [Test]
    public void GetEmoji_TShirt_ReturnsShirtEmoji()
    {
        // Arrange
        // Act
        var emoji = MiscMethods.GetEmoji(ClothingCategory.TShirt);
        // Assert
        emoji.Should().Be("👕");
    }

    [Test]
    public void GetEmoji_DressShirt_ReturnsTieEmoji()
    {
        // Arrange
        // Act
        var emoji = MiscMethods.GetEmoji(ClothingCategory.DressShirt);
        // Assert
        emoji.Should().Be("👔");
    }

    [Test]
    public void GetEmoji_Jeans_ReturnsTrousersEmoji()
    {
        // Arrange
        // Act
        var emoji = MiscMethods.GetEmoji(ClothingCategory.Jeans);
        // Assert
        emoji.Should().Be("👖");
    }

    #endregion

    #region GetEmoji(Season)

    [Test]
    public void GetEmoji_Fall_ReturnsFallEmoji()
    {
        // Arrange
        // Act
        var emoji = MiscMethods.GetEmoji(Season.Fall);
        // Assert
        emoji.Should().Be("🍂");
    }

    [Test]
    public void GetEmoji_Winter_ReturnsSnowflakeEmoji()
    {
        // Arrange
        // Act
        var emoji = MiscMethods.GetEmoji(Season.Winter);
        // Assert
        emoji.Should().Be("❄️");
    }

    [Test]
    public void GetEmoji_Summer_ReturnsSunEmoji()
    {
        // Arrange
        // Act
        var emoji = MiscMethods.GetEmoji(Season.Summer);
        // Assert
        emoji.Should().Be("☀️");
    }

    #endregion

    #region GetNameWithSpacesFromEnum

    [Test]
    public void GetNameWithSpacesFromEnum_CamelCaseEnum_InsertsSpaces()
    {
        // Arrange
        // Act
        var result = MiscMethods.GetNameWithSpacesFromEnum(ClothingCategory.DressShirt);
        // Assert
        result.Should().Be("Dress Shirt");
    }

    [Test]
    public void GetNameWithSpacesFromEnum_CompoundSeason_InsertsSpaces()
    {
        // Arrange
        // Act
        var result = MiscMethods.GetNameWithSpacesFromEnum(Season.FallAndWinter);
        // Assert
        result.Should().Be("Fall And Winter");
    }

    #endregion

    #region IsValidBase64

    [Test]
    public void IsValidBase64_WhenValidBase64_ReturnsTrue()
    {
        // Arrange
        var validBase64 = Convert.ToBase64String(new byte[] { 1, 2, 3, 4 });
        // Act
        var result = MiscMethods.IsValidBase64(validBase64);
        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsValidBase64_WhenInvalidBase64_ReturnsFalse()
    {
        // Arrange
        const string invalidBase64 = "not-valid-base64!!!";
        // Act
        var result = MiscMethods.IsValidBase64(invalidBase64);
        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsValidBase64_WhenEmptyString_ReturnsFalse()
    {
        // Arrange
        // Act
        var result = MiscMethods.IsValidBase64(string.Empty);
        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region CreateDefaultNewOrEditedClothingItemDTO

    [Test]
    public void CreateDefaultNewOrEditedClothingItemDTO_WhenCalled_ReturnsPopulatedDto()
    {
        // Arrange
        // Act
        var dto = MiscMethods.CreateDefaultNewOrEditedClothingItemDTO();
        // Assert
        using (new AssertionScope())
        {
            dto.Should().NotBeNull();
            dto.Name.Should().NotBeNullOrEmpty();
            dto.Category.Should().Be(ClothingCategory.TShirt);
            dto.Season.Should().Be(Season.Fall);
        }
    }

    #endregion
}