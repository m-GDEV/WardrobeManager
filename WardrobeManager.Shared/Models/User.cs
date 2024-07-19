using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardrobeManager.Shared.Models;
public class User(string userid)
{
    public int Id { get; set; }
    public string UserId { get; set; } = userid; // auth0 id

    public List<ServerClothingItem>? ServerClothingItems { get; set;}
}
