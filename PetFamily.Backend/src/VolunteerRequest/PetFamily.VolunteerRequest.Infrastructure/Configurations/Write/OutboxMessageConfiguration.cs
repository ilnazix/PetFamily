using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteerRequest.Infrastructure.Outbox;

namespace PetFamily.VolunteerRequest.Infrastructure.Configurations.Write;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(Tables.OutboxMessages);

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Type)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(m => m.Payload)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(m => m.OccurredOnUtc)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            .IsRequired();

        builder.Property(m => m.ProcessedOnUtc)
            .HasConversion(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)
            .IsRequired(false);

        builder.HasIndex(m => new
        {
            m.ProcessedOnUtc,
            m.OccurredOnUtc,
        })
        .HasDatabaseName("idx_outbox_messages_unprocessed")
        .IncludeProperties(m => new
        {
            m.Id,
            m.Type,
            m.Payload
        })
        .HasFilter("processed_on_utc IS NULL");
    }
}
