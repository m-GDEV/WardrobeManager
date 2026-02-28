#region

using Microsoft.AspNetCore.Components.WebAssembly.Http;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Presentation;

/// <summary>
/// Handler to ensure cookie credentials are automatically sent over with each request.
/// </summary>
public class CustomHttpMessageHandler(INotificationService notificationService) : DelegatingHandler
{
    /// <summary>
    /// Main method to override for the handler.
    /// </summary>
    /// <param name="request">The original request.</param>
    /// <param name="cancellationToken">The token to handle cancellations.</param>
    /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // include cookies!
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        request.Headers.Add("X-Requested-With", ["XMLHttpRequest"]);
        
        try
        {
            // This sends the actual HTTP request
            var response = await base.SendAsync(request, cancellationToken);

            // 1. Check for standard HTTP errors (like 404 Not Found, 500 Server Error)
            if (!response.IsSuccessStatusCode)
            {
                notificationService.AddNotification($"Server Error: {(int)response.StatusCode}", NotificationType.Error);
            }

            return response;
        }
        catch (HttpRequestException ex) // 2. Catch network failures (offline, CORS, server dead)
        {
            notificationService.AddNotification($"HttpRequestException: {ex.Message}", NotificationType.Error);
            // rethrow so exception is not lost, we just want to log it here.
            // If we don't rethrow code called after the http request will still run
            throw;  
        }
    }
}
