namespace Pivot_SignalR.Models;

/// <summary>
/// Represents an order used in the Pivot Table SignalR sample.
/// </summary>
public class Order
{
    /// <summary>
    /// Gets or sets the unique order identifier.
    /// </summary>
    public int OrderID { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee identifier.
    /// </summary>
    public int EmployeeID { get; set; }

    /// <summary>
    /// Gets or sets the freight amount.
    /// </summary>
    public double Freight { get; set; }

    /// <summary>
    /// Gets or sets the shipping city.
    /// </summary>
    public string ShipCity { get; set; } = string.Empty;
}