using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfSpeciesExists;

public record AnyPetOfScpeciesExistsQuery(Guid SpeciesId) : IQuery;

public class AnyPetOfSpeciesExistsQueryHandler : IQueryHandler<bool, AnyPetOfScpeciesExistsQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;

    public AnyPetOfSpeciesExistsQueryHandler(
        IVolunteersReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<bool> Handle(AnyPetOfScpeciesExistsQuery query, CancellationToken cancelationToken = default)
    {
        return _readDbContext
            .Pets
            .AnyAsync(p => 
                p.SpeciesId == query.SpeciesId,
                cancelationToken);
    }
}
