using HelloHost.Application;

public sealed class HelloHostWebApplication
{
    public WebApplication WebApplication { get; private set; }

    public HelloHostWebApplication(WebApplication webApplication)
    {
        WebApplication = webApplication;
    }

    internal static CancellationTokenSource CancellationSource { get; } = new CancellationTokenSource();

    public static CancellationToken StoppingToken => CancellationSource.Token;

    public static HelloHostWebApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.secret.json", true, true)
            .AddEnvironmentVariables();

        builder.UseSerilog();

        builder.Services.AddOptions<AppSettings>();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        return new HelloHostWebApplicationBuilder(builder);
    }

    public static implicit operator WebApplication(HelloHostWebApplication host)
    {
        return host.WebApplication;
    }
}
