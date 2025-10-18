using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.DTOs;

namespace PetFamily.Discussions.Application.Queries.GetDiscussionByRelationId;

public class GetDiscussionByRelationIdQueryHandler 
    : IQueryHandler<DiscussionDto?, GetDiscussionByRelationIdQuery>
{
    private readonly IDiscussionsReadDbContext _readDbContext;

    public GetDiscussionByRelationIdQueryHandler(IDiscussionsReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<DiscussionDto?> Handle(
        GetDiscussionByRelationIdQuery query, 
        CancellationToken cancelationToken = default)
    {
        return _readDbContext.Discussions
            .Include(d => d.Messages)
            .FirstOrDefaultAsync(d => d.RelationId == query.RelationId, cancelationToken);
    }
}
