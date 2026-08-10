namespace Pivot_SignalR.Services;

public class UpdateConfiguration
{
    public bool IsRunning { get; set; } = true;

    public int Interval { get; set; } = 5000;
}