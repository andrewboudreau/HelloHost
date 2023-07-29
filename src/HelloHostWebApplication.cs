using Microsoft.AspNetCore.Builder;
using Serilog;

public class HelloHostWebApplication : WebApplication
{
    public static IWebHostBuilder CreateBuilder(string[] args)
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
