namespace HelloHost;

public static class ApplicationExtensions
{
    public static async Task RunAsyncWithShutdown(this WebApplication app)
    {
        try
        {
            await app.RunAsync(HelloHostWebApplication.StoppingToken);
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
            Log.CloseAndFlush();
        }
    }
}
