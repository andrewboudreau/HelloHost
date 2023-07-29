using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

public static class SerilogExtensions
{
    public static IHostBuilder UseSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((host, sp, logConfig) =>
        {
            var appConfig = sp.GetRequiredService<IConfiguration>();

            logConfig
                .ReadFrom.Configuration(appConfig)
                .WriteTo.File("logs.txt")
                .WriteTo.Console(theme: AnsiConsoleTheme.Code);
        });

        return builder;
    }
}
