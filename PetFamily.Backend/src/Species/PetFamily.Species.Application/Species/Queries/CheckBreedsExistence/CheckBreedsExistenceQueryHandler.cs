using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Application.Database;

namespace PetFamily.Species.Application.Species.Queries.CheckIfBreedsExistsQuery
{
    public class CheckBreedsExistenceQueryHandler : IQueryHandler<bool, CheckBreedsExistenceQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public CheckBreedsExistenceQueryHandler(
            ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<bool> Handle(CheckBreedsExistenceQuery query, CancellationToken cancelationToken = default)
        {
            var result = await _readDbContext
                   .Breeds
                   .AnyAsync(b => b.Id == query.BreedId 
                        && b.SpeciesId == query.SpeciesId);

            return result;
        }
    }
}
