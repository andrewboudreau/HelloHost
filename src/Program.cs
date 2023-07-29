//
// A sample configuration and startup for a web application.

using HelloHost;

var builder = HelloHostWebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/", () => Results.Ok("Hello, World!"));

app.Consume<MyMessage>(message => 
{
    // Your handler code here
    return Task.CompletedTask;
});

await app.RunAsyncWithShutdown();

