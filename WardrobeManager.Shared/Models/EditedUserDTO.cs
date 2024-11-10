using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Models;

/// <summary> DTO that client sends to edit their user details</summary>
public class EditedUserDTO
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public EditedUserDTO() { } // ONLY FOR DESERIALIZER. THIS SHIT BETTER HAVE NO REFERENCES
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    public EditedUserDTO
        (string name, string profilePictureBase64)
    {
        this.Name = name;
        this.ProfilePictureBase64 = profilePictureBase64;
    }

    public string Name { get; set; }
    // Based64 validation is left to service that adds this to db
    public string ProfilePictureBase64 { get; set; }
}
