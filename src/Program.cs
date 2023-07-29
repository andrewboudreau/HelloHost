//
// A sample configuration and startup for a web application.

using HelloHost;

var builder = HelloHostWebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/", () => Results.Ok("Hello, World!"));

await app.RunAsyncWithShutdown();

