using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Domain.Models;



namespace PetFamily.Species.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id).HasConversion(id => id.Value, value => BreedId.Create(value));

        builder.Property(b => b.Title).IsRequired().HasMaxLength(Breed.MAX_BREED_TITLE_LENGTH);
    }
}
