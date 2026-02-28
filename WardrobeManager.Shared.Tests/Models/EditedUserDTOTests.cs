using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.DTOs;

namespace WardrobeManager.Shared.Tests.Models;

public class EditedUserDTOTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void EditedUserDTO_InitializeWithProperties_HasPropertiesSet()
    {
        // Arrange
        const string name = "John Doe";
        const string profilePic = "base64profilepic";

        // Act
        var dto = new EditedUserDTO(name, profilePic);

        // Assert
        using (new AssertionScope())
        {
            dto.Name.Should().Be(name);
            dto.ProfilePictureBase64.Should().Be(profilePic);
        }
    }

    [Test]
    public void EditedUserDTO_WhenPropertiesAreUpdated_ReflectsChanges()
    {
        // Arrange
        var dto = new EditedUserDTO("Original Name", "original-pic");

        // Act
        dto.Name = "Updated Name";
        dto.ProfilePictureBase64 = "updated-pic";

        // Assert
        using (new AssertionScope())
        {
            dto.Name.Should().Be("Updated Name");
            dto.ProfilePictureBase64.Should().Be("updated-pic");
        }
    }

    [Test]
    public void EditedUserDTO_WhenNameIsEmpty_AcceptsEmptyString()
    {
        // Arrange
        // Act
        var dto = new EditedUserDTO(string.Empty, string.Empty);

        // Assert
        using (new AssertionScope())
        {
            dto.Name.Should().Be(string.Empty);
            dto.ProfilePictureBase64.Should().Be(string.Empty);
        }
    }
}
