using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Application.DTOs;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetPet;

public class GetPetQueryHandler : IQueryHandler<PetDto?, GetPetQuery>
{
    private readonly IVolunteersReadDbContext _readDbContext;

    public GetPetQueryHandler(IVolunteersReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PetDto?> Handle(
        GetPetQuery query,
        CancellationToken cancelationToken = default)
    {
        var pet = await _readDbContext
            .Pets
            .FirstOrDefaultAsync(p => p.Id == query.Id,
                cancelationToken);

        if (pet is null) return null;

        pet.Photos = pet.Photos.OrderByDescending(p => p.IsMain).ToArray();

        return pet;
    }
}
