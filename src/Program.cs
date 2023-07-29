using Serilog;

var builder = HelloHostWebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseHsts();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Map("/", () => Results.Ok("Hello, World!"));

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
