using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Volunteer;
using System.Net;

namespace PetFamily.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasConversion(id => id.Value, value => PetId.Create(value));

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            builder.ComplexProperty(p => p.PetType, ptb =>
            {
                ptb.Property(pt => pt.SpeciesId)
                    .IsRequired()
                    .HasColumnName("species_id")
                    .HasConversion(speciesId => speciesId.Value, value => SpeciesId.Create(value));

                ptb.Property(pt => pt.BreedId)
                    .IsRequired()
                    .HasColumnName("breeed_id");
            });

            builder
                .Property(p => p.Status)
                .HasConversion(status => status.Value, value => PetStatus.Create(value).Value);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

            builder.Property(p => p.Color)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            builder.OwnsOne(p => p.MedicalInformation, mib =>
            {
                mib.Property(mi => mi.IsVaccinated).HasColumnName("is_vaccinated");
                mib.Property(mi => mi.IsCastrated).HasColumnName("is_castrated");
                mib.Property(mi => mi.Height).HasColumnName("height");
                mib.Property(mi => mi.Weight).HasColumnName("weight");
                mib.Property(mi => mi.HealthInformation)
                    .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
            });

            builder.OwnsOne(p => p.Address, ab =>
            {
                ab.Property(a => a.Country).HasMaxLength(Address.FIELD_MAX_LENGTH).HasColumnName("country");
                ab.Property(a => a.State).HasMaxLength(Address.FIELD_MAX_LENGTH).HasColumnName("state");
                ab.Property(a => a.City).HasMaxLength(Address.FIELD_MAX_LENGTH).HasColumnName("city");
                ab.Property(a => a.Street).HasMaxLength(Address.FIELD_MAX_LENGTH).HasColumnName("street");
                ab.Property(a => a.HouseNumber).HasMaxLength(Address.FIELD_MAX_LENGTH).HasColumnName("house_number");
                ab.Property(a => a.HouseNumber).HasColumnName("apartment_number");
            });

            builder.Property(p => p.OwnerPhoneNumber)
                .HasConversion(phoneNumber => phoneNumber.Value, value => PhoneNumber.Create(value).Value);

            builder.Property(p => p.DateOfBirth);
            builder.Property(p => p.CreatedAt);

            builder.OwnsOne(p => p.RequisitesList, rlb =>
            {
                rlb.ToJson();

                rlb.OwnsMany(v => v.Requisites, rb =>
                {
                    rb.Property(r => r.Title)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                    rb.Property(r => r.Description)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
                });
            });
        }
    }
}
