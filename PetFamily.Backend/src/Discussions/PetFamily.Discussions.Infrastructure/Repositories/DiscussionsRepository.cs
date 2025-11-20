using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Discussions.Application.Commands;
using PetFamily.Discussions.Domain;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Infrastructure.Repositories;

internal class DiscussionsRepository : IDiscussionsRepository
{
    private readonly DiscussionsWriteDbContext _dbContext;

    public DiscussionsRepository(DiscussionsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Discussion discussion, CancellationToken cancellationToken = default)
    {
        await  _dbContext.AddAsync(discussion, cancellationToken);

        return discussion.Id;
    }

    public async Task<Result<Discussion, Error>> GetById(
        DiscussionId id, 
        CancellationToken cancellationToken = default)
    {
        var discussion = await _dbContext.Discussions
            .Include(d => d.Messages)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (discussion is null)
            return Errors.General.NotFound(id);

        return discussion;
    }

    public async Task<Result<Discussion, Error>> GetByRelationId(
        Guid relationId, 
        CancellationToken cancellationToken = default)
    {
        var discussion = await _dbContext.Discussions
           .Include(d => d.Messages)
           .FirstOrDefaultAsync(d => d.RelationId == relationId, cancellationToken);

        if (discussion is null)
            return Errors.General.NotFound(relationId);

        return discussion;
    }
}
