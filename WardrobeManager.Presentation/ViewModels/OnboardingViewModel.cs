using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Dumpify;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Sysinfocus.AspNetCore.Components;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Pages.Public;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

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

    public int NumberOfSteps = 3;

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
            StepperStates[i] = StepperState.Pending;
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
        else
        {
            notificationService.AddNotification("You must specify a valid stepper state!", NotificationType.Warning);
        }

        CurrentStepIndex++;
    }

    public void GoToPreviousSection()
    {
        if (StepperStates.TryGetValue(CurrentStepIndex, out var stepperState))
        {
            StepperStates[CurrentStepIndex] = StepperState.Pending;
        }
        else
        {
            notificationService.AddNotification("You must specify a valid stepper state!", NotificationType.Warning);
        }

        CurrentStepIndex--;
    }

    public async Task CreateAdminUser()
    {
        if (NewAdminCredentials.Email == string.Empty || NewAdminCredentials.Password == string.Empty)
        {
            notificationService.AddNotification("You must specify a username and password!", NotificationType.Warning);
        }

        var res = await apiService.CreateAdminUserIfMissing(NewAdminCredentials);
        notificationService.AddNotification(res.Item2);

        // If added the admin user was sucessful
        if (res.Item1 is true)
        {
            GoToNextSection();
        }
    }

    public StepperState GetStepperStateSafely(int key)
    {
        return StepperStates.GetValueOrDefault(key, StepperState.Failed);
    }
}