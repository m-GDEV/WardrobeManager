#region

using System.Text.Json.Serialization;

#endregion

namespace WardrobeManager.Api.Database.Entities;

// As of right now, the AppUser class is only used for authentication. 
// The User class is used to connect an AppUser with User and give them a profile in the app
// In the future it might be a good idea to merge these
public class User(string Auth0Id)
{
    // Because the user is created solely when we only know their Auth0Id,
    // everything in this class has a sensisble default. When the user
    // wants to change their profile details they may perform a PUT request
    // with a DTO object


    // Personal info
    public int Id { get; set; }
    // used to identify user from auth0
    public string Auth0Id { get; set; } = Auth0Id;
    public string Name { get; set;  } = "Default Name";
    // stored directly in db instead of file since there won't be many users & its convenient
    public string ProfilePictureBase64 { get; set; } = string.Empty;
    public DateTime AccountCreationDate = DateTime.UtcNow;

    // Data relationships
    // 1-many relationship with serverclothingitems
    // We don't care to return this info when return user info via api
    [JsonIgnore]
    public List<ServerClothingItem> ServerClothingItems { get; set;} = new List<ServerClothingItem>();
}
