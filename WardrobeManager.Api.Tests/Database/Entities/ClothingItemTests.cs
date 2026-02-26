using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Database.Entities;

public class ClothingItemTests
{
    private ClothingItem _clothingItem;

    [SetUp]
    public void Setup()
    {
        _clothingItem = new ClothingItem("T-Shirt", ClothingCategory.TShirt, Season.Fall,
            WearLocation.HomeAndOutside, false, 3, null);
    }

    [Test]
    public void Wear_WhenCalledOnce_IncrementsCounters()
    {
        // Arrange - item has 0 wears

        // Act
        _clothingItem.Wear();

        // Assert
        using (new AssertionScope())
        {
            _clothingItem.TimesWornTotal.Should().Be(1);
            _clothingItem.TimesWornSinceWash.Should().Be(1);
        }
    }

    [Test]
    public void Wear_WhenCalledMultipleTimes_IncrementsCountersCorrectly()
    {
        // Arrange
        const int wearCount = 5;

        // Act
        for (int i = 0; i < wearCount; i++)
        {
            _clothingItem.Wear();
        }

        // Assert
        using (new AssertionScope())
        {
            _clothingItem.TimesWornTotal.Should().Be(wearCount);
            _clothingItem.TimesWornSinceWash.Should().Be(wearCount);
        }
    }

    [Test]
    public void Wash_AfterWearing_ResetsTimesWornSinceWash()
    {
        // Arrange
        _clothingItem.Wear();
        _clothingItem.Wear();

        // Act
        _clothingItem.Wash();

        // Assert
        using (new AssertionScope())
        {
            _clothingItem.TimesWornSinceWash.Should().Be(0);
            _clothingItem.TimesWornTotal.Should().Be(2); // total does not reset on wash
        }
    }

    [Test]
    public void Wear_WhenCalled_UpdatesLastWornDate()
    {
        // Arrange
        var beforeWear = DateTime.UtcNow;

        // Act
        _clothingItem.Wear();

        // Assert
        _clothingItem.LastWorn.Should().BeOnOrAfter(beforeWear);
    }

    [Test]
    public void ClothingItem_WhenCreated_HasCorrectDefaults()
    {
        // Arrange - item created in Setup

        // Assert
        using (new AssertionScope())
        {
            _clothingItem.Name.Should().Be("T-Shirt");
            _clothingItem.Category.Should().Be(ClothingCategory.TShirt);
            _clothingItem.Season.Should().Be(Season.Fall);
            _clothingItem.WearLocation.Should().Be(WearLocation.HomeAndOutside);
            _clothingItem.Favourited.Should().BeFalse();
            _clothingItem.DesiredTimesWornBeforeWash.Should().Be(3);
            _clothingItem.TimesWornTotal.Should().Be(0);
            _clothingItem.TimesWornSinceWash.Should().Be(0);
        }
    }
}
