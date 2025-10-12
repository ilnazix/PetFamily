using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Application.DTOs;
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

        builder.Property(d => d.Users)
            .HasConversion(d =>
                     JsonSerializer.Serialize(d.Select(
                         d => new Participant(d.Id, d.Email)).ToList(), JsonSerializerOptions.Default),

                     json => JsonSerializer.Deserialize<IReadOnlyList<Participant>>(json, JsonSerializerOptions.Default)!
                         .Select(d => User.Create(d.Id, d.Email).Value)
                         .ToList(),

                     new ValueComparer<IReadOnlyList<User>>(
                             (c1, c2) => c1!.SequenceEqual(c2!),
                             c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                             c => c.ToList()));
    }
}
