using Pivot_SignalR.Models;

namespace Pivot_SignalR.Services;

public class OrderDataService
{
    private readonly List<Order> orders =
    [
        new() { OrderID = 1, CustomerName = "John Smith", EmployeeID = 101, Freight = 32.50, ShipCity = "New York" },
        new() { OrderID = 2, CustomerName = "Andrew Fuller", EmployeeID = 102, Freight = 45.75, ShipCity = "London" },
        new() { OrderID = 3, CustomerName = "Nancy Davolio", EmployeeID = 103, Freight = 28.25, ShipCity = "Berlin" },
        new() { OrderID = 4, CustomerName = "Robert King", EmployeeID = 104, Freight = 62.00, ShipCity = "Paris" },
        new() { OrderID = 5, CustomerName = "Margaret Peacock", EmployeeID = 105, Freight = 19.80, ShipCity = "Madrid" },
        new() { OrderID = 6, CustomerName = "Steven Buchanan", EmployeeID = 106, Freight = 75.40, ShipCity = "Rome" },
        new() { OrderID = 7, CustomerName = "Laura Callahan", EmployeeID = 107, Freight = 88.90, ShipCity = "Tokyo" },
        new() { OrderID = 8, CustomerName = "Michael Suyama", EmployeeID = 108, Freight = 54.25, ShipCity = "Sydney" },
        new() { OrderID = 9, CustomerName = "Janet Leverling", EmployeeID = 109, Freight = 29.50, ShipCity = "Toronto" },
        new() { OrderID = 10, CustomerName = "Anne Dodsworth", EmployeeID = 110, Freight = 66.75, ShipCity = "Singapore" },
        new() { OrderID = 11, CustomerName = "David Wilson", EmployeeID = 111, Freight = 41.20, ShipCity = "Dubai" },
        new() { OrderID = 12, CustomerName = "Emma Thompson", EmployeeID = 112, Freight = 93.60, ShipCity = "Amsterdam" },
        new() { OrderID = 13, CustomerName = "Chris Martin", EmployeeID = 113, Freight = 22.10, ShipCity = "Brussels" },
        new() { OrderID = 14, CustomerName = "Sophia Brown", EmployeeID = 114, Freight = 71.85, ShipCity = "Stockholm" },
        new() { OrderID = 15, CustomerName = "Daniel Green", EmployeeID = 115, Freight = 58.30, ShipCity = "Oslo" },
        new() { OrderID = 16, CustomerName = "Olivia Harris", EmployeeID = 116, Freight = 49.90, ShipCity = "Helsinki" },
        new() { OrderID = 17, CustomerName = "Liam Walker", EmployeeID = 117, Freight = 85.45, ShipCity = "Copenhagen" },
        new() { OrderID = 18, CustomerName = "Ava Scott", EmployeeID = 118, Freight = 38.70, ShipCity = "Vienna" },
        new() { OrderID = 19, CustomerName = "Noah Adams", EmployeeID = 119, Freight = 77.15, ShipCity = "Prague" },
        new() { OrderID = 20, CustomerName = "Mia Baker", EmployeeID = 120, Freight = 26.95, ShipCity = "Budapest" },
        new() { OrderID = 21, CustomerName = "Ethan Clark", EmployeeID = 121, Freight = 68.40, ShipCity = "Warsaw" },
        new() { OrderID = 22, CustomerName = "Charlotte Lewis", EmployeeID = 122, Freight = 44.80, ShipCity = "Athens" },
        new() { OrderID = 23, CustomerName = "James Hall", EmployeeID = 123, Freight = 91.25, ShipCity = "Lisbon" },
        new() { OrderID = 24, CustomerName = "Amelia Young", EmployeeID = 124, Freight = 35.60, ShipCity = "Zurich" },
        new() { OrderID = 25, CustomerName = "Benjamin Allen", EmployeeID = 125, Freight = 59.75, ShipCity = "Dublin" }
    ];

    public List<Order> GetOrders()
    {
        return orders;
    }
}