using HelloHost.Application;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog();
builder.Services.AddRazorPages();
builder.Services.AddOptions<AppSettings>();
builder.Services.AddHostedService<HostServiceBus>();


var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHsts();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
