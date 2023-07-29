public sealed class HelloHostWebApplicationBuilder : IHostApplicationBuilder
{
    private readonly WebApplicationBuilder webApplicationBuilder;

    public HelloHostWebApplicationBuilder(WebApplicationBuilder webApplicationBuilder)
    {
        this.webApplicationBuilder = webApplicationBuilder;
    }

    public WebApplication Build(Action<HelloHostWebApplication>? configure = default, Action<WebApplication>? first = default)
    {
        var app = webApplicationBuilder.Build();

        app.UseSerilogRequestLogging();
        first?.Invoke(app);

        app.UseHsts();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        var host = new HelloHostWebApplication(app);
        configure?.Invoke(host);

        return (WebApplication)host;
    }

    public WebApplicationBuilder WebApplicationBuilder => webApplicationBuilder;

    #region WebApplicationBuilder passthroughs
    public IConfigurationManager Configuration => webApplicationBuilder.Configuration;
    public IHostEnvironment Environment => webApplicationBuilder.Environment;
    public ILoggingBuilder Logging => webApplicationBuilder.Logging;
    public IDictionary<object, object> Properties => ((IHostApplicationBuilder)webApplicationBuilder).Properties;
    public IServiceCollection Services => webApplicationBuilder.Services;
    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
        => ((IHostApplicationBuilder)webApplicationBuilder).ConfigureContainer(factory, configure);
    #endregion
}
