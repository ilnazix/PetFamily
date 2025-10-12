using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Domain;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write;

internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(Tables.Messages);

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(id => id.Value,
                v => MessageId.Create(v));

        builder.Property(m => m.UserId)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.IsEdited)
            .IsRequired();
    }
}
