using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Database;
using PetFamily.Discussions.Infrastructure.DbContexts;

namespace PetFamily.Discussions.Infrastructure.Utilities;

internal class DiscussionsDbMigrator : IDbMigrator
{
    private readonly DiscussionsWriteDbContext _dbContext;

    public DiscussionsDbMigrator(DiscussionsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Migrate() => _dbContext.Database.Migrate();
}