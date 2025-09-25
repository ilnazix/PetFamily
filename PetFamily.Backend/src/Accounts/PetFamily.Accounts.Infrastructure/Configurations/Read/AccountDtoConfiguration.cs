using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Application.DTOs;


namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

internal class AccountDtoConfiguration : IEntityTypeConfiguration<AccountDto>
{
    public void Configure(EntityTypeBuilder<AccountDto> builder)
    {
        builder.ToTable(Tables.USERS);

        builder.HasKey(a => a.Id);

        builder
           .HasOne(u => u.AdminAccount)
           .WithOne()
           .HasForeignKey<AdminAccountDto>("user_id");

        builder
            .HasOne(u => u.ParticipantAccount)
            .WithOne()
            .HasForeignKey<ParticipantAccountDto>("user_id");

        builder
            .HasOne(u => u.VolunteerAccount)
            .WithOne()
            .HasForeignKey<VolunteerAccountDto>("user_id");

        builder.Navigation(u => u.AdminAccount).AutoInclude();
        builder.Navigation(u => u.ParticipantAccount).AutoInclude();
        builder.Navigation(u => u.VolunteerAccount).AutoInclude();
    }
}