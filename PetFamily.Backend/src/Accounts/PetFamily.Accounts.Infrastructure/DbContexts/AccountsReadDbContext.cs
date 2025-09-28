using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Database;
using PetFamily.Accounts.Application.DTOs;

namespace PetFamily.Accounts.Infrastructure.DbContexts;

internal class AccountsReadDbContext : DbContext, IAccountsReadDbContext
{
    public IQueryable<AccountDto> Accounts => Set<AccountDto>();

    public AccountsReadDbContext(DbContextOptions<AccountsReadDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Constants.SCHEMA);

        builder.ApplyConfigurationsFromAssembly(typeof(AccountsReadDbContext).Assembly,
            type => type.FullName?.Contains(Constants.READ_DB_CONTEXT_CONFIGURATIONS) ?? false);
    }
}