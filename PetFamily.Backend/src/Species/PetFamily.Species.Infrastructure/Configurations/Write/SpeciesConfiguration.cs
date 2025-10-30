using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Species.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Domain.Models.Species>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Species> builder)
    {
        builder.ToTable(Tables.Species);

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasConversion(id => id.Value, value => SpeciesId.Create(value));

        builder.Property(s => s.Title).IsRequired().HasMaxLength(Domain.Models.Species.SPECIES_TITLE_MAX_LENGTH);

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id");
    }
}
