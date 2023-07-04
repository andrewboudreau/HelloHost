using Microsoft.AspNetCore.Builder;
using Serilog;

public static class LoggingExtensions
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }

    public static WebApplicationFactory UseLogging(this WebApplicationFactory builder)
    {
        builder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());
        return builder;
    }
}
