using HelloHost.Application;

using Serilog;

var cancellationSource = new CancellationTokenSource();
var stoppingToken = cancellationSource.Token;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.secret.json", true, true)
    .AddEnvironmentVariables();

builder.UseSerilog();

builder.Services.AddOptions<AppSettings>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
app.Map("/", () => Results.Ok("Hello, World!"));

app.UseSerilogRequestLogging();
app.UseHsts();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

try
{
    await app.RunAsync(stoppingToken);
}
catch (TaskCanceledException)
{
    Log.Information("Host has been cancelled, shutting down.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    await app.DisposeAsync();
    Log.Information("App Disposed.");
    Log.CloseAndFlush();
}
