using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;

public class HostServiceBus : BackgroundService
{
    private const string connectionString = "<your_connection_string>";
    private const string queueName = "host.all";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var client = new ServiceBusClient(connectionString);

        // create a publisher
        ServiceBusSender sender = client.CreateSender(queueName);

        // create a processor
        ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

        // register a message handler
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        // start processing
        await processor.StartProcessingAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            // publish a message
            await sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes("Hello, Host!")));

            await Task.Delay(1000, stoppingToken);
        }

        // stop processing
        await processor.StopProcessingAsync();
    }

    static Task MessageHandler(ProcessMessageEventArgs args)
    {
        string messageBody = args.Message.Body.ToString();
        Console.WriteLine($"Received: {messageBody}");
        return Task.CompletedTask;
    }

    static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
        return Task.CompletedTask;
    }
}
