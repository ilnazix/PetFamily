using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Core.Database;

namespace PetFamily.Accounts.Infrastructure.Utilities;

internal class AccountsDbMigrator : IDbMigrator
{
    private readonly AccountsWriteDbContext _context;
    public AccountsDbMigrator(AccountsWriteDbContext context) => _context = context;
    public void Migrate() => _context.Database.Migrate();
}