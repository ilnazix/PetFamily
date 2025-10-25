using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Database;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;

namespace PetFamily.VolunteerRequest.Infrastructure.Utilities;

internal class VolunteerRequestsDbMigrator : IDbMigrator
{
    private readonly VolunteerRequestsWriteDbContext _dbContext;

    public VolunteerRequestsDbMigrator(VolunteerRequestsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Migrate() => _dbContext.Database.Migrate();
    
}