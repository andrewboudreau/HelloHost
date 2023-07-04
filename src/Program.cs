
using Serilog;

var builder = WebApplication
	.CreateBuilder(args)
    .UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseSerilogRequestLogging(); // <-- Add this line
app.UseHsts();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
