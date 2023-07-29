// IMessageHandler.cs
public interface IMessageHandler<in T>
{
    Task Handle(T message);
    Task HandleError(T message, Exception ex);
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
