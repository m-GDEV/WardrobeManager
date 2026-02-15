using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace WardrobeManager.Api.Database.Entities;

public class User : IdentityUser, DatabaseEntity
{
    public IEnumerable<IdentityRole>? Roles { get; set; }
    
    // Personal info
    public int Id { get; set; }
    public string Name { get; set;  } = "Default Name";
    public string ProfilePictureBase64 { get; set; } = string.Empty; // stored directly in db instead of file since there won't be many users & its convenient
    public DateTime AccountCreationDate = DateTime.UtcNow;

    // Data relationships
    // 1-many relationship with serverclothingitems
    // We don't care to return this info when return user info via api
    [JsonIgnore]
    public List<ServerClothingItem> ServerClothingItems { get; set;} = new List<ServerClothingItem>();
}
