using Pivot_SignalR.Components;
using Pivot_SignalR.Hubs;
using Pivot_SignalR.Services;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Syncfusion
builder.Services.AddSyncfusionBlazor();

// Register application services
builder.Services.AddSingleton<OrderDataService>();

// Register Custom Adaptor
builder.Services.AddScoped<OrderAdaptor>();

builder.Services.AddSingleton<UpdateConfiguration>();

// Register SignalR
builder.Services.AddSignalR();

// Register Background Service
builder.Services.AddHostedService<OrderUpdateBackgroundService>();

var app = builder.Build();

// Register Syncfusion license
// Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map SignalR Hub
app.MapHub<PivotHub>("/pivothub");

app.Run();