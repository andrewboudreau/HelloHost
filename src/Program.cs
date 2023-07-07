using HelloHost.Application;

using Microsoft.Extensions.Azure;

using Serilog;

namespace HelloHost;

public class Program
{
    public static async Task Main(string[] args)
    {
        var cancellationSource = new CancellationTokenSource();
        var stoppingToken = cancellationSource.Token;

        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.secret.json", true, true)
            .AddEnvironmentVariables();

        builder.Logging.AddSerilog();
        builder.Services.AddRazorPages();
        builder.Services.AddOptions<AppSettings>();
        builder.Services.AddHostedService<HostServiceBus>();


        var app = builder.Build();

        app.UseSerilogRequestLogging();
        app.UseHsts();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapRazorPages();

        try
        {
            await app.RunAsync(stoppingToken);
        }
        catch (TaskCanceledException)
        {
            Log.Warning("Host has been cancelled, shutting down.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}