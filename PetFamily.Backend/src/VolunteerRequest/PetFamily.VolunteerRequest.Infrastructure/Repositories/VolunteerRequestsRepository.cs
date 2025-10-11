using PetFamily.VolunteerRequest.Application.Commands;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;

namespace PetFamily.VolunteerRequest.Infrastructure.Repositories;

internal class VolunteerRequestsRepository : IVolunteerRequestsRepository
{
    private readonly VolunteerRequestsWriteDbContext _dbContext;

    public VolunteerRequestsRepository(VolunteerRequestsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Domain.VolunteerRequest request, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(request, cancellationToken);

        return request.Id;
    }
}
