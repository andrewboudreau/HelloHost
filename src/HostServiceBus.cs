using Azure.Messaging.ServiceBus;

using HelloHost.Application;

using Microsoft.Extensions.Options;

using System.Text;

public class HostServiceBus : BackgroundService
{
    private readonly ServiceBusClient serviceBusClient;
    private readonly string queueName;

    public HostServiceBus(ServiceBusClient serviceBusClient, IOptions<AppSettings> appSettings)
    {
        this.serviceBusClient = serviceBusClient;
        
        queueName = appSettings.Value.AzureServiceBusQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ServiceBusSender sender = serviceBusClient.CreateSender(queueName);

        // create a processor
        ServiceBusProcessor processor = serviceBusClient.CreateProcessor(queueName);

        // register a message handler
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        // start processing
        await processor.StartProcessingAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            // publish a message
            await sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes("Hello, Host!")));
            await sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes("Hello, Error!")));

            await Task.Delay(1000, stoppingToken);
        }

        // stop processing
        await processor.StopProcessingAsync();
    }

    static Task MessageHandler(ProcessMessageEventArgs args)
    {
        string messageBody = args.Message.Body.ToString();
        Console.WriteLine($"Received: {messageBody}");

        if (messageBody.Contains("Error"))
            throw new InvalidOperationException("Hello, Error!");

        return Task.CompletedTask;
    }

    static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
        return Task.CompletedTask;
    }
}
