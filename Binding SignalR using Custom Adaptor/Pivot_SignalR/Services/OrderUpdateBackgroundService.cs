using Microsoft.AspNetCore.SignalR;
using Pivot_SignalR.Hubs;

namespace Pivot_SignalR.Services;

public class OrderUpdateBackgroundService : BackgroundService
{
    private readonly IHubContext<PivotHub> hubContext;
    private readonly OrderDataService orderDataService;
    private readonly UpdateConfiguration configuration;
    private readonly Random random = new();

    public OrderUpdateBackgroundService(
        IHubContext<PivotHub> hubContext,
        OrderDataService orderDataService,
        UpdateConfiguration configuration)
    {
        this.hubContext = hubContext;
        this.orderDataService = orderDataService;
        this.configuration = configuration;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(
                configuration.Interval,
                stoppingToken);

            if (!configuration.IsRunning)
            {
                continue;
            }

            var orders = orderDataService.GetOrders();

            if (orders.Count > 0)
            {
                foreach (var order in orders)
                {
                    order.Freight += random.Next(1, 10);
                }
            }

            await hubContext.Clients.All.SendAsync(
                "ReceiveOrderUpdate",
                orders,
                cancellationToken: stoppingToken);
        }
    }
}