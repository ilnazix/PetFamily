using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;
using System.Text.Json;

namespace PetFamily.Infrastructure.Configurations.Write
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasConversion(
                    id => id.Value,
                    value => VolunteerId.Create(value));

            builder.ComplexProperty(v => v.FullName, fnb =>
            {
                fnb.Property(fn => fn.FirstName)
                    .IsRequired()
                    .HasMaxLength(FullName.NAME_MAX_LENGTH)
                    .HasColumnName("first_name");

                fnb.Property(fn => fn.LastName)
                    .IsRequired()
                    .HasMaxLength(FullName.NAME_MAX_LENGTH)
                    .HasColumnName("last_name");

                fnb.Property(fn => fn.MiddleName)
                    .IsRequired()
                    .HasMaxLength(FullName.NAME_MAX_LENGTH)
                    .HasColumnName("middle_name");
            });

            builder.Property(v => v.Email)
                .HasConversion(email => email.Value, value => Email.Create(value).Value)
                .IsRequired();

            builder.Property(v => v.Description)
                .HasConversion(description => description == null ? null : description.Value,
                    value => string.IsNullOrEmpty(value) ? null : Description.Create(value).Value)
                .IsRequired(false)
                .HasMaxLength(Description.DESCRIPTION_MAX_LENGTH);

            builder.Property(v => v.WorkExperienceInYears)
                .HasConversion(we => we.Value, value => Experience.Create(value).Value);

            builder.Property(v => v.PhoneNumber)
                .HasConversion(pn => pn.Value, value => PhoneNumber.Create(value).Value)
                .IsRequired();

            builder.HasMany(v => v.Pets)
                .WithOne()
                .HasForeignKey("volunteer_id");

            builder.Property(v => v.Requisites)
                .HasConversion(req => 
                    JsonSerializer.Serialize(req.Select(r => 
                        new RequisiteDto
                        {
                            Title = r.Title,
                            Description = r.Description
                        }).ToList(), JsonSerializerOptions.Default),

                    json => JsonSerializer.Deserialize<List<RequisiteDto>>(json, JsonSerializerOptions.Default)!
                        .Select(d => Requisite.Create(d.Title, d.Description).Value)
                        .ToList(),

                    new ValueComparer<List<Requisite>>(
                            (c1, c2) => c1!.SequenceEqual(c2!),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => (List<Requisite>)c.ToList()));

            builder
                .Property(v => v.SocialMedias)
                .HasConversion(sm => JsonSerializer.Serialize(sm.Select(sm =>
                        new SocialMediaDto { 
                            Title = sm.Title,
                            Link = sm.Link
                        }
                    ).ToList(), JsonSerializerOptions.Default),

                    json => JsonSerializer.Deserialize<List<SocialMediaDto>>(json, JsonSerializerOptions.Default)!
                        .Select(d => SocialMedia.Create(d.Link, d.Title).Value)
                        .ToList(),

                    new ValueComparer<List<SocialMedia>>(
                            (c1, c2) => c1!.SequenceEqual(c2!),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => (List<SocialMedia>)c.ToList()));
        }
    }
}
