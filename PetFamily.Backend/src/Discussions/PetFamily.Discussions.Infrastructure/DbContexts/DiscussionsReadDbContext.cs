using Microsoft.EntityFrameworkCore;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.DTOs;

namespace PetFamily.Discussions.Infrastructure.DbContexts;

internal class DiscussionsReadDbContext : DbContext, IDiscussionsReadDbContext
{
    IQueryable<DiscussionDto> IDiscussionsReadDbContext.Discussions => Set<DiscussionDto>();

    public DiscussionsReadDbContext(
        DbContextOptions<DiscussionsReadDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.SCHEMA);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscussionsReadDbContext).Assembly,
            type => type.FullName?.Contains(Constants.READ_DB_CONTEXT_CONFIGURATIONS) ?? false);
    }
}