public sealed class HelloHostWebApplicationBuilder : IHostApplicationBuilder
{
    private readonly WebApplicationBuilder webApplicationBuilder;

    public HelloHostWebApplicationBuilder(WebApplicationBuilder webApplicationBuilder)
    {
        this.webApplicationBuilder = webApplicationBuilder;
    }

    public WebApplication Build()
    {
        var app = webApplicationBuilder.Build();

        app.UseSerilogRequestLogging();

        app.UseHsts();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        return (WebApplication)new HelloHostWebApplication(app);
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
