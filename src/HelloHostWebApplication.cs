using HelloHost.Application;

public class HelloHostWebApplication
{
    public static CancellationTokenSource CancellationSource { get; } = new CancellationTokenSource();
    public static CancellationToken StoppingToken => CancellationSource.Token;

    public static WebApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.secret.json", true, true)
            .AddEnvironmentVariables();

        builder.UseSerilog();

        builder.Services.AddOptions<AppSettings>();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        return builder;
    }
}
