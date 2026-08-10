# Real-time Blazor Pivot Table Dashboard powered by SignalR & Syncfusion

A production-ready sample demonstrating **real-time data synchronization** between a server-side data source and the **Syncfusion Blazor Pivot Table** using **ASP.NET Core SignalR**. A background service pushes live order updates to all connected clients over a WebSocket hub, while a custom data adaptor feeds the Pivot Table with the latest aggregated state — no polling, no manual refresh.

The sample ships with **runtime controls** (Start/Stop and a configurable feed-delay input) so you can pause, resume, and tune the live update cadence directly from the UI. The Pivot Table is kept in sync automatically by calling `RefreshAsync(true)` whenever the hub broadcasts new data, and the background loop mutates **all** records each cycle so the changes are immediately visible across rows and columns.

---

## Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [Technology Stack](#technology-stack)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Usage Examples](#usage-examples)
- [Live Update Controls](#live-update-controls)
- [Update Configuration Service](#update-configuration-service)
- [Configuration](#configuration)
- [Performance Considerations](#performance-considerations)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgements](#acknowledgements)

---

## Overview

### What problem does it solve?

Traditional dashboards that visualize aggregated business data typically rely on **periodic polling** — the client periodically asks the server for updates. This approach wastes bandwidth, increases server load, and produces a **stale, "stuttering"** user experience where numbers jump in chunks instead of flowing smoothly.

This sample replaces polling with a **push-based model**. A long-running background service mutates the underlying order data and, every 5 seconds, broadcasts the full collection to every connected client through a SignalR hub. Blazor receives the broadcast, refreshes the Pivot Table by calling `RefreshAsync(true)`, and re-renders — giving the illusion of a **live, breathing dashboard**.

### Why does it exist?

This repository serves as a **reference implementation** for developers who need to:

- Bind the Syncfusion Blazor Pivot Table to a **custom data adaptor** (not a REST URL).
- Integrate SignalR push notifications with a Syncfusion Pivot Table.
- Build a real-time BI/analytics dashboard in .NET 10 / Blazor Server.
- Control the live update cadence at runtime (Start/Stop + configurable feed delay).
- Perform a full Pivot engine refresh using `RefreshAsync(true)` on each broadcast.

### Primary use cases

- Live freight / sales / inventory monitoring dashboards.
- Multi-user broadcast dashboards where all viewers must see the same numbers simultaneously.
- Prototyping real-time aggregation UIs on top of an in-memory or external data source.

### Business & technical benefits

| Benefit | Description |
|---------|-------------|
| Lower server load | One push per update instead of N client polls. |
| Lower latency | Updates reflected in milliseconds, not at the next poll interval. |
| Consistent UX | All connected clients see the same value at the same time. |
| Pluggable data source | The `OrderAdaptor` decouples the Pivot Table from where the data comes from. |
| Extensible | Drop in a database, an API, or a message broker behind `OrderDataService` without touching the UI. |

---

## Key Features

### Real-Time Updates via SignalR

A background service mutates order data every 5 seconds (default) and broadcasts the full collection to all connected clients through the `PivotHub`.

**Benefits:**
- Live data refresh with zero client polling.
- All clients stay in sync automatically.
- Automatic reconnection handled by the Blazor `HubConnection`.
- The update interval can be tuned at runtime (minimum **2000 ms**) and the feed can be paused/resumed without reloading the application.

### Custom Data Adaptor

The Syncfusion Pivot Table is bound through a custom `OrderAdaptor` (extends `DataAdaptor`) instead of a REST URL. This decouples the pivot table from any hard-coded endpoint and lets you source data from any service.

**Benefits:**
- Reuse business logic already registered in DI.
- No need to expose a public REST API just to feed the Pivot Table.
- Full control over read, filter, sort, and aggregation behavior.

### Built-in Field List

The `SfPivotView` ships with `ShowFieldList="true"`, so end users can drag-and-drop fields (CustomerName, EmployeeID, Freight) between rows, columns, and values at runtime.

**Benefits:**
- End-user-driven pivoting without redeployment.
- Faster ad-hoc analytics exploration.

### Automatic Refresh on Push

The Razor component subscribes to the `ReceiveOrderUpdate` event from the hub and calls `PivotObj.RefreshAsync(true)` + `StateHasChanged()`, so the aggregated cells recompute and render the moment new data arrives.

> **Note:** `RefreshAsync(true)` performs a **full Pivot engine refresh** and reloads data from the Custom Adaptor, enabling live updates without requiring a browser refresh.

**Benefits:**
- Aggregations stay correct after each mutation.
- No manual refresh button required.
- All records are updated during each refresh cycle, so changes are immediately visible across the grid.

### Live Update Controls

The UI ships with Start/Stop buttons and a `SfNumericTextBox` for the feed delay, so users can pause, resume, and re-tune the live update cadence at runtime without reloading the application.

**Benefits:**
- Pause and resume live updates through a shared `UpdateConfiguration` service.
- Dynamically change the update interval (minimum **2000 ms**).
- The hub exposes an `UpdateSettings(bool isRunning, int interval)` RPC method so any connected client can reconfigure the background loop.

### In-Memory Seed Data

`OrderDataService` ships with 25 sample orders across 25 cities, providing an immediately runnable demo with realistic data volumes.

**Benefits:**
- Works out-of-the-box with no database setup.
- Easy to swap with a real persistence layer.

---

## Technology Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET SDK | 10.0 | Target framework |
| ASP.NET Core | 10.0 | Host & endpoint routing |
| Blazor | Server (Interactive) | UI framework |
| SignalR | 10.0.10 (`Microsoft.AspNetCore.SignalR.Client`) | Real-time push channel |
| Syncfusion.Blazor.PivotTable | 34.2.2 | Pivot table component |
| Syncfusion.Blazor.Themes | 34.2.2 | Fluent2 theme |
| .NET Hosted Services | Built-in | Background push loop |

---

## Installation

### Prerequisites

- **.NET SDK 10.0** — [download](https://dotnet.microsoft.com/download)
- A **Syncfusion license key** (community license available for individuals & small businesses — [get one](https://www.syncfusion.com/products/communitylicense)).
- Any modern browser.

### Clone Repository

```bash
git clone https://github.com/SyncfusionExamples/blazor-pivot-table-integrations.git
cd Binding SignalR using Custom Adaptor
```

> Replace the URL above with the canonical repository URL if forking.

### Install Dependencies

```bash
cd Pivot_SignalR
dotnet restore
```

### Register Syncfusion License

Open `Pivot_SignalR/Program.cs` and uncomment the licensing line, replacing `YOUR LICENSE KEY` with your actual Syncfusion community / commercial license key:

```csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
```

### Run Application

```bash
dotnet run --project Pivot_SignalR
```

Open the terminal URL printed by .NET (typically `https://localhost:5xxx`) in your browser.

---

## Quick Start

1. **Clone and restore** (see [Installation](#installation)).
2. **Register your Syncfusion license** in `Program.cs`.
3. **Run:**

   ```bash
   dotnet run --project Pivot_SignalR
   ```

4. **Observe** the Pivot Table on the home page. Every 5 seconds (default) the `Freight` of **all** orders increases by 1–10, the background service broadcasts the change, and the pivot table refreshes automatically via `RefreshAsync(true)`.

### Expected result

A pivot table laid out as:

- **Rows:** `CustomerName`
- **Columns:** `EmployeeID`
- **Values:** `Freight` (sum)

The freight totals will silently update in place every 5 seconds — no manual refresh needed. Open the app in two browser tabs to confirm both clients receive updates simultaneously. Use the **Start Updating** / **Stop Updating** buttons and the **Feed Delay (ms)** input on the page to pause, resume, or re-tune the update cadence at runtime (minimum supported interval is **2000 ms**).

---

## Architecture

### System architecture

```text
┌─────────────────────────────────────────────────────────────┐
│                        Browser (Blazor Server)              │
│                                                             │
│  Home.razor                                                 │
│   ├── SfPivotView (bound via OrderAdaptor)                   │
│   └── HubConnection ── listens for "ReceiveOrderUpdate"     │
│                                                             │
└───────────────────────────────┬─────────────────────────────┘
                                │  WebSocket (SignalR)
                                ▼
┌─────────────────────────────────────────────────────────────┐
│                       ASP.NET Core (.NET 10)                │
│                                                             │
│  PivotHub  (/pivothub)                                       │
│   ├── SubscribeToOrders()    ── group membership            │
│   ├── UpdateSettings(bool, int) ── runtime reconfigure      │
│   └── ReceiveOrderUpdate     ── outbound broadcast event    │
│                                                             │
│  OrderUpdateBackgroundService  (interval-based loop)        │
│   ├── Reads Interval/IsRunning from UpdateConfiguration     │
│   ├── Mutates all orders' Freight per cycle                 │
│   └── hubContext.Clients.All.SendAsync("ReceiveOrderUpdate")│
│                                                             │
│  UpdateConfiguration  (singleton, IsRunning + Interval)     │
│                                                             │
│  OrderDataService   (singleton, in-memory List<Order>)      │
│                                                             │
│  OrderAdaptor : DataAdaptor  (scoped)                       │
│   └── ReadAsync() returns current orders to SfPivotView      │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Component roles

| Component | Lifetime | Responsibility |
|-----------|----------|----------------|
| `PivotHub` | Transient per connection | Manages client connections and provides subscription APIs, plus an `UpdateSettings(bool, int)` RPC method for runtime reconfiguration. |
| `OrderDataService` | Singleton | Holds the in-memory `List<Order>` and exposes `GetOrders()`. |
| `OrderAdaptor` | Scoped | Bridges Syncfusion `DataManager` requests to `OrderDataService`. |
| `OrderUpdateBackgroundService` | Hosted singleton | Mutates **all** orders on each loop, then pushes changes to all clients. |
| `UpdateConfiguration` | Singleton | Shared state holding `IsRunning` and `Interval` used to pause/resume and re-tune the push loop. |
| `Home.razor` | Per-connection | Renders the `SfPivotView`, the live update controls, and listens to the hub. |

### Data flow

1. Client opens the page → `HubConnection` connects to `/pivothub`.
2. `SfPivotView` triggers `OrderAdaptor.ReadAsync()` → returns current `List<Order>`.
3. Every **interval** (default 5s, configurable down to 2000 ms), `OrderUpdateBackgroundService` increments **all** orders' `Freight` by a random 1–10.
4. The service calls `SendAsync("ReceiveOrderUpdate", orders)` on `Clients.All`.
5. `Home.razor` receives the event → invokes `PivotObj.RefreshAsync(true)` → `StateHasChanged()`.
6. The pivot table re-aggregates and re-renders with the new data.
7. Users can **Start/Stop** the loop or change the **Feed Delay** from the UI; the change flows through the hub's `UpdateSettings` RPC into the shared `UpdateConfiguration` singleton.

### Key design decisions

- **In-memory store** keeps the demo dependency-free; swap with EF Core / a REST API / a message broker behind `OrderDataService`.
- **Custom adaptor over URL adaptor** keeps business logic server-side and DI-resolvable.
- **Background service for writes** simulates an external producer (e.g., an order intake system) without coupling the UI to the mutation cadence.
- **`Clients.All`** is used for simplicity; in production, use **groups** (`SubscribeToOrders` is already scaffolded in the hub) to scope broadcasts to interested clients only.

---

## Project Structure

```text
Pivot_SignalR/
├── Components/
│   ├── _Imports.razor            # Global using directives
│   ├── App.razor                 # Root component, loads Syncfusion theme + scripts
│   ├── Routes.razor              # Route assembly root
│   ├── Layout/
│   │   ├── MainLayout.razor      # Page shell
│   │   ├── MainLayout.razor.css
│   │   ├── NavMenu.razor         # Home navigation link
│   │   └── NavMenu.razor.css
│   └── Pages/
│       ├── Error.razor           # Error boundary UI
│       └── Home.razor            # Pivot Table + SignalR + live update controls
├── Hubs/
│   └── PivotHub.cs               # SignalR hub: connection lifecycle + groups
├── Models/
│   └── Order.cs                  # Order domain model
├── Services/
│   ├── OrderDataService.cs              # In-memory order store (singleton)
│   ├── OrderAdaptor.cs                  # Custom Syncfusion DataAdaptor (scoped)
│   ├── OrderUpdateBackgroundService.cs  # Interval-based mutation + push loop (hosted)
│   └── UpdateConfiguration.cs           # Shared IsRunning/Interval config (singleton)
├── Properties/
│   └── launchSettings.json       # HTTPS / HTTP launch profiles
├── wwwroot/                      # Static assets (Bootstrap, icons, fonts)
├── appsettings.json              # Logging + allowed hosts config
├── Program.cs                    # Service registration, hub mapping, middleware
├── Pivot_SignalR.csproj          # Project file (targets net10.0)
└── Pivot_SignalR.sln             # Solution file
```

### Folder purposes

- **`Components/`** — Razor components, layout and pages.
- **`Hubs/`** — SignalR hub exposing client-facing RPC methods.
- **`Models/`** — Plain domain objects shared between server and client.
- **`Services/`** — DI-registered services: data store, custom adaptor, background push service.
- **`wwwroot/`** — Static web assets served as-is.

---

## Usage Examples

### Basic: Bind the Pivot Table through a custom adaptor

```razor
<SfPivotView TValue="Order" Width="1000" Height="500" ShowFieldList="true">
    <PivotViewDataSourceSettings TValue="Order" ExpandAll="false" EnableSorting="true">

        <SfDataManager Adaptor="Adaptors.CustomAdaptor"
                       AdaptorInstance="@typeof(OrderAdaptor)">
        </SfDataManager>

        <PivotViewRows>
            <PivotViewRow Name="CustomerName"></PivotViewRow>
        </PivotViewRows>

        <PivotViewColumns>
            <PivotViewColumn Name="EmployeeID"></PivotViewColumn>
        </PivotViewColumns>

        <PivotViewValues>
            <PivotViewValue Name="Freight" Caption="Freight"></PivotViewValue>
        </PivotViewValues>

    </PivotViewDataSourceSettings>

    <PivotViewGridSettings ColumnWidth="120"></PivotViewGridSettings>
</SfPivotView>
```

### Intermediate: Wire up a SignalR client in a Razor component

```csharp
protected override async Task OnInitializedAsync()
{
    hubConnection = new HubConnectionBuilder()
        .WithUrl(Navigation.ToAbsoluteUri("/pivothub"))
        .WithAutomaticReconnect()
        .Build();

    hubConnection.On<List<Order>>("ReceiveOrderUpdate", async _ =>
    {
        await InvokeAsync(async () =>
        {
            // RefreshAsync(true) performs a full Pivot engine refresh and
            // reloads data from the Custom Adaptor, enabling live updates.
            if (PivotObj is not null)
            {
                await PivotObj.RefreshAsync(true);
            }
            StateHasChanged();
        });
    });

    await hubConnection.StartAsync();
}
```

### Advanced: Push updates from a background service

```csharp
public class OrderUpdateBackgroundService : BackgroundService
{
    private readonly IHubContext<PivotHub> hubContext;
    private readonly OrderDataService orderDataService;
    private readonly UpdateConfiguration configuration;
    private readonly Random random = new();

    public OrderUpdateBackgroundService(IHubContext<PivotHub> hubContext,
                                         OrderDataService orderDataService,
                                         UpdateConfiguration configuration)
    {
        this.hubContext = hubContext;
        this.orderDataService = orderDataService;
        this.configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Dynamic delay controlled through UpdateConfiguration.Interval.
            await Task.Delay(configuration.Interval, stoppingToken);

            // Pause support: skip the cycle when IsRunning is false.
            if (!configuration.IsRunning)
            {
                continue;
            }

            var orders = orderDataService.GetOrders();
            if (orders.Count > 0)
            {
                // Update all records each cycle so changes are visible across the grid.
                foreach (var order in orders)
                {
                    order.Freight += random.Next(1, 10);
                }
            }

            // Broadcast the updated data to all connected clients.
            await hubContext.Clients.All
                .SendAsync("ReceiveOrderUpdate", orders, cancellationToken: stoppingToken);
        }
    }
}
```

The interval is never allowed below **2000 ms** — the `SfNumericTextBox` on the page enforces `Min="2000"`, and the client calls the hub's `UpdateSettings(bool, int)` RPC to push the new value into the shared `UpdateConfiguration` singleton.

### Scoping broadcasts to subscribed clients (production pattern)

The hub already exposes `SubscribeToOrders` / `UnsubscribeFromOrders`. Replace `Clients.All` with `Clients.Group("Orders")`:

```csharp
await hubContext.Clients.Group("Orders")
    .SendAsync("ReceiveOrderUpdate", orders, cancellationToken: stoppingToken);
```

Have clients call `SubscribeToOrders()` after `StartAsync()` so only interested dashboards receive the traffic.

---

## Live Update Controls

The home page (`Home.razor`) exposes Syncfusion controls that let the user pause, resume, and re-tune the live update cadence at runtime — without reloading the application.

### Features

- **Start Updating** / **Stop Updating** buttons that toggle the background push loop via the hub.
- A **Feed Delay (ms)** input backed by `SfNumericTextBox` for dynamic update interval configuration.
- The minimum supported refresh interval is **2000 ms**.
- The current interval and running state are held in the shared `UpdateConfiguration` singleton, so any client's change takes effect for the background service immediately.

### Markup

```razor
<SfNumericTextBox TValue="int"
                  @bind-Value="FeedDelay"
                  Width="150px"
                  Min="2000"
                  Step="1000"
                  Format="N0">
</SfNumericTextBox>

<SfButton OnClick="StartUpdating">
    Start Updating...
</SfButton>

<SfButton OnClick="StopUpdating">
    Stop Updating...
</SfButton>
```

The button click handlers call the hub's `UpdateSettings(bool isRunning, int interval)` RPC method, which writes the new values into `UpdateConfiguration`. The `OrderUpdateBackgroundService` reads `IsRunning` and `Interval` on every loop iteration, so the change is applied on the next cycle.

### Expected behavior

1. The Pivot Table initially loads data through the Custom Adaptor.
2. The background service periodically updates **all** records.
3. SignalR broadcasts the updated data to connected clients.
4. The Pivot Table automatically refreshes using `RefreshAsync(true)`.
5. Users can start or stop live updates at runtime.
6. Users can dynamically change the update interval.
7. The minimum supported interval is **2000 ms**.
8. No browser refresh is required to view updated values.

---

## Update Configuration Service

The live update behavior is coordinated through a small shared state object registered as a singleton in DI.

### Definition

```csharp
public class UpdateConfiguration
{
    public bool IsRunning { get; set; } = true;

    public int Interval { get; set; } = 5000;
}
```

| Property | Default | Purpose |
|----------|---------|---------|
| `IsRunning` | `true` | Pauses (`false`) or resumes (`true`) the background push loop. |
| `Interval` | `5000` | Delay in milliseconds between update cycles (minimum `2000`). |

### Service registration

```csharp
builder.Services.AddSingleton<UpdateConfiguration>();
```

### Runtime updates

Connected clients reconfigure the loop by invoking the `UpdateSettings` hub method:

```csharp
public Task UpdateSettings(bool isRunning, int interval)
{
    configuration.IsRunning = isRunning;
    configuration.Interval = interval;

    return Task.CompletedTask;
}
```

Because `OrderUpdateBackgroundService` reads `configuration.Interval` and `configuration.IsRunning` inside its `while` loop, any change pushed through the hub takes effect on the very next cycle — there is no need to restart the application or the background service.

### Background service integration

The dynamic delay and start/stop support are implemented as:

```csharp
// Dynamic delay controlled through UpdateConfiguration.Interval.
await Task.Delay(configuration.Interval, stoppingToken);

// Start/Stop support — skip the cycle when paused.
if (!configuration.IsRunning)
{
    continue;
}
```

Each cycle mutates **all** orders and then broadcasts the full collection:

```csharp
foreach (var order in orders)
{
    order.Freight += random.Next(1, 10);
}
```

This approach clearly demonstrates real-time data changes in the Pivot Table, since every visible cell updates on every tick.

---

## Configuration

### `Program.cs` service registrations

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSyncfusionBlazor();
builder.Services.AddSingleton<OrderDataService>();   // shared in-memory store
builder.Services.AddScoped<OrderAdaptor>();         // one per connection
builder.Services.AddSingleton<UpdateConfiguration>(); // shared IsRunning/Interval state
builder.Services.AddSignalR();                     // SignalR infrastructure
builder.Services.AddHostedService<OrderUpdateBackgroundService>(); // push loop
```

### Hub endpoint mapping

```csharp
app.MapHub<PivotHub>("/pivothub");
```

### `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Optional customization

| Setting | Where | Description |
|---------|-------|-------------|
| Push cadence | `UpdateConfiguration.Interval` (default 5000) | Change via the **Feed Delay (ms)** `SfNumericTextBox` on the page (min 2000 ms) or by calling `UpdateSettings` on the hub. |
| Pause / resume | `UpdateConfiguration.IsRunning` | Toggle from the UI's **Start/Stop** buttons; the background loop skips cycles while `false`. |
| Mutation range | `OrderUpdateBackgroundService` | `random.Next(1, 10)` sets how much each `Freight` increments per tick — applied to **all** orders each cycle. |
| Pivot table dims | `Home.razor` | Adjust `PivotViewRows`, `PivotViewColumns`, `PivotViewValues`. |
| Theme | `App.razor` | `_content/Syncfusion.Blazor.Themes/fluent2.css` — swap for `bootstrap5.css`, `tailwind.css`, etc. |
| Data source | `OrderDataService` | Replace the in-memory list with EF Core, Dapper, an API call, or a message broker. |

### Syncfusion license

The license line is **commented by default** so the sample builds without requiring a key. For production usage uncomment and register it:

```csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
```

---

## Performance Considerations

### Scaling guidance

- **Replace `Clients.All` with `Clients.Group("Orders")`** — only push to clients who actually want order updates. The plumbing is already in `PivotHub`.
- **Use Azure SignalR Service** (`builder.Services.AddSignalR().AddAzureSignalR(...)`) when scaling beyond a single server instance. The in-process backplane does not fan out across multiple servers.
- **Prefer delta payloads** — for larger datasets, send only the changed order(s) rather than the entire `List<Order>` every interval. Mutate client-side state and refresh only the affected rows.

### Optimization recommendations

- Keep `OrderDataService` as a **singleton** so all clients and the background service share one in-memory copy (current behavior). If you switch to a database, prefer a short-lived cache + lazy refresh over per-request DB queries.
- Throttle the push cadence to the smallest interval that still provides business value. The default 5000 ms loop (minimum configurable 2000 ms) balances "feels live" with CPU / bandwidth cost.
- Disable automatic reconnection retries in scenarios where stale data is worse than a hard reconnect (`WithAutomaticReconnect()` can be dropped or configured with a custom retry policy).

### Best practices

- **Never block** inside `Hub` methods or the Razor `OnInitializedAsync` await chain.
- Always dispose the `HubConnection` — `Home.razor` already implements `IAsyncDisposable`.
- Keep the SignalR payload serializable (POCOs like `Order`, no cyclic references, no `Task` / `Action` fields).

---

## Troubleshooting

| Symptom | Likely cause | Fix |
|---------|--------------|-----|
| `Syncfusion.Licensing` warning at startup | License not registered | Uncomment the `RegisterLicense` line in `Program.cs` and add your key. |
| Pivot table renders empty | `OrderAdaptor.ReadAsync` returning empty list | Verify `OrderDataService.GetOrders()` is not empty. Confirm `SfDataManager.AdaptorInstance` is set. |
| Pivot table data does not update after a broadcast | `RefreshAsync(true)` not called or called without `true` | Ensure the `ReceiveOrderUpdate` handler calls `PivotObj.RefreshAsync(true)` (full engine refresh) followed by `StateHasChanged()`. |
| No real-time updates | Background service not started | Check DI: `AddHostedService<OrderUpdateBackgroundService>()` must be present in `Program.cs`. |
| Updates stop unexpectedly | `UpdateConfiguration.IsRunning` set to `false` | Click **Start Updating...** on the page or call `UpdateSettings(true, interval)` on the hub. |
| Interval change does not take effect | Value below 2000 ms or hub call missing | The `SfNumericTextBox` enforces `Min="2000"`. Confirm the Start/Stop handlers invoke `UpdateSettings(bool, int)`. The new interval applies on the next background cycle. |
| Hub connection fails with `ERR_CONNECTION_REFUSED` | Wrong hub URL or app not running | Confirm `app.MapHub<PivotHub>("/pivothub")` exists and the app is listening. Use `Navigation.ToAbsoluteUri("/pivothub")` (already in `Home.razor`) so the URL matches the host. |
| Updates only in one browser tab | `Clients.All` working but other client is on a different server | Use Azure SignalR Service for multi-server fan-out. |
| `dotnet restore` fails | .NET 10 SDK missing | Install the .NET 10 SDK from [dotnet.microsoft.com](https://dotnet.microsoft.com/download). Confirm with `dotnet --version`. |
| HTTPS dev cert prompt on first run | Missing dev certificate | Run `dotnet dev-certs https --trust` and accept the prompt. |
| Stale license warning despite key set | Key still commented | Re-open `Program.cs`; make sure the `RegisterLicense` line is uncommented **and** `using` Syncfusion.Licensing where required. |
| CPU spikes every interval on a large client count | Full payload broadcast to all clients | Switch to delta payloads + scoped group broadcasts (see [Performance Considerations](#performance-considerations)). |

---

## Contributing

### Development Setup

1. Fork the repository and clone your fork.
2. Install the **.NET 10 SDK**.
3. Run `dotnet restore` in the repo root.
4. Open the solution in your IDE of choice (Code Studio, Visual Studio, Rider).
5. Run via `dotnet run --project Pivot_SignalR` or the IDE’s run button.

### Coding Standards

- Follow the existing C# 12+ style: file-scoped namespaces, `var` for locals, `using` directives at the top.
- Use nullable reference types consistently (`<Nullable>enable</Nullable>` is already on).
- Keep `Models` classes simple POCOs; put business logic in `Services/`.
- Prefer constructor injection over service locators.
- Keep Razor `.razor` files focused on rendering; move non-trivial logic into a service or `@code` block helpers.

### Pull Request Guidelines

- One concern per PR; small and focused.
- Include a clear description: what changed, why, and how to verify.
- Run `dotnet build` cleanly before requesting review.
- Don’t introduce new dependencies without justification.
- Never commit your Syncfusion license key.

### Issue Reporting Guidelines

- Use the issue template (or include the following): .NET SDK version, browser, OS, repro steps, expected vs. actual behavior, and relevant logs.
- Search existing issues before opening a new one.
- Tag the issue with `bug`, `enhancement`, or `question`.

