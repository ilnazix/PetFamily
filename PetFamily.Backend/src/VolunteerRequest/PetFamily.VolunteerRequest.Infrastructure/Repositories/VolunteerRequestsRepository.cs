using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
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

    public async Task<Result<Domain.VolunteerRequest, Error>> GetById(VolunteerRequestId id, CancellationToken cancellationToken = default)
    {
        var request = await _dbContext.VolunteerRequests
            .FirstOrDefaultAsync(vr => vr.Id == id, cancellationToken);

        if (request is null)
            return Errors.General.NotFound();

        return request;
    }
}
