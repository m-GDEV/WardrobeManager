#region

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

#endregion

namespace WardrobeManager.Presentation;

public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigation)
        : base(provider, navigation)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://localhost:7026" });
            //scopes: new[] { "example.read", "example.write" });
    }
}