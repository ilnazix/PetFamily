using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Database;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure.Utilities;

internal class VolunteersDbMigrator : IDbMigrator
{
    private readonly VolunteersWriteDbContext _dbContext;

    public VolunteersDbMigrator(VolunteersWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Migrate() => _dbContext.Database.Migrate();
}