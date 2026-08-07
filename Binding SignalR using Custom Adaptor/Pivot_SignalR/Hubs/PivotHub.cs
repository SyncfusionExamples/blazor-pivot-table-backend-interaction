using Microsoft.AspNetCore.SignalR;

namespace Pivot_SignalR.Hubs;

public class PivotHub : Hub
{
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
}