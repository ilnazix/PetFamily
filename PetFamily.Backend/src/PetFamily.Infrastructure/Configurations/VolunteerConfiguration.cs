using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer;

namespace PetFamily.Infrastructure.Configurations
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

            builder
               .OwnsOne(v => v.Requisites, rlb =>
                {
                    rlb.ToJson();

                    rlb.OwnsMany(v => v.Requisites, rb =>
                    {
                        rb.Property(r => r.Title)
                            .IsRequired()
                            .HasMaxLength(Requisite.REQUISITE_TITLE_MAX_LENGTH);

                        rb.Property(r => r.Description)
                            .IsRequired()
                            .HasMaxLength(Requisite.REQUISITE_DESCRIPTION_MAX_LENGTH);
                    });
                });

            builder
                .OwnsOne(v => v.SocialMediaList, smb =>
                {
                    smb.ToJson();

                    smb.OwnsMany(v => v.SocialMedias, sb =>
                    {
                        sb.Property(s => s.Link)
                            .IsRequired()
                            .HasMaxLength(SocialMedia.SOCIAL_MEDIA_LINK_MAX_LENGTH);

                        sb.Property(s => s.Title)
                            .IsRequired()
                            .HasMaxLength(SocialMedia.SOCIAL_MEDIA_TITLE_MAX_LENGTH);
                    });
                });
        }
    }
}
