using Microsoft.AspNetCore.Builder;
using System;

public static class WebApplicationExtensions
{
    public static void Consume<T>(this WebApplication app, string queueName, Action<IServiceProvider, T> handler)
    {
        var consumer = app.Services.GetService<BackgroundConsumer>();
        consumer.RegisterHandler(queueName, handler);
    }
}
