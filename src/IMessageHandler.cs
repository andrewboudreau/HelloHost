public interface IMessageHandler<in T>
{
    // Define your methods here
    Task HandleError(T message, Exception ex);
}
