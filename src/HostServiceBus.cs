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

        // create a consumer
        ServiceBusReceiver receiver = client.CreateReceiver(queueName);

        while (!stoppingToken.IsCancellationRequested)
        {
            // publish a message
            await sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes("Hello, Host!")));

            // consume a message
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            string messageBody = receivedMessage.Body.ToString();

            Console.WriteLine($"Received: {messageBody}");

            await Task.Delay(1000, stoppingToken);
        }
    }
}
