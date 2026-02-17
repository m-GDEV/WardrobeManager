using System.ComponentModel.DataAnnotations;

namespace WardrobeManager.Api.Database.Entities;

public interface IDatabaseEntity
{
    [Key]
    // Using a non-standard primary key name because the IdentityUser class has an 'Id' property of type string which makes 
    // the user class incapable of using this interface. So a new primary key name was made and User.cs has two keys.
    int PrimaryKeyId { get; }
}