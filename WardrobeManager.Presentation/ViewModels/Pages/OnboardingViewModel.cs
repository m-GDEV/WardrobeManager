using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Sysinfocus.AspNetCore.Components;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Presentation.ViewModels.Pages;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class OnboardingViewModel(
    INotificationService notificationService,
    IApiService apiService,
    IMvvmNavigationManager navManager
)
    : ViewModelBase
{
    [ObservableProperty] private int _currentStepIndex = 0;
    [ObservableProperty] private Dictionary<int, StepperState> _stepperStates = new();
    [ObservableProperty] private AuthenticationCredentialsModel _newAdminCredentials = new();

    public const int NumberOfSteps = 4;

    public override async Task OnInitializedAsync()
    {
        // var exists = await apiService.DoesAdminUserExist();
        // if (exists)
        // {
        //     navManager.NavigateTo<LoginViewModel>();
        // }

        // Initialize all stepper states as not completed
        CurrentStepIndex = 0; // handles edgecase where you go to login page but navigate back and CurrentStepIndex is still whatever it was
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
        if (CurrentStepIndex == 0)
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

    public void FinishOnboarding()
    {
        navManager.NavigateTo<LoginViewModel>();
    }

    public async Task CreateAdminUser()
    {
        var valid = StaticValidators.Validate<AuthenticationCredentialsModel>(NewAdminCredentials);
        if (!valid.Success)
        {
            foreach (var error in valid.Message.Split(".", StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrEmpty(error)) continue; // catches any blanks (i.e. last element)
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
        return CollectionExtensions.GetValueOrDefault(StepperStates, key, StepperState.Failed);
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