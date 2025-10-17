using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.VolunteerRequest.Application.Commands;

public interface IVolunteerRequestsRepository 
{
    Task<Guid> Add(Domain.VolunteerRequest request, CancellationToken cancellationToken = default);
    Task<Result<Domain.VolunteerRequest, Error>> GetById(VolunteerRequestId id, CancellationToken cancellationToken = default);
}
