using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardrobeManager.Shared.Models;
public class User(string Auth0Id)
{
    // Personal info
    public int Id { get; set; }
    public string Auth0Id { get; set; } = Auth0Id; // used to identify user from auth0

    // Data relationships
    // 1-many relationship with serverclothingitems
    public List<ServerClothingItem> ServerClothingItems { get; set;} = new List<ServerClothingItem>();
}
