using Microsoft.AspNetCore.Components;

namespace WardrobeManager.Presentation.Tests.Helpers;

/// <summary>
/// A testable concrete implementation of the abstract NavigationManager,
/// used to allow ViewModels that depend on NavigationManager to be instantiated
/// in unit tests without a real Blazor host.
/// </summary>
public class FakeNavigationManager : NavigationManager
{
    public FakeNavigationManager(string baseUri = "https://localhost/")
    {
        Initialize(baseUri, baseUri);
    }

    protected override void NavigateToCore(string uri, bool forceLoad)
    {
        // No-op in tests
    }
}
