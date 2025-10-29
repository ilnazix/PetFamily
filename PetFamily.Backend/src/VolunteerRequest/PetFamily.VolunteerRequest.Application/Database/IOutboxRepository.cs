namespace PetFamily.VolunteerRequest.Application.Database;

public interface IOutboxRepository
{
    Task Add<T>(T message, CancellationToken cancellationToken);
}
