using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Volunteers;
using System.Text.Json;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable(Tables.Volunteers);

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
    }
}
