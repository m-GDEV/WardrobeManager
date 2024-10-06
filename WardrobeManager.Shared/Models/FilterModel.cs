﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Models;

public class FilterModel
{
    public SortByCategories SortBy { get; set; } = SortByCategories.None;
    public bool IsAscending { get; set; } = true;

    public bool HasImage { get; set; }
    public bool Favourited { get; set; }
    public bool RecentlyAdded { get; set; }
    // Ensures backed doesn't filter based on one of these enums by default
    public ClothingCategory Category { get; set; } = ClothingCategory.None;
    public Season Season { get; set; } = Season.None;
    public DateTime DateAddedFrom { get; set; } = DateTime.UtcNow;
    public DateTime DateAddedTo { get; set; } = DateTime.UtcNow;
    public DateTime LastWornFrom { get; set; } = DateTime.UtcNow;
    public DateTime LastWornTo { get; set; } = DateTime.UtcNow;
    public DateTime LastEditedFrom { get; set; } = DateTime.UtcNow;
    public DateTime LastEditedTo { get; set; } = DateTime.UtcNow;
    public int TimesWorn { get; set; }
    public int TimesWornSinceWash { get; set; }
}
