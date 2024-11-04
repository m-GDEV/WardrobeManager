using Microsoft.Extensions.Hosting;
using WardrobeManager.Presentation.Services.Implementation;
using WardrobeManager.Presentation.Services.Interfaces;

namespace WardrobeManager.Presentation.Services.Implementation;


// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-8.0&tabs=visual-studio
public class BackgroundWorkService : BackgroundService, IDisposable
{
    private Timer? _timer = null;
    private readonly IApiService _apiService;
    private readonly INotificationService _notificationService;
    private readonly IServiceProvider _serviceProvider;
    private int _executionCount;

    public BackgroundWorkService(IServiceProvider serviceProvider)
    {
        var scope = _serviceProvider.CreateScope();
        _apiService = scope.ServiceProvider.GetRequiredService<IApiService>();
        _notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        
        scope.ServiceProvider.GetRequiredService<ILogger<BackgroundWorkService>>().LogWarning("this is running");
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // When the timer should have no due-time, then do the work once now.
        await DoWork();

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await DoWork();
        }
    }

    // Could also be a async method, that can be awaited in ExecuteAsync above
    private async Task DoWork()
    {
        int count = Interlocked.Increment(ref _executionCount);
        
        // check api connectivity 
        var res = await _apiService.CheckApiConnection();

        if (!res.IsSuccessStatusCode)
        {
            _notificationService.AddNotification("Cannot reach API, application will not work as expected", NotificationType.Error);
        }
    }
}