using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Application.DTOs;


namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

internal class AdminAccountDtoConfiguration : IEntityTypeConfiguration<AdminAccountDto>
{
    public void Configure(EntityTypeBuilder<AdminAccountDto> builder)
    {
        builder.ToTable(Tables.ADMIN_ACCOUNTS);
        builder.HasKey(a => a.Id);
    }
}