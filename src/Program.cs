
using Infrastructure;

var builder = WebApplication
	.CreateBuilder(args)
    .UseLogging();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseLogging();
app.UseHsts();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
