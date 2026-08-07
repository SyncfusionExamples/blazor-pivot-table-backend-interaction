using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Pivot_SignalR.Models;

namespace Pivot_SignalR.Services;

public class OrderAdaptor : DataAdaptor
{
    private readonly OrderDataService orderDataService;

    public OrderAdaptor(OrderDataService orderDataService)
    {
        this.orderDataService = orderDataService;
    }

    public override object Read(DataManagerRequest dataManagerRequest,string? key = null)
    {
        var orders = orderDataService.GetOrders();

        return new DataResult
        {
            Result = orders,
            Count = orders.Count
        };
    }

    public override Task<object> ReadAsync(DataManagerRequest dataManagerRequest,string? key = null)
    {
        var orders = orderDataService.GetOrders();

        return Task.FromResult<object>(
            new DataResult
            {
                Result = orders,
                Count = orders.Count
            });
    }
}