// IMessageHandler.cs

using Azure.Messaging.ServiceBus;

public interface IConsumer<TMessage>
{
    Task Handle(ConsumerContext<TMessage> message);

    Task HandleError(ConsumerContext<TMessage> message, Exception ex);
}

public class ConsumerContext<TMessage>
{
    public ConsumerContext(string user, BinaryData messageData)
    {
        User = user;
        MessageData = messageData;
        typedMessage ??= MessageData.ToObjectFromJson<TMessage>()
            ?? throw new InvalidCastException($"There was an error casting MessageData to {typeof(TMessage)}. Value: '{messageData}'.");
    }

    private TMessage typedMessage;

    public string User { get; }

    public BinaryData MessageData { get; }

    public TMessage Message =>
        typedMessage ??= MessageData.ToObjectFromJson<TMessage>();
}

// BackgroundConsumer.cs 
public class BackgroundConsumer : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public BackgroundConsumer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }

    public void RegisterHandler<TMessage>(Action<IServiceProvider, TMessage> handler, Func<TMessage, Exception, Task> errorHandler = null)
    {
        // register handler and error handler in DI
    }
}

// ServiceCollectionExtensions.cs
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumer(this IServiceCollection services)
    {
        services.AddHostedService<BackgroundConsumer>();
        return services;
    }

    public static IServiceCollection AddHandler<T>(this IServiceCollection services, Func<T, Task> handler, Func<T, Exception, Task> errorHandler = null)
    {
        return services;
    }
}
