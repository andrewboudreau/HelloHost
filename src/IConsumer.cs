// IMessageHandler.cs

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
    private IServiceProvider provider;

    public BackgroundConsumer(IServiceProvider provider)
    {
        this.provider = provider;
    }

    protected override Task ExecuteAsync(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }

    public void RegisterHandler<T>(Action<IServiceProvider, T> handler, Func<T, Exception, Task> errorHandler = null)
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
