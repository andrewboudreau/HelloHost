using Azure.Messaging.ServiceBus;
using HelloHost;

var builder = HelloHostWebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/", () => Results.Ok("Hello, World!"));

app.MapServiceBus<MyMessage>("hellohost-mymessage", (myMessage, sp) =>
{
    Console.WriteLine($"Received message from ServiceBus: {myMessage.Id} at {myMessage.Date}");
});

app.MapRabbitMq<MyMessage>("hellohost-mymessage", (myMessage, sp) =>
{
    Console.WriteLine($"Received message from RabbitMq: {myMessage.Id} at {myMessage.Date}");
});

app.MapStorageQueue<MyMessage>("hellohost-mymessage", (myMessage, sp) =>
{
    Console.WriteLine($"Received message from StorageQueue: {myMessage.Id} at {myMessage.Date}");
});


await app.RunAsyncWithShutdown();

public record MyMessage(DateTime Date, Guid Id);

public static class WebApplicationExtensions
{
    public static void MapServiceBus<TMessage>(this WebApplication app, string queueName, Action<TMessage, IServiceProvider> handler)
    {
        var serviceBusConnectionString = "";  // You should fetch this from a secure place
        var client = new ServiceBusClient(serviceBusConnectionString);
        var processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += async args =>
        {
            using var scope = app.Services.CreateScope();
            var message = args.Message.Body.ToObjectFromJson<TMessage>();
            handler(message, scope.ServiceProvider);
        };

        processor.ProcessErrorAsync += ExceptionReceivedHandler;

        // Start the processor
        processor.StartProcessingAsync();
    }

    static Task ExceptionReceivedHandler(ProcessErrorEventArgs args)
    {
        // Handle any exceptions that occur during message reception
        return Task.CompletedTask;
    }

    public static void MapRabbitMq<TMessage>(this WebApplication app, string queueName, Action<TMessage, IServiceProvider> handler)
    {
        var consumer = app.Services.GetService<BackgroundConsumer>();
        //consumer.RegisterHandler(queueName, handler);
    }

    public static void MapStorageQueue<TMessage>(this WebApplication app, string queueName, Action<TMessage, IServiceProvider> handler)
    {
        var consumer = app.Services.GetService<BackgroundConsumer>();
        //consumer.RegisterHandler(queueName, handler);
    }

    public static WebApplication MapServiceBusQueue(this WebApplication app, string queueName, Delegate function)
    {
        var serviceBusConnectionString = "";  // You should fetch this from a secure place
        var client = new ServiceBusClient(serviceBusConnectionString);
        var processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += async args =>
        {
            using var scope = app.Services.CreateScope();
            var parameters = function.Method.GetParameters();
            var arguments = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                arguments[i] = scope.ServiceProvider.GetRequiredService(parameters[i].ParameterType);
            }

            await (Task)function.DynamicInvoke(arguments)!;
        };

        processor.ProcessErrorAsync += ExceptionReceivedHandler;

        // Start the processor
        processor.StartProcessingAsync();

        return app;
    }

    static Task ExceptionReceivedHandler(ProcessErrorEventArgs args)
    {
        // Handle any exceptions that occur during message reception
        return Task.CompletedTask;
    }
}
