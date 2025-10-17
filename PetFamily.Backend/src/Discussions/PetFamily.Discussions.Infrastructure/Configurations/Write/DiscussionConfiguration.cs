using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Domain;
using PetFamily.SharedKernel.ValueObjects.Ids;
using System.Text.Json;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write;

internal class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.ToTable(Tables.Discussions);

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(id => id.Value,
                v => DiscussionId.Create(v));

        builder.Property(d => d.RelationId)
            .IsRequired();

        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey("discussion_id");

        builder.Property(d => d.IsClosed)
            .HasDefaultValue(false);

        builder.Property(d => d.ParticipantIds)
         .HasConversion(
             v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
             v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null) ?? new List<Guid>(),
             new ValueComparer<IReadOnlyList<Guid>>(
                 (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                 c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                 c => c.ToList() 
             )
         );
    }
}
