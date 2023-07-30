using Azure.Messaging.ServiceBus;

using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;

public class HostServiceBus : BackgroundService
{
    //private readonly ServiceBusClient serviceBusClient;
    private readonly ServiceBusProcessor processor;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly Func<object, Task> handler;
    private readonly Func<object, Task> errorHandler;

    //private readonly string queueName;

    public HostServiceBus(
        ServiceBusProcessor processor,
        IServiceScopeFactory scopeFactory,
        Func<object, Task> handler,
        Func<object, Task> errorHandler)
    {
        this.processor = processor;
        this.scopeFactory = scopeFactory;
        this.handler = handler;
        this.errorHandler = errorHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // register a message handler
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        await processor.StartProcessingAsync(stoppingToken);

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }
        catch (TaskCanceledException)
        {
            await processor.StopProcessingAsync(CancellationToken.None);
        }
    }

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string messageBody = args.Message.Body.ToString();
        Console.WriteLine($"Received: {messageBody}");

        if (messageBody.Contains("Error"))
        {
            throw new InvalidOperationException("Hello, Error!");
        }

        await handler(args.Message);
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
        return Task.CompletedTask;
    }
}
