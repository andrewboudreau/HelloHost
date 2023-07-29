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
}

public sealed class HelloHostWebApplicationBuilder : IHostApplicationBuilder
{
    private readonly WebApplicationBuilder webApplicationBuilder;

    public HelloHostWebApplicationBuilder(WebApplicationBuilder webApplicationBuilder)
    {
        this.webApplicationBuilder = webApplicationBuilder;
    }

    public HelloHostWebApplication Build()
    {
        return new HelloHostWebApplication(webApplicationBuilder.Build());
    }

    public WebApplicationBuilder WebApplicationBuilder => webApplicationBuilder;

    public IConfigurationManager Configuration => webApplicationBuilder.Configuration;

    public IHostEnvironment Environment => webApplicationBuilder.Environment;

    public ILoggingBuilder Logging => webApplicationBuilder.Logging;

    public IDictionary<object, object> Properties => ((IHostApplicationBuilder)webApplicationBuilder).Properties;

    public IServiceCollection Services => webApplicationBuilder.Services;

    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
    {
        ((IHostApplicationBuilder)webApplicationBuilder).ConfigureContainer(factory, configure);
    }
}
