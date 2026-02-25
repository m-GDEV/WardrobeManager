#region

using Microsoft.AspNetCore.Components.Authorization;
using WardrobeManager.Presentation.Identity.Models;

#endregion

namespace WardrobeManager.Presentation.Identity;

/// <summary>
/// Account management services.
/// </summary>
public interface IAccountManagement
{
    /// <summary>
    /// Login service.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <param name="password">User's password.</param>
    /// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
    public Task<FormResult> LoginAsync(string email, string password);

    /// <summary>
    /// Log out the logged in user.
    /// </summary>
    /// <returns>Whether the logout was sucessful</returns>
    public Task<bool> LogoutAsync();

    /// <summary>
    /// Registration service.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <param name="password">User's password.</param>
    /// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
    public Task<FormResult> RegisterAsync(string email, string password);
    
    
    /// <summary>
    /// Gets the authentication state of the current user
    /// </summary>
    /// <returns>AuthenticationState object</returns>
    public Task<AuthenticationState> GetAuthenticationStateAsync();
}