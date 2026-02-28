using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Tests.Models;

public class FilterModelTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void FilterModel_WhenCreated_HasCorrectDefaults()
    {
        // Arrange
        // Act
        var model = new FilterModel();

        // Assert
        using (new AssertionScope())
        {
            model.SearchQuery.Should().Be(string.Empty);
            model.SortBy.Should().Be(SortByCategories.None);
            model.IsAscending.Should().BeTrue();
            model.HasImage.Should().BeFalse();
            model.Favourited.Should().BeFalse();
            model.RecentlyAdded.Should().BeFalse();
            model.Category.Should().Be(ClothingCategory.None);
            model.Season.Should().Be(Season.None);
            model.DateAddedFrom.Should().BeNull();
            model.DateAddedTo.Should().BeNull();
        }
    }

    [Test]
    public void FilterModel_WhenPropertiesAreSet_ReflectsChanges()
    {
        // Arrange
        var model = new FilterModel();

        // Act
        model.SearchQuery = "blue shirt";
        model.Favourited = true;
        model.Category = ClothingCategory.TShirt;
        model.Season = Season.Summer;

        // Assert
        using (new AssertionScope())
        {
            model.SearchQuery.Should().Be("blue shirt");
            model.Favourited.Should().BeTrue();
            model.Category.Should().Be(ClothingCategory.TShirt);
            model.Season.Should().Be(Season.Summer);
        }
    }
}
