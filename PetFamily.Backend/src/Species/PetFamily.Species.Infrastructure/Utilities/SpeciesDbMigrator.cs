using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Database;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure.Utilities;

internal class SpeciesDbMigrator : IDbMigrator
{
    private readonly SpeciesWriteDbContext _dbContext;

    public SpeciesDbMigrator(SpeciesWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Migrate() => _dbContext.Database.Migrate();
}