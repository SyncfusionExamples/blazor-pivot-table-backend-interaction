using Microsoft.AspNetCore.SignalR;
using Pivot_SignalR.Services;

namespace Pivot_SignalR.Hubs;

public class PivotHub : Hub
{
    private readonly UpdateConfiguration configuration;

    public PivotHub(UpdateConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SubscribeToOrders()
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            "Orders");
    }

    public async Task UnsubscribeFromOrders()
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            "Orders");
    }

    public Task UpdateSettings(bool isRunning, int interval)
    {
        configuration.IsRunning = isRunning;
        configuration.Interval = interval;

        return Task.CompletedTask;
    }
}