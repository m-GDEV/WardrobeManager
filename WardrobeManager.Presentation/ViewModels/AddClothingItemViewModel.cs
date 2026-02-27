using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using Microsoft.AspNetCore.Components.Forms;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Presentation.ViewModels;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class AddClothingItemViewModel(
    IMvvmNavigationManager navManager,
    IApiService apiService,
    INotificationService notificationService,
    IConfiguration configuration
)
    : ViewModelBase
{
    // Public Properties
    [ObservableProperty] private NewClothingItemDTO _newClothingItem = new NewClothingItemDTO();

    public ICollection<ClothingCategory> ClothingCategories { get; set; } =
        MiscMethods.ConvertEnumToCollection<ClothingCategory>();

    public ICollection<ClothingSize> ClothingSizes { get; set; } = MiscMethods.ConvertEnumToCollection<ClothingSize>();

    public async Task SubmitAsync()
    {
        // Crude error checking, in the future i'd prefer to use a form with error validation
        var res = StaticValidators.Validate(NewClothingItem);
        if (!res.Success)
        {
            notificationService.AddNotification(res.Message, NotificationType.Error);
            return;
        }

        await apiService.AddNewClothingItem(NewClothingItem);
        notificationService.AddNotification($"Clothing Item \"{NewClothingItem.Name}\" Added!",
            NotificationType.Success);
        NewClothingItem = new NewClothingItemDTO();
    }

    public async Task UploadImage(InputFileChangeEventArgs e)
    {
        try
        {
            var img = new MemoryStream();

            // 5MB default max file size
            var maxFileSize = configuration["WM_MAX_IMAGE_UPLOAD_SIZE_IN_MB"];
            int maxFileSizeNum;
            if (maxFileSize == null)
            {
                maxFileSizeNum = ProjectConstants.MaxImageSizeInMBFallback;
            }
            else
            {
                maxFileSizeNum = Convert.ToInt32(maxFileSize);
            }

            maxFileSizeNum *= 1024 * 1024; // int to megabytes

            await e.File.OpenReadStream(maxAllowedSize: maxFileSizeNum).CopyToAsync(img);

            NewClothingItem.ImageBase64 = Convert.ToBase64String(img.ToArray());

            if (NewClothingItem.ImageBase64 == string.Empty)
            {
                // If the image is too large it become an empty string. This is an edgecase but I can't remember how to reproduce it.
                notificationService.AddNotification("Image size too large, try again.", NotificationType.Warning);
            }
        }
        catch (IOException ex)
        {
            notificationService.AddNotification($"Error uploading image: {ex.Message}", NotificationType.Error);
        }
    }
}