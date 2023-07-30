using Azure.Messaging.ServiceBus;

using Microsoft.AspNetCore.Builder;

using System;

public static class WebApplicationExtensions
{
    public static void Consume<T>(this WebApplication app, string queueName, Action<IServiceProvider, T> handler)
    {
        var consumer = app.Services.GetService<BackgroundConsumer>();
        consumer.RegisterHandler(queueName, handler);
    }

    public static WebApplication MapServiceBus(this WebApplication app, string queueName, Delegate function)
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
