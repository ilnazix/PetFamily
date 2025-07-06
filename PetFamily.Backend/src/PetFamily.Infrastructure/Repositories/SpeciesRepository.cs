using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species.Commands;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Volunteers;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly ApplicationWriteDbContext _dbContext;

        public SpeciesRepository(ApplicationWriteDbContext context)
        {
            _dbContext = context;    
        }

        public async Task<Guid> Add(Species species, CancellationToken cancellationToken = default)
        {
            await _dbContext.Species.AddAsync(species, cancellationToken);
            await _dbContext.SaveChangesAsync();

            return species.Id;
        }

        public async Task<Result<Guid, Error>> Delete(Species species, CancellationToken cancelationToken)
        {
            _dbContext.Species.Remove(species);
            await _dbContext.SaveChangesAsync(cancelationToken);

            return species.Id.Value;
        }

        public async Task<Result<Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default)
        {
            var species = await _dbContext.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if(species is null)
            {
                return Errors.General.NotFound(id.Value);
            }

            return species;
        }

        public async Task<Result<Guid, Error>> Save(Species species, CancellationToken cancellationToken = default)
        {
            _dbContext.Species.Attach(species);
            await _dbContext.SaveChangesAsync();

            return species.Id.Value;
        }
    }
}
