using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
             .ToTable("users");

        builder
            .HasOne(u => u.AdminAccount)
            .WithOne()
            .HasForeignKey<AdminAccount>("user_id");

        builder
            .HasOne(u => u.ParticipantAccount)
            .WithOne()
            .HasForeignKey<ParticipantAccount>("user_id");

        builder
            .HasOne(u => u.VolunteerAccount)
            .WithOne()
            .HasForeignKey<VolunteerAccount>("user_id");

        builder
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        builder.Navigation(u => u.AdminAccount).AutoInclude();
        builder.Navigation(u => u.ParticipantAccount).AutoInclude();
        builder.Navigation(u => u.VolunteerAccount).AutoInclude();
    }
}
