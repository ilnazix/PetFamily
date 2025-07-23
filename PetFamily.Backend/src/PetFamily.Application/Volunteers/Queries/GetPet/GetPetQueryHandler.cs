using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Queries.GetPet
{
    public class GetPetQueryHandler : IQueryHandler<PetDto?, GetPetQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetPetQueryHandler(IReadDbContext readDbContext)
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
}
