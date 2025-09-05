using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Configurations;

namespace PetFamily.Accounts.Infrastructure;

internal class AccountsDbContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }


    public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.HasDefaultSchema(Constants.SCHEMA);

        builder.ApplyConfiguration(new UserConfiguration());

        builder
            .Entity<Role>()
            .ToTable("roles")
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermission>();

        builder.Entity<RolePermission>().ToTable("role_permission");

        builder
            .Entity<Permission>()
            .ToTable("permissions")
            .HasIndex(p => p.Code)
            .IsUnique();

        builder.Entity<ParticipantAccount>().ToTable("participant_accounts");
        builder.Entity<VolunteerAccount>().ToTable("volunteer_accounts");
        builder.Entity<AdminAccount>().ToTable("admin_accounts");

        builder
            .Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");

        builder
            .Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        builder
            .Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        builder
            .Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");

        builder
            .Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
    }
}
