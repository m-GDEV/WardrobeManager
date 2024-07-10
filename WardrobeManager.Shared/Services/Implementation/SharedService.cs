﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Services.Interfaces;

namespace WardrobeManager.Shared.Services.Implementation;
public class SharedService : ISharedService
{

    public bool IsValid(ServerClothingItem item)
    {
        if (item == null || item.Name == null)
        {
            return false;
        }

        return true;
    }

    private bool IsValidBase64(string input)
    {
        try
        {
            Convert.FromBase64String(input);
            return true;
        }
        catch
        {
            return false;

        }
    }

    public ServerClothingItem CreateDefaultServerClothingItem()
    {
        return new ServerClothingItem
            (
                name: "Default Name",
                ClothingCategory.TShirt,
                null
            );
    }
}
