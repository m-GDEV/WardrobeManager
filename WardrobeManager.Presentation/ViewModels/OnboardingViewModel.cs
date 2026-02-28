using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Sysinfocus.AspNetCore.Components;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Pages.Public;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Presentation.ViewModels;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class OnboardingViewModel(
    INotificationService notificationService,
    IApiService apiService,
    IMvvmNavigationManager navManager,
    ILogger<OnboardingViewModel> logger
)
    : ViewModelBase
{
    [ObservableProperty] private int _currentStepIndex;
    [ObservableProperty] private Dictionary<int, StepperState> _stepperStates = new();
    [ObservableProperty] private AuthenticationCredentialsModel _newAdminCredentials = new();

    public const int NumberOfSteps = 3;

    public override async Task OnInitializedAsync()
    {
        var exists = await apiService.DoesAdminUserExist();
        if (exists)
        {
            navManager.NavigateTo<LoginViewModel>();
        }

        // Initialize all stepper states as not completed
        for (int i = 0; i < NumberOfSteps; i++)
        {
            StepperStates[i] = i == 0 ? StepperState.Current : StepperState.Pending;
        }
    }

    public void GoToNextSection()
    {
        if (CurrentStepIndex == NumberOfSteps - 1)
        {
            navManager.NavigateTo<LoginViewModel>();
            return;
        }

        if (StepperStates.TryGetValue(CurrentStepIndex, out var stepperState))
        {
            StepperStates[CurrentStepIndex] = StepperState.Complete;
        }

        CurrentStepIndex++;

        if (StepperStates.TryGetValue(CurrentStepIndex, out var nextStepperState))
        {
            StepperStates[CurrentStepIndex] = StepperState.Current;
        }
    }

    public void GoToPreviousSection()
    {
        if (CurrentStepIndex == NumberOfSteps)
        {
            return;
        }

        if (StepperStates.TryGetValue(CurrentStepIndex, out var stepperState))
        {
            StepperStates[CurrentStepIndex] = StepperState.Pending;
        }

        CurrentStepIndex--;

        if (StepperStates.TryGetValue(CurrentStepIndex, out var oldStepperState))
        {
            StepperStates[CurrentStepIndex] = StepperState.Current;
        }
    }

    public async Task CreateAdminUser()
    {
        var valid = StaticValidators.Validate(NewAdminCredentials);
        if (!valid.Success)
        {
            foreach (var error in valid.Message.Split("."))
            {
                if (string.IsNullOrEmpty(error)) return; // catches the last element in the split
                notificationService.AddNotification(error, NotificationType.Error);
            }

            return;
        }

        var res = await apiService.CreateAdminUserIfMissing(NewAdminCredentials);
        if (!res.Success)
        {
            notificationService.AddNotification(res.Message, NotificationType.Error);
            return;
        }

        // If added the admin user was sucessful
        GoToNextSection();
    }

    public StepperState GetStepperStateSafely(int key)
    {
        return StepperStates.GetValueOrDefault(key, StepperState.Failed);
    }
    
    // Stupid that i'm doing this but its the easiest solution and idk what the best method is
    public void SetEmail(string email)
    {
        NewAdminCredentials.Email = email;
    }

    public void SetPassword(string password)
    {
        NewAdminCredentials.Password = password;
    }
}