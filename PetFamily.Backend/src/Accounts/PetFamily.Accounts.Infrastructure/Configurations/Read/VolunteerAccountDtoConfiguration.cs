using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Application.DTOs;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel.ValueObjects;
using System.Text.Json;


namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

internal class VolunteerAccountDtoConfiguration : IEntityTypeConfiguration<VolunteerAccountDto>
{
    public void Configure(EntityTypeBuilder<VolunteerAccountDto> builder)
    {
        builder.ToTable(Tables.VOLUNTEER_ACCOUNTS);
        builder.HasKey(a => a.Id);


        builder.Property(v => v.Requisites)
            .HasConversion(
                req => JsonSerializer.Serialize(
                    req.Select(r =>
                        new RequisiteInfo
                        {
                            Title = r.Title,
                            Description = r.Description
                        }).ToList(),
                    JsonSerializerOptions.Default),

                json => JsonSerializer
                    .Deserialize<IReadOnlyList<RequisiteInfo>>(json, JsonSerializerOptions.Default)!
                    .Select(d => Requisite.Create(d.Title, d.Description).Value)
                    .ToList(),

                new ValueComparer<IReadOnlyList<Requisite>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                )
            );

        builder
           .Property(v => v.SocialMedias)
           .HasConversion(
               sm => JsonSerializer.Serialize(
                   sm.Select(sm =>
                       new SocialMediaInfo()
                       {
                           Title = sm.Title,
                           Link = sm.Link
                       }).ToList(),
                   JsonSerializerOptions.Default),

               json => JsonSerializer
                   .Deserialize<IReadOnlyList<SocialMediaInfo>>(json, JsonSerializerOptions.Default)!
                   .Select(d => SocialMedia.Create(d.Link, d.Title).Value)
                   .ToList(),

               new ValueComparer<IReadOnlyList<SocialMedia>>(
                   (c1, c2) => c1!.SequenceEqual(c2!),
                   c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                   c => c.ToList()
               )
           );
    }
}