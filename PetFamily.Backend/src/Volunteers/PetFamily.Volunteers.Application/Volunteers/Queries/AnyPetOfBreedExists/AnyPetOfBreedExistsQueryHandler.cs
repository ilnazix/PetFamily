using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.AnyPetOfBreedExists;

public class AnyPetOfBreedExistsQueryHandler : IQueryHandler<bool, AnyPetOfBreedExistsQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;

    public AnyPetOfBreedExistsQueryHandler(
        IVolunteersReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task<bool> Handle(AnyPetOfBreedExistsQuery query, 
        CancellationToken cancelationToken = default)
    {
        var result = _readDbContext
            .Pets
            .AnyAsync(p => p.BreedId == query.BreedId, cancelationToken);

        return result;
    }
}
