using Microsoft.AspNetCore.SignalR;
using Pivot_SignalR.Hubs;
using Pivot_SignalR.Models;

namespace Pivot_SignalR.Services;

public class OrderUpdateBackgroundService : BackgroundService
{
    private readonly IHubContext<PivotHub> hubContext;
    private readonly OrderDataService orderDataService;
    private readonly Random random = new();

    public OrderUpdateBackgroundService(IHubContext<PivotHub> hubContext, OrderDataService orderDataService)
    {
        this.hubContext = hubContext;
        this.orderDataService = orderDataService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5000, stoppingToken);

            var orders = orderDataService.GetOrders();

            if (orders.Count > 0)
            {
                var index = random.Next(orders.Count);

                orders[index].Freight += random.Next(1, 10);
            }

            await hubContext.Clients.All.SendAsync("ReceiveOrderUpdate", orders, cancellationToken: stoppingToken);
        }
    }
}