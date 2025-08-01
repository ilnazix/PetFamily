using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Application.Species.Commands;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly SpeciesWriteDbContext _dbContext;

        public SpeciesRepository(SpeciesWriteDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Guid> Add(Domain.Models.Species species, CancellationToken cancellationToken = default)
        {
            await _dbContext.Species.AddAsync(species, cancellationToken);

            return species.Id;
        }

        public async Task<Result<Guid, Error>> Delete(Domain.Models.Species species, CancellationToken cancelationToken)
        {
            _dbContext.Species.Remove(species);

            return species.Id.Value;
        }

        public async Task<Result<Domain.Models.Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken = default)
        {
            var species = await _dbContext.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (species is null)
            {
                return Errors.General.NotFound(id.Value);
            }

            return species;
        }
    }
}
