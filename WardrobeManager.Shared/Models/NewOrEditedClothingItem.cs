using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardrobeManager.Shared.Models;
public class NewOrEditedClothingItem
{
    public NewOrEditedClothingItem() { } // ONLY FOR DESERIALIZER. THIS SHIT BETTER HAVE NO REFERENCES

    public NewOrEditedClothingItem(ServerClothingItem originalClothingItem, string ImageBase64 = "")
    {
        this.ClothingItem = originalClothingItem;
        this.ImageBase64 = ImageBase64;
    }


    public ServerClothingItem ClothingItem { get; set; }
    public string ImageBase64 { get; set; } // if this is empty the server will not update the image guid
}
