
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHsts();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
