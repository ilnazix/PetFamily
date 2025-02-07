namespace PetFamily.Application.Messaging
{
    public interface IMessageQueue<TMessage>
    {
        Task<TMessage> ReadAsync(CancellationToken cancellationToken);
        Task WriteAsync(TMessage message, CancellationToken cancellationToken);
    }
}
