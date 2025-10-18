using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Application.DTOs;

namespace PetFamily.Discussions.Infrastructure.Configurations.Read;

internal class MessageDtoConfiguration : IEntityTypeConfiguration<MessageDto>
{
    public void Configure(EntityTypeBuilder<MessageDto> builder)
    {
        builder.ToTable(Tables.Messages);

        builder.HasKey(m => m.Id);
    }
}

internal class DiscussionDtoConfiguration : IEntityTypeConfiguration<DiscussionDto>
{
    public void Configure(EntityTypeBuilder<DiscussionDto> builder)
    {
        builder.ToTable(Tables.Discussions);

        builder.HasKey(d => d.Id);

        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey("discussion_id"); ;
    }
}
