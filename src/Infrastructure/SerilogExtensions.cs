using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

public static class SerilogExtensions
{
    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((host, sp, loggingConfig) =>
        {
            var appConfig = sp.GetRequiredService<IConfiguration>();

            loggingConfig
                .ReadFrom.Configuration(appConfig)
                .WriteTo.File("logs.txt")
                .WriteTo.Console(theme: AnsiConsoleTheme.Code);
        });

        return builder;
    }
}
