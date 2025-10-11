namespace PetFamily.VolunteerRequest.Application.Commands;

public interface IVolunteerRequestsRepository 
{
    Task<Guid> Add(Domain.VolunteerRequest request, CancellationToken cancellationToken = default);
}
