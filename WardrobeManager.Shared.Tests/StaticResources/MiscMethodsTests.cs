using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Shared.Tests.StaticResources;

public class MiscMethodsTests
{
    private MiscMethods _miscMethods;

    [SetUp]
    public void Setup()
    {
        _miscMethods = new MiscMethods();
    }

    [Test]
    public void GenerateRandomId_DefaultScenario_CorrectFormat()
    {
        // Arrange
        // Act
        var randomId = _miscMethods.GenerateRandomId();
        // Assert
        randomId.Length.Should().Be(10);
        randomId.Should().StartWith("id");
    }

    [Test]
    public void GenerateRandomId_CalledTwice_ProducesUniqueIds()
    {
        // Arrange
        // Act
        var id1 = _miscMethods.GenerateRandomId();
        var id2 = _miscMethods.GenerateRandomId();
        // Assert
        id1.Should().NotBe(id2);
    }

    #region GetEmoji(ClothingCategory)

    [Test]
    public void GetEmoji_TShirt_ReturnsShirtEmoji()
    {
        var emoji = _miscMethods.GetEmoji(ClothingCategory.TShirt);
        emoji.Should().Be("👕");
    }

    [Test]
    public void GetEmoji_DressShirt_ReturnsTieEmoji()
    {
        var emoji = _miscMethods.GetEmoji(ClothingCategory.DressShirt);
        emoji.Should().Be("👔");
    }

    [Test]
    public void GetEmoji_Jeans_ReturnsTrousersEmoji()
    {
        var emoji = _miscMethods.GetEmoji(ClothingCategory.Jeans);
        emoji.Should().Be("👖");
    }

    [Test]
    public void GetEmoji_Sweater_ReturnsCoatEmoji()
    {
        var emoji = _miscMethods.GetEmoji(ClothingCategory.Sweater);
        emoji.Should().Be("🧥");
    }

    [Test]
    public void GetEmoji_Shorts_ReturnsShortsEmoji()
    {
        var emoji = _miscMethods.GetEmoji(ClothingCategory.Shorts);
        emoji.Should().Be("🩳");
    }

    [Test]
    public void GetEmoji_Sweatpants_ReturnsTrousersEmoji()
    {
        var emoji = _miscMethods.GetEmoji(ClothingCategory.Sweatpants);
        emoji.Should().Be("👖");
    }

    [Test]
    public void GetEmoji_DressPants_ReturnsTrousersEmoji()
    {
        var emoji = _miscMethods.GetEmoji(ClothingCategory.DressPants);
        emoji.Should().Be("👖");
    }

    [Test]
    public void GetEmoji_NoneCategory_ReturnsEmptyString()
    {
        var emoji = _miscMethods.GetEmoji(ClothingCategory.None);
        emoji.Should().Be(string.Empty);
    }

    #endregion

    #region GetEmoji(Season)

    [Test]
    public void GetEmoji_Fall_ReturnsFallEmoji()
    {
        var emoji = _miscMethods.GetEmoji(Season.Fall);
        emoji.Should().Be("🍂");
    }

    [Test]
    public void GetEmoji_Winter_ReturnsSnowflakeEmoji()
    {
        var emoji = _miscMethods.GetEmoji(Season.Winter);
        emoji.Should().Be("❄️");
    }

    [Test]
    public void GetEmoji_Summer_ReturnsSunEmoji()
    {
        var emoji = _miscMethods.GetEmoji(Season.Summer);
        emoji.Should().Be("☀️");
    }

    [Test]
    public void GetEmoji_Spring_ReturnsBlossomEmoji()
    {
        var emoji = _miscMethods.GetEmoji(Season.Spring);
        emoji.Should().Be("🌸");
    }

    [Test]
    public void GetEmoji_FallAndWinter_ReturnsCombinedEmoji()
    {
        var emoji = _miscMethods.GetEmoji(Season.FallAndWinter);
        emoji.Should().Be("🍂❄️");
    }

    [Test]
    public void GetEmoji_SpringAndSummer_ReturnsCombinedEmoji()
    {
        var emoji = _miscMethods.GetEmoji(Season.SpringAndSummer);
        emoji.Should().Be("🌸☀️");
    }

    [Test]
    public void GetEmoji_SummerAndFall_ReturnsCombinedEmoji()
    {
        var emoji = _miscMethods.GetEmoji(Season.SummerAndFall);
        emoji.Should().Be("☀️🍂");
    }

    #endregion

    #region GetNameWithSpacesFromEnum

    [Test]
    public void GetNameWithSpacesFromEnum_ClothingCategory_ReturnsWithEmojiAndSpaces()
    {
        // GetNameWithSpacesFromEnum delegates to GetNameWithSpacesAndEmoji for ClothingCategory
        var result = _miscMethods.GetNameWithSpacesFromEnum(ClothingCategory.DressShirt);
        using (new AssertionScope())
        {
            result.Should().Contain("Dress");
            result.Should().Contain("Shirt");
            result.Should().Contain("👔");
        }
    }

    [Test]
    public void GetNameWithSpacesFromEnum_Season_ReturnsWithEmojiAndSpaces()
    {
        // GetNameWithSpacesFromEnum delegates to GetNameWithSpacesAndEmoji for Season
        var result = _miscMethods.GetNameWithSpacesFromEnum(Season.FallAndWinter);
        using (new AssertionScope())
        {
            result.Should().Contain("Fall");
            result.Should().Contain("Winter");
            result.Should().Contain("🍂");
        }
    }

    [Test]
    public void GetNameWithSpacesFromEnum_OtherEnum_ReturnsWithSpaces()
    {
        // For non-ClothingCategory, non-Season enums it falls through to GetNameWithSpaces
        var result = _miscMethods.GetNameWithSpacesFromEnum(WearLocation.HomeAndOutside);
        using (new AssertionScope())
        {
            result.Should().Contain("Home");
            result.Should().Contain("Outside");
        }
    }

    #endregion

    #region IsValidBase64

    [Test]
    public void IsValidBase64_WhenValidBase64_ReturnsTrue()
    {
        var validBase64 = Convert.ToBase64String(new byte[] { 1, 2, 3, 4 });
        var result = _miscMethods.IsValidBase64(validBase64);
        result.Should().BeTrue();
    }

    [Test]
    public void IsValidBase64_WhenInvalidBase64_ReturnsFalse()
    {
        const string invalidBase64 = "not-valid-base64!!!";
        var result = _miscMethods.IsValidBase64(invalidBase64);
        result.Should().BeFalse();
    }

    [Test]
    public void IsValidBase64_WhenEmptyString_ReturnsFalse()
    {
        var result = _miscMethods.IsValidBase64(string.Empty);
        result.Should().BeFalse();
    }

    #endregion

    #region GetNameWithSpacesAndEmoji(ClothingCategory)

    [Test]
    public void GetNameWithSpacesAndEmoji_TShirt_ReturnsShirtWithEmoji()
    {
        // regex [A-Z][a-z]+ extracts "Shirt" from "TShirt"
        var result = _miscMethods.GetNameWithSpacesAndEmoji(ClothingCategory.TShirt);
        using (new AssertionScope())
        {
            result.Should().Contain("Shirt");
            result.Should().Contain("👕");
        }
    }

    [Test]
    public void GetNameWithSpacesAndEmoji_DressShirt_ReturnsDressShirtWithEmoji()
    {
        var result = _miscMethods.GetNameWithSpacesAndEmoji(ClothingCategory.DressShirt);
        using (new AssertionScope())
        {
            result.Should().Contain("Dress");
            result.Should().Contain("Shirt");
            result.Should().Contain("👔");
        }
    }

    #endregion

    #region GetNameWithSpacesAndEmoji(Season)

    [Test]
    public void GetNameWithSpacesAndEmoji_Fall_ReturnsFallWithEmoji()
    {
        var result = _miscMethods.GetNameWithSpacesAndEmoji(Season.Fall);
        using (new AssertionScope())
        {
            result.Should().Contain("Fall");
            result.Should().Contain("🍂");
        }
    }

    [Test]
    public void GetNameWithSpacesAndEmoji_Summer_ReturnsSummerWithEmoji()
    {
        var result = _miscMethods.GetNameWithSpacesAndEmoji(Season.Summer);
        using (new AssertionScope())
        {
            result.Should().Contain("Summer");
            result.Should().Contain("☀️");
        }
    }

    #endregion

    #region ConvertEnumToCollection

    [Test]
    public void ConvertEnumToCollection_ClothingCategory_ReturnsAllValues()
    {
        var result = _miscMethods.ConvertEnumToCollection<ClothingCategory>();
        result.Count.Should().Be(Enum.GetValues<ClothingCategory>().Length);
    }

    [Test]
    public void ConvertEnumToCollection_Season_ReturnsAllValues()
    {
        var result = _miscMethods.ConvertEnumToCollection<Season>();
        result.Count.Should().Be(Enum.GetValues<Season>().Length);
    }

    #endregion
}
