namespace PetFamily.Core.Messaging;

public interface IMessageQueue<TMessage>
{
    Task<TMessage> ReadAsync(CancellationToken cancellationToken);
    Task WriteAsync(TMessage message, CancellationToken cancellationToken);
}
