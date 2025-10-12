using Microsoft.EntityFrameworkCore;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Infrastructure.DbContexts;

internal class DiscussionsWriteDbContext : DbContext
{
    public DbSet<Discussion> Discussions { get; set; }

    public DiscussionsWriteDbContext(
        DbContextOptions<DiscussionsWriteDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.SCHEMA);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscussionsWriteDbContext).Assembly,
            type => type.FullName?.Contains(Constants.WRITE_DB_CONTEXT_CONFIGURATIONS) ?? false);
    }
}