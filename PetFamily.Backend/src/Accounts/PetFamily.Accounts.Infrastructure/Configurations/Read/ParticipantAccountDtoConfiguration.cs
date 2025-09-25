using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Application.DTOs;


namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

internal class ParticipantAccountDtoConfiguration : IEntityTypeConfiguration<ParticipantAccountDto>
{
    public void Configure(EntityTypeBuilder<ParticipantAccountDto> builder)
    {
        builder.ToTable(Tables.PARTICIPANT_ACCOUNTS);
        builder.HasKey(a => a.Id);
    }
}
