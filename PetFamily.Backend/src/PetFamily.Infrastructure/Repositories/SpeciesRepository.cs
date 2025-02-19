using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Infrastructure.DbContexts;


namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly ApplicationWriteDbContext _context;

        public SpeciesRepository(ApplicationWriteDbContext context)
        {
            _context = context;    
        }

        public async Task<Result<Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default)
        {
            var species = await _context.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == id);

            if(species is null)
            {
                return Errors.General.NotFound(id.Value);
            }

            return species;
        }
    }
}
