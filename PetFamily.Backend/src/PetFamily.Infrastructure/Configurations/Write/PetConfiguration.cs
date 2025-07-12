using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Volunteers;
using System.Text.Json;

namespace PetFamily.Infrastructure.Configurations.Write
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable(Tables.Pets);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasConversion(id => id.Value, value => PetId.Create(value));

            builder.Property(p => p.Name)
                .HasConversion(name => name.Value, value => PetName.Create(value).Value)
                .IsRequired()
                .HasMaxLength(PetName.PET_NAME_MAX_LENGTH);

            builder.ComplexProperty(p => p.PetType, ptb =>
            {
                ptb.Property(pt => pt.SpeciesId)
                    .IsRequired()
                    .HasColumnName("species_id")
                    .HasConversion(speciesId => speciesId.Value, value => SpeciesId.Create(value));

                ptb.Property(pt => pt.BreedId)
                    .IsRequired()
                    .HasColumnName("breed_id");
            });

            builder.Property(p => p.Position)
                .HasConversion(p => p.Value, value => Position.Create(value).Value);

            builder.Property(p => p.Status)
                .HasConversion(status => status.Value, value => PetStatus.Create(value).Value);

            builder.Property(p => p.Description)
                .HasConversion(description => description.Value, value => Description.Create(value).Value)
                .IsRequired()
                .HasMaxLength(Description.DESCRIPTION_MAX_LENGTH);

            builder.Property(p => p.Color)
                .HasConversion(color => color.Title, title => Color.Create(title).Value)
                .IsRequired(false)
                .HasMaxLength(Color.MAX_COLOR_TITLE_LENGTH);

            builder.OwnsOne(p => p.MedicalInformation, mib =>
            {
                mib.Property(mi => mi.IsVaccinated).HasColumnName("is_vaccinated");
                mib.Property(mi => mi.IsCastrated).HasColumnName("is_castrated");
                mib.Property(mi => mi.Height).HasColumnName("height");
                mib.Property(mi => mi.Weight).HasColumnName("weight");
                mib.Property(mi => mi.HealthInformation)
                    .HasMaxLength(MedicalInformation.MAX_HEALTH_INFO_LENGTH);
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

            builder.Property(v => v.Requisites)
                 .HasConversion(req =>
                     JsonSerializer.Serialize(req.Select(r =>
                         new RequisiteDto
                         {
                             Title = r.Title,
                             Description = r.Description
                         }).ToList(), JsonSerializerOptions.Default),

                     json => JsonSerializer.Deserialize<IReadOnlyList<RequisiteDto>>(json, JsonSerializerOptions.Default)!
                         .Select(d => Requisite.Create(d.Title, d.Description).Value)
                         .ToList(),

                     new ValueComparer<IReadOnlyList<Requisite>>(
                             (c1, c2) => c1!.SequenceEqual(c2!),
                             c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                             c => (IReadOnlyList<Requisite>)c.ToList()));

            builder.Property(v => v.Photos)
                .HasConversion(photo =>
                    JsonSerializer.Serialize(photo.Select(ph =>
                        new PhotoDto
                        {
                            FileName = ph.FileName,
                            Path = ph.Path,
                            IsMain = ph.IsMain
                        }).ToList(), JsonSerializerOptions.Default),

                    json => JsonSerializer.Deserialize<IReadOnlyList<PhotoDto>>(json, JsonSerializerOptions.Default)!
                        .Select(d => Photo.Create(d.Path, d.FileName, d.IsMain).Value)
                        .ToList(),

                    new ValueComparer<IReadOnlyList<Photo>>(
                            (c1, c2) => c1!.SequenceEqual(c2!),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => (IReadOnlyList<Photo>)c.ToList()));

        }
    }
}
