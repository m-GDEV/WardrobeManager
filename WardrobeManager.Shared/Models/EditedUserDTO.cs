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
    public EditedUserDTO() { } // ONLY FOR DESERIALIZER. THIS SHIT BETTER HAVE NO REFERENCES


    public EditedUserDTO
        (string name, string profilePictureBase64) {
            this.Name = name;
            this.ProfilePictureBase64 = profilePictureBase64;
        }

    public string Name { get; set; }
    // Based64 validation is left to service that adds this to db
    public string ProfilePictureBase64 { get; set; }
}
